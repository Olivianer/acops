using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraints
{
    /// <summary>
    /// Diese Klasse implementiert das Interface IHeuristicVariable.
    /// </summary>
    /// <remarks>
    /// Die nächste zu belegende Variable wird anhand der MRV-Heuristik ermittelt.
    /// </remarks>
    public class MinimumRemainingValuesHeuristic : IHeuristicVariable
    {
        #region IHeuristicVariable Members

        //protected Random _random = new Random(0);

        /// <summary>
        /// Liefert die nächste zu belegende Variable zurück.
        /// </summary>
        /// <remarks>Die Auswahl findet dabei
        /// über die MRV-Heuristic statt(Minimum Remaining Values).
        /// Haben mehrere Variablen die gleiche Anzahl verbleibender Werte, dann
        /// wird die GradHeuristic genutzt, d.h. es wird die Variable genommen, die in
        /// den meisten Randbedingungen vorkommt.
        /// </remarks>
        /// <param name="configuration">Die Konfiguration, die noch nicht belegte Variablen enthält.</param>
        /// <param name="constraintList">Die Liste mit den Randbedingungen.</param>
        /// <returns>
        /// Gibt die als nächstes zu belegende Variable zurück.
        /// </returns>
        public Variable GetHeuristicVariable(ConstraintConfiguration configuration, ConstraintList constraintList)
        {
            Variable result = null;
            int minDomainValues = 0;
            List<Variable> mrvVariables = new List<Variable>();

            //// Zufall
            //SortedList<double, Variable> sortedList = new SortedList<double, Variable>(configuration.Variables.GetLength(0));
            //foreach (Variable var in configuration.Variables)
            //{
            //    sortedList.Add(_random.NextDouble(), var);
            //}

            //// search the variables with the minimum remaining values
            //foreach(Variable var in sortedList.Values)
            foreach (Variable var in configuration.Variables)
            {
                if (!var.HasAsignedValue)
                {
                    if (result == null)
                    {
                        result = var;
                        minDomainValues = var.Domain.Count;
                        mrvVariables.Add(var);
                    }
                    else if (minDomainValues > var.Domain.Count)
                    {
                        mrvVariables.Clear();
                        result = var;
                        minDomainValues = var.Domain.Count;
                        mrvVariables.Add(var);
                    }
                    else if (minDomainValues == var.Domain.Count)
                    {
                        mrvVariables.Add(var);
                    }
                }
            }

            // are there more then one variables we should use the variable with the most constraints
            if (mrvVariables.Count != 1)
            {
                int countConstraints = 0;
                int varCountConstraints = 0;

                foreach (Variable var in mrvVariables)
                {
                    varCountConstraints = constraintList.GetConstraints(var.Name).Count;
                    if (varCountConstraints > countConstraints)
                    {
                        result = var;
                        countConstraints = varCountConstraints;
                    }
                }
            }
            return result;
        }

        #endregion
    }
}
