# PriconneReTL-AutoUpdater
A [preloader patcher](https://docs.bepinex.dev/master/articles/dev_guide/preloader_patchers.html) for the [BepInEx](https://github.com/BepInEx/BepInEx) plugin framework to automatically update your [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch installation.

## WARNING!  
## As of PriconneReTL version 20240430a, due to the changes in the modloader framework, this patcher plugin and the [PriconneReTL-AutoUpdaterApp](https://github.com/tynave/PriconneReTL-AutoUpdaterApp) does not work!!! A new version is in progress, please wait for it.

## Installation
### Manual
Extract the files found in the release archive to the `priconner` folder (keep the folder structure in the archive!)  
(The goal is to have the patcher dll from this release, and the files of the [PriconneReTLAutoUpdaterApp](https://github.com/tynave/PriconneReTL-AutoUpdaterApp) together inside the `BepInEx\patchers` folder.  
They can be directly in the root of the `patchers` folder, or in any subfolder, just have them "beside" each other. But if you just extract the release archive as-is into the `priconner` folder, you should be good.)

### Automated
Coming soon!

## Requirements / Dependencies
- [PriconneRe-TL](https://github.com/ImaterialC/PriconneRe-TL) english patch.  
The autoupdater can only update existing patch installations, it cannot do a fresh install automatically.
- [PriconneReTLAutoUpdaterApp](https://github.com/tynave/PriconneReTL-AutoUpdaterApp)  
The plugin is only responsible for checking the currently installed and the latest available versions.
The actual update operation is perfomed by the application.

## Disclaimer
Use at your own risk.  
Although the application has been thoroughly tested, bugs, errors or undesired operation may happen.  
The author takes no responsibility for any eventual damage, data loss or any other negative effect cause by the above listed occurences or any misuse of the application.
