using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Von dieser Schnittstelle müssen alle Operaitionen abgeleitet sein, die in einem Constraint verwendet werden.
    /// <remarks>
    /// Operationen können endlos ineinander verschachtelt werden.
    /// </remarks>
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Es wird versucht, die Operation auszuführen. 
        /// <remarks>
        /// Das Ergebnis wird in dem Parameter result zurückgegeben. Ist es nicht möglich diese Operation
        /// durchzuführen, weil eine Variable noch nicht mit einem Wert belegt ist wird false zurückgeliefert.
        /// </remarks>
        /// </summary>
        /// <param name="result">In diesem Parameter wird das Ergebnis gespeichert</param>
        /// <returns>Es wird false zurückgegeben, wenn die Operation nicht durchführbar ist</returns>
        bool DoOperation(out double result);

        /// <summary>
        /// Gibt die Liste der Operatoren als Array zurück.
        /// </summary>
        /// <returns>Die Operatoren als Array.</returns>
        IOperation[] GetOperatorList();

        /// <summary>
        /// Gibt die Form der operation zurück
        /// <example>
        /// 5*x = 7 hat die Form =(*(n,v),n)
        /// </example>
        /// <param name="form">Gibt die Form zurück</param>
        /// <param name="varList">Enthält alle Variablen der Form</param>
        /// <param name="numberList">Enthält alle Nummern der Form</param>
        /// </summary>
        void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList);
    }
}
