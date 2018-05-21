using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Reflection;
using System.Xml;
using OKConstraintVariables;
using OKConstraintOperations;
using Microsoft.Extensions.Logging;

namespace OKConstraints
{
    /// <summary>
    /// Diese Klasse repr�sentiert ein Constraint bzw. eine Randbedingung.
    /// <example>
    /// x1 + x2 = 7
    /// </example>
    /// </summary>
    public class Constraint : IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Jedes Constraint enth�lt nur eine Operation. Diese kann aber wiederum aus mehreren Operationen bestehen.
        /// <remarks>
        /// Bei dem Constraint x1 + x2 = 7 w�re die Operation =. Der linke Operand w�re wiederum eine Operaton n�mlich +.
        /// </remarks>
        /// </summary>
        protected IOperation _operation;
        /// <summary>
        /// Diese Liste enth�lt alle Namen von den Variablen, die in diesem Constraint vorkommen. 
        /// </summary>
        protected List<string> _variableNames = new List<string>();
        /// <summary>
        /// Diese Liste enth�lt alle im Constraint verwendeten Variablenoperatoren nocheinmal separat.
        /// <remarks>
        /// Dadurch gelingt es schneller f�r jeden VariablenOperator die Variable auszutauschen.
        /// </remarks>
        /// </summary>
        protected List<VariablesOperator> _varOperators = new List<VariablesOperator>();
        /// <summary>
        /// todo: Name wird bis jetzt nur bei objFunction genutzt. Dies muss umgestellt werden.
        /// Name k�nnte die Reihenfolge der Eingabe wiederspiegeln, wie z.B. 1.Constraint,
        /// </summary>
        protected String _name;
        /// <summary>
        /// Zeigt an, ob dieses Constraint f�r die Pr�fung der Grenzenkonsistenz genutzt werden kann
        /// </summary>
        protected bool _boundsConsistencyPossible = true;
        /// <summary>
        /// Wird genutzt, um einen eindeutigen Wert f�r das Constraint zu nutzen -> bei GetHashCode()
        /// </summary>
        protected Guid _uniqueId;
        ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<Constraint>();
        #endregion

        #region Constructor
        /// <summary>
        /// Standard Constructor, wird gebraucht f�r Xml-Serialisation
        /// </summary>
        internal Constraint()
        {
            _uniqueId = Guid.NewGuid();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="op">the operation of the constraint</param>
        public Constraint(IOperation op)
        {
            _uniqueId = Guid.NewGuid();
            _operation = op;
            _boundsConsistencyPossible = true;
            FillVariableInformation();
        }
        #endregion

        #region Internal Member
        /// <summary>
        /// todo: Das System der boundsConsistency muss �berdacht werden.
        /// </summary>
        internal bool BoundsConsistencyPossible
        {
            get
            {
                return _boundsConsistencyPossible;
            }
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt true zur�ck, wenn alle im Constraint verwendeten Variablen mit einem festen Wert belegt sind.
        /// </summary>
        public bool FullAssigned
        {
            get
            {
                foreach (VariablesOperator varOperator in _varOperators)
                {
                    if (varOperator.Var.HasAsignedValue == false)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Gibt die Anzahl der nicht belegten Variablen zur�ck. 
        /// </summary>
        public int CountUnAssignedVars
        {
            get
            {
                int counter = 0;
                foreach (VariablesOperator varOperator in _varOperators)
                {
                    if (varOperator.Var.HasAsignedValue == false)
                        counter++;
                }
                return counter;
            }
        }

        /// <summary>
        /// Gibt zur�ck, ob dieses Constraint bzw. diese Randbedingung erf�llt ist.
        /// <remarks>
        /// Es wird gepr�ft, ob die zugrundeliegende Operation erf�llt ist.
        /// </remarks>
        /// <example>
        /// 3 = 5 ist nicht erf�llt
        /// </example>
        /// </summary>
        public bool IsComplied
        {
            get
            {
                double result;
                if (_operation.DoOperation(out result) == true)
                {
                    if (result == 1.0)
                        return true;
                    else
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Liefert die Namensliste der Variablen, die in diesem Constraint verwendet werden.
        /// </summary>
        public List<string> VariableNames
        {
            get
            {
                return _variableNames;
            }
        }

        /// <summary>
        /// Gets the operation of the constraint 
        /// </summary>
        public IOperation Operation
        {
            get
            {
                return _operation;
            }
            set
            {
                _operation = value;
                FillVariableInformation();
            }
        }
        #endregion

        #region Internal Functions
        /// <summary>
        /// todo: Das System der boundsConsistency muss �berdacht werden.
        /// </summary>
        internal void forbidBoundsConsistency()
        {
            _boundsConsistencyPossible = false;
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// todo: Das System der boundsConsistency muss �berdacht werden.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="varList"></param>
        /// <param name="numberList"></param>
        public void GetForm(ref StringBuilder form, ref List<Variable> varList, ref List<double> numberList)
        {
            _operation.GetForm(ref form, ref varList, ref numberList);
        }

        /// <summary>
        /// Das Constraint wird mit den Variablen und  Werten der �bergebenen Konfiguration initialisiert. 
        /// </summary>
        /// <remarks>
        /// Die Randbedingung geh�rt nicht zu einer Konfiguration. Das erspart deren Kopieren bei Erstellung einer
        /// neuen Konfiguration. Stattdessen wird die Randbedingung vor deren Pr�fung mit der zu pr�fenden Konfiguration
        /// initialisier.
        /// </remarks>
        /// <param name="configuration">Mit dieser Konfiguration wird die Randbedingung initialisiert.</param>
        public void SetCurrentConfiguration(ConstraintConfiguration configuration)
        {
            foreach (VariablesOperator varOperator in _varOperators)
            {
                varOperator.Var = configuration.GetVariable(varOperator.Name);
            }
        }

        /// <summary>
        /// Ein VariablenOperator wird dem Constraint hinzugef�gt.
        /// <remarks>
        /// Es wird nur ein VariablenOperator hinzugef�gt, 
        /// wenn dieser nicht schon hinzugef�gt wurd (wird anhand des gleichen Namens erkannt)
        /// </remarks>
        /// </summary>
        /// <param name="varOperator"></param>
        private void AddVariablesOperator(VariablesOperator varOperator)
        {
            if (!_variableNames.Contains(varOperator.Name))
            {
                _variableNames.Add(varOperator.Name);
                _varOperators.Add(varOperator);
            }
        }

        /// <summary>
        /// Liefert alle Variablen, die noch nicht belegt wurden.
        /// </summary>
        /// <returns>Die nicht belegten Variablen</returns>
        public Variable[] GetUnAssignedVars()
        {
            Variable[] result = new Variable[CountUnAssignedVars];
            int counter = 0;
            foreach (VariablesOperator varOperator in _varOperators)
            {
                if (varOperator.Var.HasAsignedValue == false)
                {
                    result[counter] = varOperator.Var;
                    counter++;
                }
            }
            return result;
        }

        void FillVariableInformation()
        {
            _variableNames.Clear();
            _varOperators.Clear();
            FillVariableInformation(_operation);
        }

        void FillVariableInformation(IOperation operation)
        {
            if (_operation == null)
                return;

            if (operation.GetType() == typeof(VariablesOperator))
            {
                VariablesOperator variablesOperator = (VariablesOperator)operation;
                AddVariablesOperator(variablesOperator);
            }

            foreach(IOperation childOperation in operation.GetOperatorList())
            {
                FillVariableInformation(childOperation);
            }
        }


        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        public override int GetHashCode()
        {
 	        return this._uniqueId.GetHashCode();
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

            Assembly assembly = Assembly.GetAssembly(typeof(IOperation));

            try
            {
                string operation = reader.Name;
                Type type = assembly.GetType(assembly.GetName().Name + "." + operation);
                // Operationen holen
                XmlSerializer xmlSerializer = new XmlSerializer(type);
                _operation = xmlSerializer.Deserialize(reader) as IOperation;
                reader.Read();
                // Informationen 
                FillVariableInformation();
                // Beispiel f�r Erzeugung einer Instanz �ber Reflektion
                //object ob = Activator.CreateInstance(assemblyName, typeof(Or).FullName);
                //object ob = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                Logger.LogCritical("ReadXML throws an exception: ", ex);
            }
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteElementString("Name", _name);
            if (_operation != null)
            {
                XmlSerializer xmlSerializer = new XmlSerializer(_operation.GetType());
                xmlSerializer.Serialize(writer, _operation);
            }
        }

        #endregion
    }
}
