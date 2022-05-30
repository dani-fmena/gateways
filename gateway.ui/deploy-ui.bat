@echo off

set NODE_VER=null
set NODE_EXEC=node-v16.15.0-x64.msi
set SETUP_DIR=%CD%
node -v >tmp.txt
set /p NODE_VER=<tmp.txt
del tmp.txt

IF %NODE_VER% NEQ v16.15.0 (
	echo node v16.15.0 is not installed ...
) ELSE (
	echo Node %NODE_VER% is already installed. Proceeding ...
)

call npm run deploy
