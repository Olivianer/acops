using System;
using System.Collections.Generic;

namespace OKSearchRoom
{
    // todo: Funtioniert noch nicht
    /// <summary>
    /// Diese Klasse realisiert die Tiefensuche mit beschr�nkter Breite in einem Suchbaum.
    /// </summary>
    /// <remarks>
    /// Mit dieser Klasse ist es m�glich, auf einem Suchbaum eine Tiefensuche mit beschr�nkter Breite 
	/// durchzuf�hren. Die Suchbreite beginnt bei eins. Wird beim Erzeugen des Suchbaums, und dem Durchsuchen der
	/// Zielknoten nicht gefunden, wird die Suchbreite erh�ht. Mit  Suchbreite ist die Anzahl der Nachfolger eines Knotens gemeint.
	/// Diese Suche eignet sich f�r Probleme bei denen z.B. ein Suchbaum mit unendlich vielen Nachfolgern bei einem Knoten entstehen k�nnte.
    /// </remarks>
    public class IterativeBreadthFirstSearch : DepthFirstSearch
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="searchProblem"></param>
        public IterativeBreadthFirstSearch(ISearchProblem searchProblem)
            : base(searchProblem)
        {
        }

        /// <summary>
        /// Der Suchalgorithmus wird gestartet. Er kann nur beendet werden, indem innerhalb
        /// der Methode des Eventhandlers Cancel auf true gesetzt wird. Sonst endet die
        /// Suche mit dem Finden des Zielknotens, oder nach erfolglosem Absuchen des
        /// gesamten Suchraums.
        /// </summary>
        protected override void Search()
        {
            int count = 0;
            int breadth = 0;
            int nodeBreadth = 0;
            INode[] generatedNodes;

            // Wurzelknoten w�hlen
            ChooseNode();

            while (!_searchProblem.CompareNodes(_currentNode))
            {
                count = _inspectedNodes;		// Die Anzahl wird mit gel�cht, deshalb vorher sichern
                Init();
                _inspectedNodes = count;		// und zur�ckschreiben
                breadth++;

                while (!_searchProblem.CompareNodes(_currentNode) && _nodes.Count > 0)
                {
                    generatedNodes = _searchProblem.GenerateChildren(_currentNode, breadth);

                    if (generatedNodes.GetLength(0) == 0)
                    {
                        if (!ChooseNode())
                        {
                            if (nodeBreadth < breadth)
                            {
                                _searchProblem.OnFoundNoneDestination();
                                return;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (TreeNode node in generatedNodes)
                        {
                            _nodes.Push(node);
                        }

                        nodeBreadth = Math.Max(nodeBreadth, generatedNodes.GetLength(0));

                        ChooseNode();
                    }

                    EmitSearchEvent(_nodes.Count);

                    if (_cancel)
                    {
                        _searchProblem.OnFoundNoneDestination();
                        return;
                    }
                }

                _searchProblem.OnFoundDestination(_currentNode, this);
            }
        }
    }
}

