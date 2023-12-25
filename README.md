# eShop

## Overview

eShop Application - containing four parts
1) Product API - to place the order 
2) Order service - to create or update the order via grpc service
3) notification service 1 - to recieve the notification from rmq exchange
4) notification service 2 - to recieve the notification from rmq topic

## Table of Contents

- [Installation](#installation)

## Installation

Include instructions on how to install your application. This might include:

- Prerequisites
    1) Dotnet 8
    2) Rabbit MQ
    3) Windows machine
- Installation steps
    1) Install dot net 8 if not install https://dotnet.microsoft.com/en-us/download/dotnet/8.0
    2) Install rabbit mq https://www.rabbitmq.com/download.html
    3) Git clone 
        git clone https://github.com/jinjinesh/eShop.git
    3) Update the configuration (appsettings.json file) with rabbit mq credentials in all project
        - .\src\OrderService\appsettings.json
        - .\src\NotificationService2\appsettings.json
        - .\src\NotificationService1\appsettings.json
```json
    "RabbitMQ": {
        "HostName": "localhost",
        "UserName": "guest",
        "Password": "guest"
    }
```
    4) Goto root folder and run powershell to build and run projects Build-And-Run-Script.ps1 via powershell
