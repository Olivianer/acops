using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Schnittstelle definiert die Funktionen, die zur Nutzung eines Wertebereichs n�tig sind
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
        /// Gibt den maximalen Wert zur�ck, oder setzt diesen.
        /// </summary>
        double Max
        {
            get;
            set;
        }

        /// <summary>
        /// Gibt den minimalen Wert zur�ck, oder setzt diesen.
        /// </summary>
        double Min
        {
            get;
            set;
        }

        /// <summary>
        /// Liefert die Schrittweite zur�ck.
        /// </summary>
        /// <value>Die Schrittweite.</value>
        double StepSize
        {
            get;
        }

        /// <summary>
        /// Gibt die Werte der Domain als eine Liste von Intervallen zur�ck.
        /// </summary>
        /// <value>The interval list.</value>
        List<Interval> IntervalList
        {
            get;
        }

        #endregion

        #region Functions
        /// <summary>
        /// Entfernt den �bergebenen Wert aus dem Wertebereich.
        /// </summary>
        /// <param name="value">Der Wert, der entfernt wird.</param>
        void Remove(double value);

        /// <summary>
        /// Kopiert einen Wertebereich
        /// <remarks>
        /// Damit ist es m�glich eine Kopieraktion zwischen zwei verschieden Auspr�gungen dieser
        /// Schnittstelle durchzuf�hren. Das hei�t zwei verschiedene Auspr�gungen k�nnen in einander �berf�hrt werden.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        IDomain Copy();
        #endregion
    }
}
