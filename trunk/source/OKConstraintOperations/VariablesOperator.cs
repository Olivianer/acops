using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse repräsentiert eine Variable innerhalb eines Constraints.
    /// <remarks>
    /// Es handelt sich hierbei mehr um eine Klasse, die eine Variable ummantelt.
    /// Hintergrund dabei ist, dass der VariablenOperator nicht kopiert werden braucht, sonder immer nur
    /// mit der Variable der aktuellen Konstellation arbeitet.
    /// </remarks>
    /// </summary>
    public class VariablesOperator : Operation, IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Enthält die Variable, mit der dieser Operator gerade arbeitet.
        /// </summary>
        protected Variable _var = null;
        /// <summary>
        /// Enthält den Namen des VariablenOperators, der mit dem Namen der Variablen übereinstimmt. 
        /// </summary>
        protected string _name;
        /// <summary>
        /// Während des XML-Deserialisierens werden sich in diesem Dictionary die VariablenOperatoren gemerkt
        /// todo: Achtung, das muss geändert werden, falls mehrere Probleme gleichzeitig deserialisiert werden sollen
        /// </summary>
        protected static Dictionary<string, VariablesOperator> _varOperatorList = new Dictionary<string, VariablesOperator>();
        #endregion

        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        internal VariablesOperator()
        {
        }
        /// <summary>
        /// Der Konstruktor, dem der Name übergeben wird.
        /// </summary>
        /// <param name="name"></param>
        public VariablesOperator(string name)
        {
            _name = name;
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Liefert den Zugriff auf die aktuelle Variable, oder setzt diese.
        /// </summary>
        public Variable Var
        {
            get
            {
                return _var;
            }
            set
            {
                _var = value;
            }
        }

        /// <summary>
        /// Gibt den Namen des VariablenOperators zurück
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Während des XML-Deserialisierens werden sich in diesem Dictionary die VariablenOperatoren gemerkt
        /// todo: Achtung, das muss geändert werden, falls mehrere Probleme gleichzeitig deserialisiert werden sollen
        /// </summary>
        public static Dictionary<string, VariablesOperator> VarOperatorList
        {
            get
            {
                return _varOperatorList;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Liefert den Wert der Belegung der aktuellen Variable, wenn die Variable belegt wurde.
        /// </summary>
        /// <param name="result">Liefert den Wert der Belegung der aktuellen Variable.</param>
        /// <returns>Liefert true, wenn der Variablen ein fester Wert zugewiesen wurde, sonst false.</returns>
        public override bool DoOperation(out double result)
        {
            if (!_var.HasAsignedValue)
            {
                result = 0.0;
                return false;
            }

            result = _var.Value;
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
            form.Append("v");
            if (_var == null)
                throw new Exception("the variable was not set for this variablesoperator");
            varList.Add(_var);
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
            _name = reader.ReadElementContentAsString();
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public override void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Name", _name);
        }

        #endregion
    }
}
