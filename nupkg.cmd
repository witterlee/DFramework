@setlocal
@ECHO off

SET CMDHOME=%~dp0.
SET CONFIGURATION=Release
SET NUGETEXE=NUGET.exe
del %CMDHOME%\*.nupkg
"%NUGETEXE%" pack DFramework\DFramework.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Autofac\DFramework.Autofac.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\CouchbaseCache\DFramework.CouchbaseCache.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Log4net\DFramework.Log4net.csproj -Prop Configuration=%CONFIGURATION%
"%NUGETEXE%" pack Plusins\Memcached\DFramework.Memcached.csproj -Prop Configuration=%CONFIGURATION%
@echo ===== wait for generate nupkg =====
"%NUGETEXE%" push DFramework.*.nupkg
@echo ===== press any key ... =====
pause