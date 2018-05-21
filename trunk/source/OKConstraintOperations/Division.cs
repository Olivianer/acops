using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repräsentiert eine zweistellige Division.
    /// </summary>
    public class Division : Operation
    {
        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        internal Division()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem zwei Operatoren übergeben werden.
        /// </summary>
        /// <param name="op1">Der erste Operator (Dividend) .</param>
        /// <param name="op2">Der zweite Operator (Divisor) .</param>
        public Division(IOperation op1, IOperation op2)
        {
            _operatorList = new IOperation[2];
            _operatorList[0] = op1;
            _operatorList[1] = op2;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt das Ergebnis der Division im Parameter zurück.
        /// </summary>
        /// <remarks>
        /// Achtung, Division durch 0 wird nicht abgefangen.
        /// </remarks>
        /// <param name="result">Liefert das Ergebnis der Division.</param>
        /// <returns>Gibt an, ob die Operation durchführbar war.</returns>
        public override bool DoOperation(out double result)
        {
            double operator1;
            double operator2;

            if (_operatorList[0].DoOperation(out operator1) == false || _operatorList[1].DoOperation(out operator2) == false)
            {
                result = 0.0;
                return false;
            }

            result = operator1 / operator2;
            return true;
        }

        /// <summary>
        /// Gibt die Form der operation zurück
        /// <example>
        /// 5*x = 7 hat die Form =(*(n,v),n)
        /// </example>
        /// <param name="form">Gibt die Form zurück</param>
        /// <param name="varList">Enthält alle Variablen der Form</param>
        /// <param name="numberList">Enthält alle Nummern der Form</param>
        /// </summary>
        public override void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList)
        {
            form.Append("/(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(",");
            _operatorList[1].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
