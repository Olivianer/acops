using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Klasse repräsentiert ein Intervall.
    /// <remarks>
    /// Mit Hilfe eines Intervalls wird die Größe des Suchraums für eine Variable definiert.
    /// </remarks>
    /// <example>
    /// From = 3, To = 10, StepSize = 1
    /// </example>
    /// </summary>
    public class Interval : IComparable
    {
        #region Private Member
        /// <summary>
        /// Definiert den Start des Intervalls.
        /// </summary>
        double _from;
        /// <summary>
        /// Definiert das Ende des Intervalls.
        /// </summary>
        double _to;
        /// <summary>
        /// Definiert die Schrittweite des Intervalls.
        /// </summary>
        double _stepSize;
        #endregion

        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public Interval()
        {
        }

        /// <summary>
        /// Der Konstruktor, der das komplette Intervall definiert
        /// </summary>
        /// <param name="from">Start des Intervalls.</param>
        /// <param name="to">Ende des Intervalls.</param>
        /// <param name="stepSize">Schrittweite des Intervalls.</param>
        public Interval(double from, double to, double stepSize)
        {
            if (stepSize <= 0.0)
            {
                throw new Exception("step has to be greater 0");
            }

            if (from > to)
            {
                double help = from;
                from = to;
                to = help;
            }

            _from = from;
            _to = to;
            _stepSize = stepSize;
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Liefert den Startwert des Intervalls oder setzt diesen.
        /// </summary>
        public double From
        {
            get { return _from; }
            set { _from = value; }
        }

        /// <summary>
        /// Liefert den Endwert des Intervalls oder setzt diesen.
        /// </summary>
        public double To
        {
            get { return _to; }
            set { _to = value; }
        }

        /// <summary>
        /// Liefert die Anzahl der Werte, die sich innerhalb des Intervalls befinden.
        /// </summary>
        public int Count
        {
            get 
            {
                double range = _to - _from;
                double count = range / _stepSize + 1;
                return Convert.ToInt32(count); 
            }
        }

        /// <summary>
        /// Liefert die Schrittweite des Intervalls oder setzt diese.
        /// </summary>
        public double StepSize
        {
            get { return _stepSize; }
            set { _stepSize = value; }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
        /// </returns>
        public override string ToString()
        {
            return "(" + From.ToString() + "," + To.ToString() + ")";
        }
        #endregion

        #region IComparable Members

        /// <summary>
        /// Vergleicht zwei Intervalle anhand ihres Starts.
        /// </summary>
        /// <param name="obj">Das zu vergleichend Intervall</param>
        /// <returns>Gibt das Ergebnis der CompareTo-Funktion vom double-Wert zurück.</returns>
        public int CompareTo(object obj)
        {
            if (obj is Interval)
            {
                Interval interval = (Interval)obj;

                return _from.CompareTo(interval._from);
            }

            throw new ArgumentException("object is not a Temperature"); 
        }

        #endregion
    }
}
