using System;

namespace OKSearchRoom
{
    // todo: Funtioniert noch nicht
    /// <summary>
    /// Diese Klasse realisiert die Tiefensuche mit beschränkter Breite in einem Suchbaum.
    /// </summary>
    /// <remarks>
    /// Mit dieser Klasse ist es möglich, auf einem Suchbaum eine Tiefensuche mit beschränkter Tiefe
	/// durchzuführen. Die Suchtiefe beginnt bei eins. Wird beim Erzeugen des Suchbaums, und dem Durchsuchen der
	/// Zielknoten nicht gefunden, wird die Suchtiefe erhöht. Diese Suche eignet sich für Probleme bei denen 
	/// z.B. ein Suchbaum mit unendlicher Suchtiefe entstehen könnte.
    /// </remarks>
    public class IterativeDepthFirstSearch : DepthFirstSearch
    {
        /// <summary>
        /// Die aktuell maximal zu untersuchende Tiefe.
        /// </summary>
        private int _maxDepth = int.MaxValue;

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="searchProblem"></param>
        public IterativeDepthFirstSearch(ISearchProblem searchProblem)
            : base(searchProblem)
        {
        }


        /// <summary>
        /// Konstruktor von <see cref="IterativeDepthFirstSearch"/> class.
        /// </summary>
        /// <param name="searchProblem">Das Suchproblem.</param>
        /// <param name="maxDepth">Die maximale Suchtiefe, bis zu der gesucht werden soll.</param>
        public IterativeDepthFirstSearch(ISearchProblem searchProblem, int maxDepth)
            : base(searchProblem)
        {
            _maxDepth = maxDepth;
        }

        /// <summary>
        /// Der Suchalgorithmus wird gestartet. Er kann nur beendet werden, indem innerhalb
        /// der Methode des Eventhandlers Cancel auf true gesetzt wird. Sonst endet die
        /// Suche mit dem Finden des Zielknotens, oder nach erfolglosem Absuchen des
        /// gesamten Suchraums.
        /// </summary>
        protected override void Search()
        {
            int count;
            int depth = 0;
            int depthNode = 0;
            INode[] generatedNodes;

            while (true)
            {
                // Reset 
                count = _inspectedNodes;		// Die Anzahl wird mit gelöcht, deshalb vorher sichern
                Init();
                _inspectedNodes = count;		// und zurückschreiben

                // Wurzelknoten wählen
                ChooseNode();

                // Suchproblem Bescheid sagen, dass wir neu anfangen mit suchen
                // Das ist wichtig z.B. beim Schiebepuzzle müssen alle gespeicherten Situationen gelöscht werden
                _searchProblem.OnStartSearch();

                // Die Suchtiefe erhöhen
                depth++;

                // Wenn die maximal zu durchsuchende Tiefe erreicht wurde, wars das
                if (depth > _maxDepth)
                {
                    _searchProblem.OnFoundNoneDestination();
                    return;
                }

                while (true)
                {
                    if (_currentNode.Depth < depth)
                    {
                        generatedNodes = _searchProblem.GenerateChildren(_currentNode, 0);
                    }
                    else
                    {
                        generatedNodes = null;
                    }

                    if (generatedNodes == null || generatedNodes.Length == 0)
                    {
                        if (!ChooseNode())
                        {
                            if (depthNode < depth)
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
                        foreach (INode node in generatedNodes)
                        {
                            _nodes.Push(node);
                        }

                        ChooseNode();
                    }

                    depthNode = Math.Max(depthNode, _currentNode.Depth);

                    EmitSearchEvent(_nodes.Count);

                    if (_cancel)
                    {
                        _searchProblem.OnFoundNoneDestination();
                        return;
                    }

                    if (_searchProblem.CompareNodes(_currentNode))
                    {
                        if (_searchProblem.OnFoundDestination(_currentNode, this))
                            return;
                    }
                }
            }
        }
    }
}

