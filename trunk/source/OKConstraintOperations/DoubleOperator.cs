using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repr�sentiert einen Gleitkommawert in einem Constraint.
    /// <remarks>
    /// Die Klasse stellt nur eine H�lle um eine double-Zahl dar, um die Funktion DoOperation verwenden zu k�nnen.
    /// </remarks>
    /// </summary>
    public class DoubleOperator : Operation
    {
        #region Protected Member
        /// <summary>
        /// Der Wert des Operators.
        /// </summary>
        protected double _value;
        #endregion

        #region Constructor
        /// <summary>
        /// Standardkonstruktor f�r XML-Serialisierung
        /// </summary>
        internal DoubleOperator()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem ein Gleitkommawert �bergeben wird
        /// </summary>
        /// <param name="value"></param>
        public DoubleOperator(double value)
        {
            _value = value;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Liefert den Gleitkommawert zur�ck. 
        /// </summary>
        /// <param name="result">Der Gleitkommawert wird zur�ckgegeben.</param>
        /// <returns>Diese Operation ist immer durchf�hrbar (Liefert immer true).</returns>
        public override bool DoOperation(out double result)
        {
            result = _value;
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
            form.Append("n");
            numberList.Add(_value);
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt den Wert des Operators zur�ck, oder setzt diesen
        /// </summary>
        /// <value>Der Wert.</value>
        public virtual double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public override void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();
            _value = reader.ReadElementContentAsDouble();
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Value", _value.ToString());
        }

        #endregion
    }
}

