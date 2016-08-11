using System;
using System.Collections.Generic;
using System.Linq;
using SoftFX.Extended;
using log4net;

namespace RHost
{
    static class FdkBarsMerger
    {

        static readonly ILog Log = LogManager.GetLogger(typeof(FdkBarsMerger));

        internal static BarData[] ProcessedBarsResult(BarData[] barsDataBid, BarData[] barsDataAsk)
        {
            var maxCount = Math.Max(barsDataBid.Length, barsDataAsk.Length);
            var resultBars = new List<BarData>(maxCount * 2);
           
            BarCursor bidCursor = new BarCursor(barsDataBid);
            BarCursor askCursor = new BarCursor(barsDataAsk);

            while (bidCursor.CanContinue || askCursor.CanContinue)
            {
                BarData barAsk = askCursor.Current;
                BarData barBid = bidCursor.Current;
                if (barAsk.From == barBid.From)
                {
                    AddBarPairs(resultBars, barBid, barAsk);
                    askCursor.Next();
                    bidCursor.Next();
                }
                else if (barAsk.From > barBid.From )
                {
                    BarData bidUndefined = CalculateBarUndefined(barAsk.From, barAsk.To, barBid);
                    AddBarPairs(resultBars, bidUndefined, barAsk);
                    askCursor.Next();
                }
                else if (barBid.From > barAsk.From)
                {
                    BarData askUndefined = CalculateBarUndefined(barBid.From, barBid.To, barAsk);
                    AddBarPairs(resultBars, barBid, askUndefined);
                    bidCursor.Next();
                }
                else
                    throw new InvalidOperationException("This case should never be hit!");
            }

            return resultBars.ToArray();
        }

        internal static void AddBarPairs(List<BarData> resultBars, BarData barBid, BarData barAsk)
        {
            resultBars.Add(barBid);
            resultBars.Add(barAsk);
        }

        private static BarData CalculateBarUndefined(DateTime from, DateTime to, BarData previous)
        {
            var undefinedBar = new BarData(from, to,
                open: previous.Open, close: previous.Close,
                low: previous.Low, high: previous.High,
                volume: previous.Volume
                );
            return undefinedBar;
        }
        
    }
}