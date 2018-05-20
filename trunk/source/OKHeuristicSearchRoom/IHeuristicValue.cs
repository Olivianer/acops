using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;

namespace OKHeuristicSearchRoom
{
    /// <summary>
    /// Das Interface dient der Rückgabe eines Wertes für einen bestimmten Suchknoten. 
    /// Der Werte bestimmt dabei die Güte des Knotens. Je kleiner er ist, desto besser ist er.
    /// </summary>
    /// <remarks>
    /// Damit ist es möglich verwendete Heuristiken durch andere zu ersetzen
    /// </remarks>
    public interface IHeuristicValue
    {
        /// <summary>
        /// Liefert den Wert der heuristischen Funktion für den übergebenen Knoten.
        /// </summary>
        /// <returns>Gibt den Wert der Heuristik zurück.</returns>
        /// <param name="node">Der Knoten, dessen Heuristik ermittelt werden soll.</param>
        /// <param name="searchProblem">Das Problem, dass es zu lösen gilt.</param>
        /// <param name="searchMethod">Die Suchmethode, die gerade abläuft.</param>
        double GetHeuristicValue(INode node, IHeuristicSearchProblem searchProblem, ISearchMethod searchMethod);
    }
}
