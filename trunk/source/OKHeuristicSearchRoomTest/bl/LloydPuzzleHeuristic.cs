using System;
using System.Collections.Generic;
using System.Text;
using OKHeuristicSearchRoom;

namespace OKHeuristicSearchRoomTest
{
    class LloydPuzzleHeuristic : IHeuristicValue
    {
        #region IHeuristicValue Members

        public double GetHeuristicValue(OKSearchRoom.INode node, IHeuristicSearchProblem searchProblem, OKSearchRoom.ISearchMethod searchMethod)
        {
            double heuristic = 0.0;
            LloydPuzzleProblem puzzleProblem = searchProblem as LloydPuzzleProblem;
            LloydPuzzleSituation sitDest = puzzleProblem.Destination.Data as LloydPuzzleSituation;
            LloydPuzzleSituation sitNode = (LloydPuzzleSituation)node.Data;
            int count = sitDest.Dimension * sitDest.Dimension;


            for (int i = 0; i < count; i++)
            {
                heuristic += sitDest.GetDistance(i, sitNode.IndexOf(sitDest[i]));
            }

            return heuristic;
        }

        #endregion
    }
}
