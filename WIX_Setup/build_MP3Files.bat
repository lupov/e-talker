rem This script build MP3Files.wxs file are dependent on MP3 files.

"%WIX%\bin\heat.exe" dir ..\Verbs -dr INSTALLLOCATION -cg MP3Files_Id -gg -sfrag -out MP3Files.wxs -wixvar -var var.UI