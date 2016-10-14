# FdkRLib
Added the SoftFX R wrapper package over FDK (Financial Development Kit)

# Prerequisites
If you see this error: "You are probably missing the Visual C++ Redistributable for Visual Studio 2013", then please download it from here:
https://www.microsoft.com/en-us/download/details.aspx?id=40784

# How to install it?
```
install.packages("install.load")
library(install.load)
install_load("data.table", "stringi")

installRPackage <- function(url){
	package = basename(url)
    download.file(url, destfile = package)
	install.packages(package, repos = NULL, type = "source")
	file.remove(package)
}

installRPackage("https://github.com/SoftFx/rFdk-/raw/master/Lib/RClr/rClr_0.7-4.zip");
installRPackage("https://github.com/SoftFx/rFdk-/releases/download/v1.0.0/rFdk.zip");
```

# How to test it?
You have sample code inside examples/sample_bars.r with various snippets of code. 

A simple code sample code is the following:
```

ttConnect()

#Get configuration information of your account
head(ttConf.Symbol())
head(ttConf.Currency())

#Quotes in the last 5 minutes
now <-as.POSIXct(Sys.time(), tz="GMT")
# 300 seconds from present
prevNow <-as.POSIXct(now-(5*60))
ttFeed.TickBestHistory("EURUSD", startTime = prevNow, endTime=now)
```
Follow this link with expanded example and output:

Configuration:
http://rpubs.com/ihalankou/configuration

Feed History:
http://rpubs.com/ihalankou/feed_history

Trades:
http://rpubs.com/ciplogic/107674

Real-time feed sample
http://rpubs.com/ihalankou/realtime_quotes

# What's new
New update (1.0.20160315) contains the following changes:
https://github.com/SoftFx/rFdk/blob/master/What's_new.pdf

 
