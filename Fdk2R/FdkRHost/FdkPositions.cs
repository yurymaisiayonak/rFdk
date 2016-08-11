
using System;
using System.Collections.Generic;
using System.Linq;
using FdkMinimal;
using SoftFX.Extended;
using log4net;
using RHost;

namespace FdkRHost
{
	/// <summary>
	/// Description of FdkPositions.
	/// </summary>
	public class FdkPosition
	{
		static DataTrade Trade
		{
			get { return FdkHelper.Trade; }
		}

		public static string GetTradePositions()
		{
			try
			{
                Position[] tradeRecords = Trade.Cache.Positions;
				
				Log.InfoFormat("FdkPositions.GetPositionPositions()");
				
				var varName = FdkVars.RegisterVariable(tradeRecords, "positions");
				return varName;
			}
			catch (Exception ex)
			{
				Log.Error(ex);
				throw;
			}     
        }

        static readonly ILog Log = LogManager.GetLogger(typeof(FdkTrade));


        public static double[] GetPositionAgentCommission(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.AgentCommission);
        }
        
        public static double[] GetPositionBuyAmount(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.BuyAmount);
        }
        public static double[] GetPositionBuyPrice(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.BuyPrice ?? double.NaN);
        }
        public static double[] GetPositionCommission(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.Commission);
        }
        
        public static double[] GetPositionProfit(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
			return tradeData.SelectToArray(it => it.Profit?? double.NaN);
        }
        public static double[] GetPositionSellAmount(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.SellAmount);
        }
        public static double[] GetPositionSellPrice(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.SellPrice ?? double.NaN);
        }
        
        public static double[] GetPositionSettlementPrice(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.SettlementPrice);
        }

        
        public static double[] GetPositionSwap(string varName)
        {
            var tradeData = FdkVars.GetValue<Position[]>(varName);
            return tradeData.SelectToArray(it => it.Swap);
        }

		public static string[] GetPositionSymbol(string varName)
		{
			var tradeData = FdkVars.GetValue<Position[]>(varName);
			return tradeData.SelectToArray(it => it.Symbol);
		}
    }
}
