using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{ 
    // todo: Funtioniert noch nicht
    /*public class BitMaskDomain : IEnumerator<int>, IEnumerable<int>, IDomain
    {
        protected List<System.UInt64> _list;
        protected int _count;
        static protected System.UInt64[] _mask = new System.UInt64[65];
        static bool _maskIsReady = false;
        protected double _currentValue;
        protected int _currentIndex;
        protected int _offset;

        public BitMaskDomain(List<Interval> intervalList)
        {
            if (intervalList.Count != 1)
                throw new Exception("BitDomain can only one interval");
            
            if (_maskIsReady == false)
            {
                BuildMask();
            }

            if (intervalList[0].From < 0 || intervalList[0].To < 0)
            {
                throw new Exception("Only a positive domain is possible");
            }

            _list = new List<UInt64>();

            _count = intervalList[0].To - intervalList[0].From + 1;

            _offset = (intervalList[0].From / 64) * 64;
            int start = intervalList[0].From - _offset;
            int end = intervalList[0].To - _offset;
            int counter = 0;

            _list.Add(0);
            for (int i = start; i <= end; i++)
            {
                int j = i - counter * 64;
                if (j == 64)
                {
                    j = 0;
                    counter++;
                    _list.Add(0);
                }
                _list[counter] = _list[counter] | _mask[j + 1];
            }
        }

        public BitMaskDomain(BitMaskDomain domain)
        {
            _list = new List<UInt64>(domain._list);
            _count = domain._count;
            _offset = domain._offset;
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public double Min
        {
            get
            {
                double result = 0;
                if (_list.Count == 0)
                    return -1.0;
                foreach (double value in Values)
                {
                    result = value;
                    break;
                }
                return result;
            }
            set 
            {
                foreach (double val in Values)
                {
                    if (val < value)
                        Remove(val);
                    else
                        break;
                }
                //throw new Exception("Not Implemented"); 
            }
        }

        public double Max
        {
            get
            {
                double result = 0;
                if (_list.Count == 0)
                    return -1.0;
                foreach (double value in Values)
                {
                    result = value;
                }
                return result;
            }
            set 
            {
                foreach (double val in Values)
                {
                    if (val > value)
                        Remove(val);
                }
            }
        }


        public void Remove(double value)
        {
            value = value - _offset;
            int listIndex = GetListIndex(value);
            int listPosition = GetListPosition(value, listIndex);
            _list[listIndex] = _list[listIndex] & ~_mask[listPosition + 1];
            _count--;
        }

        public void Clear()
        {
            _list.Clear();
            _count = 0;
        }
        
        protected static void BuildMask()
        {
            System.UInt64 shiftValue = 1;
            _mask[0]=0;
            for (int i = 1; i < 65; i++)
            {
                _mask[i] = shiftValue;
                shiftValue = shiftValue << 1;
            }
            _maskIsReady = true;
        }

        protected static int GetListIndex(int intValue)
        {
            return intValue / 64;
        }

        protected static int GetListPosition(int intValue, int listIndex)
        {
            return intValue - listIndex * 64;
        }

        #region IEnumerator<int> Members

        public double Current
        {
            get
            {
                if (_currentIndex == -1.0)
                    throw new Exception("current index is -1");
                return _currentValue + _offset;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get 
            {
                if (_currentIndex == -1)
                    throw new Exception("current index is -1");
                return _currentValue + _offset; 
            }
        }

        public bool MoveNext()
        {
            _currentIndex++;
            if (_currentIndex >= _count)
                return false;
            InternalMoveNext();
            return true;
        }

        protected void InternalMoveNext()
        {
            _currentValue++;
            int listIndex = GetListIndex(_currentValue);
            int listPosition = GetListPosition(_currentValue, listIndex);
            if ((_list[listIndex] & _mask[listPosition+1])!=0)
                return;
            InternalMoveNext();
        }

        public void Reset()
        {
            _currentIndex = -1;
            _currentValue = -1.0;
        }

        #endregion

        #region IEnumerable<int> Members

        public IEnumerator<double> GetEnumerator()
        {
            _currentIndex = -1;
            _currentValue = -1.0;
            return this;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            _currentIndex = -1;
            _currentValue = -1;
            return this;
        }

        #endregion

        public IEnumerable<double> Values
        {
            get
            {
                return this;
            }
        }

        public IDomain Copy()
        {
            return new BitMaskDomain(this);
        }
    }*/
}
