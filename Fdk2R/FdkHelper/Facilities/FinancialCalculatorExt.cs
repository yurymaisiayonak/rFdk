using log4net;
using SoftFX.Extended;
using SoftFX.Extended.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FdkMinimal.Facilities
{
    public static class FinancialCalculatorExt
    {
        static ILog Log = LogManager.GetLogger(typeof(FinancialCalculatorExt));

        public static void Init(this FinancialCalculator calculator, DataFeed dataFeed)
        {
            if (calculator == null)
                throw new ArgumentNullException("calculator");

            if (dataFeed == null)
                throw new ArgumentNullException("dataFeed");

            
            var symbols = new SymbolInfo[0];
            try
            {
                calculator.Currencies.Clear();
                calculator.Symbols.Clear();

                symbols = dataFeed.Server.GetSymbols();

                dataFeed.Server.SubscribeToQuotes(symbols.Select(s => s.Name), 1);
                InitializeCalculator(calculator, dataFeed, symbols);
            }
            finally
            {
                dataFeed.Server.UnsubscribeQuotes(symbols.Select(symbol => symbol.Name));
            }
        }

        private static void InitializeCalculator(FinancialCalculator calculator, DataFeed dataFeed, SymbolInfo[] symbols)
        {
            var dtUtcNow = DateTime.UtcNow;
            var currenciesHashSet = new HashSet<string>();

            symbols.ToList().ForEach(s =>
            {
                var symbolEntry = new SymbolEntry(calculator, s.Name, s.SettlementCurrency, s.Currency)
                {
                    ContractSize = s.RoundLot,
                    MarginFactor = s.MarginFactor,
                    Hedging = s.MarginHedge
                };
                calculator.Symbols.Add(symbolEntry);

                if (currenciesHashSet.Add(s.Currency))
                    calculator.Currencies.Add(s.Currency);

                if (currenciesHashSet.Add(s.SettlementCurrency))
                    calculator.Currencies.Add(s.SettlementCurrency);
                try
                {
                    double priceBid = 0;
                    double priceAsk = 0;
                    TryGetBidAsk(dataFeed, s.Name, ref priceBid, ref priceAsk);

                    try
                    {
                        calculator.Prices.Update(s.Name, priceBid, priceAsk);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("Failed to update calculator for symbol: {0} exception: {1}", s.Name, ex));
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Failed to get bid/ask for symbol {0}", s.Name));
                }
            });

            currenciesHashSet.Clear();
        }

        private static void TryGetBidAsk(DataFeed dataFeed, string symbol, ref double priceBid, ref double priceAsk)
        {
            var bidReceived = false;
            var askReceived = false;
            var numberOfAttempts = 5;
            do
            {
                if (!askReceived)
                    askReceived = dataFeed.Cache.TryGetAsk(symbol, out priceAsk);

                if (!bidReceived)
                    bidReceived = dataFeed.Cache.TryGetBid(symbol, out priceBid);

                numberOfAttempts--;

                if (bidReceived && askReceived)
                    return;
                else
                    Thread.Sleep(100);

            }
            while (numberOfAttempts > 0);
        }
    }
}
