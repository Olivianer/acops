using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse bildet den Cosinuswert der enthaltenen Operationen, z.B. cos(0) = 1.
    /// </summary>
    public class Cosine : Operation
    {
        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public Cosine()
        {
            _operatorList = new IOperation[0];
        }

        /// <summary>
        /// Der Konstruktor, dem ein Operator �bergeben wird.
        /// </summary>
        /// <param name="op">Der Operator, auf dem die Operation Cosinus ausgef�hrt werden soll.</param>
        public Cosine(IOperation op)
        {
            _operatorList = new IOperation[1];
            _operatorList[0] = op;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt als Ergebnis den Cosinuswert des Operators im Parameter zur�ck.
        /// </summary>
        /// <param name="result">Liefert den Cosinuswert des Operators.</param>
        /// <returns>Gibt an, ob die Operation durchf�hrbar war</returns>
        public override bool DoOperation(out double result)
        {
            if (_operatorList[0].DoOperation(out result) == false)
            {
                result = 0.0;
                return false;
            }

            result = Math.Cos(result);
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
            form.Append("cos(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
