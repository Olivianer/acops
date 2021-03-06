using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repr�sentiert eine zweistellige Addition.
    /// </summary>
    public class Addition : Operation
    {

        #region Constructor
        /// <summary>
        /// Standardkonstruktor f�r XML-Serialisierung
        /// </summary>
        internal Addition()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem zwei Operatoren �bergeben werden.
        /// </summary>
        /// <param name="op1">Der erste Operator.</param>
        /// <param name="op2">Der zweite Operator</param>
        public Addition(IOperation op1, IOperation op2)
        {
            _operatorList = new IOperation[2];
            _operatorList[0] = op1;
            _operatorList[1] = op2;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt das Ergebnis der Addition im Parameter zur�ck.
        /// </summary>
        /// <param name="result">Liefert die Summe.</param>
        /// <returns>Gibt an, ob die Operation durchf�hrbar war.</returns>
        public override bool DoOperation(out double result)
        {
            double operator1;
            double operator2;

            if (_operatorList[0].DoOperation(out operator1) == false || _operatorList[1].DoOperation(out operator2) == false)
            {
                result = 0.0;
                return false;
            }

            result = operator1 + operator2;
            return true;
        }

        /// <summary>
        /// Gibt die Form der operation zur�ck
        /// <example>
        /// 5*x = 7 hat die Form =(*(n,v),n)
        /// </example>
        /// <param name="form">Gibt die Form zur�ck</param>
        /// <param name="varList">Enth�lt alle Variablen der Form</param>
        /// <param name="numberList">Enth�lt alle Nummern der Form</param>
        /// </summary>
        public override void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList)
        {
            form.Append("+(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(",");
            _operatorList[1].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
