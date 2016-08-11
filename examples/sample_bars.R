
# Connect to server before running the sample
# ttConnect()
ttConnect()
bars = ttFeed.BarHistory("EURUSD", "Bid", "H1")

boxplot(bars$highs)

plot(highs, data = bars)

endTime <- as.POSIXlt(Sys.time(), tz="GMT")
startTime <- strptime("20/3/2 11:16:16.683", "%d/%m/%y %H:%M:%OS")

st1 <- as.POSIXct(startTime, tz="GMT")
et1 <- as.POSIXct(endTime, tz="GMT")

quotes <- ttFeed.TickBestHistory("EURUSD", st1, et1)
plot(quotes$ask, type="o")

bars = ttFeed.BarHistory(symbol = "EURUSD", barPeriodStr = "M15", priceTypeStr = "Ask", barCount = 100000000)

barRange = ttFeed.BarHistory(symbol = "EURUSD", barPeriodStr = "M1", priceTypeStr = "Ask", startTime = startTime)
