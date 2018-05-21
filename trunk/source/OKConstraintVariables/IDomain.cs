using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Schnittstelle definiert die Funktionen, die zur Nutzung eines Wertebereichs nötig sind
    /// </summary>
    public interface IDomain : IEnumerable<double>
    {
        #region Member
        /// <summary>
        /// Liefert die Anzahl der Werte.
        /// </summary>
        int Count
        {
            get;
        }

        /// <summary>
        /// Gibt den maximalen Wert zurück, oder setzt diesen.
        /// </summary>
        double Max
        {
            get;
            set;
        }

        /// <summary>
        /// Gibt den minimalen Wert zurück, oder setzt diesen.
        /// </summary>
        double Min
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert die Schrittweite zurück.
        /// </summary>
        /// <value>Die Schrittweite.</value>
        double StepSize
        {
            get;
        }

        /// <summary>
        /// Gibt die Werte der Domain als eine Liste von Intervallen zurück.
        /// </summary>
        /// <value>The interval list.</value>
        List<Interval> IntervalList
        {
            get;
        }

        #endregion

        #region Functions
        /// <summary>
        /// Entfernt den übergebenen Wert aus dem Wertebereich.
        /// </summary>
        /// <param name="value">Der Wert, der entfernt wird.</param>
        void Remove(double value);

        /// <summary>
        /// Kopiert einen Wertebereich
        /// <remarks>
        /// Damit ist es möglich eine Kopieraktion zwischen zwei verschieden Ausprägungen dieser
        /// Schnittstelle durchzuführen. Das heißt zwei verschiedene Ausprägungen können in einander überführt werden.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        IDomain Copy();
        #endregion
    }
}
