using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repr�sentiert eine mehrstellige Oparation, wobei das Minimum aller Operatoren bestimmt wird.
    /// </summary>
    public class Minimum : Operation
    {

        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public Minimum()
        {
            _operatorList = new IOperation[0];
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt als Ergebnis das Minimum aller Operatoren im Parameter zur�ck.
        /// </summary>
        /// <param name="result">Liefert das Minimum aller Operatoren.</param>
        /// <returns>Gibt an, ob die Operation durchf�hrbar war</returns>
        public override bool DoOperation(out double result)
        {
            if (_operatorList.GetLength(0) == 0)
                throw new Exception("the list of operators for the minimum function is 0.");

            result = double.MaxValue;
            double operatorValue;

            // Bei der Maximum-Operation ist es nur wichtig, dass mindestens ein Element vollst�ndig zugewiesen ist
            foreach (IOperation operation in _operatorList)
            {
                if (operation.DoOperation(out operatorValue) == true)
                {
                    if (operatorValue < result)
                        result = operatorValue;
                }
            }

            // Wenn gar kein Element vollst�ndig zugewiesen wurde, muss false zur�ckgegeben werden
            if (result == double.MaxValue)
                return false;

            //System.Console.WriteLine(maxValue.ToString());
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
            form.Append("min(");
            foreach (IOperation op in _operatorList)
            {
                op.GetForm(ref form, ref varList, ref numberList);
                form.Append(",");
            }
            form.Replace(",", "", form.Length - 1, 1);
            form.Append(")");
        }
        #endregion
    }
}
