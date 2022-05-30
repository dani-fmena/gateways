@ECHO OFF
setlocal
setlocal enabledelayedexpansion

@REM check admin priviledges
echo [94m[+] Checking for Adming shell SDK[0m
NET SESSION >nul 2>&1
IF %ERRORLEVEL% NEQ 0 (
    echo [91m[*]This setup needs admin permissions. Please run this file as admin ... exiting[0m
	goto exiting
)
echo [92m[+] Running with admin priviledge. Proceeding ...[0m
echo.

@REM check if .net core is installed
echo [94m[+] Checking for .Net Core 3.1 SDK[0m
dotnet --version > net.txt
type net.txt | find "3.1"

IF %ERRORLEVEL% == 0 (
    echo [92m[+] .Net Core 3.1 SDK is present. Proceeding ...[0m
    echo.
    goto netcoresdkinstalled
)
echo [93m[*]This Solution needs .Net Core 3.1 SDK ... trying to download and install[0m
echo.
goto downloadNetCore

:downloadNetCore
@REM downloading .net core
IF NOT EXIST NetCore3.1.25.exe (
    echo [94m[+] Downloding .Net Core 3.1 SDK [0m
    powershell -Command "Invoke-WebRequest https://download.visualstudio.microsoft.com/download/pr/5c201d4c-7d2e-4814-89ec-6c5ef533c5ce/1882c3649dd6d55f2b9fc9e906408528/dotnet-sdk-3.1.419-win-x64.exe  -OutFile NetCore3.1.25.exe"
    echo [92m[OK] Downloaded [0m
    echo.
)

@REM try to install .net core 
IF EXIST NetCore3.1.25.exe (
    echo [94m[+] Installing .Net Core 3.1 SDK [0m
    NetCore3.1.25.exe
        
    echo [92m[+] .Net Core 3.1 was Instaled [0m
    echo.
    
    @REM check again if .net core is installed
    echo [94m[+] Checking again for .Net Core 3.1 SDK[0m
    dotnet --version > net.txt
    type net.txt | find "3.1"
    
    IF %ERRORLEVEL% NEQ 0 (
        @REM quiting
        echo [91m[*] This Solution needs .Net Core 3.1 SDK ... exiting [0m
        echo.
        goto exiting
    ) 
)

:netcoresdkinstalled
@REM installing node
ECHO [94m[+] Installing Node [0m
SET NODE_VER=null
SET NODE_EXEC=node-v16.15.0-x64.msi
SET SETUP_DIR=%CD%

@REM checking for the presence of node
node -v > tmp.txt
SET /p NODE_VER=<tmp.txt
IF EXIST tmp.txt DEL /F tmp.txt  @REM deleting the temp file
SET NODE_LTS=false 

IF NOT x%NODE_VER:v14=%==x%NODE_VER% (
    SET NODE_LTS=true   
) 

IF NOT x%NODE_VER:v16=%==x%NODE_VER% (
    SET NODE_LTS=true
)

REM so nothe isn't here we need to bring it
IF %NODE_LTS% NEQ true (	
	IF NOT EXIST tmp/ ( MKDIR tmp ) 
	IF NOT EXIST tmp/%NODE_EXEC% (
	    ECHO [94m[+] Node setup file does not exist. Downloading ...[0m
		powershell -Command "Invoke-WebRequest https://nodejs.org/dist/v16.15.0/node-v16.15.0-x64.msi  -OutFile node-v16.15.0-x64.msi"
		MOVE %NODE_EXEC% %SETUP_DIR%/tmp
	)
	CD %SETUP_DIR%/tmp
	START /WAIT %NODE_EXEC%
	CD %SETUP_DIR%
	
	echo [92m[+] Node was Instaled [0m
	echo.
) ELSE (
    ECHO [92m[+] Node %NODE_VER% is already installed. Proceeding ...[0m
    echo.
)

REM installing quaser
ECHO [94m[+] Installing Quasar [0m
CD ../..
CALL npm i -g @quasar/cli
REM echo INSTALLING @quasar/cli ...
CD %SETUP_DIR%
echo [92m[+] Quasar was Instaled [0m
echo.

REM building solution
ECHO [94m[+] Building .NetCore Solution (... finger crossed =) [0m
dotnet build Gateways.sln --no-incremental

IF %ERRORLEVEL% NEQ 0 (
    echo.
    echo [91m[*] Somthing bad happends ... exitig [0m
    goto exiting
) ELSE (
    echo.
    echo [92m[+] Solution projects was builded sucessfully [0m
    echo.
)

REM instaling UI deps


:exiting

@REM removing temporal file
IF EXIST net.txt DEL /F net.txt
IF EXIST tmp RMDIR /S /Q tmp

endlocal
