using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Von dieser Schnittstelle m�ssen alle Operaitionen abgeleitet sein, die in einem Constraint verwendet werden.
    /// <remarks>
    /// Operationen k�nnen endlos ineinander verschachtelt werden.
    /// </remarks>
    /// </summary>
    public interface IOperation
    {
        /// <summary>
        /// Es wird versucht, die Operation auszuf�hren. 
        /// <remarks>
        /// Das Ergebnis wird in dem Parameter result zur�ckgegeben. Ist es nicht m�glich diese Operation
        /// durchzuf�hren, weil eine Variable noch nicht mit einem Wert belegt ist wird false zur�ckgeliefert.
        /// </remarks>
        /// </summary>
        /// <param name="result">In diesem Parameter wird das Ergebnis gespeichert</param>
        /// <returns>Es wird false zur�ckgegeben, wenn die Operation nicht durchf�hrbar ist</returns>
        bool DoOperation(out double result);

        /// <summary>
        /// Gibt die Liste der Operatoren als Array zur�ck.
        /// </summary>
        /// <returns>Die Operatoren als Array.</returns>
        IOperation[] GetOperatorList();

        /// <summary>
        /// Gibt die Form der operation zur�ck
        /// <example>
        /// 5*x = 7 hat die Form =(*(n,v),n)
        /// </example>
        /// <param name="form">Gibt die Form zur�ck</param>
        /// <param name="varList">Enth�lt alle Variablen der Form</param>
        /// <param name="numberList">Enth�lt alle Nummern der Form</param>
        /// </summary>
        void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList);
    }
}
