# Gateway App (gateway.ui)

The ui must also offer an operation for displaying information about all stored gateways (and their devices) and an operation for displaying details for a single gateway. Finally, it must be possible to add and remove a device from a gateway.

## -- automated build --
Before you must execute the `install.cmd` script located in root of main folder
### Start the app using -- automated build --
```bash
# in windows
C:\Users\WORKSPACE\gateways\gateway.ui> deploy-ui.bat

# in linux
$ ./deploy-ui.sh
```

## -- manual build --

## Install the dependencies
```bash
yarn
# or
npm install
```

### Start the app (manually)
```bash
npm run deploy
```

### Start the app in development mode (hot-code reloading, error reporting, etc.)
```bash
quasar dev
```


## Others command

### Format the files
```bash
yarn format
# or
npm run format
```



### Build the app for production
```bash
quasar build
```

### Customize the configuration
See [Configuring quasar.config.js](https://v2.quasar.dev/quasar-cli-vite/quasar-config-js).
