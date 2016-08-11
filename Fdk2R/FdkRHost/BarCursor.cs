using System;
using System.Collections.Generic;
using System.Linq;

namespace RHost
{
    class BarCursor
    {
        public BarCursor(BarData[] data)
        {
            _data = data;
        }


        int _position;

        static BarCursor()
        {
            undefined = new BarData(DateTime.MinValue, DateTime.MinValue, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN);
        }

        static readonly BarData undefined;
        private BarData[] _data;

        public bool CanContinue
        {
            get
            {
                return _position < _data.Length;
            }
        }

        public BarData Current
        {
            get
            {
                if (CanContinue)
                    return _data[_position];
                return new BarData(DateTime.MinValue, DateTime.MinValue, Previous.Open, Previous.Close, Previous.Low, Previous.High, Previous.Volume);
            }
        }

        public BarData Previous
        {
            get
            {
                if (_position == 0)
                    return undefined;
                return _data[_position - 1];
            }
        }

        public void CopyRange(List<BarData> resultBars, BarData undefinedBar, bool isUndefinedBid)
        {
            BarData[] barsData = _data;
            int position = _position;

            while (CanContinue)
            {
                if (isUndefinedBid)
                {
                    FdkBarsMerger.AddBarPairs(resultBars, undefinedBar, Current);
                }
                else
                {
                    FdkBarsMerger.AddBarPairs(resultBars, Current, undefinedBar);
                }
                Next();
            }
        }
        internal void Next()
        {
            _position++;
        }
    }
}