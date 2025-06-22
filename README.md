## FNA.NET.ContentPipeline

This project's goal is to make it easy to build FNA Effects(Shaders) with the MGCB content pipeline.

## Prerequisites

This content pipeline extension lib already wraps the fxc.exe tool in. If you're in Windows, it just works.

If you're int non-Windows, you have to install wine to run the fxc.exe tool.

### (Linux/macOS) Installing Wine

To install Wine and winetricks on **Linux**, refer to your distribution's package database. Typically the package names will simply be `wine` and `winetricks`.

To install Wine and winetricks on **macOS**:

- Install Homebrew from https://brew.sh/
- Install wine with `brew cask install wine-stable`
- Install winetricks with `brew install winetricks`
- (If you already have these installed, update with: `brew update`, `brew upgrade wine-stable`, `brew upgrade winetricks`)

Once Wine and winetricks are installed:

- Setup Wine with `winecfg`

## How to use

Build this project and copy `FNA.NET.ContentPipeline.dll` to the folder containing your `Content.mgcb` file.

In your `Content.mgcb` file, add lines similar to the following:

```
#-------------------------------- References --------------------------------#

/reference:FNA.NET.ContentPipeline.dll

#---------------------------------- Content ---------------------------------#

# begin Effects/Grayscale.fx
/importer:EffectImporter
/processor:FxcEffectProcessor
/processorParam:DebugMode = Auto
/processorParam:Defines=
/build:Effects/Grayscale.fx
```

## Credit

- https://gist.github.com/LennardF1989/a5d7d54c89cb6cd0e6bc9551b6fa6a48
- https://github.com/AndrewRussellNet/FNA-Template/blob/master/build/BuildShaders.targets