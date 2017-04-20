
# Vokram.Plugins.MarkovBrain.Trainer
This part of the application is responsible for reading IRC chatlogs, create and searialize a markov chain based "brain" that the bot can use to generate random sentences.

# Installation
Clone the repository. Run `dotnet restore` and build with `dotnet build`.

```bash
kenwi@aleph:~/vokram/Vokram.Plugins.MarkovBrain.Trainer$ dotnet restore
  Restoring packages for /home/kenwi/vokram/Vokram.Plugins.MarkovBrain.Trainer/Vokram.Plugins.MakrovBrain.Trainer.csproj...
  Restoring packages for /home/kenwi/vokram/Vokram.Core/Vokram.Core.csproj...
  Lock file has not changed. Skipping lock file write. Path: /home/kenwi/vokram/Vokram.Core/obj/project.assets.json
  Restore completed in 43.04 ms for /home/kenwi/vokram/Vokram.Core/Vokram.Core.csproj.
  Restoring packages for /home/kenwi/vokram/Vokram.Plugins/Vokram.Plugins.csproj...
  Lock file has not changed. Skipping lock file write. Path: /home/kenwi/vokram/Vokram.Plugins/obj/project.assets.json
  Lock file has not changed. Skipping lock file write. Path: /home/kenwi/vokram/Vokram.Plugins.MarkovBrain.Trainer/obj/project.assets.json
  Restore completed in 58.45 ms for /home/kenwi/vokram/Vokram.Plugins.MarkovBrain.Trainer/Vokram.Plugins.MakrovBrain.Trainer.csproj.
  Restore completed in 5.35 ms for /home/kenwi/vokram/Vokram.Plugins/Vokram.Plugins.csproj.
  Restoring packages for /home/kenwi/vokram/Vokram.Plugins.MarkovBrain/Vokram.Plugins.MarkovBrain.csproj...
  Lock file has not changed. Skipping lock file write. Path: /home/kenwi/vokram/Vokram.Plugins.MarkovBrain/obj/project.assets.json
  Restore completed in 2.22 ms for /home/kenwi/vokram/Vokram.Plugins.MarkovBrain/Vokram.Plugins.MarkovBrain.csproj.

  NuGet Config files used:
      /home/kenwi/.nuget/NuGet/NuGet.Config

  Feeds used:
      https://api.nuget.org/v3/index.json
kenwi@aleph:~/vokram/Vokram.Plugins.MarkovBrain.Trainer$ dotnet build
Microsoft (R) Build Engine version 15.1.548.43366
Copyright (C) Microsoft Corporation. All rights reserved.

  Vokram.Core -> /home/kenwi/vokram/Vokram.Core/bin/Debug/net461/Vokram.Core.dll
  Vokram.Plugins -> /home/kenwi/vokram/Vokram.Plugins/bin/Debug/net461/Vokram.Plugins.dll
  Vokram.Plugins.MarkovBrain -> /home/kenwi/vokram/Vokram.Plugins.MarkovBrain/bin/Debug/net461/Vokram.Plugins.MarkovBrain.dll
  Vokram.Plugins.MakrovBrain.Trainer -> /home/kenwi/vokram/Vokram.Plugins.MarkovBrain.Trainer/bin/Debug/net461/Vokram.Plugins.MakrovBrain.Trainer.exe

Build succeeded.
    0 Warning(s)
    0 Error(s)

Time Elapsed 00:00:02.00

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
kenwi@aleph:~/vokram/Vokram.Plugins.MarkovBrain.Trainer$ dotnet run -- --load=Logs/130494-herbert.freenode.net-/#nff.txt --save=nff.dat --sections=20 --reports=4 --samples=10
[8:30:06 PM] Load=Logs/130494-herbert.freenode.net-/#nff.txt, Save=nff.dat, Sections=20, Reports=4, Samples=10, Filter=
[8:30:06 PM] Initializing trainer 'Logs/130494-herbert.freenode.net-/#nff.txt'
[8:30:06 PM] Loading 'Logs/130494-herbert.freenode.net-/#nff.txt'
[8:30:06 PM] Number of lines: 86,851
[8:30:06 PM] Removing IRC events from log
[8:30:06 PM] Splitting log into 20 parts. Generating text from one part
[8:30:06 PM] Number of messages: 3,532
[8:30:06 PM] Processing messages
[8:30:06 PM] Processed: 0 %, 0 words, 0 sentences, logtime 2017-04-14 19:12:46
[8:30:07 PM] Processed: 25 %, 6,859 words, 1,730 sentences, logtime 2017-04-07 11:48:47
[8:30:08 PM] Processed: 50 %, 13,708 words, 3,470 sentences, logtime 2017-04-04 11:02:04
[8:30:09 PM] Processed: 75 %, 20,993 words, 5,343 sentences, logtime 2017-03-30 13:18:12
[8:30:11 PM] Finished training
[8:30:11 PM] Unique words in brain: 7,725
[8:30:11 PM] Saving 'nff.dat'
[8:30:12 PM] Saved to 'nff.dat'
[8:30:12 PM] Loading 'nff.dat'
[8:30:12 PM] Generating samples
[8:30:12 PM] 0: 'This is a trap.'
[8:30:12 PM] 1: 'Drikker sprit og lagt fyllmasser og neppe.'
[8:30:12 PM] 2: 'Artikkel så det kan dislike en slik pakke når jeg nå.'
[8:30:12 PM] 3: 'T=296597 hurr hurr durr.'
[8:30:12 PM] 4: 'Å HUFF DA.'
[8:30:12 PM] 5: 'Aceton peroksid burde tatt.'
[8:30:12 PM] 6: 'Du mener hun jo klart.'
[8:30:12 PM] 7: 'Eller hva mener at det jeg fant ut.'
[8:30:12 PM] 8: 'My0bo i artikkelen.'
[8:30:12 PM] 9: 'Rent pragmatisk enn 4 minutter.'
[8:30:12 PM] Done

```
