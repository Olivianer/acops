using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;
using OKPriorityQueues;

namespace OKHeuristicSearchRoom
{
    /// <summary>
    /// nodocu
    /// </summary>
    public class SoftHillClimbingFirstSearch : DepthFirstSearch
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="searchProblem"></param>
        public SoftHillClimbingFirstSearch(IHeuristicSearchProblem searchProblem) : base(searchProblem)
        {
        }

        /// <summary>
        /// nodocu
        /// </summary>
        protected override void Search()
        {
            //DateTime timeStamp = DateTime.Now;
            int counter = 0;
            int treshold = 150; 

            INode[] generatedNodes;
            PriorityQueue<double, INode> sortedNodes = new PriorityQueue<double, INode>();
            //FibonacciHeap<double, INode> sortedNodes = new FibonacciHeap<double, INode>();
            IHeuristicSearchProblem problem = (IHeuristicSearchProblem)_searchProblem;

            while (ChooseNode())
            {
                counter++;
                // Wenn zu lange gesucht wurde, wird mal ein BestSearch angewandt
                //TimeSpan time = DateTime.Now - timeStamp;
                //if (time.Milliseconds > 200)
                if (counter > treshold)
                {
                    counter = 0;
                    //timeStamp = DateTime.Now;

                    foreach (INode node in _nodes)
                    {
                        // Es wird hier die negative Heuristik genutzt, um bei einem pop zu erst den schlechtesten Knoten zu bekommen
                        sortedNodes.Push(-problem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem) _searchProblem, this), node);
                    }

                    _nodes.Clear();

                    while (sortedNodes.Count != 0)
                    {
                        INode node = sortedNodes.Pop();
                        _nodes.Push(node);
                    }
                }

                if (_searchProblem.CompareNodes(_currentNode))
                {
                    counter = 0;
                    treshold = 150;
                    if (_searchProblem.OnFoundDestination(_currentNode, this))
                        return;
                    else
                        continue;
                }

                generatedNodes = _searchProblem.GenerateChildren(_currentNode, 0);

                foreach (INode node in generatedNodes)
                {
                    // Es wird hier die negative Heuristik genutzt, um bei einem pop zu erst den schlechtesten Knoten zu bekommen
                    sortedNodes.Push(-problem.HeuristicValue.GetHeuristicValue(node, (IHeuristicSearchProblem) _searchProblem, this), node);
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
    }
}
