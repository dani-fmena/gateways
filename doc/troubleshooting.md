## 🚩 Troubleshooting

This is not Troubleshooting section per se, at least not a classical one. However, this is a checklist that can comes in handy
if something bad happened:

1. PowerShell, Node and DotNet need to be online to install the solutions dependencies. Check firewall if you have one running on the
   PC.
2. The installations script needs to have admins privileges.
3. If you have to use a full SQL Server you have to setup the _ConnectionStrings_ section properly in the following files:
    - gateway.api/appsettings.development.json
    - gateway.factory/settings.development.json
    
    3.1 If you **database name** is other tan **gateway** so you have to setup the _ConnectionStrings_ section properly located in the
    the same files listed previously.
   
   

