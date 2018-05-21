using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repräsentiert die KleinerAlsOperation.
    /// </summary>
    public class LessThan : Operation
    {

        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        internal LessThan()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem zwei Operatoren übergeben werden.
        /// </summary>
        /// <param name="op1">Der erste Operator.</param>
        /// <param name="op2">Der zweite Operator</param>
        public LessThan(IOperation op1, IOperation op2)
        {
            _operatorList = new IOperation[2];
            _operatorList[0] = op1;
            _operatorList[1] = op2;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Liefert im Parameter zurück, ob der erste Operator kleiner als der zweite ist.
        /// </summary>
        /// <param name="result">Liefert 1.0, wenn der erste Operator kleiner als der zweite ist, sonst 0.0.</param>
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

            if (operator1 < operator2)
                result = 1.0;
            else
                result = 0.0;

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
            form.Append("<(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(",");
            _operatorList[1].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
