using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;
using OKPriorityQueues;

namespace OKHeuristicSearchRoom
{
    /// <summary>
    /// Mit dieser Klasse ist es möglich, auf einem Suchbaum die Tiefensuche nach dem Bergsteigen-Prinzip
	/// durchzuführen. 
    /// <remarks>
    /// Dabei findet nach jedem Erzeugen der Nachfolgerknoten eine sortiertes
	/// Hinzufügen statt, wobei der Knoten mit der kleinsten Heuristik als nächstes
	/// expandiert wird.
    /// </remarks>
    /// </summary>
    public class HillClimbingFirstSearch : DepthFirstSearch
    {
        #region Constructor
        /// <summary>
        /// Dem Konstruktor wird ein Suchproblem mit Heuristik übergeben.
        /// </summary>
        /// <param name="searchProblem">Stellt das Suchproblem mit Heuristik dar, auf welches das
        /// Suchverfahren angewendet wird.</param>
        public HillClimbingFirstSearch(IHeuristicSearchProblem searchProblem) : base(searchProblem)
        {
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Der Suchalgorithmus wird gestartet. Er kann nur beendet werden, indem
        /// innerhalb der Methode des Eventhandlers Cancel auf true gesetzt wird.
        /// Sonst endet die Suche mit dem Finden des Zielknotens, oder nach erfolglosem
        /// Absuchen des gesamten Suchraums.
        /// </summary>
        protected override void Search()
        {
            //DateTime timeStamp = DateTime.Now;

            INode[] generatedNodes;
            PriorityQueue<double, INode> sortedNodes = new PriorityQueue<double, INode>();
            //FibonacciHeap<double, INode> sortedNodes = new FibonacciHeap<double, INode>();
            IHeuristicSearchProblem problem = (IHeuristicSearchProblem)_searchProblem;

            while (ChooseNode())
            {

                if (_searchProblem.CompareNodes(_currentNode))
                {
                    if (_searchProblem.OnFoundDestination(_currentNode, this))
                        return;
                    else
                        continue;
                }

                generatedNodes = _searchProblem.GenerateChildren(_currentNode, 0);

                foreach (INode node in generatedNodes)
                {
                    // Es wird hier die negative Heuristik genutzt, um bei einem pop zu erst den schlechtesten Knoten zu bekommen
                    sortedNodes.Push(-problem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem)_searchProblem, this), node);
                }

                while (sortedNodes.Count != 0)
                {
                    INode node = sortedNodes.Pop();
                    _nodes.Push(node);
                }

                EmitSearchEvent(_nodes.Count);

                if (_cancel)
                {
                    _searchProblem.OnFoundNoneDestination();
                    return;
                }
            }

            problem.OnFoundNoneDestination();

        }
        #endregion
    }
}
