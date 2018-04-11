@echo off
cls
del "%~dp0\Environments\Assets\Neodroid"
mklink /d "%~dp0\Examples\Assets\Neodroid" ..\..\Neodroid
REM <-comment del "%~dp0\Environments\Assets\SteamVR"
REM <-comment mklink /d "%~dp0\Examples\Assets\SteamVR" ..\..\SteamVR
pause