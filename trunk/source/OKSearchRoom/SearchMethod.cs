///////////////////////////////////////////////////////////
//  SearchMethod.cs
//  Implementation of the Class SearchMethod
//  Generated by Enterprise Architect
//  Created on:      29-Jul-2006 18:36:24
//  Original author: Oliver Kuehne
///////////////////////////////////////////////////////////



using System.Collections.Generic;
using OKSearchRoom;
namespace OKSearchRoom {
	/// <summary>
	/// Die Standardimplementierung des ISearchMethod-Interfaces. Alle Methoden sind
	/// virtual und k�nnen von der abgeleiteten Klasse �berschrieben werden.
	/// </summary>
	public class SearchMethod : ISearchMethod
    {
        #region Protected Member
        /// <summary>
        /// Dient der Unterbechung der Suche.
        /// </summary>
        protected bool _cancel;
        /// <summary>
        /// Enth�lt den aktuellen Knoten
        /// </summary>
		protected INode _currentNode;
        /// <summary>
        /// Enth�lt eine Referenz auf einen von au�en definierten Eventhandler oder ist null
        /// </summary>
		protected ISearchEventHandler _eventHandler;
        /// <summary>
        /// Enth�lt die Anzahl der bisher untersuchten Knoten
        /// </summary>
		protected int _inspectedNodes;
        /// <summary>
        /// Enth�lt eine Referenz auf das Suchproblem
        /// </summary>
		protected ISearchProblem _searchProblem;
        #endregion

        #region Constructor
        /// <summary>
		/// Dem Konstruktor wird ein Suchproblem �bergeben.
		/// </summary>
		/// <param name="searchProblem">Stellt das Suchproblem dar, auf welches das
		/// Suchverfahren angewendet wird.</param>
		public SearchMethod(ISearchProblem searchProblem)
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
        /// Die Suche wird angesto�en.
        /// </summary>
        protected virtual void Search()
        {
        }

        /// <summary>
        /// Das Ereignis f�r den externen Eventhandler wird aufgerufen.
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
		/// Eine unterbrochene Suche wird fortgesetzt.
		/// </summary>
		public virtual void Continue()
		{
            _cancel = false;
            Search();
		}	

		/// <summary>
		/// Ein Suchverfahren kann mit einem Eventhandler versorgt werden. Mit Hilfe dieses
		/// Eventhandlers ist es m�glich, dass von Au�en Informationen w�hrend der Suche
		/// empfangen werden. Ebenso ist mittels der Eventhandler ein Stoppen des
		/// Suchverfahrens m�glich.
		/// </summary>
		public ISearchEventHandler EventHandler{
			get{
				return _eventHandler;
			}
			set{
				_eventHandler = value;
			}
		}	

		/// <summary>
		/// Die Suche wird gestartet.
		/// </summary>
		public virtual void Run()
		{
            Init();
            _searchProblem.OnStartSearch();
            Search();
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
        /// Gibt die Knotenmenge zur�ck oder setzt diese
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
    }//end SearchMethod

}//end namespace OKSearchRoom