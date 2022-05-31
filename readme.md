Gateways
===

This repo contains the whole Gateway ecosystem. It was build on top of the following components:

- **gateway.factory** Handles the DB context and cares of migrations initial (seeded / populated) data  [_Console Application_]

- **gateway.domain** Holds domain code related to definitions, entities, DTOs and other shared logic [_DLL_]

- **gateway.api** The REST API exposing the endpoints and the Swagger / OpenAPI documentation [_.Net Core WEB API_]
  
- **gateway.dal** "Externally" define DB context, constituting the Data Access Layer (DAL) for this case  [_DLL_]

- **gateway.api.tests** Automated test for all the exposed endpoints [_Node App_]
  
- **gateway.api.ui** Endpoints unit-test project based on Jet and Node [_Node App_]


_technology stack_

![](https://img.shields.io/badge/DataBase-MongoDb-blueviolet?style=flat&logo=MongoDB&logoColor=47A248)
![](https://img.shields.io/badge/DataBase-Postgres-blueviolet?style=flat&logo=PostgreSQL&logoColor=4169E1)
![](https://img.shields.io/badge/DataBase-Redis-blueviolet?style=flat&logo=Redis&logoColor=4169E1)
![](https://img.shields.io/badge/Tech-.NetCore3.1-informational?style=flat&logo=.NET&logoColor=512BD4)
![](https://img.shields.io/badge/Tech-Json-informational?style=flat&logo=JSON&logoColor=000000)
![](https://img.shields.io/badge/Tech-Bearer-informational?style=flat&logo=JSONWebTokens&logoColor=000000)
![](https://img.shields.io/badge/Tech-C%23-informational?style=flat&logo=CSharp&logoColor=239120)
![](https://img.shields.io/badge/Tech-PLpgSQL-informational?style=flat&logo=PostgreSQL&logoColor=4169E1)
![](https://img.shields.io/badge/Tech-Jest-informational?style=flat&logo=Jest&logoColor=C21325)
![](https://img.shields.io/badge/build-passing-brightgreen?style=flat)
![](https://img.shields.io/badge/release-v0.0.0-inactive?style=flat)
![](https://img.shields.io/badge/coverage-0%25-critical?style=flat)
![](https://img.shields.io/badge/reposize-0MB-inactive?style=flat)

## 📑 Table of content

- ### 📜 [Requirements]
- ### 🚀 [Deploy]
- ### 🧪 [Test]
- ### 🚩 [Troubleshooting]
- ### 🖋 [Notes]

## 🗂 ApiDoc

http://\<running ip\>:\<port\>/swagger/index.html

[Requirements]: doc/requirements.md
[Deploy]: doc/deployment.md
[Test]: doc/testenv.md
[Troubleshooting]: doc/troubleshooting.md
[Notes]: doc/notes.md



