using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;

namespace OKHeuristicSearchRoom
{
    /// <summary>
    /// Das Interface dient der R�ckgabe eines Wertes f�r einen bestimmten Suchknoten. 
    /// Der Werte bestimmt dabei die G�te des Knotens. Je kleiner er ist, desto besser ist er.
    /// </summary>
    /// <remarks>
    /// Damit ist es m�glich verwendete Heuristiken durch andere zu ersetzen
    /// </remarks>
    public interface IHeuristicValue
    {
        /// <summary>
        /// Liefert den Wert der heuristischen Funktion f�r den �bergebenen Knoten.
        /// </summary>
        /// <returns>Gibt den Wert der Heuristik zur�ck.</returns>
        /// <param name="node">Der Knoten, dessen Heuristik ermittelt werden soll.</param>
        /// <param name="searchProblem">Das Problem, dass es zu l�sen gilt.</param>
        /// <param name="searchMethod">Die Suchmethode, die gerade abl�uft.</param>
        double GetHeuristicValue(INode node, IHeuristicSearchProblem searchProblem, ISearchMethod searchMethod);
    }
}
