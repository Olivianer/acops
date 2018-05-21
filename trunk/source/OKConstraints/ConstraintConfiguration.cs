using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;
using System.Xml;
using OKConstraintVariables;
using OKConstraintOperations;

namespace OKConstraints
{
    /// <summary>
    /// Diese Klasse repräsentiert eine Konfiguration, die alle Variablen enthält.
    /// <remarks>
    /// Während der Suche werden pro Knoten im Suchbaum Konfigurationen erstellt.
    /// Am Anfang wird gewöhnlich mit einer Konfiguration begonnen, wo alle Variablen nicht
    /// belegt sind. Für jede Zuweisung einer Variablen wird eine neue Konfiguration erzeugt. 
    /// Pro Ebene im Suchbaum wird immer eine Variable mit einem Wert belegt, so dass am
    /// Ende eine Konfiguration entsteht, wo alle Variablen mit genau einem Wert belegt sind.
    /// Diese Konfiguration stellt dann die Lösung des ConstrainProblems dar.
    /// </remarks>
    /// </summary>
    public class ConstraintConfiguration : IXmlSerializable
    {
        #region Protected Member
        /// <summary>
        /// Enthält alle Variablen und stellt den Zugriff über den Namen bereit
        /// </summary>
        protected Dictionary<string, Variable> _variablesList = new Dictionary<string, Variable>();
        /// <summary>
        /// Enthält alle Konstanten. Konstanten gelten global und ändern sich nicht. 
        /// Deshalb werden sie als static deklariert
        /// </summary>
        protected static Dictionary<string, double> _constantsList = new Dictionary<string, double>();
        ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<ConstraintConfiguration>();
        #endregion

        #region Constructor
        /// <summary>
        /// Der Konstruktor.
        /// </summary>
        public ConstraintConfiguration()
        {
        }

        /// <summary>
        /// Der Kopierkonstruktor.
        /// </summary>
        /// <param name="configuration">Die Konfiguration von der kopiert wird.</param>
        public ConstraintConfiguration(ConstraintConfiguration configuration)
        {
            foreach (Variable var in configuration._variablesList.Values)
            {
                // Eine zugewiesene Variable braucht nicht kopiert werden
                if (var.HasAsignedValue)
                {
                    _variablesList.Add(var.Name, var);
                }
                else
                {
                    Variable newVar = new Variable(var);
                    _variablesList.Add(newVar.Name, newVar);
                }
            }
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Gibt die Variablen als Array zurück
        /// </summary>
        public Variable[] Variables
        {
            get
            {
                Variable[] result = new Variable[_variablesList.Count];
                _variablesList.Values.CopyTo(result, 0);
                return result;
            }
        }

        /// <summary>
        /// Gibt true zurück, wenn alle Variablen mit einem Wert belegt sind.
        /// </summary>
        public bool CompleteAssigned
        {
            get
            {
                foreach (Variable var in _variablesList.Values)
                {
                    if (!var.HasAsignedValue)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Gibt die Konstanten als Array zurück
        /// </summary>
        public static Dictionary<string, double> ConstantList
        {
            get
            {
                return _constantsList;
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Liefert die Anzahl der verfügbaren Werte aller Variablen.
        /// <remarks>
        /// Anhand dieser Eigenschaft kann man Aussagen über den Schwierigkeitsgrad treffen.
        /// </remarks>
        /// </summary>
        /// <returns></returns>
        public double GetCountDomainValues()
        {
            double countDomainValues = 0;
            double counter = 0;
            foreach (Variable var in _variablesList.Values)
            {
                if (!var.HasAsignedValue)
                {
                    countDomainValues -= var.Domain.Count;
                    counter++;
                }
            }
            double result = -counter / countDomainValues * counter;
            return result;
        }

        /// <summary>
        /// Eine Variable wird der Konfiguration hinzugefügt.
        /// </summary>
        /// <param name="var"></param>
        public void AddVariable(Variable var)
        {
            _variablesList.Add(var.Name, var);
        }

        /// <summary>
        /// Liefert die Variable, die den übergebenen Namen trägt.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Variable GetVariable(string name)
        {
            return _variablesList[name];
        }

        /// <summary>
        /// Prüft, ob die übergebene Konfiguration knotenkonsistent ist.
        /// <remarks>
        /// Ein unäres (einstelliges) Constraint c(v) ist knotenkonsistent, wenn das Constraint für alle Belegungen b aus dem Wertebereich der Variablen v erfüllt ist.
        /// Eine Konfiguration ist knotenkonsistent, wenn alle enthaltenen Constraints knotenkonsistent sind.
        /// Anhand dieser Definition finden Beschränkungen im Wertebereich der Variablen von der übergebenen Konfiguration statt.
        /// </remarks>
        /// </summary>
        /// <param name="onlyOneUnassignedVar">
        /// Gibt an, ob nur unäre Constraints überprüft werden sollen,
        /// oder ob auch Constraints überprüft werden, die zwar mehr Variablen enthalten, wo
        /// aber nur eine davon noch nicht belegt ist.
        /// </param>
        /// <param name="constraint"></param>
        /// <returns>Gibt true zurück, wenn die Konfiguration knotenkonsistent ist, sonst false.</returns>
        public bool CheckNodeConsistency(Constraint constraint, bool onlyOneUnassignedVar)
        {
            if (constraint.CountUnAssignedVars != 1 && onlyOneUnassignedVar)
                return true;

            if (constraint.CountUnAssignedVars == 0)
                return true;

            List<double> removeList = new List<double>();
            foreach (string name in constraint.VariableNames)
            {
                removeList.Clear();
                Variable var = GetVariable(name);
                if (!var.HasAsignedValue)
                {
                    //foreach (double value in var.Domain.Values)
                    foreach (int value in var.Domain)
                    {
                        var.Value = value;
                        if (constraint.IsComplied == false)
                        {
                            removeList.Add(var.Value);
                        }
                        var.ResetValue();
                    }

                    //_log.Debug("Start CheckNodeConsistency Remove");
                    foreach (double removeValue in removeList)
                    {
                        var.Domain.Remove(removeValue);
                    }

                    //_log.Debug("End CheckNodeConsistency Remove");

                    // if the size of the domain is empty the constraint configuration has no node consistency
                    if (var.Domain.Count == 0)
                        return false;

                    // we found the unassigned variable, so we can go out
                    if (onlyOneUnassignedVar)
                        return true;
                }
            }
            return true;
        }

        /*public bool CheckLimitConsistency(Constraint constraint)
        {
            if (constraint.CountUnAssignedVars != 1)
                return true;

            List<int> removeList = new List<int>();
            
            foreach (string name in constraint.VariableNames)
            {
                int min = int.MinValue;
                int max = int.MinValue;
                removeList.Clear();
                Variable var = GetVariable(name);

                List<int> valueList = (List<int>)var.Domain.Values;

                if (!var.HasAsignedValue)
                {
                    int i;
                    for (i = 0; i < valueList.Count; i++)
                    {
                        var.Value = valueList[i];
                        if (constraint.IsComplied == false)
                        {
                            min = valueList[i];
                        }
                        else
                        {
                            var.ResetValue();
                            break;
                        }
                        var.ResetValue();
                    }

                    for (i = valueList.Count - 1; i >= 0; i--)
                    {
                        var.Value = valueList[i];
                        if (constraint.IsComplied == false)
                        {
                            max = valueList[i];
                        }
                        else
                        {
                            var.ResetValue();
                            break;
                        }
                        var.ResetValue();
                    }
                    if (min > max && max != int.MinValue)
                    {
                        var.Domain.Clear();
                    }
                    else
                    {
                        if (min != int.MinValue)
                            var.Domain.Min = min+1;
                        if (max != int.MinValue)
                            var.Domain.Max = max-1;
                    }

                    // if the size of the domain is empty the constraint configuration has no node consistency
                    if (var.Domain.Count == 0)
                        return false;

                    // we found the unassigned variable, so we can go out
                    //return true;
                }
            }
            return true;
        }*/

        /// <summary>
        /// Reduce the domain from variables which are depending on this constraint
        /// If the domain of one of this varibles 0, the function return false
        /// (Vorabübergabe, or Constraint Propagation with deep 1 or 1-konsistent or nodekonsistent) 
        /// </summary>
        /// <param name="constraint">Das Constraint</param>
        /// <param name="changing">Gibt True zurück, wenn die Konfiguration verändert wurde.</param>
        /// <returns></returns>
        /// 
        public bool CheckEdgeConsistency(Constraint constraint, ref bool changing)
        {
            //if (constraint.CountUnAssignedVars != 2 )
            //    return true;

            List<double> removeList = new List<double>();
            bool valueIsNotPossible = false;

            Variable[] unAssignedVars = constraint.GetUnAssignedVars();

            //_log.Debug("CheckEdgeConsistency for" + unAssignedVars[0].Name + " and " + unAssignedVars[1].Name);
            //_log.Debug("Starting Situation");
            //_log.Debug(this.ToString());

            // arc consistency for the first variable
            foreach (double value1 in unAssignedVars[0].Domain)
            {
                unAssignedVars[0].ResetValue();
                unAssignedVars[0].Value = value1;

                foreach (double value2 in unAssignedVars[1].Domain)
                {
                    valueIsNotPossible = true;
                    unAssignedVars[1].ResetValue();
                    unAssignedVars[1].Value = value2;
                    if (constraint.IsComplied == true)
                    {
                        valueIsNotPossible = false;
                        break;
                    }
                }

                if (valueIsNotPossible == true)
                {
                    changing = true;
                    removeList.Add(value1);
                }
            }

            //_log.Debug("Start CheckEdgeConsistency1 Remove");

            foreach (double value in removeList)
            {
                unAssignedVars[0].Domain.Remove(value);
            }

            //_log.Debug("End CheckEdgeConsistency1 Remove");

            unAssignedVars[0].ResetValue();
            unAssignedVars[1].ResetValue();

            //_log.Debug("Middle Situation");
            //_log.Debug(this.ToString());

            // if the size of the domain is empty the constraint configuration has no arc consistency
            if (unAssignedVars[0].Domain.Count == 0)
                return false;

            // clear the list of the remove values from unAssignedVars[0]
            removeList.Clear();
            
            // arc consistency for the first variable
            foreach (double value2 in unAssignedVars[1].Domain)
            {
                unAssignedVars[1].ResetValue();
                unAssignedVars[1].Value = value2;

                foreach (double value1 in unAssignedVars[0].Domain)
                {
                    valueIsNotPossible = true;
                    unAssignedVars[0].ResetValue();
                    unAssignedVars[0].Value = value1;
                    if (constraint.IsComplied == true)
                    {
                        valueIsNotPossible = false;
                        break;
                    }
                }

                if (valueIsNotPossible == true)
                {
                    changing = true;
                    removeList.Add(value2);
                }
            }

            //_log.Debug("Start CheckEdgeConsistency2 Remove");

            foreach (double value in removeList)
            {
                unAssignedVars[1].Domain.Remove(value);
            }

            //_log.Debug("End CheckEdgeConsistency2 Remove");

            unAssignedVars[0].ResetValue();
            unAssignedVars[1].ResetValue();

            //_log.Debug("End Situation");
            //_log.Debug(this.ToString());

            // if the size of the domain is empty the constraint configuration has no arc consistency
            if (unAssignedVars[1].Domain.Count == 0)
                return false;

            return true;
        }

        /// <summary>
        /// Es wird die Konfiguration als String dargestellt.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string output = "";
            foreach (Variable var in _variablesList.Values)
            {
                output += var.ToString() + " ";
            }
            return output;
        }

        /// <summary>
        /// Die Konfiguration wird als String auf die Konsole geschrieben
        /// </summary>
        public void Show()
        {
            //System.Console.WriteLine(ToString());
            //Trace.WriteLine(ToString());
            Debug.WriteLine(ToString());
        }

        /// <summary>
        /// Prüft, ob die Konfiguration eine Variable mit diesem Namen besitzt
        /// </summary>
        /// <param name="name">Der Name der Variable.</param>
        /// <returns>Gibt true zurück, wenn eine solche Variable exisitert</returns>
        public bool ContainsVariable(string name)
        {
            return _variablesList.ContainsKey(name);
        }

        /// <summary>
        /// Der AC3-Algorithmus zur Herstellung der Kantenkonsistenz
        /// </summary>
        /// <param name="problem">Das Suchproblem</param>
        /// <returns>Gibt true zurück, wenn eine Kantenkonsistenz vorliegt.</returns>
        public bool AC3(ConstraintProblem problem)
        {
            Logger.LogDebug("Start AC3");
            List<Constraint> arcConstraintList = new List<Constraint>();
            // Die Constraints werden doppelt gehalten, in einer Queue und in einem Dictionary für schnellere Kontrolle, ob das Constraint schon in der List ist.
            Queue<Constraint> queue = new Queue<Constraint>();
            Dictionary<int, Constraint> dictConstraints = new Dictionary<int,Constraint>();

            // Test für Performance
            int counter = 0;
     
            foreach (Constraint c in problem.Constraints.GetConstraints())
            {
                if (c.CountUnAssignedVars == 2)
                {
                    queue.Enqueue(c);
                    dictConstraints.Add(c.GetHashCode(),c);
                    arcConstraintList.Add(c);
                }
            }

            while (queue.Count != 0)
            {
                counter++;
                Constraint currentConstraint = queue.Dequeue();
                dictConstraints.Remove(currentConstraint.GetHashCode());
                Variable[] unassingedVars = currentConstraint.GetUnAssignedVars();
                bool firstResult = Revise(unassingedVars[0], unassingedVars[1], currentConstraint);
                bool secondResult = Revise(unassingedVars[1], unassingedVars[0], currentConstraint);

                if (firstResult == true && secondResult == true)
                {
                    // Prüfen, ob die Domain schon leer ist
                    if (unassingedVars[0].Domain.Count == 0 || unassingedVars[1].Domain.Count == 0)
                    {
                        return false;
                    }

                    // Hinzufügen der nötigen Constraints zur Queue
                    foreach (Constraint arcConstraint in arcConstraintList)
                    {
                        Variable[] arcVars = arcConstraint.GetUnAssignedVars();
                        if (arcVars[0] == unassingedVars[0] || arcVars[1] == unassingedVars[0])
                        {
                            if (dictConstraints.ContainsKey(arcConstraint.GetHashCode()) == false)
                            {
                                queue.Enqueue(arcConstraint);
                                dictConstraints.Add(arcConstraint.GetHashCode(), arcConstraint);
                            }
                        }
                        else if (arcVars[0] == unassingedVars[1] || arcVars[1] == unassingedVars[1])
                        {
                            if (dictConstraints.ContainsKey(arcConstraint.GetHashCode()) == false)
                            {
                                queue.Enqueue(arcConstraint);
                                dictConstraints.Add(arcConstraint.GetHashCode(), arcConstraint);
                            }
                        }
                    }
                }
                else if (firstResult == true)
                {
                    // Prüfen, ob die Domain schon leer ist
                    if (unassingedVars[0].Domain.Count == 0)
                    {
                        return false;
                    }

                    // Hinzufügen der nötigen Constraints zur Queue
                    foreach (Constraint arcConstraint in arcConstraintList)
                    {
                        Variable[] arcVars = arcConstraint.GetUnAssignedVars();
                        if (arcVars[0] == unassingedVars[0] || arcVars[1] == unassingedVars[0])
                        {
                            if (dictConstraints.ContainsKey(arcConstraint.GetHashCode()) == false)
                            {
                                queue.Enqueue(arcConstraint);
                                dictConstraints.Add(arcConstraint.GetHashCode(), arcConstraint);
                            }
                        }
                    }
                }
                else if (secondResult == true)
                {
                    // Prüfen, ob die Domain schon leer ist
                    if (unassingedVars[1].Domain.Count == 0)
                    {
                        return false;
                    }

                    // Hinzufügen der nötigen Constraints zur Queue
                    foreach (Constraint arcConstraint in arcConstraintList)
                    {
                        Variable[] arcVars = arcConstraint.GetUnAssignedVars();
                        if (arcVars[0] == unassingedVars[1] || arcVars[1] == unassingedVars[1])
                        {
                            if (dictConstraints.ContainsKey(arcConstraint.GetHashCode()) == false)
                            {
                                queue.Enqueue(arcConstraint);
                                dictConstraints.Add(arcConstraint.GetHashCode(), arcConstraint);
                            }
                        }
                    }
                }
            }

            Logger.LogDebug("End Ac3: Counter is " + counter.ToString());

            return true;
        }

        /// <summary>
        /// Schränkt den Wertebereich der zweiten Variablen anhand eines Constraints ein.
        /// </summary>
        /// <param name="first">erste Variable</param>
        /// <param name="second">zweite Variable</param>
        /// <param name="constraint">das Constraint</param>
        /// <returns>Liefert True, wenn der Wertebereich verändert wurde.</returns>
        static public bool Revise(Variable first, Variable second, Constraint constraint)
        {
            bool result = false;
            List<double> removeList = new List<double>();
            bool feasible = false;
            foreach (double value1 in first.Domain)
            {
                feasible = false;
                first.Value = value1;
                foreach (double value2 in second.Domain)
                {
                    second.Value = value2;
                    if (constraint.IsComplied == true)
                    {
                        feasible = true;
                        second.ResetValue();
                        break;
                    }
                    second.ResetValue();
                }
                if (feasible == false)
                    removeList.Add(value1);

                first.ResetValue();
            }
            if (removeList.Count > 0)
            {
                result = true;

                //_log.Debug("Start Revise Remove");

                foreach (double value in removeList)
                {
                    first.Domain.Remove(value);
                }

                //_log.Debug("End Revise Remove");
            }
            return result;
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Variable));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Variable var = xmlSerializer.Deserialize(reader) as Variable;
                AddVariable(var);
                // Es wird hier für jede Variable gleich der VariablenOperator angelegt
                VariablesOperator.VarOperatorList.Add(var.Name, new VariablesOperator(var.Name));
            }
            reader.Read();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Variable));

            foreach (Variable var in _variablesList.Values)
            {
                xmlSerializer.Serialize(writer, var);
            }
        }

        #endregion
    }
}
