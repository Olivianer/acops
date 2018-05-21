using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;

namespace OKConstraints
{
    /// <summary>
    /// Beschreibt ein Interface, um Informationen nach dem Finden einer nach Außen zu geben.
    /// </summary>
    public interface IFoundEventHandler
    {
        /// <summary>
        /// Diese Funktion wird nach jedem Finden einer neuen Lösung aufgerufen.
        /// </summary>
        /// <param name="searchMethod">Referenz auf die Suchmethode, die diese Funktion aufgerufen hat.</param>
        /// <param name="searchProblem">Referenz auf das zugrundeliegende Suchproblem</param>
        /// <param name="destination">Die neu gefundene Lösung in einem Knoten ummantelt.</param>
        /// <param name="objValue">Der Wert der Zielfunktion der neuen Lösung.</param>
        void FoundEvent(ISearchMethod searchMethod, ISearchProblem searchProblem, INode destination, double objValue);
    }
}
