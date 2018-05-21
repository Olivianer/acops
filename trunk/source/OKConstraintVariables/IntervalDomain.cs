using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Todo_ Funktioniert noch nicht
    /// </summary>
    public class IntervalDomain : IDomain
    {
        /// <summary>
        /// Enthält alle Intervalle in einer Liste
        /// </summary>
        protected List<Interval> _list = new List<Interval>();
        /// <summary>
        /// Die Schrittweite
        /// </summary>
        protected double _stepSize;
        //List<double> _testList;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalDomain"/> class.
        /// </summary>
        /// <param name="intervalList">The interval list.</param>
        public IntervalDomain(List<Interval> intervalList)
        {
            if (intervalList.Count != 1)
                throw new Exception("IntervalDomain can only one interval");

            Interval interval = new Interval(intervalList[0].From, intervalList[0].To, intervalList[0].StepSize);
            _list.Add(interval);
            _stepSize = interval.StepSize;
            //_testList = new List<double>(Count);
            //_log.Info("Count1:" + Count.ToString());
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IntervalDomain"/> class.
        /// </summary>
        /// <param name="domain">The domain.</param>
        public IntervalDomain(IntervalDomain domain)
        {
            foreach (Interval interval in domain._list)
            {
                Interval copyInterval = new Interval(interval.From, interval.To, interval.StepSize);
                _list.Add(copyInterval);
            }
            _stepSize = domain.StepSize;
            //_testList = new List<double>(Count);
            //_log.Info("Count2:" + Count.ToString());
        }

        /// <summary>
        /// Gibt den minimalen Wert zurück, oder setzt diesen.
        /// </summary>
        /// <value></value>
        public double Min
        {
            get
            {
                try
                {
                    return _list[0].From;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                //_log.Info("Set Min " + value.ToString());
                try
                {
                    if (value > Max)
                        throw new Exception("min is greater than max");
                    List<Interval> removeList = new List<Interval>();
                    for (int i = 0; i < _list.Count; i++)
                    {
                        if (value >= _list[i].From && value <= _list[i].To)
                        {
                            _list[i].From = value;
                            break;
                        }
                        removeList.Add(_list[i]);
                    }
                    foreach (Interval removeInterval in removeList)
                    {
                        _list.Remove(removeInterval);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                //_log.Info("Set Min Count:" + _list.Count.ToString());
            }
        }

        /// <summary>
        /// Gibt den maximalen Wert zurück, oder setzt diesen.
        /// </summary>
        /// <value></value>
        public double Max
        {
            get
            {
                try
                {
                    return _list[_list.Count - 1].To;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            set
            {
                if (value > Max)
                {
                    //_log.Fatal("The value for set max(" + value.ToString() + ") is greater than max(" + Max.ToString() + ")");
                    return;
                }
                try
                {
                    if (value < Min)
                        throw new Exception("max is lesser than min");
                    List<Interval> removeList = new List<Interval>();
                    for (int i = _list.Count - 1; i >= 0; i--)
                    {
                        if (value >= _list[i].From && value <= _list[i].To)
                        {
                            _list[i].To = value;
                            break;
                        }
                        removeList.Add(_list[i]);
                    }
                    foreach (Interval removeInterval in removeList)
                    {
                        _list.Remove(removeInterval);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Liefert die Anzahl der Werte.
        /// </summary>
        /// <value></value>
        public int Count
        {
            get
            {
                int result = 0;
                foreach (Interval interval in _list)
                {
                    result += interval.Count;
                }
                return result;
            }
        }

        /// <summary>
        /// Liefert die Schrittweite zurück.
        /// </summary>
        /// <value>Die Schrittweite.</value>
        public double StepSize
        {
            get
            {
                return _stepSize;
            }
        }

        /// <summary>
        /// Gibt die Werte der Domain als eine Liste von Intervallen zurück.
        /// </summary>
        /// <value>The interval list.</value>
        public List<Interval> IntervalList
        {
            get
            {
                return _list;
            }
        }

        /// <summary>
        /// Entfernt den übergebenen Wert aus dem Wertebereich.
        /// </summary>
        /// <param name="value">Der Wert, der entfernt wird.</param>
        public void Remove(double value)
        {
            //_log.Info("Remove " + value.ToString());
            //_log.Info("Before " + ToString());
            try
            {
                if (value < Min || value > Max)
                    throw new Exception("the value to remove is out of range");
                Interval removeInterval = null;
                foreach (Interval interval in _list)
                {
                    if (value >= interval.From && value <= interval.To)
                    {
                        removeInterval = interval;
                        break;
                    }
                }
                if (removeInterval == null)
                    throw new Exception("found no interval");

                // Wenn nur noch ein Wert im Intervall ist, wird es gelöscht
                if (removeInterval.From == value && removeInterval.To == value)
                {
                    _list.Remove(removeInterval);
                }
                // Wenn eine Einschränkung des Intervalls von Unten her erfolgt
                else if (removeInterval.From == value)
                {
                    removeInterval.From = removeInterval.From + 1;
                }
                // Wenn eine Einschränkung des Intervalls von Oben her erfolgt
                else if (removeInterval.To == value)
                {
                    removeInterval.To = removeInterval.To - 1;
                }
                // Wenn eine Einschränkung des Intervalls in der Mitte erfolgt
                else
                {
                    Interval newBottomInterval = new Interval(removeInterval.From, value - 1, removeInterval.StepSize);
                    Interval newTopInterval = new Interval(value + 1, removeInterval.To, removeInterval.StepSize);
                    // Entfernen des alten Intervalls
                    _list.Remove(removeInterval);
                    // Hinzufügen der zwei neuen Intervalle
                    _list.Add(newBottomInterval);
                    _list.Add(newTopInterval);
                    // Neue Sortierung der Intervalle
                    _list.Sort();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            //_log.Info("After " + ToString());
        }

        /// <summary>
        /// Kopiert einen Wertebereich
        /// <remarks>
        /// Damit ist es möglich eine Kopieraktion zwischen zwei verschiedenen Ausprägungen dieser
        /// Schnittstelle durchzuführen. Das heißt zwei verschiedene Ausprägungen können in einander überführt werden.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public IDomain Copy()
        {
            return new IntervalDomain(this);
        }

        #region Public Functions
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            string result = "";
            for (int i = 0; i < _list.Count; i++)
            {
                result += _list[i].ToString();
            }
            return result;
        }
        #endregion

        #region IEnumerable<double> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<double> GetEnumerator()
        {
            return new IntervalDomainEnum(_list);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new IntervalDomainEnum(_list);
        }

        #endregion

        #region Nested Classes
        /// <summary>
        /// Stellt den Enumerator für die Intervaldomain bereit.
        /// </summary>
        public class IntervalDomainEnum : IEnumerator<double>
        {
            /// <summary>
            /// Enthält eine Referenz auf die Liste mit den Intervallen
            /// </summary>
            protected List<Interval> _list;
            /// <summary>
            /// Enthält den aktuellen Wert
            /// </summary>
            protected double _current;
            /// <summary>
            /// Enthält den Index des aktuellen Intervalls, sprich wo der aktuelle Wert herkommt
            /// </summary>
            protected int _currentInterval;
            /// <summary>
            /// Initializes a new instance of the <see cref="IntervalDomainEnum"/> class.
            /// </summary>
            /// <param name="list">The list.</param>
            public IntervalDomainEnum(List<Interval> list)
            {
                _list = list;
                _current = double.MinValue;
                _currentInterval = int.MinValue;
            }


            #region IEnumerator<double> Members

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            public double Current
            {
                get
                {
                    return _current;
                }
            }

            #endregion

            #region IDisposable Members

            /// <summary>
            /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
            /// </summary>
            public void Dispose()
            {
            }

            #endregion

            #region IEnumerator Members

            /// <summary>
            /// Gets the element in the collection at the current position of the enumerator.
            /// </summary>
            /// <value></value>
            /// <returns>The element in the collection at the current position of the enumerator.</returns>
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return _current;
                }
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>
            /// true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.
            /// </returns>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public bool MoveNext()
            {
                if (_list.Count == 0)
                    return false;

                if (_current == double.MinValue)
                {
                    _current = _list[0].From;
                    _currentInterval = 0;
                    return true;
                }

                _current += _list[_currentInterval].StepSize;

                if (_current <= _list[_currentInterval].To)
                    return true;

                _currentInterval += 1;

                if (_currentInterval >= _list.Count)
                    return false;

                _current = _list[_currentInterval].From;
                return true;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                _current = double.MinValue;
                _currentInterval = int.MinValue;
            }

            #endregion
        }
        #endregion
    }

    
}
