symbol <- "USDSEK" 
startTime <- as.POSIXct(0, origin=ISOdatetime(2016,05,01,0,00,00))
endTime <- as.POSIXct(0, origin=ISOdatetime(2016,05,01,1,59,59))
#get Bars history by "Bid"
barBid<-ttFeed.BarHistory(symbol, "Bid", "M1", startTime, endTime)
# get Bars history by "Ask"
barAsk<-ttFeed.BarHistory(symbol, "Ask", "M1", startTime, endTime)

# Calculating amount of AskBars
 countBarAsk =  nrow(barAsk)
# Calculating amount of BidBars
 countBarBid = nrow(barBid)
 head(countBarAsk)
 head(countBarBid)
