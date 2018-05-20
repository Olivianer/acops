using System;
using System.Collections.Generic;
using System.Text;
using OKSearchRoom;

namespace OKHeuristicSearchRoom
{

    /// <summary>
    /// Die Standardimplementierung des IHeuristicSearchMethod-Interfaces. Alle Methoden sind
    /// virtual und können von der abgeleiteten Klasse überschrieben werden.
    /// </summary>
    public class HeuristicSearchMethod : ISearchMethod
    {
        #region Protected Member
        /// <summary>
        /// Enthält die Referenz auf das Suchproblem
        /// </summary>
        protected IHeuristicSearchProblem _searchProblem;
        /// <summary>
        /// Dient der Unterbrechung der Suche
        /// </summary>
        protected bool _cancel;
        /// <summary>
        /// Enthält eine Referenz auf einen von außen definierten Eventhandler oder ist null
        /// </summary>
        protected ISearchEventHandler _eventHandler;
        /// <summary>
        /// Enthält die Anzahl der bisher untersuchten Knoten
        /// </summary>
        protected int _inspectedNodes;
        /// <summary>
        /// Enthält den aktuellen Knoten
        /// </summary>
        protected INode _currentNode;
        #endregion

        #region Constructor
        /// <summary>
        /// Dem Konstruktor wird ein Suchproblem mit Heuristik übergeben.
        /// </summary>
        /// <param name="searchProblem">Stellt das Suchproblem mit Heuristik dar, auf welches das
        /// Suchverfahren angewendet wird.</param>
        public HeuristicSearchMethod(IHeuristicSearchProblem searchProblem)
        {
            _searchProblem = searchProblem;
            _cancel = false;
            _inspectedNodes = 0;
        }
        #endregion

        #region Protected Functions
        /// <summary>
        /// Initialisiert das Suchverfahren.
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// Die Suche wird angestoßen.
        /// </summary>
        protected virtual void Search()
        {
        }
        /// <summary>
        /// Das Ereignis für den externen Eventhandler wird aufgerufen.
        /// </summary>
        /// <param name="countNodes"></param>
        protected virtual void EmitSearchEvent(int countNodes)
        {
            if (_eventHandler != null)
            {
                _eventHandler.SearchEvent(_inspectedNodes, countNodes, ref _cancel);
            }
        }
        #endregion

        #region Public Functions
        /// <summary>
        /// Die Suche wird gestartet.
        /// </summary>
        public virtual void Run()
        {
            Init();
            Search();
        }

        /// <summary>
        /// Ein unterbrochene Suche wird fortgesetzt.
        /// </summary>
        public virtual void Continue()
        {
            _cancel = false;
            Search();
        }

       

        /// <summary>
        /// Ein Suchverfahren kann mit einem Eventhandler versorgt werden. Mit Hilfe dieses
        /// Eventhandlers ist es möglich, dass von Außen Informationen während der Suche
        /// empfangen werden. Ebenso ist mittels der Eventhandler ein Stoppen des
        /// Suchverfahrens möglich.
        /// </summary>
        public virtual ISearchEventHandler EventHandler
        {
            get
            {
                return _eventHandler;
            }
            set
            {
                _eventHandler = value;
            }
        }

        

        /// <summary>
        /// Liefert die Anzahl der durchsuchten Knoten
        /// </summary>
        public int InspectedNodes
        {
            get
            {
                return _inspectedNodes;
            }
        }

        /// <summary>
        /// Gibt die Knotenmenge zurück oder setzt diese
        /// </summary>
        public virtual IEnumerable<INode> NodeSet
        {
            get
            {
                return null;
            }
            set
            {
            }
        }
        #endregion
    }
}
