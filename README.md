# FdkRLib
Added the SoftFX R wrapper package over FDK (Financial Development Kit)

# Prerequisites
If you see this error: "You are probably missing the Visual C++ Redistributable for Visual Studio 2013", then please download it from here:
https://www.microsoft.com/en-us/download/details.aspx?id=40784

# How to install it?
```"data.table")) 
  install.packages("data.table")
library("data.table")
if(!require("stringi"))
  install.packag
if(!require(es("stringi")
library("stringi")
installBinaryHttr <- function(fdkRLibPackage){
  basicUrl = "https://github.com/SoftFx/FdkRLib/raw/master/dist/"
  fullUrl = paste(basicUrl, fdkRLibPackage, sep = "")
  download.file(fullUrl,destfile = fdkRLibPackage, method = "libcurl")
  install.packages(fdkRLibPackage, repos = NULL, type = "source", dependencies = TRUE)
  file.remove(fdkRLibPackage)
}
if(!require("rClr")) 
    installBinaryHttr("rClr_0.7-4.zip")
installBinaryHttr("rFdk_1.0.20160315.zip")
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

 
