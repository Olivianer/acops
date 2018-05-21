using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;
using OKHeuristicSearchRoom;
using OKConstraintVariables;
using Microsoft.Extensions.Logging;

namespace OKConstraints
{
    // todo: Zu überdenken, zumindest das OptimizationConstraint
    /// <summary>
    /// Die Klasse dient der Beschreibung eines OptimizationConstraintProblem
    /// <remarks>
    /// Unter einem OptimizationConstraintproblem kann man sich ein ConstraintProblem vorstellen,
    /// für das mehrere Lösungen existierien. Aus den möglichen Lösungen soll die "beste" Lösung
    /// gefunden werden.
    /// Zur Charakterisierung dient eine sogenannte Zielfunktion.
    /// <example>
    /// Eine Zielfunktion könnte z.B. so aussehen: max(x1+x2)
    /// </example>
    /// </remarks>
    /// </summary>
    public class ConstraintOptimizationProblem : ConstraintProblem
    {
        #region Protected Member
        /// <summary>
        /// Das ZielfunktionsConstraint.
        /// <remarks>
        /// Wurde eine erste Lösung gefunden, dann fungiert die Zielfunktion als Constraint.
        /// Mit dessen Hilfe werden untere oder obere Schranken gebildet.
        /// </remarks>
        /// </summary>
        protected OptimizationConstraint _objectiveConstraint = null;
        /// <summary>
        /// Ein EventHandler, der nach dem Finden einer Lösung aufgerufen wird.
        /// </summary>
        protected IFoundEventHandler _eventHandler = null;
        protected override ILogger Logger { get; } =
            ApplicationLogging.CreateLogger<ConstraintOptimizationProblem>();
        
        #endregion

        #region Constructor
        /// <summary>
        /// Der Kopierkonstrukor.
        /// </summary>
        /// <param name="configuration">Die Konfiguration von der kopiert wird.</param>
        public ConstraintOptimizationProblem(ConstraintConfiguration configuration)
            : base(configuration)
        {
            _heuristicValue = new ConstraintOptimizationHeuristic();
        }
        #endregion

        #region Private Functions
        /// <summary>
        /// Todo: Ändern dieser Vorgehensweise
        /// Kleiner Trick, um das Ergebnis der Zielfunktion auch anzeigbar zu machen.
        /// </summary>
        private void AddOptVariable()
        {
            if (_solutionList.Count == 1)
            {
                ConstraintConfiguration constel = (ConstraintConfiguration)_solutionList[0].Data;
                //Variable var = new Variable("objFunction", new Domain(0, 0), constel);
                Variable var = new Variable("objFunction", new Domain());
                constel.AddVariable(var);
                var.Value = _objectiveConstraint.ObjectiveValue;
            }
        }
        #endregion

        #region Public Functions

        /// <summary>
        /// Legt das ZielfunktionsConstraint fest.
        /// </summary>
        /// <param name="objectiveConstraint"></param>
        public void SetObjectiveFunction(OptimizationConstraint objectiveConstraint)
        {
            _objectiveConstraint = objectiveConstraint;
        }

        /// <summary>
        /// Gibt das ZielfunktionsConstraint zurück.
        /// </summary>
        public OptimizationConstraint GetObjectiveFunction()
        {
            return _objectiveConstraint;
        }

        /// <summary>
        /// Legt den EventHandler fest, der aufgerufen wird, wenn eine Lösung gefunden wird.
        /// </summary>
        /// <param name="eventHandler"></param>
        public void SetFoundEventHandler(IFoundEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        /// <summary>
        /// Dies Funktion wird aufgerufen, wenn eine Lösung gefunden wurde.
        /// </summary>
        /// <param name="destination">Gibt den Knoten an, der die Lösungskonfiguration ummantelt.</param>
        /// <param name="searchMethod">
        /// Gibt die Suchmethode an, mit der die Lösung gefunden wurde.
        /// <remarks>
        /// Dies ist notwendig, um den Zugriff auf die Knotenmenge der Suchmethode zu erhalten und diese dann ändern zu können.
        /// </remarks>
        /// </param>
        /// <returns></returns>
        public override bool OnFoundDestination(INode destination, ISearchMethod searchMethod)
        {
            // Bei einem Optimierungsproblem wird sich nur die beste Lösung gemerkt
            if (_solutionList.Count == 0)
            {
                _solutionList.Add(destination as Node);
            }
            else
            {
                _solutionList[0] = destination as Node;
            }
            ConstraintConfiguration configuration = (ConstraintConfiguration)destination.Data;
            
            _objectiveConstraint.SetCurrentConfiguration(configuration);
            _objectiveConstraint.SetObjectiveValue();

            List<INode> newNodeSet = new List<INode>();
            /*foreach (INode node in searchMethod.NodeSet)
            {
                configuration = (ConstraintConfiguration)node.Data;
                _objectiveConstraint.setCurrentConfiguration(configuration);
                if (_objectiveConstraint.IsComplied)
                {
                    if (CheckNodeConsistency(configur5ation, false) == true)
                        newNodeSet.Add(node);
                    else
                        configuration = null;
                }
            }*/

            configuration = (ConstraintConfiguration)_begin.Data;

            if (_eventHandler != null)
            {
                _eventHandler.FoundEvent(searchMethod, this, destination, _objectiveConstraint.ObjectiveValue);
            }

            SetCurrentConfiguration(configuration);

            if (CheckBoundsConsistency(configuration) == false)
            {
                AddOptVariable();
                return true;
            }

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachSolution, ConsistencyType.Node) == true
                && CheckNodeConsistency(configuration, false) == false)
            {
                AddOptVariable();
                return true;
            }

            if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachSolution, ConsistencyType.Arc) == true
                && CheckArcConsistency(configuration) == false)
            {
                AddOptVariable();
                return true;
            }

            //if (CheckLimitConsistency(configuration) == false)
            //    return true;

            Logger.LogDebug("##### Node the begin node has the following configuration");
            Logger.LogDebug(((ConstraintConfiguration)_begin.Data).ToString()); 

            newNodeSet.Add(_begin);

            Logger.LogDebug("##### Arc the begin node has the following configuration");
            Logger.LogDebug(((ConstraintConfiguration)_begin.Data).ToString());

            searchMethod.NodeSet = newNodeSet;

            return false;
        }

        /// <summary>
        /// Wird nicht genutzt.
        /// </summary>
        public override void OnFoundNoneDestination()
        {
            AddOptVariable();
        }

        /*public OptimizationConstraint OptConstraint
        {
            get
            {
                return _objectiveConstraint;
            }
        }*/

        /// <summary>
        /// Für den übergebenen Knoten werden neue Unterknoten generiert.
        /// </summary>
        /// <returns>Gibt die Menge der generierten Knoten als Array zurück.</returns>
        /// <param name="node">Anhand des übergebenen Knotens werden neue Unterknoten
        /// generiert.</param>
        /// <param name="maxCount">Beschränkt die Anzahl der Unterknoten.</param>
        public override INode[] GenerateChildren(INode node, int maxCount)
        {
            try
            {
                ConstraintConfiguration parentConfiguration = (ConstraintConfiguration)node.Data;
                Variable nextVar = _heuristicVariable.GetHeuristicVariable(parentConfiguration, _constraintList);
                List<INode> generatedNodes = new List<INode>();

                /*if (_destination != null)
                {
                    if (!CheckEdgeConsistency(parentConfiguration))
                        return new Node[0]; ;
                }*/

                /*if (!CheckNodeConsistency(parentConfiguration, true))
                {
                    return new Node[0];
                }*/

                //// Zufall
                //SortedList<double, double> sortedList = new SortedList<double, double>(nextVar.Domain.Count);
                //foreach (double value in nextVar.Domain)
                //{
                //    sortedList.Add(_random.NextDouble(), value);
                //}

                //foreach (double value in sortedList.Values)
                foreach (double value in nextVar.Domain)
                {
                    ConstraintConfiguration configuration = new ConstraintConfiguration(parentConfiguration);

                    Variable currentVar = configuration.GetVariable(nextVar.Name);
                    currentVar.Value = value;

                    List<Constraint> constraints = _constraintList.GetConstraints(nextVar.Name);
                    bool operationFailed = false;
                    foreach (Constraint cons in constraints)
                    {
                        cons.SetCurrentConfiguration(configuration);
                        //if (cons.IsComplied == false)
                        //{
                        //    operationFailed = true;
                        //    break;
                        //}
                        if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachNodeGenerating, ConsistencyType.Node) 
                            && configuration.CheckNodeConsistency(cons, false) == false)
                        {
                            operationFailed = true;
                            break;
                        }
                        else if (_consistencyOptions.GetCheckConsistencyActive(ConsistencyCheckRegion.EachNodeGenerating, ConsistencyType.Arc) == true
                            && CheckArcConsistency(configuration) == false)
                        {
                            operationFailed = true;
                            break;
                        }
                        /*else if (!configuration.CheckLimitConsistency(cons))
                        {
                            operationFailed = true;
                            break;
                        }*/
                        /*else if (_destination != null && !configuration.CheckEdgeConsistency(cons, ref operationFailed))
                        {
                            operationFailed = true;
                            break;
                        }
                        operationFailed = false;*/
                    }
                    if (!operationFailed)
                    {
                        // Prüfe zusätzlich die Zielfunktion der Optimierung, wenn schon eine Lösung existiert
                        if (_solutionList.Count != 0)
                        {
                            _objectiveConstraint.SetCurrentConfiguration(configuration);
                            if (_objectiveConstraint.IsComplied == false)
                                operationFailed = true;
                        }
                    }
                    if (!operationFailed)
                    {
                        // Die Domaene kann gelöscht werden
                        currentVar.Domain = null;
                        generatedNodes.Add(CreateNode(node, configuration));  

                        /*if (generatedNodes.Count > 10)
                            break;*/
                        //configuration.Show();
                        //_log.Debug("OptConfiguration: " + configuration.ToString());
                    }
                }


                INode[] result = new INode[generatedNodes.Count];
                generatedNodes.CopyTo(result);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        ///// <summary>
        ///// Für ein ConstraintOptimierungsProblem wird eine spezielle Heuristik genutzt.
        ///// </summary>
        ///// <param name="node">Der Knoten, dessen Heuristik ermittelt werden soll.</param>
        ///// <returns>Gibt den Wert der Heuristik zurück.</returns>
        //public override double GetHeuristic(INode node)
        //{
        //    // Solange noch keine lösung gefunden wurde, nutze die normale Heuristik
        //    ConstraintConfiguration configuration = (ConstraintConfiguration)node.Data;
        //    if (_solutionList.Count == 0)
        //    {
        //        //return -node.Depth*10;
        //        return configuration.GetCountDomainValues();
        //    }
        //    double result;
        //    _objectiveConstraint.setCurrentConfiguration(configuration);
        //    IOperation operation = _objectiveConstraint.ObjectiveFunction;
        //    if (operation.DoOperation(out result) != true)
        //    {
        //        return -double.MaxValue;
        //    }
        //    else
        //    {
        //        //return configuration.GetCountDomainValues();
        //        if (operation.GetType() == typeof(Minimum))
        //            return result + configuration.GetCountDomainValues();
        //            //return configuration.GetCountDomainValues();
        //            //return (configuration.GetCountDomainValues()+result);
        //            //return (result + configuration.GetCountDomainValues());
        //            //return (result + _objectiveConstraint.ObjectiveValue * configuration.GetCountDomainValues());
        //            //return configuration.GetCountDomainValues();
        //            //return -(_objectiveConstraint.ObjectiveValue - result) + configuration.GetCountDomainValues() * result;
        //            //return -(_objectiveConstraint.ObjectiveValue - result) + configuration.GetCountDomainValues() * _objectiveConstraint.ObjectiveValue;
        //            //return -node.Depth * (_objectiveConstraint.ObjectiveValue - result);
        //            //return result - _objectiveConstraint.ObjectiveValue;
        //            //return -node.Depth * (result - _objectiveConstraint.ObjectiveValue);
        //        else
        //            //return node.Depth * (_objectiveConstraint.ObjectiveValue - result);
        //            return configuration.GetCountDomainValues() - result;
        //    }
        //}

        #endregion
    }
}
