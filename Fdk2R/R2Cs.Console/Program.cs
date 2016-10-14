using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using RDotNet;
using SoftFX.Extended;
using FdkMinimal;
using RHost;

namespace R2Cs
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                Console.WriteLine("Enter 1-ttConnect, 2-BarHistory, 3-disconnect, 4-exit");
                try {
                    var command = Console.ReadLine();
                    switch (command)
                    {
                        case "1":
                            FdkStatic.ConnectToFdk("", "", "", "");
                            break;
                        case "2":
                            //var history = FdkBars.ComputeBarsRangeTime("EURUSD", "Bid", "M1", DateTime.Now.AddDays(-5), DateTime.Now, 0);
                            var history = FdkLevel2.GetQuotePacked("EURUSD", new DateTime(2016, 02, 18), new DateTime(2016, 02, 20), 1);
                            Console.WriteLine(history);
                            break;
                        case "3": FdkStatic.Disconnect();
                            break;
                        case "4": return;
                    }
                }
                catch(Exception ex)
                {
                    //Console.WriteLine(ex.Message);
                } 
            }

            //FdkHelper.Reconnect("");

            //REngine.SetEnvironmentVariables();
            //// There are several options to initialize the engine, but by default the following suffice:
            //var engine = REngine.GetInstance();
            //var bars = FdkHelper.Storage.Online.GetBars("EURUSD", PriceType.Ask, BarPeriod.M1, DateTime.Now, -1000000).ToArray();
            //WriteCsv(bars, "process.csv");

            //// .NET Framework array to R vector.
            //var group1 = engine.CreateNumericVector(new double[] { 30.02, 29.99, 30.11, 29.97, 30.01, 29.99 });
            //engine.SetSymbol("group1", group1);
            //// Direct parsing from R script.

            //var lows = engine.CreateNumericVector(bars.Select(b => b.Low).ToArray());
            //engine.SetSymbol("bar_lows", lows);
            //var high = engine.CreateNumericVector(bars.Select(b => b.High).ToArray());
            //engine.SetSymbol("bar_high", high);
            //var opens = engine.CreateNumericVector(bars.Select(b => b.Open).ToArray());
            //engine.SetSymbol("bar_opens", opens);
            //var volumes = engine.CreateNumericVector(bars.Select(b => b.Volume).ToArray());
            //engine.SetSymbol("bar_volumes", volumes);



            //var group2 = engine.Evaluate("group2 <- c(29.89, 29.93, 29.72, 29.98, 30.02, 29.98)").AsNumeric();
            //var dataR = engine.Evaluate("data <- read.csv('process.csv')");

            //var meanHigh = engine.Evaluate("meanHigh <- mean(data$High)").AsNumeric();
            //// Test difference of mean and get the P-value.
            //var testResult = engine.Evaluate("t.test(group1, group2)").AsList();
            //var p = testResult["p.value"].AsNumeric().First();


            //Console.WriteLine("Group1: [{0}]", string.Join(", ", group1));
            //Console.WriteLine("Group2: [{0}]", string.Join(", ", group2));


            //Console.WriteLine("Meanhigh: [{0}]", string.Join(", ", meanHigh));
            //Console.WriteLine("P-value = {0:0.000}", p);

            //// you should always dispose of the REngine properly.
            //// After disposing of the engine, you cannot reinitialize nor reuse it
            //engine.Dispose();

        }

        public static void CallTimed(Action action, string message)
        {
            var sw = Stopwatch.StartNew();
            action();
            Console.WriteLine("{1}: {0}", sw.ElapsedMilliseconds, message);
        }
        private static void WriteCsv(Bar[] bars, string path)
        {
            CallTimed(() =>
            {
                using (var w = new StreamWriter(path))
                {
                    w.WriteLine("High,Low,Open,Volume");
                    foreach (var barId in bars)
                    {
                        var sb = new StringBuilder();
                        sb.Append(barId.High);
                        sb.Append(",");
                        sb.Append(barId.Low);
                        sb.Append(",");
                        sb.Append(barId.Open);
                        sb.Append(",");
                        sb.Append(barId.Volume);
                        w.WriteLine(sb.ToString());
                    }
                }
            }, "Csv generation time");
        }
    }
}
