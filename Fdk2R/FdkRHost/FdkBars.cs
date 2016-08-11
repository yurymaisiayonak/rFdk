using System;
using System.Linq;
using FdkMinimal;
using SoftFX.Extended;
using SoftFX.Extended.Storage;
using log4net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace RHost
{
    
	public static class FdkBars
	{
		static readonly ILog Log = LogManager.GetLogger(typeof(FdkBars));
		#region Bars
		
		//10 million
		public const int HugeCount = 10000000;
        internal static string CombinedBarsRangeTime(string symbol, BarPeriod barPeriod, DateTime startTime, DateTime endTime, double barCountDbl)
        {
            var isTimeZero = FdkHelper.IsTimeZero(startTime);


            BarData[] barsDataBid;
            BarData[] barsDataAsk;
            if (FdkHelper.IsTimeZero(startTime))
            {
                var barCount = (int)barCountDbl;
                if (barCount == 0)
                {
                    barCount = FdkBars.HugeCount;
                }
                barsDataAsk = CalculateBarsForSymbolArray(symbol, PriceType.Ask, endTime, barPeriod, barCount);
                barsDataBid = CalculateBarsForSymbolArray(symbol, PriceType.Bid, endTime, barPeriod, barCount);
            }
            else
            {
                barsDataAsk = CalculateBarsForSymbolArrayRangeTime(symbol, PriceType.Ask, startTime, endTime, barPeriod);
                barsDataBid = CalculateBarsForSymbolArrayRangeTime(symbol, PriceType.Bid, startTime, endTime, barPeriod);
            }
            ReversIfNeed(barsDataAsk);
            ReversIfNeed(barsDataBid);

            var barsData = FdkBarsMerger.ProcessedBarsResult(barsDataBid, barsDataAsk);
            //LogBars(barsData);
            var bars = FdkVars.RegisterVariable(barsData, "bars");

            return bars;
        }

        private static void ReversIfNeed(BarData[] bars)
        {
            if (bars.Length > 0 && (bars[0].From < bars[bars.Length - 1].From))
                Array.Reverse(bars);
        }

        private static void LogBars(BarData[] barsData)
        {
            Debugger.Launch();
            var sb = new StringBuilder();
            foreach (var data in barsData)
            {
                sb.AppendLine(data.ToString());
            }
            File.WriteAllText(@"d:\merged_bars.txt", sb.ToString());
        }

        public static string ComputeBarsRangeTime(string symbol, string priceTypeStr, string barPeriodStr,
            DateTime startTime, DateTime endTime, double barCountDbl)
		{
            Stopwatch stopWatch = Stopwatch.StartNew();
			try
			{
				var barPeriod = FdkHelper.GetFieldByName<BarPeriod>(barPeriodStr);
				if (barPeriod == null)
					return string.Empty;

                if(priceTypeStr == "BidAsk")
                {
                    return CombinedBarsRangeTime(symbol, barPeriod, startTime, endTime, barCountDbl);
                }

				var priceType = FdkHelper.ParseEnumStr<PriceType>(priceTypeStr);
				if (priceType == null)
					return string.Empty;

				Log.InfoFormat("FdkBars.ComputeBarsRangeTime( symbol: {0}, barPeriod: {1}, startTime: {2}, endTime: {3}, barCount: {4} priceType: {5})",
					symbol, barPeriodStr, startTime, endTime, barCountDbl, priceType);
				
				BarData[] barsData;
				if (FdkHelper.IsTimeZero(startTime))
				{
					var barCount = (int) barCountDbl;
					if(barCount == 0)
					{
						barCount = HugeCount;
					}
					barsData = CalculateBarsForSymbolArray(symbol, priceType.Value, endTime, barPeriod, barCount);
				}else
				{
					barsData = CalculateBarsForSymbolArrayRangeTime(symbol, priceType.Value, startTime, endTime, barPeriod);
				}

                Log.InfoFormat("Elapsed time: {0} ms for {1} items", stopWatch.ElapsedMilliseconds, barsData.Length);

                var bars = FdkVars.RegisterVariable(barsData, "bars");
				return bars;
			}
			catch(Exception ex)
			{
				Log.Error(ex);
				throw;
			}
        }

        public static BarData ToBarData(this Bar bar)
        {
            return new BarData(bar.From, bar.To, bar.Open, bar.Close, bar.Low, bar.High, bar.Volume);
        }
        #region Fdk direct wrapper
        static BarData[] CalculateBarsForSymbolArray(
			string symbol, PriceType priceType, DateTime startTime, BarPeriod barPeriod, int barCount)
		{
			return FdkHelper.Storage.Online.GetBars(symbol, priceType, barPeriod, startTime, -barCount)
                .SelectToArray(bar => bar.ToBarData());
		}

		static BarData[] CalculateBarsForSymbolArrayRangeTime(
			string symbol, PriceType priceType, DateTime startTime, DateTime endTime, BarPeriod barPeriod)
		{
			return FdkHelper.Storage.Online.GetBars(symbol, priceType, barPeriod, startTime, endTime)
                .SelectToArray(bar => bar.ToBarData()); 
		}

		static HistoryInfo GetQuotesInfo(string symbol, int depth)
		{
			return FdkHelper.Storage.Online.GetQuotesInfo(symbol, depth);
		}

		static HistoryInfo GetBarsInfo(string symbol, PriceType priceType, BarPeriod period)
		{
			return FdkHelper.Storage.Online.GetBarsInfo(symbol, priceType, period);
		}

		#endregion

        public static DateTime[] ComputeGetQuotesInfo(string symbol, int depth)
        {
            var barsData = GetQuotesInfo(symbol, depth);
            var bars = new[]
            {
                barsData.AvailableFrom,
                barsData.AvailableTo
            };
            return bars;
        }

        public static DateTime[] ComputeGetBarsInfo(string symbol, string priceTypeStr, string barPeriodStr)
        {
            var barPeriod = FdkHelper.GetFieldByName<BarPeriod>(barPeriodStr);
            if (barPeriod == null)
                return new DateTime[0];
            var priceType = FdkHelper.ParseEnumStr<PriceType>(priceTypeStr);
            if (priceType == null)
                return new DateTime[0];
            var barsData = GetBarsInfo(symbol, priceType.Value, barPeriod);
            var bars = new[]
            {
                barsData.AvailableFrom,
                barsData.AvailableTo
            };
            return bars;
        }



        public static string ComputeGetPairBarsRange(string symbol, string barPeriodStr, DateTime startTime, DateTime endTime)
        {
            var barPeriod = FdkHelper.GetFieldByName<BarPeriod>(barPeriodStr);
            if (barPeriod == null)
                return string.Empty;
            var barsData = FdkBarPairs.GetPairBarsSymbolArrayRangeTime(symbol, barPeriod, startTime, endTime);
            var bars = FdkVars.RegisterVariable(barsData, "barPairs");
            return bars;
        }

        #endregion

        #region BarData fields
        public static double[] BarHighs(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsHigh(barData);
        }


        public static double[] BarLows(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsLow(barData);
        }

        public static double[] BarVolumes(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsVolume(barData);
        }

        public static double[] BarOpens(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsOpen(barData);
        }

        public static double[] BarCloses(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsClose(barData);
        }

        public static DateTime[] BarFroms(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsFrom(barData);
        }

        public static DateTime[] BarTos(string bars)
        {
            var barData = FdkVars.GetValue<BarData[]>(bars);

            return GetBarsTo(barData);
        }


        public static double[] GetBarsHigh(BarData[] barData)
        {
            return barData.SelectToArray(b => b.High);
        }

        public static double[] GetBarsLow(BarData[] barData)
        {
            return barData.SelectToArray(b => b.Low);
        }

        public static double[] GetBarsVolume(BarData[] barData)
        {
            return barData.SelectToArray(b => b.Volume);
        }

        public static double[] GetBarsOpen(BarData[] barData)
        {
            return barData.SelectToArray(b => b.Open);
        }

        public static double[] GetBarsClose(BarData[] barData)
        {
            return barData.SelectToArray(b => b.Close);
        }


        internal static DateTime[] GetBarsFrom(BarData[] barData)
        {
            return barData.SelectToArray(b => b.From.SpecifyToUtc());
        }

        internal static DateTime[] GetBarsTo(BarData[] barData)
        {
            return barData.SelectToArray(b => b.To.SpecifyToUtc());
        }
        #endregion
    }
}