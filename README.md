# CWMouseWheel
With this mod you can scroll your items with the mouse wheel. 

To use the camera zoom, there is a separate key (Z by default).

### Compilation from source

### Requirements
1. Any .NET implementation that supports ``netstandard2.1`` and C# 10+.
2. An installed **Content Warning** on your PC.

#### Compilation steps:
1. Set the game directory variable (e.g. ``D:\SteamLibrary\steamapps\common\Content Warning``):

**For Windows cmd.exe**
```batch
set CW_DIR="game directory here".
```

**For PowerShell**
```powershell
$env:CW_DIR="game directory here"
```

**For bash**
```bash
CW_DIR="game directory here"
```

2. Run ``dotnet build -P:Configuration=Release`` to compile the mod from source with *Release* configuration.


*yes, I just copied MouseBind README file*
