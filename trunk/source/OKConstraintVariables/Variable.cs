using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace OKConstraintVariables
{
    /// <summary>
    /// Diese Klasse repräsentiert eine Variable innerhalb einer ConstraintConfiguration
    /// </summary>
    public class Variable : IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Enthält den Wertebereich der Variablen.
        /// </summary>
        protected IDomain _domain;
        /// <summary>
        /// Enthält den Namen der Variablen.
        /// </summary>
        protected string _name;
        /// <summary>
        /// Enthält die Belegung der Variablen, also deren festen Wert.
        /// </summary>
        protected double _value = 0.0;
        /// <summary>
        /// Gibt an ob die Variable mit einem Wert belegt wurde.
        /// </summary>
        protected bool _hasValue;
        #endregion

        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        internal Variable()
        {
        }
        /// <summary>
        /// Konstruktor, über den eine Variable definiert wird.
        /// </summary>
        /// <param name="name">Der Name der Variable</param>
        /// <param name="domain">Der Wertebereich der Variablen.</param>
        public Variable(string name, IDomain domain)
        {
            _name = name;
            _domain = domain;
            _hasValue = false;
        }

        /// <summary>
        /// Der Kopierkonstruktor
        /// </summary>
        /// <param name="var"></param>
        public Variable(Variable var)
        {
            _name = var.Name;
            _domain = var._domain.Copy();
            _hasValue = var._hasValue;
            _value = var._value;
            if (_value != 0.0 && _hasValue == false)
                throw new Exception("value is wrong");
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt zurück, ob die Variable mit einem Wert belegt wurde.
        /// </summary>
        public bool HasAsignedValue
        {
            get
            {
                return _hasValue;
            }
        }

        /// <summary>
        /// Gibt den Wert der Variablen zurück, oder setzt diesen.
        /// <remarks>
        /// Wurde die Variable noch nicht mit einem Wert belegt,
        /// dann wird eine Exception geworfen.
        /// Ebenso wird eine Exception geworfen, wenn versucht wird der Variablen
        /// einen Wert zuzuweisen, obwohl die Variable schon belegt wurde.
        /// </remarks>
        /// </summary>
        public double Value
        {
            get
            {
                if (!_hasValue)
                    throw new Exception("No value is assigned");
                return _value;
            }
            set
            {
                if (_hasValue)
                    throw new Exception("value is already assigned");
                _hasValue = true;
                _value = value;
            }
        }

        /// <summary>
        /// Gibt den Namen der
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
        }

        /// <summary>
        /// Gibt den Wertebereich der Variablen zurück.
        /// </summary>
        public IDomain Domain
        {
            get
            {
                return _domain;
            }
            set
            {
                _domain = value;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Die Variable wird wieder freigegeben, d.h. die Belegung wird rückgängig gemacht.
        /// </summary>
        public void ResetValue()
        {
            _hasValue = false;
            _value = 0;
        }

        /// <summary>
        /// Liefert die Informationen der Variablen in Form eines Strings.
        /// </summary>
        /// <returns>Darstellung der Variablen als String.</returns>
        public override string ToString()
        {
            if (_hasValue)
                return _name + ": " + _value.ToString();
            return _name + ": " + "(" + Domain.Min.ToString() + "-" + Domain.Max.ToString() + ")";
        }
        #endregion

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();
            _name = reader.ReadElementContentAsString();
            XmlSerializer xmlSerializer;
            if (reader.Name == "Domain")
            {
                xmlSerializer = new XmlSerializer(typeof(Domain));
                _domain = xmlSerializer.Deserialize(reader) as Domain;
                _hasValue = false;
            }
            else
            {
                _value = reader.ReadElementContentAsDouble();
                _hasValue = true;
            }
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Name", _name);
            XmlSerializer xmlSerializer;
            if (_hasValue == false)
            {
                xmlSerializer = new XmlSerializer(_domain.GetType());
                xmlSerializer.Serialize(writer, _domain);
            }
            else
            {
                xmlSerializer = new XmlSerializer(_value.GetType());
                xmlSerializer.Serialize(writer, _value);
            }
        }

        #endregion
    }
}
