#!/bin/bash


deploy_app() {
  if which node; then
    echo 'Node installed!'

    echo "NODE VERSION:  "
    node -v
    echo "NPM VERSION:  "
    npm -v

#    echo "installing dependencies"
#    npm i

    echo "deploy"
    npm run deploy
  else
    echo 'Your system does not have Node'
  fi
}

deploy_app
