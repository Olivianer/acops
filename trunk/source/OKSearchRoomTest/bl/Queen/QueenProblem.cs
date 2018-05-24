using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;

namespace OKSearchRoomTest
{
    public class QueenProblem : ISearchProblem
    {
        protected TreeNode _begin;
        protected List<QueenConstellation> _createdConstellations;
        protected TreeNode _destination;

        public QueenProblem(int boardDimension)
        {
            _createdConstellations = new List<QueenConstellation>();
            QueenConstellation startConstellation = new QueenConstellation(boardDimension);
            _begin = new TreeNode(startConstellation);
            _destination = null;
        }

        public bool CompareNodes(INode node)
        {
            QueenConstellation constellation = (QueenConstellation)node.Data;
            return constellation.AllQueensOnTheBoard;
        }

        public INode Destination
        {
            get { return _destination; }
        }

        public INode[] FirstNodes
        {
            get
            {
                TreeNode[] nodes = new TreeNode[1];
                nodes[0] = _begin;
                return nodes;
            }
        }

        public double GetHeuristic(INode node)
        {
            QueenConstellation constellation = (QueenConstellation)node.Data;
            return constellation.LastQueenOccupations;
        }

        public INode[] GenerateChildren(INode node, int maxCount)
        {
            TreeNode child;
            QueenConstellation newConstellation;
            List<INode> generatedNodes = new List<INode>();

            QueenConstellation constellation = (QueenConstellation)node.Data;
            int freeColumn = constellation.FreeColumn;
            if (freeColumn == -1)
                throw new Exception("No FreeColumn available");
            for (int i = 0; i < constellation.BoardDimension; i++)
            {
                if (constellation.TestQueenPosition(freeColumn, i))
                {
                    newConstellation = new QueenConstellation(constellation);
                    newConstellation.SetQueen(freeColumn, i);
                    child = new TreeNode((TreeNode)node, newConstellation);
                    generatedNodes.Add(child);
                }
                if (maxCount > 0 && generatedNodes.Count == maxCount)
                    break;
            }

            INode[] result = new TreeNode[generatedNodes.Count];
            generatedNodes.CopyTo(result);
            return result;
        }

        public void FoundNoneDestination()
        {
        }

        public void FoundDestination(INode destination)
        {
            _destination = (TreeNode)destination;
        }

        public void OnFoundNoneDestination()
        {
        }

        public bool OnFoundDestination(INode destination, ISearchMethod searchMethod)
        {
            _destination = (TreeNode)destination;
            return true;
        }

        public void OnStartSearch()
        {
            // L鰏chen der gemerkten Situationen
            _createdConstellations.Clear();
        }
    }
}
