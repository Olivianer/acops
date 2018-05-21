using System;
using System.Collections.Generic;
using System.Text;
using OKHeuristicSearchRoom;
using OKSearchRoom;

namespace OKConstraints
{
    internal class CountDomainValueHeuristic : IHeuristicValue
    {

        #region IHeuristic Members

        public double GetHeuristicValue(OKSearchRoom.INode node, IHeuristicSearchProblem searchProblem, ISearchMethod searchMethod)
        {
            ConstraintConfiguration configuration = (ConstraintConfiguration)node.Data;
            return configuration.GetCountDomainValues();
        }

        #endregion
    }
}
