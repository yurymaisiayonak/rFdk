if(!require("lubridate")) {  
install.packages("lubridate", dependencies = TRUE,  repos = "http://cran.us.r-project.org")  
library(lubridate)  
} 
symbol <- "EUR/USD"
# get bars M1 by EUR/USD for 1 year (from 2012-01-01 to 2013-01-01)
startTime=ymd(20120101)
endTime= startTime + years(1)
View(ttFeed.BarHistory(symbol, "BidAsk", "M1", startTime, endTime))
