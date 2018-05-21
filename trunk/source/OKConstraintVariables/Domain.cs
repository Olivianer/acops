using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Klasse stellt den Wertebereich einer Variablen dar
    /// <remarks>
    /// Alle möglichen Werte werden in einer Liste gehalten (speicherintensiv) .
    /// </remarks>
    /// </summary>
    public class Domain : IDomain, IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Liste enthält alle Werte des Wertebereichs
        /// </summary>
        protected List<double> _list;

        /// <summary>
        /// Enthält die Schrittweite
        /// </summary>
        protected double _stepSize;
        #endregion

        #region Constructor
        /// <summary>
        /// Der Standardkonstruktor
        /// <remarks>
        /// Es wird ein leerer Wertebereich angelegt.
        /// </remarks>
        /// </summary>
        public Domain()
        {
            _list = new List<double>();
        }

        /// <summary>
        /// Ein Konstruktor, dem eine IntervallListe übergeben wird.
        /// </summary>
        /// <remarks>
        /// Anhand der IntervallListe werden alle Werte erzeugt und in eine Liste eingefügt.
        /// </remarks>
        /// <param name="intervalList">IntervallListe zur Definition der möglichen Werte</param>
        public Domain(List<Interval> intervalList)
        {
            _list = new List<double>(intervalList.Count);

            if (intervalList.Count > 0)
            {
                _stepSize = intervalList[0].StepSize;
                foreach (Interval interval in intervalList)
                {
                    for (double i = interval.From; i <= interval.To; i += interval.StepSize)
                    {
                        _list.Add(i);
                    }
                }
            }  
        }

        /// <summary>
        /// Der Kopierkonstruktor.
        /// </summary>
        /// <param name="domain"></param>
        public Domain(Domain domain)
        {
            _list = new List<double>(domain._list);
            _stepSize = domain._stepSize;
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt die Anzahl der Werte des Wertebereichs zurück.
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
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
                if (_list.Count == 0)
                    return -1;
                return _list[0];
            }
            set 
            {
                if (_list.Count == 0)
                    return;
                int removeIndex = int.MaxValue;
                for (int i = 0; i < _list.Count; i++)
                {
                    if (_list[i] < value)
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex != int.MaxValue)
                {
                    _list.RemoveRange(0, removeIndex);
                }
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
                if (_list.Count == 0)
                    return -1;
                return _list[_list.Count-1];
            }
            set 
            {
                if (_list.Count == 0)
                    return;
                int removeIndex = int.MinValue;
                for (int i=0; i<_list.Count;i++)
                {
                    if (_list[i]>value)
                    {
                        removeIndex = i;
                        break;
                    }
                }
                if (removeIndex != int.MinValue)
                {
                    _list.RemoveRange(removeIndex, _list.Count - removeIndex);
                }
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
                List<Interval> result = new List<Interval>();
                double lastValue = double.NaN;
                Interval interval = null;
                foreach (double value in _list)
                {
                    if (lastValue == double.NaN || value != lastValue + _stepSize)
                    {
                        if (interval != null)
                            result.Add(interval);
                        interval = new Interval();
                        interval.From = value;
                        interval.StepSize = _stepSize;
                        lastValue = value;
                    }
                    else
                    {
                        lastValue = value;
                        interval.To = value;
                    }
                }

                // Das letzte Interval noch mit reinpacken
                if (interval != null)
                    result.Add(interval);

                return result;
            }
        }
        
        #endregion

        #region Public Functions

        /// <summary>
        /// Entfernt einen Wert aus dem Wertebereich
        /// </summary>
        /// <param name="value">Der Wert, der entfernt wird.</param>
        public void Remove(double value)
        {
            _list.Remove(value);
        }

        /// <summary>
        /// Gibt ein neu erzeugtes Domain-Objekt zurück, dass eine Kopie dieses Objekts darstellt.
        /// </summary>
        /// <returns>Das neu erzeugte Objekt.</returns>
        public IDomain Copy()
        {
            return new Domain(this);
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Interval));
            Interval interval = xmlSerializer.Deserialize(reader) as Interval;
            for (double i = interval.From; i <= interval.To; i += interval.StepSize)
            {
                _list.Add(i);
            }
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            // todo: für mehrere Intervalle implementieren
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Interval));
            if (_list.Count > 0)
            {
                Interval interval;
                if (_list.Count == 1)
                {
                    interval = new Interval(_list[0], _list[0], 1);
                }
                else
                {
                    interval = new Interval(Min, Max, _list[1] - _list[0]);
                }
                xmlSerializer.Serialize(writer, interval);
            }
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
            return new DomainEnum(_list);
        }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new DomainEnum(_list);
        }

        #endregion

        #region Nested Classes
        /// <summary>
        /// Stellt den Enumerator für die Domain bereit.
        /// </summary>
        public class DomainEnum : IEnumerator<double>
        {
            private List<double> _list;
            private int _position;

            /// <summary>
            /// Initializes a new instance of the <see cref="DomainEnum"/> class.
            /// </summary>
            /// <param name="list">The list.</param>
            public DomainEnum(List<double> list)
            {
                _list = list;
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
                    try
                    {
                        return _list[_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
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
                    try
                    {
                        return _list[_position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
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
                return (_position < _list.Count);
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
