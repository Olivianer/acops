using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintOperations;

namespace OKConstraints
{
    /// <summary>
    /// Diese Klasse charakterisiert die Zielfunktion
    /// </summary>
    public class OptimizationConstraint : Constraint
    {
        #region Protected Member
        /// <summary>
        /// Enth�lt die Zielfunktion
        /// </summary>
        protected IOperation _objectiveFunction;
        /// <summary>
        /// Enth�lt den Wert der Schranke. Dieser wird anhand des Ergebnisses der Zielfunktion gesetzt.
        /// </summary>
        protected ObjCritOperator _objCritOp;
        #endregion

        #region Constructor
        /// <summary>
        /// Der Konstruktor, dem eine KleinerAlsOperation �bergeben wird.
        /// <remarks>
        /// Der rechte Oparator enth�lt die komplette Zielfunktion, aber es muss eine Maximum-Funktion sein, z.B. max(x1+x2).
        /// Der linke Operator muss vom Typ ObjCritOperator sein, und darf keine Schranke enthalten.
        /// </remarks>
        /// </summary>
        /// <param name="op">Die KleinerAlsOperation</param>
        public OptimizationConstraint(LessThan op) : base(op)
        {
            _name = "objFunction";
            // Pr�fen, ob es sich wirklich um eine g�ltige Operation handelt
            if (op == null)
                throw new Exception("null pointer");
            IOperation[] opList = op.GetOperatorList();
            // Schauen, ob linker Operator vom Typ ObjCritOperator ist
            if (opList[0].GetType() != typeof(ObjCritOperator))
                throw new Exception("left operator has to be a ObjCritOperator");
            _objCritOp = (ObjCritOperator)opList[0];
            _objectiveFunction = opList[1];
        }

        /// <summary>
        /// Der Konstruktor, dem eine Gr��erAlsOperation �bergeben wird.
        /// <remarks>
        /// Der rechte Oparator enth�lt die komplette Zielfunktion, aber es muss eine Minimum-Funktion sein, z.B. min(x1+x2).
        /// Der linke Operator muss vom Typ ObjCritOperator sein, und darf keine Schranke enthalten.
        /// </remarks>
        /// </summary>
        /// <param name="op">Die KleinerAlsOperation</param>
        public OptimizationConstraint(GreaterThan op) : base(op)
        {
            _name = "objFunction";
            // Pr�fen, ob es sich wirklich um eine g�ltige Operation handelt
            if (op == null)
                throw new Exception("null pointer");
            IOperation[] opList = op.GetOperatorList();
            // Schauen, ob linker Operator vom Typ ObjCritOperator ist
            if (opList[0].GetType() != typeof(ObjCritOperator))
                throw new Exception("left operator has to be a ObjCritOperator");
            _objCritOp = (ObjCritOperator)opList[0];
            _objectiveFunction = opList[1];
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt als Wert das Ergebnis der Zielfunktion zur�ck. 
        /// <remarks>Dieser Wert kann dann als obere oder untere Schranke
        /// f�r die Suche nach neuen L�sungen genutzt werden.
        /// </remarks>
        /// </summary>
        public double ObjectiveValue
        {
            get
            {
                if (_objCritOp.HasValue)
                    return _objCritOp.Value;
                else
                {
                    if (_objectiveFunction.GetType() == typeof(Minimum))
                        return double.MaxValue;
                    else if (_objectiveFunction.GetType() == typeof(Maximum))
                        return double.MinValue;
                    else
                        throw new Exception("Operation not supported");
                }
            }
        }

        /// <summary>
        /// Gibt die Zielfunktion zur�ck.
        /// </summary>
        public IOperation ObjectiveFunction
        {
            get
            {
                return _objectiveFunction;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Es wird der Zielfunktionswert gesetzt. Dieser wird bei der Suche nach neuen L�sungen als obere oder
        /// untere Schranke genutzt.
        /// </summary>
        public void SetObjectiveValue()
        {
            _objCritOp.Value = CalcObjFunction();
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Berechnet den Wert der Zielfunktion.
        /// </summary>
        /// <returns></returns>
        protected double CalcObjFunction()
        {
            double result;
            if (_objectiveFunction.DoOperation(out result) != true)
            {
                throw new Exception("Operation is not fullfilled");
            }
            return result;
        }
        #endregion
    }
}
