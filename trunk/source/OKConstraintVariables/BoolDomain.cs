using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Klasse stellt den Wertebereich einer Variablen dar
    /// <remarks>
    /// Der Wertebereich kann nur die Werte 0 und 1 besitzen. Diese Domain ist geeignet für die Lösung von sogenannten SAT-Problemen.
    /// Bei diesen Problemen geht es darum, für eine gegebene Menge von boolsche Variablen und in CNF (conjunctive normal form, z.B. x1 or x2 or x3)
    /// angegebenen Klauseln eine gültige Belegung zu finden.
    /// </remarks>
    /// </summary>
    public class BoolDomain : IDomain
    {
        static int BoolValueEmpty = 0;
        static int BoolValueZero = 1;
        static int BoolValueOne = 2;
        
        #region Protected Member
        /// <summary>
        /// nodocu
        /// </summary>
        protected int _values;
        #endregion

        #region Constructor
        /// <summary>
        /// Der Standardkonstruktor
        /// <remarks>
        /// Es wird ein leerer Wertebereich angelegt.
        /// </remarks>
        /// </summary>
        public BoolDomain()
        {
            _values = BoolValueEmpty;
        }

        /// <summary>
        /// Ein Konstruktor, dem eine IntervallListe übergeben wird.
        /// </summary>
        /// <remarks>
        /// Anhand der IntervallListe werden alle Werte erzeugt und in eine Liste eingefügt.
        /// </remarks>
        /// <param name="intervalList">IntervallListe zur Definition der möglichen Werte</param>
        public BoolDomain(List<Interval> intervalList)
        {
            if (intervalList.Count > 1 || intervalList.Count  < 0)
            {
                throw new DomainException("The intervalList contains " + intervalList.Count.ToString() + ". The boolean domain can only take one interval between 0 and 1.");
            }

            Interval interval = intervalList[0];

            if (interval.Count > 2)
            {
                throw new DomainException("The interval contains " + interval.Count.ToString() + ". The boolean domain can only hold two values (0 and 1).");
            }
            
            if (interval.From != 0.0 && interval.From != 1.0)
            {
                throw new DomainException("The minimum value for a boolean domain can not be " + interval.From.ToString());
            }

            if (interval.To != 0.0 && interval.To != 1.0)
            {
                throw new DomainException("The maximum value for a boolean domain can not be " + interval.To.ToString());
            }

            for (double i = interval.From; i <= interval.To; i += interval.StepSize)
            {
                if (i == 0.0)
                    _values |= BoolValueZero;
                else
                    _values |= BoolValueOne;
            }
        }

        /// <summary>
        /// Der Kopierkonstruktor.
        /// </summary>
        /// <param name="domain"></param>
        public BoolDomain(BoolDomain domain)
        {
            _values = domain._values;
        }
        #endregion

        #region public Member
        /// <summary>
        /// Gibt die Anzahl der Werte des Wertebereichs zurück.
        /// </summary>
        public int Count
        {
            get
            {
                if (_values == 3)
                    return 2;
                else if (_values == 2 || _values == 1)
                    return 1;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Gibt den kleinsten Wert zurück, oder setzt diesen
        /// <remarks>
        /// Durch Setzen dieses Wertes wird der Wertebereich eingeschränkt.
        /// </remarks>
        /// </summary>
        public double Min
        {
            get
            {
                if (_values == BoolValueZero || _values == 3)
                    return 0.0;
                if (_values == BoolValueOne)
                    return 1.0;

                throw new DomainException("The minimum is not available. The count of the boolean domain is zero.");
            }
            set
            {
                if (value == 1.0)
                    _values &= ~BoolValueZero;
            }
        }

        /// <summary>
        /// Gibt den größten Wert zurück, oder setzt diesen
        /// <remarks>
        /// Durch Setzen dieses Wertes wird der Wertebereich eingeschränkt.
        /// </remarks>
        /// </summary>
        public double Max
        {
            get
            {
                if (_values == BoolValueOne || _values == 3)
                    return 1.0;
                if (_values == BoolValueZero)
                    return 0.0;

                throw new DomainException("The maximum is not available. The count of the boolean domain is zero.");
            }
            set
            {
                if (value == 0.0)
                    _values &= ~BoolValueOne;
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
                return 1.0;
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
                List<Interval> result = new List<Interval>();
                Interval interval=null;
                if (Count > 0)
                {
                    if (_values == 3)
                    {
                        interval = new Interval(0.0, 1.0, 1.0);
                        result.Add(interval);
                    }
                    else if (_values == BoolValueZero)
                    {
                        interval = new Interval(0.0, 0.0, 1.0);
                        result.Add(interval);
                    }
                    else if (_values == BoolValueOne)
                    {
                        interval = new Interval(1.0, 1.0, 1.0);
                        result.Add(interval);
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Entfernt einen Wert aus dem Wertebereich
        /// </summary>
        /// <param name="value">Der Wert, der entfernt wird.</param>
        public void Remove(double value)
        {
            if (value != 0.0 && value != 1.0)
                throw new DomainException("You can only remove the values 0 and 1 to a boolean domain.");
            if (value == 0.0)
                _values &= ~BoolValueZero;
            else
                _values &= ~BoolValueOne;
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Gibt ein neu erzeugtes Domain-Objekt zurück, dass eine Kopie dieses Objekts darstellt.
        /// </summary>
        /// <returns>Das neu erzeugte Objekt.</returns>
        public IDomain Copy()
        {
            return new BoolDomain(this);
        }
        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<double> GetEnumerator()
        {
            bool zero = false;
            bool one = false;
            if (_values == BoolValueOne || _values == 3)
                one = true;
            if (_values == BoolValueZero || _values == 3)
                zero = true;
            return new BoolDomainEnum(zero, one);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            bool zero = false;
            bool one = false;
            if (_values == BoolValueOne || _values == 3)
                one = true;
            if (_values == BoolValueZero || _values == 3)
                zero = true;
            return new BoolDomainEnum(zero, one);
        }

        #endregion



        #region Nested Classes
        /// <summary>
        /// Stellt den Enumerator für die Booldomain bereit.
        /// </summary>
        public class BoolDomainEnum : IEnumerator<double>
        {
            private bool _zero;
            private bool _one;
            private int _position;

            /// <summary>
            /// Initializes a new instance of the <see cref="BoolDomainEnum"/> class.
            /// </summary>
            /// <param name="zero">if set to <c>true</c> [zero].</param>
            /// <param name="one">if set to <c>true</c> [one].</param>
            public BoolDomainEnum(bool zero, bool one)
            {
                _zero = zero;
                _one = one;
                _position = -1;
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
                    if (_position == 0.0)
                    {
                        if (_zero)
                            return 0.0;
                        else
                            return 1.0;
                    }
                    else if (_position == 1.0)
                    {
                        return 1.0;
                    }
                    throw new InvalidOperationException();
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
                    if (_position == 0.0)
                    {
                        if (_zero)
                            return 0.0;
                        else
                            return 1.0;
                    }
                    else if (_position == 1.0)
                    {
                        return 1.0;
                    }
                    throw new InvalidOperationException();
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
                _position++;
                int count = 0;
                if (_zero && _one)
                    count = 2;
                else if (_zero || _one)
                    count = 1;
                return (_position < count);
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created. </exception>
            public void Reset()
            {
                _position = -1;
            }

            #endregion
        }
        #endregion
    }
}
