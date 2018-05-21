using System;
using System.Collections.Generic;
using System.Text;
using OKHeuristicSearchRoom;
using OKConstraintOperations;
using OKSearchRoom;

namespace OKConstraints
{
    /// <summary>
    /// Diese Klasse repräsentiert im Moment die verwendete und einzige Heuristik für die Lösung von Optimierungsproblemen.
    /// </summary>
    public class ConstraintOptimizationHeuristic : IHeuristicValue
    {
        #region IHeuristicValue Members

        /// <summary>
        /// Liefert den Wert der heuristischen Funktion für den übergebenen Knoten.
        /// </summary>
        /// <param name="node">Der Knoten, dessen Heuristik ermittelt werden soll.</param>
        /// <param name="searchProblem">Das Problem, dass es zu lösen gilt.</param>
        /// <param name="searchMethod">Die Suchmethode, die gerade abläuft.</param>
        /// <returns>Gibt den Wert der Heuristik zurück.</returns>
        public double GetHeuristicValue(OKSearchRoom.INode node, IHeuristicSearchProblem searchProblem, ISearchMethod searchMethod)
        {
            double result = 0;
            if (searchProblem is ConstraintOptimizationProblem)
            {
                ConstraintConfiguration configuration = (ConstraintConfiguration)node.Data;
                ConstraintOptimizationProblem optProblem = searchProblem as ConstraintOptimizationProblem;

                if (optProblem.SolutionList.Count == 0)
                {
                    return configuration.GetCountDomainValues();
                }
                else
                {
                    OptimizationConstraint optConstraint = optProblem.GetObjectiveFunction();
                    optConstraint.SetCurrentConfiguration(configuration);
                    IOperation operation = optConstraint.ObjectiveFunction;

                    if (operation.DoOperation(out result) != true)
                    {
                        return -double.MaxValue;
                    }
                    else
                    {
                        //return configuration.GetCountDomainValues();
                        if (operation.GetType() == typeof(Minimum))
                            return result + configuration.GetCountDomainValues();
                        //return configuration.GetCountDomainValues();
                        //return (configuration.GetCountDomainValues()+result);
                        //return (result + configuration.GetCountDomainValues());
                        //return (result + _objectiveConstraint.ObjectiveValue * configuration.GetCountDomainValues());
                        //return configuration.GetCountDomainValues();
                        //return -(_objectiveConstraint.ObjectiveValue - result) + configuration.GetCountDomainValues() * result;
                        //return -(_objectiveConstraint.ObjectiveValue - result) + configuration.GetCountDomainValues() * _objectiveConstraint.ObjectiveValue;
                        //return -node.Depth * (_objectiveConstraint.ObjectiveValue - result);
                        //return result - _objectiveConstraint.ObjectiveValue;
                        //return -node.Depth * (result - _objectiveConstraint.ObjectiveValue);
                        else
                            //return node.Depth * (_objectiveConstraint.ObjectiveValue - result);
                            return configuration.GetCountDomainValues() - result;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
