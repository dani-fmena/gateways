## 🚀 Deployment

If you have the the ```SqlLocalDb``` up running and you database name is **gateway** you can run the installation script ```install.cmd``` 
from and admin admin terminal. This script tries to get missing dependencies (.NetCore and Node) and make the build for .Net Core 
entire solution. 

For running the script go to the roo project folder and then run 

_for Powershell terminal_
```batch
> .\install.cmd
```

_for CMD terminal_
```batch
> install.cmd
```

If the script show no error and after running appears the fallowing message on the terminal screen:

```== All the build automation process were DONE, you can run the system ==```

Then you can proceed to run all the system components as fallow:

---

* **🌐 Rest API** 
   
   go to _gateway.api_ folder using a terminal and run
   ```batch
   > dotnet run
   ````
  ❗ with the default config it should run on port 7000 

---

* **📱 APP UI**
  
   go to _gateway.ui_ folder using a terminal and run
  ```batch
  > npm run deploy
  ````
  ❗ with the default config it should run on port 9000
---

* **🧪 Endpoints Tests**
  go to _gateway.api.tests_ folder using a terminal and run
  ```batch
  > npm run test
  ````
  

  
  


