using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse wird bei einem Optimierungsproblem genutzt, um ein obere oder untere Schranke des Zielkriteriums festzulegen.
    /// </summary>
    public class ObjCritOperator : DoubleOperator
    {
        #region Private Member
        /// <summary>
        /// Gibt an, ob eine Schranke festgelegt wurde, sprich, ob der Operator einen Wert enthält.
        /// </summary>
        bool _hasValue = false;
        #endregion

        #region Constructor
        /// <summary>
        /// Der StandardKonstruktor.
        /// </summary>
        public ObjCritOperator(): base(0)
        {
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Liefert oder setzt den Wert der Schranke des Zielkriteriums.
        /// </summary>
        public override double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                _hasValue = true;
            }
        }

        /// <summary>
        /// Gibt an, ob der Wert der Schranke gesetzt wurde.
        /// </summary>
        public bool HasValue
        {
            get
            {
                return _hasValue;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Gibt den Wert der Schranke im Parameter zurück, wenn dieser Wert gesetzt wurde.
        /// </summary>
        /// <param name="result">Liefert den Wert der Schranke</param>
        /// <returns>
        /// Gibt an, ob die Operation durchführbar war.
        /// </returns>
        public override bool DoOperation(out double result)
        {
            if (_hasValue == false)
            {
                result = 0.0;
                return false;
            }
            result = _value;
            return true;
        }
        #endregion
    }
}
