
# Connect to server before running the sample
# ttConnect()

# gets all open oredrs you have with the current account
tradesOpen = ttTrade.Open()
View(tradesOpen)

# gets all history trades you have with the current account 
6 tradesHistory = ttTrade.History()
7 View(tradesHistory) 
