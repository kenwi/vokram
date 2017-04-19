

# Vokram.Trainer
This part of the application is responsible for reading IRC chatlogs, create and serialize a markov chain based "brain" that the bot can use to generate random sentences.

# Installation
Clone the repository. Run `dotnet restore` and build with `dotnet build`.

```bash
PS C:\Users\wilken\vokram\Vokram.trainer> dotnet restore
  Restoring packages for C:\Users\wilken\vokram\Vokram.Core\Vokram.Core.csproj...
  Restoring packages for C:\Users\wilken\vokram\Vokram.trainer\vokram.Trainer.csproj...
  Restoring packages for C:\Users\wilken\vokram\Vokram.Plugins\Vokram.Plugins.csproj...
  Generating MSBuild file C:\Users\wilken\vokram\Vokram.Core\obj\Vokram.Core.csproj.nuget.g.props.
  Generating MSBuild file C:\Users\wilken\vokram\Vokram.Plugins\obj\Vokram.Plugins.csproj.nuget.g.props.
  Generating MSBuild file C:\Users\wilken\vokram\Vokram.trainer\obj\vokram.Trainer.csproj.nuget.g.props.
  Writing lock file to disk. Path: C:\Users\wilken\vokram\Vokram.Core\obj\project.assets.json
  Writing lock file to disk. Path: C:\Users\wilken\vokram\Vokram.trainer\obj\project.assets.json
  Writing lock file to disk. Path: C:\Users\wilken\vokram\Vokram.Plugins\obj\project.assets.json
  Restore completed in 29,09 ms for C:\Users\wilken\vokram\Vokram.Core\Vokram.Core.csproj.
  Restore completed in 40,95 ms for C:\Users\wilken\vokram\Vokram.Plugins\Vokram.Plugins.csproj.
  Restore completed in 41,09 ms for C:\Users\wilken\vokram\Vokram.trainer\vokram.Trainer.csproj.

  NuGet Config files used:
      C:\Users\wilken\AppData\Roaming\NuGet\NuGet.Config
      C:\Program Files (x86)\NuGet\Config\Microsoft.VisualStudio.Offline.config

  Feeds used:
      https://api.nuget.org/v3/index.json
      C:\Program Files (x86)\Microsoft SDKs\NuGetPackages\
PS C:\Users\wilken\vokram\Vokram.trainer> dotnet build
Microsoft (R) Build Engine version 15.1.1012.6693
Copyright (C) Microsoft Corporation. All rights reserved.

  Vokram.Core -> C:\Users\wilken\vokram\Vokram.Core\bin\Debug\net461\Vokram.Core.dll
  Vokram.Plugins -> C:\Users\wilken\vokram\Vokram.Plugins\bin\Debug\net461\Vokram.Plugins.dll
  vokram.Trainer -> C:\Users\wilken\vokram\Vokram.trainer\bin\Debug\net461\vokram.Trainer.exe

Build succeeded.
    0 Warning(s)
    0 Error(s)
```

# Training
Run with `dotnet run`

## Arguments
To add arguments to the trainer, append  `--` to `dotnet run` and select argument
* `--samples` Number of samples to be generated after the brain has been trained.
* `--selctions` Split the messages into n parts and select the last one. Helps with processing time.
* `--filter` Train only on the regex match (nick and text)
* `--reports` Adjust how noisy the processing is. 100 will report ever delta percent.
* `--load` Input file
* `--save` Output file

```bash
PS C:\Users\wilken\vokram\Vokram.trainer> dotnet run -- --reports=10 --sections=10 --samples=10
[00:29:43] Load=Logs/130494-herbert.freenode.net-/#nff.txt, Save=vokram.txt, Sections=10, Reports=10, Samples=10, Filter=
[00:29:43] Initializing trainer 'Logs/130494-herbert.freenode.net-/#nff.txt'
[00:29:43] Loading 'Logs/130494-herbert.freenode.net-/#nff.txt'
[00:29:43] Number of lines: 86 851
[00:29:43] Removing IRC events from log
[00:29:43] Splitting log into 10 parts. Generating text from one part
[00:29:43] Number of messages: 7 064
[00:29:43] Processing messages
[00:29:43] Processed: 0 %, 0 words, 0 sentences, logtime 2016-03-15 07:43:50
[00:29:44] Processed: 10 %, 5 636 words, 1 348 sentences, logtime 2016-03-19 15:55:07
[00:29:44] Processed: 20 %, 11 545 words, 2 577 sentences, logtime 2016-03-22 06:06:35
[00:29:46] Processed: 30 %, 18 218 words, 3 910 sentences, logtime 2016-03-25 02:27:14
[00:29:47] Processed: 40 %, 24 164 words, 5 223 sentences, logtime 2016-04-08 21:03:47
[00:29:49] Processed: 50 %, 29 596 words, 6 597 sentences, logtime 2016-04-14 09:52:23
[00:29:51] Processed: 60 %, 35 940 words, 8 028 sentences, logtime 2016-04-16 23:51:11
[00:29:52] Processed: 70 %, 40 916 words, 9 206 sentences, logtime 2016-04-17 18:54:04
[00:29:55] Processed: 80 %, 47 309 words, 10 603 sentences, logtime 2016-04-21 14:05:31
[00:29:57] Processed: 90 %, 52 499 words, 11 850 sentences, logtime 2016-04-25 11:03:18
[00:30:01] Processed: 100 %, 59 900 words, 13 274 sentences, logtime 2016-04-28 12:28:47
[00:30:01] Finished training
[00:30:01] Unique words in brain: 12 427
[00:30:01] Saving 'vokram.txt'
[00:30:01] Saved to 'vokram.txt'
[00:30:01] Loading 'vokram.txt'
[00:30:01] Generating samples
[00:30:01] 0: 'Hvis tråden min.'
[00:30:01] 1: 'Forste gangen at noenlunde kvalitetsdeler varer.'
[00:30:01] 2: 'Feiler decode på planlegging er kult for det dummeste jeg.'
[00:30:01] 3: 'Jeg er hjemmemekk eller noe indisk nå tenker jeg vet igrunn ikke.'
[00:30:01] 4: 'Service Access Control SAC.'
[00:30:01] 5: 'Men noen lapp for fucked.'
[00:30:01] 6: 'Nå var han å vise omverden at du fette delirisk.'
[00:30:01] 7: 'Hva dreide skepsisen seg gjennom privoy atm.'
[00:30:01] 8: 'Kickstarter for å feste.'
[00:30:01] 9: 'No eller sliter med dem.'
[00:30:01] Done
```
