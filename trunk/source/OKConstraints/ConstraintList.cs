using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;

namespace OKConstraints
{   
    /// <summary>
    /// Diese Klasse verwaltet die Constraints bzw. Randbedingungen eines ConstraintProblems.
    /// <remarks>
    /// Alle Constraints werden doppelt gehalten, aber dafür ist der Zugriff schneller.
    /// </remarks>
    /// </summary>
    public class ConstraintList : IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Enthält alle Constraints in einem Dictionary, wobei der Zurgiff über den Namen der Variable geschieht.
        /// <remarks>
        /// Es ist so einfach, alle Constraints zu bekommen, in denen eine bestimmte Variable vorkommt.
        /// </remarks>
        /// </summary>
        protected Dictionary<string, List<Constraint> > _constraints = new Dictionary<string,List<Constraint>>();
        /// <summary>
        /// Enthält alle Constraints in einer Liste.
        /// </summary>
        protected List<Constraint> _constraintList = new List<Constraint>();
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt zurück, ob die ConstraintList leer ist.
        /// </summary>
        public bool Empty
        {
            get
            {
                if (_constraints.Count > 0)
                    return true;
                return false;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Es wird ein Constraint hinzugefügt.
        /// </summary>
        /// <remarks>
        /// Ein Constraint wird mehrmals hinzugefügt, wenn mehrere Variablen in dem Constraint vorkommen.
        /// </remarks>
        /// <param name="varName">Der Name der Variablen, die in dem Constraint vorkommt.</param>
        /// <param name="constraint">Das Constraint was hinzugefügt werden soll.</param>
        public void Add(string varName, Constraint constraint)
        {
            if (!_constraints.ContainsKey(varName))
            {
                _constraints[varName] = new List<Constraint>();
            }
            _constraints[varName].Add(constraint);
            if (!_constraintList.Contains(constraint))
                _constraintList.Add(constraint);
        }

        

        /// <summary>
        /// Liefert eine Liste von Constraints, in denen die Variable vorkommt.
        /// </summary>
        /// <param name="variableName">Der Name der Variablen, die in den Constraints vorkommt</param>
        /// <returns>Gibt eine Liste von Constraints zurück.</returns>
        public List<Constraint> GetConstraints(string variableName)
        {
            if (!_constraints.ContainsKey(variableName))
                return new List<Constraint>();
            return _constraints[variableName];
        }

        /// <summary>
        /// Liefert alle Constraints eines ConstraintProblems zurück.
        /// </summary>
        /// <returns></returns>
        public List<Constraint> GetConstraints()
        {
            return _constraintList;
        }
        #endregion

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer xmlConstraintSerializer = new XmlSerializer(typeof(Constraint));

            foreach (Constraint constraint in _constraintList)
            {
                xmlConstraintSerializer.Serialize(writer, constraint);
            }

            // Serialize each BizEntity this collection holds
            //foreach (string key in this.Dictionary.Keys)
            //{
            //    Serializer.Serialize(writer, this.Dictionary[key]);
            //}
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Constraint));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Constraint constraint = xmlSerializer.Deserialize(reader) as Constraint;
                foreach (string varName in constraint.VariableNames)
                {
                    Add(varName, constraint);
                }
            }
        }

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public XmlSchema GetSchema()
        {
            return null;
        }
    }
}
