
using System;
using FdkMinimal;
using RHost;

namespace TestRClrHost
{
	/// <summary>
	/// Description of ProgramTest.
	/// </summary>
	public class ProgramTest
	{
		public static void Main()
		{
			FdkHelper.ConnectToFdk("78.46.196.240", "100008", "123qwe!", "");
            var now = DateTime.UtcNow;
            var prevNow = DateTime.UtcNow.Subtract(new TimeSpan(30,0,0));
            FdkBars.ComputeBarsRangeTime("BTCUSD_L", "BidAsk", "M1", prevNow, now, 0);
            return;

            var bars = FdkTradeReports.GetTradeTransactionReportAll();
            var comission = FdkTradeReports.GetTradeComment(bars);
            FdkVars.Unregister(bars);
		}
	}
}
