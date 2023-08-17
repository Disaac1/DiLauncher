@echo off

echo Creating Pack for %1

if [%1]==[] (
  echo Argument wasn't passed
  pause
  goto exit
)

mkdir .export\%1
echo Making folder at .export\%1
.godotFiles\Godot_v4.1.1-stable_mono_win64.exe --headless --path Data\%~1\.export --export-pack "Pck" ../../../.export/%1/pack.zip
echo Exported pack.pck to 

copy Data\%1\pack.json .export\%1\pack.json

:exit
echo Finished