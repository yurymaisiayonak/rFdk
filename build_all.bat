@echo off

set MsBuildFilePath="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe"
set R_Exe="c:\Program Files\R\R-3.3.1\bin\R.exe"

if not exist %MsBuildFilePath% (
  echo Please set variable MsBuildFilePath to point to your MSBuild tool
  pause
)


if not exist %R_Exe% (
  echo Please set variable R_Exe to point to your R distribution
  pause
)


cd Fdk2R

set nuget=".nuget\nuget.exe"

"%nuget%" restore RFdk.sln
"%MsBuildFilePath%" RFdk.sln /target:Rebuild /p:Platform=x64 /p:Configuration=Release /maxcpucount /fl /flp:Verbosity=Normal;LogFile=..\build.Log /clp:NoItemAndPropertyList /verbosity:n /nologo

set libs="%cd%\FdkRHost\bin\Release\*.dll"
set appConfig="%cd%\FdkRHost\bin\Release\App.config"
set rData="%cd%\..\RPackage\Data"

if not exist %rData% mkdir %rData%

xcopy %libs% %rData% /y
xcopy %appConfig% %rData% /y

cd ..

if not exist dist mkdir dist

%R_Exe% CMD BATCH build_r.r
pause