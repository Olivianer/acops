using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse bildet den Absolutwert der enthaltenen Operationen, z.B. |3-4| = 1.
    /// </summary>
    public class Absolute : Operation
    {
        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public Absolute()
        {
            _operatorList = new IOperation[0];
        }

        /// <summary>
        /// Der Konstruktor, dem ein Operator übergeben wird.
        /// </summary>
        /// <param name="op">Der Operator, der negiert werden soll.</param>
        public Absolute(IOperation op)
        {
            _operatorList = new IOperation[1];
            _operatorList[0] = op;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt als Ergebnis den Absolutwert aller Operatoren im Parameter zurück.
        /// </summary>
        /// <param name="result">Liefert den Absolutwert aller Operatoren.</param>
        /// <returns>Gibt an, ob die Operation durchführbar war</returns>
        public override bool DoOperation(out double result)
        {
            if (_operatorList[0].DoOperation(out result) == false)
            {
                result = 0.0;
                return false;
            }

            result = Math.Abs(result);
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
            form.Append("abs(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
