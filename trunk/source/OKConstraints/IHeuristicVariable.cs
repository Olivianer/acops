using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraints
{
    /// <summary>
    /// Das Interface dient der möglichst besten Variable, die als nächstes belegt werden soll.
    /// </summary>
    public interface IHeuristicVariable
    {
        /// <summary>
        /// Gets the heuristic variable.
        /// </summary>
        /// <param name="configuration">Die Konfiguration, die noch nicht belegte Variablen enthält.</param>
        /// <param name="constraintList">Die Liste mit den Randbedingungen.</param>
        /// <returns>Gibt die als nächstes zu belegende Variable zurück.</returns>
        Variable GetHeuristicVariable(ConstraintConfiguration configuration, ConstraintList constraintList);
    }
}
