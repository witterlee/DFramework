@REM NOTE: This script must be run from a Visual Studio command prompt window

@setlocal
@ECHO off

SET CMDHOME=%~dp0.
if "%FrameworkDir%" == "" set FrameworkDir=%WINDIR%\Microsoft.NET\Framework
if "%FrameworkVersion%" == "" set FrameworkVersion=v4.0.30319

SET MSBUILDEXEDIR=%FrameworkDir%\%FrameworkVersion%
SET MSBUILDEXE=%MSBUILDEXEDIR%\MSBuild.exe
SET NUGETEXE=NUGET.exe

set PROJ=%CMDHOME%\DFramework.sln

@echo ===== Building %PROJ% =====

@echo Build Debug ==============================

SET CONFIGURATION=Debug

"%MSBUILDEXE%" /p:Configuration=%CONFIGURATION% "%PROJ%"
@if ERRORLEVEL 1 GOTO :ErrorStop
@echo BUILD ok for %CONFIGURATION% %PROJ%

@echo Build Release ============================

SET CONFIGURATION=Release

"%MSBUILDEXE%" /p:Configuration=%CONFIGURATION% "%PROJ%"
@if ERRORLEVEL 1 GOTO :ErrorStop
@echo BUILD ok for %CONFIGURATION% %PROJ%
@echo ===== wait for generate nupkg =====
del %CMDHOME%\*.nupkg
"%NUGETEXE%" pack DFramework\DFramework.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Autofac\DFramework.Autofac.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\CouchbaseCache\DFramework.CouchbaseCache.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Log4net\DFramework.Log4net.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Memcached\DFramework.Memcached.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\RabbitCommandBus\DFramework.RabbitCommandBus.csproj -Prop Configuration=%CONFIGURATION%
@echo ===== wait for generate nupkg =====
"%NUGETEXE%" push DFramework.*.nupkg -s http://10.0.0.200/ 
@echo ===== press any key ... =====
pause