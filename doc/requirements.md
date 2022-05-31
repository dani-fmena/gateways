## 📜 Requirement

The following requirements needs to be matched before running the installation script:

- This tiny project was made with ```SqlLocalDb```, not with a full SQL Server Instance. So element such as authentication elements 
  such as authentication did not have to be considered.
  - If you have to use a full SQL Server you have to setup the _ConnectionStrings_ section properly in the following files:
    - gateway.api/appsettings.development.json
    - gateway.factory/settings.development.json
  - If you **database name** is other tan **gateway** so you have to setup the _ConnectionStrings_ section properly located in the
    the same files listed previously.
- This project was made, build and test on **Windows 10**. If it will deploy on linux some problems may appears such as case sensitive problems.
  Testing this project on windows is recommended.
- The installation script use powershell for downloading some dependencies.
- The installation script need admin privileges. 

❗ You don't have to worry about other dependencies such as .Net Core SDK  and NodeJs, the installation script will try to check
download and install them.


