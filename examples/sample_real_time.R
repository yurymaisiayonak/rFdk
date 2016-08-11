ttConnect()
idMonitor <- ttFeed.Subscribe('EURUSD', 2)
# Wait for some time before running this line
# it can be run multiple times and the snapshot reflects
snapshot <- ttFeed.GetLevel2(idMonitor)
View(snapshot)

ttFeed.Unsubscribe(idMonitor)
