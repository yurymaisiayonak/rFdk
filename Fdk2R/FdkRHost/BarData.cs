using System;
using System.Linq;

namespace RHost
{
    public struct BarData
    {
        public BarData(DateTime from, DateTime to, double open, double close, double low, double high, double volume)
        {
            From = from;
            To = to;
            Open = open;
            Close = close;
            Low = low;
            High = high;
            Volume = volume;
        }

        public double Close;
        public DateTime From;
        public double High;
        public double Low;
        public double Open;
        public DateTime To;
        public double Volume;

        public override string ToString()
        {
            return string.Format("From = {0}; To = {1}; Open = {2}; Close = {3}; Low = {4}; High = {5}; Volume = {6}", this.From, this.To, this.Open, this.Close, this.Low, this.High, this.Volume);
        }
    }
}