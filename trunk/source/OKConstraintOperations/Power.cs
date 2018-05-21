using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse bildet den Potenzwert op2 der enthaltenen Operationen op1 z.B. sin(2,4) = 16.
    /// </summary>
    public class Power : Operation
    {
        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public Power()
        {
            _operatorList = new IOperation[0];
        }

        /// <summary>
        /// Der Konstruktor, dem ein Operator übergeben wird.
        /// </summary>
        /// <param name="op1">Der Operator, auf dem die Potenzierung ausgeführt werden soll.</param>
        /// <param name="op2">Der Operator, der den Potenzwert angibt.</param>
        public Power(IOperation op1, IOperation op2)
        {
            _operatorList = new IOperation[2];
            _operatorList[0] = op1;
            _operatorList[1] = op2;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt das Ergebnis der Potenzierung im Parameter zurück.
        /// </summary>
        /// <param name="result">Liefert das Ergebnis der Potenzierung.</param>
        /// <returns>Gibt an, ob die Operation durchführbar war</returns>
        public override bool DoOperation(out double result)
        {
            double operator1;
            double operator2;

            if (_operatorList[0].DoOperation(out operator1) == false || _operatorList[1].DoOperation(out operator2) == false)
            {
                result = 0.0;
                return false;
            }

            result = Math.Pow(operator1, operator2);

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
            form.Append("pow(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(",");
            _operatorList[1].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
