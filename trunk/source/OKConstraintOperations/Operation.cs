using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Reflection;
using OKConstraintVariables;

namespace OKConstraintOperations
{
    /// <summary>
    /// Diese Klasse stellt eine abstrakte Operation dar. Diese enthält eine Liste von Operatoren.
    /// </summary>
    public class Operation : IOperation, IXmlSerializable 
    {
        /// <summary>
        /// Enthält alle Operatoren
        /// </summary>
        protected IOperation[] _operatorList;

        /// <summary>
        /// Gibt die Liste der Operatoren als Array zurück.
        /// </summary>
        /// <returns>Die Operatoren als Array.</returns>
        public IOperation[] GetOperatorList()
        {
            if (_operatorList == null)
            {
                _operatorList = new IOperation[0];
            }
            return _operatorList;
        }

        /// <summary>
        /// Es wird versucht, die Operation auszuführen.
        /// <remarks>
        /// Das Ergebnis wird in dem Parameter result zurückgegeben. Ist es nicht möglich diese Operation
        /// durchzuführen, weil eine Variable noch nicht mit einem Wert belegt ist wird false zurückgeliefert.
        /// </remarks>
        /// </summary>
        /// <param name="result">In diesem Parameter wird das Ergebnis gespeichert</param>
        /// <returns>
        /// Es wird false zurückgegeben, wenn die Operation nicht durchführbar ist
        /// </returns>
        public virtual bool DoOperation(out double result)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Gibt die Form der operation zurück
        /// <example>
        /// 5*x = 7 hat die Form =(*(n,v),n)
        /// </example>	
        /// </summary>
        /// <param name="form">Gibt die Form zurück</param>
        /// <param name="varList">Enthält alle Variablen der Form</param>
        /// <param name="numberList">Enthält alle Nummern der Form</param>
        public virtual void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Es wird ein Operator hinzugefügt.
        /// </summary>
        /// <param name="operation"></param>
        public void AddOperator(IOperation operation)
        {
            if (_operatorList == null)
                _operatorList = new IOperation[0];

            // Erweitern des Arrays etwas umständlich, aber wird ja nur während des Kompilierens gemacht
            IOperation[] _newOperatorList = new IOperation[_operatorList.GetLength(0) + 1];
            for (int i = 0; i < _operatorList.GetLength(0); i++)
            {
                _newOperatorList[i] = _operatorList[i];
            }

            _newOperatorList[_newOperatorList.GetLength(0) - 1] = operation;
            _operatorList = _newOperatorList;
        }

        #region IXmlSerializable Members

        /// <summary>
        /// This method is reserved and should not be used. When implementing the IXmlSerializable interface, you should return null (Nothing in Visual Basic) from this method, and instead, if specifying a custom schema is required, apply the <see cref="T:System.Xml.Serialization.XmlSchemaProviderAttribute"/> to the class.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Xml.Schema.XmlSchema"/> that describes the XML representation of the object that is produced by the <see cref="M:System.Xml.Serialization.IXmlSerializable.WriteXml(System.Xml.XmlWriter)"/> method and consumed by the <see cref="M:System.Xml.Serialization.IXmlSerializable.ReadXml(System.Xml.XmlReader)"/> method.
        /// </returns>
        public virtual System.Xml.Schema.XmlSchema GetSchema()
        {
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public virtual void ReadXml(System.Xml.XmlReader reader)
        {
            reader.Read();

            Assembly assembly = Assembly.GetExecutingAssembly();

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                string operation = reader.Name;
                Type type = assembly.GetType(assembly.GetName().Name + "." + operation);

                // Ausnahme beim VariablenOperator
                if (type == typeof(VariablesOperator))
                {
                    VariablesOperator op;
                    reader.Read();
                    string varName = reader.ReadElementContentAsString();
                    reader.Read();
                    if (!VariablesOperator.VarOperatorList.TryGetValue(varName, out op))
                    {
                        op = new VariablesOperator(varName);
                        VariablesOperator.VarOperatorList.Add(varName, op);
                    }
                    AddOperator(op);
                }
                else
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(type);
                    IOperation op = xmlSerializer.Deserialize(reader) as IOperation;
                    AddOperator(op);
                }
            }

            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public virtual void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (IOperation op in GetOperatorList())
            {
                XmlSerializer xmlSerializer = new XmlSerializer(op.GetType());
                xmlSerializer.Serialize(writer, op);
            }
        }

        #endregion
    }
}
