using System;
using System.Collections.Generic;
using System.Text;
using OKPriorityQueues;
using OKSearchRoom;

namespace OKHeuristicSearchRoom
{
    /// <summary>
    /// Mit dieser Klasse ist es m�glich, auf einem Suchbaum eine gierige Suche durchzuf�hren.
    /// <remarks>
    /// Anhand einer Heuristik werden die erzeugten Knoten einsortiert. Es werden immer als
    /// erstes die Nachfolger des Knotens mit der besten Heuristik erzeugt. Alle aktuell zu untersuchenden Knoten
    /// werden als nach der Heuristik geordnete Knotenmenge gehalten.
    /// </remarks> 
    /// </summary>
    public class GreedyFirstSearch : HeuristicSearchMethod
    {
        #region Private Member
        /// <summary>
        /// Enth�lt die Knotenmenge in einer Vorrangswarteschlange
        /// </summary>
        private IPriorityQueue<double, INode> _nodes;
        #endregion

        #region Constructor
        /// <summary>
        /// Der Konstruktor.
        /// </summary>
        /// <param name="searchProblem"></param>
        public GreedyFirstSearch(IHeuristicSearchProblem searchProblem) : base(searchProblem)
        {
            _nodes = new PriorityQueue<double, INode>();
            //_nodes = new FibonacciHeap<double, INode>();
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Auswahl des n�chsten zu untersuchenden Knotens.
        /// </summary>
        /// <returns></returns>
        protected bool ChooseNode()
        {
            if (_nodes.Count > 0)
            {
                _currentNode = _nodes.Pop();
                _inspectedNodes++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Der Suchbaum wird geleert, nur die Wurzel bleibt erhalten.
        /// </summary>
        protected override void Init()
        {
            _inspectedNodes = 0;
            _nodes.Clear();
            INode[] nodes = _searchProblem.FirstNodes;
            foreach (INode node in nodes)
            {
                node.Clear();
                _nodes.Push(_searchProblem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem) _searchProblem, this), node);
            }
            if (_nodes.Count == 0)
                throw new Exception("There are no first nodes");
        }

        /// <summary>
        /// Der Suchalgorithmus wird gestartet. Er kann nur beendet werden, indem
        /// innerhalb der Methode des Eventhandlers Cancel auf true gesetzt wird.
        /// Sonst endet die Suche mit dem Finden des Zielknotens, oder nach erfolglosem
        /// Absuchen des gesamten Suchraums.
        /// </summary>
        protected override void Search()
        {
            INode[] generatedNodes;

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
                    _nodes.Push(_searchProblem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem) _searchProblem, this), node);
                }
                EmitSearchEvent(_nodes.Count);
                if (_cancel)
                {
                    _searchProblem.OnFoundNoneDestination();
                    return;
                }
            }
            _searchProblem.OnFoundNoneDestination();
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt die Knotenmenge zur�ck oder setzt diese
        /// </summary>
        public override IEnumerable<INode> NodeSet
        {
            get
            {
                return _nodes.Nodes;
            }
            set
            {
                _nodes.Clear();
                foreach (INode node in value)
                {
                    //m_Nodes.Push(new PriorityCostNode(node, m_SearchProblem.GetHeuristic(node)));
                    _nodes.Push(_searchProblem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem) _searchProblem, this), node);
                }
            }
        }
        #endregion
    }
}
