# Echelon
Web frontend for the irc-client.

# Vokram.Bot
The bot part of the project. Implemented for console input and output.

# Vokram.Plugins
The functionality of the bot is extended by plugins. Plugins will in the future be loaded dynamically as dll's at runtime, which will enable loading and reloading of hot updated plugins.

# Vokram.Trainer
This part of the application is responsible for reading IRC chatlogs, create and serialize a markov chain based "brain" that the bot can use to generate random sentences.

# Installation
We need to install .NET Core and msbuild


## Add repository
Ubuntu 16.04
```bash
$ sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ yakkety main" > /etc/apt/sources.list.d/dotnetdev.list'
$ sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
$ sudo apt-get update
```

Ubuntu 16.10
```bash
$ sudo sh -c 'echo "deb [arch=amd64] https://apt-mo.trafficmanager.net/repos/dotnet-release/ yakkety main" > /etc/apt/sources.list.d/dotnetdev.list'
$ sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 417A0893
$ sudo apt-get update
```

## Install the tools
```bash
$ sudo apt-get install dotnet-dev-1.0.1 git msbuild
```

## Override environment variable
To do a linux build we at the moment also need to set the value of the FrameworkPathOverride environment variable.
```bash
$ export FrameworkPathOverride=/usr/lib/mono/4.5/
```

## Clone the repository
```bash
$ git clone http://github.com/kenwi/vokram
```

## Confirm tool versioning
I'm using msbuild version 15.1.0.0 and dotnet version 1.0.1
```bash
$ msbuild /version
Microsoft (R) Build Engine version 15.1.0.0
Copyright (C) Microsoft Corporation. All rights reserved.

15.1.0.0
$ dotnet --version
1.0.1
```
## Restore packages
```bash
$ cd vokram
$ dotnet restore
```

## Build
```bash
$ dotnet build
Microsoft (R) Build Engine version 15.1.548.43366
Copyright (C) Microsoft Corporation. All rights reserved.

  Vokram.Core -> /home/kenwi/git/vokram/Vokram.Core/bin/Debug/net461/Vokram.Core.dll
  Vokram.Plugins -> /home/kenwi/git/vokram/Vokram.Plugins/bin/Debug/net461/Vokram.Plugins.dll
  Vokram.Plugins.MarkovBrain -> /home/kenwi/git/vokram/Vokram.Plugins.MarkovBrain/bin/Debug/net461/Vokram.Plugins.MarkovBrain.dll
  Vokram.Bot -> /home/kenwi/git/vokram/Vokram.Bot/bin/Debug/net461/Vokram.Bot.exe
  Vokram.Plugins.MakrovBrain.Trainer -> /home/kenwi/git/vokram/Vokram.Plugins.MarkovBrain.Trainer/bin/Debug/net461/Vokram.Plugins.MakrovBrain.Trainer.exe

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:03.84
```

`` `




