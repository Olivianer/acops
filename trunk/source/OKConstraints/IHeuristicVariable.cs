using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraints
{
    /// <summary>
    /// Das Interface dient der m�glichst besten Variable, die als n�chstes belegt werden soll.
    /// </summary>
    public interface IHeuristicVariable
    {
        /// <summary>
        /// Gets the heuristic variable.
        /// </summary>
        /// <param name="configuration">Die Konfiguration, die noch nicht belegte Variablen enth�lt.</param>
        /// <param name="constraintList">Die Liste mit den Randbedingungen.</param>
        /// <returns>Gibt die als n�chstes zu belegende Variable zur�ck.</returns>
        Variable GetHeuristicVariable(ConstraintConfiguration configuration, ConstraintList constraintList);
    }
}
