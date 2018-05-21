using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repr�sentiert die einstellige Negation.
    /// <remarks>
    /// Negation wird hier im Sinne der Arithmetik genutzt. Es wird also ein Minus vor den Operator gesetzt.
    /// Diese Negation hat nichts mit der aussagenlogischen Negation zu tun.
    /// </remarks>
    /// </summary>
    public class Negation : Operation
    {

        #region Constructor
        /// <summary>
        /// Standardkonstruktor f�r XML-Serialisierung
        /// </summary>
        internal Negation()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem ein Operator �bergeben wird.
        /// </summary>
        /// <param name="op">Der Operator, der negiert werden soll.</param>
        public Negation(IOperation op)
        {
            _operatorList = new IOperation[1];
            _operatorList[0] = op;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt das Ergebnis der Negation im Parameter zur�ck.
        /// </summary>
        /// <param name="result">Liefert das Ergebnis der Negation.</param>
        /// <returns>Gibt an, ob die Operation durchf�hrbar war.</returns>
        public override bool DoOperation(out double result)
        {
            if (_operatorList[0].DoOperation(out result) == false)
            {
                result = 0.0;
                return false;
            }

            result = -result;
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
            form.Append("-(");
            _operatorList[0].GetForm(ref form, ref varList, ref numberList);
            form.Append(")");
        }
        #endregion
    }
}
