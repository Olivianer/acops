using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repr�sentiert einen Gleitkommawert f�r eine lokale Variable wie z.B. bei "if (i=j)"
    /// </summary>
    public class LocalDoubleOperator : DoubleOperator
    {
        #region Private Member
        /// <summary>
        /// Der Name der lokalen Variablen
        /// </summary>
        string _localVariableName = "";
        #endregion

        #region Constructor
        /// <summary>
        /// Der Konstruktor, dem ein Gleitkommawert �bergeben wird
        /// </summary>
        /// <param name="localVariableName">Der Name der lokalen Variable</param>
        /// <param name="value">Der Wert der lokalen Variable</param>
        public LocalDoubleOperator(string localVariableName, double value)
            : base(value)
        {
            _localVariableName = localVariableName;
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt den Namen der lokalen Variablen zur�ck, oder setzt diesen
        /// </summary>
        /// <value>Der Name der lokalen Variable.</value>
        public string LocalVariableName
        {
            get
            {
                return _localVariableName;
            }
            set
            {
                _localVariableName = value;
            }
        }
        #endregion
    }
}
