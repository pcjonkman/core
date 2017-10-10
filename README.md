
# core


## 1

- npm install -g yo generator-aspnetcode-spa
- npm install -g webpack
- yo
    - Aspnetcore Spa
    - Aurelia
    - project.json
    - Core

## 2

- dotnet new --install Microsoft.AspNetCore.SpaTemplates::*
- dotnet new aurelia

## Fixing error pushing to Azure
https://github.com/aspnet/Hosting/issues/844

```text
HTTP Error 502.5 - Process Failure
Common causes of this issue:
* The application process failed to start
* The application process started but then stopped
* The application process started but failed to listen on the configured port
```

- Change Core.runtimeconfig.json 
  - Update runtimeOptions > framework > version in 2.0.0

```json
{
  "runtimeOptions": {
    "tfm": "netcoreapp2.0",
    "framework": {
      "name": "Microsoft.NETCore.App",
      "version": "2.0.0"
    },
    "configProperties": {
      "System.GC.Server": true
    }
  }
}
```
