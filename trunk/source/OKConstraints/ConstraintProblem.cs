using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;
using OKHeuristicSearchRoom;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Xml.Schema;
using OKConstraintVariables;
using OKConstraintOperations;
using System.Reflection;

namespace OKConstraints
{
    /// <summary>
    /// Die Klasse dient der Beschreibung eines ConstraintProblem
    /// <remarks>
    /// Unter einem Constraintproblem kann man sich folgendes vorstellen. 
    /// Es ist eine Reihe von Variablen gegeben. 
    /// Für jede Variable existiert ein Wertebereich, der oft auch als Domäne oder Domain bezeichnet wird. 
    /// Zusätzlich existieren Randbedingungen, sogenannte Constraints, die die Wertzuweisung der Variablen einschränken. 
    /// </remarks>
    /// </summary>
    public class ConstraintProblem : IHeuristicSearchProblem, IXmlSerializable
    {
        #region Potected Member
        /// <summary>
        /// Enthält die Constraints bzw. Randbedingungen des Problems
        /// </summary>
        protected ConstraintList _constraintList = new ConstraintList();
        /// <summary>
        /// Der Startknoten
        /// </summary>
        protected INode _begin = null;
        /// <summary>
        /// Die Knoten mit den Lösungen
        /// </summary>
        protected List<Node> _solutionList = new List<Node>();
        /// <summary>
        /// Anzahl der Lösungen, die gefunden werden sollen, bei 0 werden alle Lösungen gesucht.
        /// </summary>
        protected int _countSolutions = 1;
        /// <summary>
        /// Zeigt auf die verwendete Werte-Heuristik
        /// </summary>
        protected IHeuristicValue _heuristicValue = new CountDomainValueHeuristic();
        /// <summary>
        /// Zeigt auf die verwendete VariablenAuswahl-Heuristik
        /// </summary>
        protected IHeuristicVariable _heuristicVariable = new MinimumRemainingValuesHeuristic();
        /// <summary>
        /// Enthält den verwendeten Typ der Domäne
        /// </summary>
        protected DomainType _domainType = DomainType.ValueDomain;
        /// <summary>
        /// Enthält den verwendeten Typ der Knoten
        /// </summary>
        protected NodeType _nodeType = NodeType.Node;
        /// <summary>
        /// Enthält die eingestellten Optionen, die bei der Suche berücksichtigt werden.
        /// </summary>
        protected ConsistencyOptions _consistencyOptions = new ConsistencyOptions();
        protected static string _xmlVersion =  Assembly.GetExecutingAssembly().GetName().Version.ToString();

        protected virtual ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<ConstraintProblem>();

        //protected Random _random = new Random(0);

        #endregion

        #region Constructor
        /// <summary>
        /// Standardkonstruktor für XML-Serialisierung
        /// </summary>
        public ConstraintProblem()
        {
        }
        /// <summary>
        /// Der Konstruktor.
        /// <remarks>
        /// Dem Konstruktor wird eine AusgangsKonfiguration übergeben, die alle Variablen und deren Wertebereiche enthält.
        /// Diese Variablen sollen dann mit festen Werten belegt werden.
        /// </remarks>
        /// </summary>
        /// <param name="configuration">AusgangsKonfiguration</param>
        public ConstraintProblem(ConstraintConfiguration configuration)
        {
            _begin = CreateNode(null, configuration);
            ConstraintForms.Init();
        }
        #endregion

        #region Public Member
        /// <summary>
        /// Liefert den Zugriff auf die Constraints bzw. Randbedingungen des Problems
        /// </summary>
        public ConstraintList Constraints
        {
            get
            {
                return _constraintList;
            }
        }

        /// <summary>
        /// Liefert die ZielKonfiguration ummantelt in einem Suchraumknoten.
        /// </summary>
        public List<Node> SolutionList
        {
            get { return _solutionList; }
        }

        /// <summary>
        /// Liefert die AnfangsKonfiguration umhüllt in einem Suchraumknoten. Das
        /// zurückgegebene Array enthält also nur einen Knoten.
        /// </summary>
        public INode[] FirstNodes
        {
            get
            {
                INode[] nodes;
                nodes = new INode[1];
                nodes[0] = _begin;
                return nodes;
            }
        }

        /// <summary>
        /// Gibt an, wieviele Lösungen gefunden werden sollen. Bei 0 wird nach allen Lösungen gesucht
        /// </summary>
        public int CountSolutions
        {
            get
            {
                return _countSolutions;
            }
            set
            {
                _countSolutions = Math.Abs(value);
            }
        }

        /// <summary>
        /// Gibt die Optionen bezüglich der Konsistenzalgorithmen zur Steuerung der Suche zurück oder setzt sie.
        /// </summary>
        /// <value>Die Konsistenzoptionen.</value>
        public ConsistencyOptions ConsistencyOptions
        {
            get
            {
                return _consistencyOptions;
            }
            set
            {
                _consistencyOptions = value;
            }
        }

        /// <summary>
        /// Gibt den verwendeten Typ der Domäne zurück oder setzt diesen.
        /// </summary>
        /// <value>Der Typ der Domain.</value>
        public DomainType DomainType
        {
            get
            {
                return _domainType;
            }
            set
            {
                if (_domainType != value)
                {
                    if (_begin != null)
                    {
                        _domainType = value;
                        ConstraintConfiguration configuration= _begin.Data as ConstraintConfiguration;
                        foreach (Variable var in configuration.Variables)
                        {
                            var.Domain = CreateDomain(var.Domain.IntervalList);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gibt den verwendeten Knotentyp zurück, oder setzt diesen.
        /// </summary>
        /// <value>Der Knotentyp.</value>
        public NodeType NodeType
        {
            get
            {
                return _nodeType;
            }
            set
            {
                if (_nodeType != value)
                {
                    _nodeType = value;
                    if (_begin != null)
                    {
                        object obj = _begin.Data;
                        switch (_nodeType)
                        {
                            case NodeType.Node:
                                _begin = new Node(obj);
                                break;
                            case NodeType.TreeNode:
                                _begin = new TreeNode(obj);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gibt die aktuelle Xml-Version zurück. Dies deckt sich mit der Assemblyversion
        /// </summary>
        /// <value>Die Version, z.B. "0.0.2.2000".</value>
        public static string XmlVersion
        {
            get
            {
                return _xmlVersion;
            }
        }
        
        #endregion

        #region Public Functions
        public bool CheckAllConsistency()
        {
            // Ein ConstraintProblem hat nur einen Startknoten
            ConstraintConfiguration startConfiguration = (ConstraintConfiguration)FirstNodes[0].Data;

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Node) == true
                || _consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Arc) == true
                || _consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Bounds) == true)
                SetCurrentConfiguration(startConfiguration);

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Bounds) == true)
            {
                if (CheckBoundsConsistency(startConfiguration) == false)
                    return false;
            }
            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Node) == true)
            {
                if (CheckNodeConsistency(startConfiguration, true) == false)
                    return false;
            }
            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Arc) == true)
            {
                if (CheckArcConsistency(startConfiguration) == false)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Wird aufgerufen, wenn die Suche beginnt.
        /// </summary>
        public virtual void OnStartSearch()
        {
            // Ein ConstraintProblem hat nur einen Startknoten
            ConstraintConfiguration startConfiguration = (ConstraintConfiguration)FirstNodes[0].Data;

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Node) == true
                || _consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Arc) == true
                || _consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Bounds) == true)
                SetCurrentConfiguration(startConfiguration);

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Bounds) == true)
            {
                if (CheckBoundsConsistency(startConfiguration) == false)
                    throw new ConsistencyException("The constraint has no solution, because there doesn't exist a bounds consistency.");
            }
            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Node) == true)
            {
                if (CheckNodeConsistency(startConfiguration, true) == false)
                    throw new ConsistencyException("The constraint has no solution, because there doesn't exist a node consistency.");
            }
            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.Start, ConsistencyType.Arc) == true)
            {
                if (CheckArcConsistency(startConfiguration) == false)
                    throw new ConsistencyException("The constraint has no solution, because there doesn't exist an arc consistency.");
            }
        }
        /// <summary>
        /// Wird nicht genutzt.
        /// </summary>
        public virtual void OnFoundNoneDestination()
        {
        }

        /// <summary>
        /// Diese Methode wird aufgerufen, wenn eine Konfiguration gefunden wurde, wo jeder Variable der
        /// Konfiguration ein Wert zugewiesen wurde und dabei keine Randbedingung verletzt wurde.
        /// <param name="destination">Der Zielknoten, der die Konfiguration ummantelt</param>
        /// <param name="searchMethod">Die Suchmethode wird mit übergeben</param>
        /// <returns>Wird true zurückgegeben, endet die Suche, ansonsten wird weiter gesucht.</returns>
        /// </summary>
        public virtual bool OnFoundDestination(INode destination, ISearchMethod searchMethod)
        {
            // Es muss nicht geprüft werden, ob die Configuration schon als Lösung exisitert,
            // da der Suchraum zu neuen Variablen immer von einer Variablenbelegung ausgeht.
            ConstraintConfiguration configuration = destination.Data as ConstraintConfiguration;
            foreach (Node node in _solutionList)
            {
                ConstraintConfiguration alreadyFoundConfiguration = node.Data as ConstraintConfiguration;
                if (alreadyFoundConfiguration.ToString() == configuration.ToString())
                    return false;
            }

            // Lösung hinzufügen
            _solutionList.Add(destination as Node);
            // Prüfen, ob schon genug Lösungen gefunden worden
            if (_solutionList.Count == _countSolutions)
            {
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// Liefert den Wert der heuristischen Funktion für den übergebenen Knoten.
        ///// </summary>
        ///// <returns>Gibt den Wert der Heuristik zurück.</returns>
        ///// <param name="node">Der Knoten, dessen Heuristik ermittelt werden soll.</param>
        //public virtual double GetHeuristic(INode node)
        //{
        //    ConstraintConfiguration configuration = (ConstraintConfiguration)node.Data;
        //    return configuration.GetCountDomainValues();
        //}

        /// <summary>
        /// Vergleicht die aktuelle Konfiguration mit der ZielKonfiguration. Gibt true
        /// zurück, wenn beide übereinstimmen sonst false.
        /// </summary>
        /// <param name="node"></param>
        public virtual bool CompareNodes(INode node)
        {
            ConstraintConfiguration configuration = (ConstraintConfiguration)node.Data;
            return configuration.CompleteAssigned;
        }

        /// <summary>
        /// Für den übergebenen Knoten werden neue Unterknoten generiert.
        /// </summary>
        /// <param name="node">Anhand des übergebenen Knotens werden neue Unterknoten
        /// generiert.</param>
        /// <param name="maxCount">Beschränkt die Anzahl der Unterknoten.</param>
        /// <returns>
        /// Gibt die Menge der generierten Knoten als Array zurück.
        /// </returns>
        public virtual INode[] GenerateChildren(INode node, int maxCount)
        {
            ConstraintConfiguration parentConfiguration = (ConstraintConfiguration)node.Data;
            Variable nextVar = _heuristicVariable.GetHeuristicVariable(parentConfiguration, _constraintList);
            List<INode> generatedNodes = new List<INode>();

            if (nextVar == null)
                throw new Exception("nextVar is null");

            //// Zufall
            //List<KeyValuePair<double, double>> sortedList = new List<KeyValuePair<double, double>>(nextVar.Domain.Count);
            //foreach (double value in nextVar.Domain)
            //{
            //    sortedList.Add(new KeyValuePair<double,double>(_random.NextDouble(), value));
            //}
            //sortedList.Sort();

            //foreach (KeyValuePair<double,double> pair in sortedList)
            foreach (double value in nextVar.Domain)
            {
                ConstraintConfiguration configuration = new ConstraintConfiguration(parentConfiguration);
                Variable currentVar = configuration.GetVariable(nextVar.Name);
                currentVar.Value = value;

                List<Constraint> constraints = _constraintList.GetConstraints(nextVar.Name);
                bool operationFailed = false;
                foreach (Constraint cons in constraints)
                {
                    bool tempChanging = false;
                    cons.SetCurrentConfiguration(configuration);
                    // todo: Ist diese Abfrage wirklich notwendig?
                    //if (cons.IsComplied == false)
                    //{
                    //    operationFailed = true;
                    //    break;
                    //}
                    if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachNodeGenerating, ConsistencyType.Bounds) && ConstraintForms.CheckBoundsConsistency(cons, ref tempChanging) == false)
                    {
                        operationFailed = true;
                        break;
                    }
                    else if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachNodeGenerating, ConsistencyType.Node) && configuration.CheckNodeConsistency(cons, false) == false)
                    {
                        operationFailed = true;
                        break;
                    }
                    else if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachNodeGenerating, ConsistencyType.Arc) && CheckArcConsistency(configuration) == false)
                    {
                        operationFailed = true;
                        break;
                    }
                }
                if (!operationFailed)
                {
                    // Die Domaene kann gelöscht werden
                    currentVar.Domain = null;
                    generatedNodes.Add(CreateNode(node, configuration));
                    //configuration.Show();
                }
            }


            INode[] result;
            result = new INode[generatedNodes.Count];

            generatedNodes.CopyTo(result);
            return result;
        }

        /// <summary>
        /// Serialisiert die Instanz in xml.
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            StringWriter stringWriter = new StringWriter(new StringBuilder());
            XmlWriter xmlWriter = XmlWriter.Create(stringWriter);
            xmlWriter.WriteStartElement(this.GetType().Name);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ConstraintList));
            xmlSerializer.Serialize(xmlWriter, _constraintList);
            if (_begin != null && _begin.Data != null)
            {
                xmlSerializer = new XmlSerializer(_begin.Data.GetType());
                xmlSerializer.Serialize(xmlWriter, _begin.Data);
            }
            xmlWriter.WriteEndElement();
            return xmlWriter.ToString();
            //return stringWriter.ToString();
        }

        /// <summary>
        /// Erzeugt eine Domain, in die die übergebene Intervalliste gepackt wird.
        /// </summary>
        /// <remarks>Es wird der Domänentyp genutzt, der über DomainType angegeben wurde.</remarks>
        /// <param name="intervalList">Die Liste mit den Intervallen.</param>
        /// <returns></returns>
        public IDomain CreateDomain(List<Interval> intervalList)
        {
            switch (_domainType)
            {
                case DomainType.IntervalDomain:
                    return new IntervalDomain(intervalList);
                case DomainType.ValueDomain:
                    return new Domain(intervalList);
                case DomainType.BoolDomain:
                    return new BoolDomain(intervalList);
                default:
                    return new Domain(intervalList);
            }
        }

        /// <summary>
        /// Erzeugt einen neune Kindknoten, dem die Konfiguration zugewiesen wird.
        /// </summary>
        /// <remarks>Der Typ des Knotens wird über NodeType festgelegt.</remarks>
        /// <param name="parentNode">Der Elternknoten.</param>
        /// <param name="configuration">Die Konfiguration, die von dem neuen Knoten ummantelt wird.</param>
        /// <returns>Der erzeugte Kindknoten.</returns>
        public INode CreateNode(INode parentNode, ConstraintConfiguration configuration)
        {
            switch (_nodeType)
            {
                case NodeType.Node:
                    if (parentNode != null)
                        return new Node(parentNode, configuration);
                    else
                        return new Node(configuration);
                case NodeType.TreeNode:
                    if (parentNode != null)
                        return new TreeNode(parentNode as TreeNode, configuration);
                    else
                        return new TreeNode(configuration);
                default:
                    if (parentNode != null)
                        return new Node(parentNode, configuration);
                    else
                        return new Node(configuration);
            }
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Prüft, ob die übergebene Konfiguration knotenkonsistent ist.
        /// <remarks>
        /// Ein unäres (einstelliges) Constraint c(v) ist knotenkonsistent, wenn das Constraint für alle Belegungen b aus dem Wertebereich der Variablen v erfüllt ist.
        /// Eine Konfiguration ist knotenkonsistent, wenn alle enthaltenen Constraints knotenkonsistent sind.
        /// Anhand dieser Definition finden Beschränkungen im Wertebereich der Variablen von der übergebenen Konfiguration statt.
        /// </remarks>
        /// </summary>
        /// <param name="configuration">Die übergebene Konfiguration, die es zu testen gilt.</param>
        /// <param name="onlyOneUnassignedVar">
        /// Gibt an, ob nur unäre Constraints überprüft werden sollen,
        /// oder ob auch Constraints überprüft werden, die zwar mehr Variablen enthalten, wo
        /// aber nur eine davon noch nicht belegt ist.
        /// </param>
        /// <returns>Gibt true zurück, wenn die Konfiguration knotenkonsistent ist, sonst false.</returns>
        protected bool CheckNodeConsistency(ConstraintConfiguration configuration, bool onlyOneUnassignedVar)
        {
            foreach(Constraint constraint in _constraintList.GetConstraints())
            {
                if (!configuration.CheckNodeConsistency(constraint, onlyOneUnassignedVar))
                    return false;
            }
            return true;
        }

        /*protected bool CheckLimitConsistency(ConstraintConfiguration configuration)
        {
            foreach (Constraint constraint in _constraintList.GetConstraints())
            {
                constraint.setCurrentConfiguration(configuration);
                if (!configuration.CheckLimitConsistency(constraint))
                    return false;
            }
            return true;
        }*/

        /// <summary>
        /// Prüft, ob die übergebene Konfiguration kantenkonsistent ist.
        /// </summary>
        /// <remarks>
        /// Ein binäres Constraint c(v1, v2) ist kantenkonsistent, wenn das Constraint für alle Belegungen b1 aus dem Wertebereich der Variablen v1 und für alle Belegungen b2 aus dem Wertebereich der Variablen v2 erfüllt ist.
        /// Eine Konfiguration ist kantenkonsistent, wenn alle enthaltenen Constraints kantenkonsistent sind.
        /// Anhand dieser Definition finden Beschränkungen im Wertebereich der Variablen von der übergebenen Konfiguration statt.
        /// </remarks>
        /// <param name="configuration">Die übergebene Konfiguration, die es zu testen gilt.</param>
        /// <returns>Gibt true zurück, wenn die Konfiguration kantenkonsistent ist, sonst false.</returns>
        protected bool CheckArcConsistency(ConstraintConfiguration configuration)
        {
            return configuration.AC3(this);
            /*bool changing = true;
            bool tempChanging = false;

            // Performance Test
            int counter = 0;

            while (changing == true)
            {
                changing = false;
                tempChanging = false;
                foreach (Constraint constraint in _constraintList.GetConstraints())
                {
                    if (constraint.CountUnAssignedVars == 2)
                    {
                        counter++;
                        if (!configuration.CheckEdgeConsistency(constraint, ref tempChanging))
                            return false;
                    }
                    if (tempChanging == true)
                    {
                        changing = true;
                    }
                }
            }

            _log.Debug("Edge: counter is " + counter.ToString());

            return true;*/
        }

        /// <summary>
        /// todo: Muss noch mehr ausgereizt werden. Hier können anhand von arithmetischen Gleichungen oder
        /// Ungleichungen und deren Eigenschaften Beschränkungen am Wertebereich durchgeführt werden
        /// </summary>
        /// <remarks>
        /// Es finden Beschränkungen im Wertebereich der Variablen von der übergebenen Konfiguration statt.
        /// </remarks>
        /// <example>
        /// Aus x1 + x2 &lt;= 7 wird
        /// x1Max = 7 - x2Min und
        /// x2Max = 7 - X1Min
        /// </example>
        /// <param name="configuration">Die übergebene Konfiguration, die es zu testen gilt.</param>
        /// <returns>Gibt true zurück, wenn die Konfiguration grenzenkonsistent ist, sonst false.</returns>
        protected bool CheckBoundsConsistency(ConstraintConfiguration configuration)
        {
            bool changing = true;
            bool tempChanging = false;
            while (changing == true)
            {
                changing = false;
                tempChanging = false;
                foreach (Constraint constraint in _constraintList.GetConstraints())
                {
                    if (ConstraintForms.CheckBoundsConsistency(constraint, ref tempChanging) == false)
                        return false;
                    if (tempChanging == true)
                    {
                        changing = true;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Alle Constraints werden mit den Werten der übergebenen Konfiguration initialisiert. 
        /// </summary>
        /// <remarks>
        /// Die Randbedingungen gehören nicht zu einer Konfiguration. Das erspart deren Kopieren bei Erstellung einer
        /// neuen Konfiguration. Stattdessen werden die Randbedingungen vor deren Prüfungen mit der zu prüfenden Konfiguration
        /// initialisier.
        /// </remarks>
        /// <param name="configuration">Mit dieser Konfiguration werden die Randbedingungen initialisiert.</param>
        protected void SetCurrentConfiguration(ConstraintConfiguration configuration)
        {
            foreach (Constraint constraint in _constraintList.GetConstraints())
            {
                constraint.SetCurrentConfiguration(configuration);
            }
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
            //return new XmlSchema();
            //throw new Exception("The method or operation is not implemented.");
            return null;
        }

        /// <summary>
        /// Generates an object from its XML representation.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            // die Variablenoperatoren werden im Problem gemerkt
            // Das Dictionary kann später wieder geleert werden, wenn die Variablenoperatoren in den Constraints sind
            VariablesOperator.VarOperatorList.Clear();
            reader.Read();
            _xmlVersion = reader.ReadElementContentAsString();
            XmlSerializer xmlSerializer;
            if (reader.Name == "ConstraintConfiguration")
            {
                xmlSerializer = new XmlSerializer(typeof(ConstraintConfiguration));
                _begin = CreateNode(null, xmlSerializer.Deserialize(reader) as ConstraintConfiguration);
                //_begin.Data = xmlSerializer.Deserialize(reader) as ConstraintConfiguration;
            }
            xmlSerializer = new XmlSerializer(typeof(ConstraintList));
            _constraintList = xmlSerializer.Deserialize(reader) as ConstraintList;
            reader.Read();
            VariablesOperator.VarOperatorList.Clear();
        }

        /// <summary>
        /// Converts an object into its XML representation.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("ConstraintProblem");
            writer.WriteElementString("Version", _xmlVersion);
            XmlSerializer xmlSerializer;
            if (_begin != null && _begin.Data != null)
            {
                xmlSerializer = new XmlSerializer(_begin.Data.GetType());
                xmlSerializer.Serialize(writer, _begin.Data);
            }
            xmlSerializer = new XmlSerializer(typeof(ConstraintList));
            xmlSerializer.Serialize(writer, _constraintList);
            writer.WriteEndElement();
        }

        #endregion

        #region IHeuristicSearchProblem Members

        /// <summary>
        /// Liefert oder setzt die Werte-Heuristik, die verwendet wird.
        /// </summary>
        /// <value></value>
        public virtual IHeuristicValue HeuristicValue
        {
            get
            {
                return _heuristicValue;
            }
            set
            {
                _heuristicValue = value;
            }
        }

        #endregion

        /// <summary>
        /// Gibt die Implementation der HeuristicVariable zurück, oder setzt diese.
        /// </summary>
        /// <remarks>
        /// Innerhalb der IHeuristicVariable Implementation wird die Heuristik für die Auswahl der nächsten zu belegenen Variable ermittelt
        /// </remarks>
        public virtual IHeuristicVariable HeuristicVariable
        {
            get
            {
                return _heuristicVariable;
            }
            set
            {
                _heuristicVariable = value;
            }
        }

        private static string GetXmlVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}
