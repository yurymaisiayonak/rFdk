# Connect to server before running the sample
# ttConnect(fdkPath = file.path(getwd(), "rfdk"))
symbolData =  ttConf.Symbol()
View(symbolData)

#gets the currency data
currencyData = ttConf.Currency()
View(currencyData)
