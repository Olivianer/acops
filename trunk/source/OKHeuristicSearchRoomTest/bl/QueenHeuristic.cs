using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;
using OKHeuristicSearchRoom;

namespace OKHeuristicSearchRoomTest
{
    public class QueenHeuristic : IHeuristicValue
    {
        #region IHeuristicValue Members

        public double GetHeuristicValue(OKSearchRoom.INode node, IHeuristicSearchProblem searchProblem, OKSearchRoom.ISearchMethod searchMethod)
        {
            QueenConstellation constellation = (QueenConstellation)node.Data;
            return constellation.LastQueenOccupations;
        }

        #endregion
    }
}
