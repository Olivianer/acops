using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repräsentiert das zweistellige aussagenlogische ODER.
    /// </summary>
    public class Or : Operation
    {

        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        internal Or()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem zwei Operatoren übergeben werden.
        /// </summary>
        /// <param name="op1">Der erste Operator</param>
        /// <param name="op2">Der zweite Operator</param>
        public Or(IOperation op1, IOperation op2)
        {
            _operatorList = new IOperation[2];
            _operatorList[0] = op1;
            _operatorList[1] = op2;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt das Ergebnis der boolschen Operation ODER im Parameter zurück.
        /// </summary>
        /// <param name="result">Liefert das Ergebnis der ODER-Operation.</param>
        /// <returns>Gibt an, ob die Operation durchführbar war.</returns>
        public override bool DoOperation(out double result)
        {
            if (_operatorList[0].DoOperation(out result) == false)
                return false;

            // if left side is 1, we don't need the calculation of the right side
            if (result == 1.0)
            {
                result = 1.0;
                return true;
            }

            return _operatorList[1].DoOperation(out result);
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
            form.Append("Or(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(",");
            _operatorList[1].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
