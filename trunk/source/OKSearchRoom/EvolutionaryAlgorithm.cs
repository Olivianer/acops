using System;
using System.Collections.Generic;
using OKPriorityQueues;
using System.Linq;


namespace OKSearchRoom
{
	/// <summary>
	/// Zusammendfassende Beschreibung für EvolutionaryFirstSearch.
	/// </summary>
	public class EvolutionaryAlgorithm : ISearchMethod
	{
		protected ISearchEventHandler m_EventHandler;	
        protected PriorityQueue<double,INode> m_PriorityNodes;
		protected Node m_CurrentNode;
		protected int m_InspectedNodes;
		protected IEvolutionaryProblem m_SearchProblem;
		protected bool m_Cancel;
		Random m_Random;

		public EvolutionaryAlgorithm(IEvolutionaryProblem searchProblem)
		{
			m_SearchProblem = searchProblem;
			m_Cancel = false;
			m_Random = new Random(unchecked((int)DateTime.Now.Ticks));
			m_PriorityNodes = new PriorityQueue<double, INode>();
		}

		public ISearchEventHandler EventHandler 
		{ 
			get
			{
				return m_EventHandler;
			}
			set
			{
				m_EventHandler = value;
			}
		}

		public void Run()
		{
			Init();
			Search();
		}

		public void Continue()
		{
			m_Cancel = false;
			Search();
		}

		protected bool ChooseNode()
		{
			if (m_PriorityNodes.Count > 0)
			{
				m_CurrentNode = m_PriorityNodes.Peek() as Node;
				m_InspectedNodes++;
				return true;
			}
			return false;
		}

		protected void Init()
		{
			m_InspectedNodes = 0;
			m_PriorityNodes.Clear();
			
			foreach(Node node in m_SearchProblem.FirstNodes)
			{
                m_PriorityNodes.Push(m_SearchProblem.GetObjectiveFunction(node), node);
			}

			if (!ChooseNode())
				throw new Exception("There are no first nodes");
		}

		protected virtual void Search()
		{
			int fatherIndex;
			int motherIndex;
			Node father;
			Node mother;
			Node child;
            PriorityQueue<double, INode> tempNodes = new PriorityQueue<double, INode>();

			if (m_CurrentNode == null)
			{
				m_SearchProblem.OnFoundNoneDestination();
				return;
			}

			while (!m_SearchProblem.CompareNodes(m_CurrentNode))
			{
				tempNodes.Clear();

				// Rekombination
				for (int i=0; i<m_SearchProblem.ChildrenPopulation; i++)
				{
					fatherIndex = Convert.ToInt32(Math.Round(m_Random.NextDouble()*(m_PriorityNodes.Count-1)));
					motherIndex = Convert.ToInt32(Math.Round(m_Random.NextDouble()*(m_PriorityNodes.Count-1)));
                    father = m_PriorityNodes.Nodes.ToList()[fatherIndex] as Node;
                    mother = m_PriorityNodes.Nodes.ToList()[motherIndex] as Node;
					// Rekombination
					child = m_SearchProblem.DoRecombination(father, mother);
					// Mutation
					m_SearchProblem.DoMutation(ref child);
					// Die Kinder werden in eine temporäre Population gelegt
                    tempNodes.Push(m_SearchProblem.GetObjectiveFunction(child), child);
				}

				// Die Elten kommen mit in die temporäre Population
				foreach(Node node in m_PriorityNodes.Nodes)
				{
                    tempNodes.Push(m_SearchProblem.GetObjectiveFunction(node), node);
				}

				// Löschen der alten Population
				m_PriorityNodes.Clear();

				// Selektieren der besten Individuen aus der temporären Population
				for (int i=0; i<m_SearchProblem.ParentPopulation; i++)
				{
                    var node = tempNodes.Pop() as Node;
                    m_PriorityNodes.Push(m_SearchProblem.GetObjectiveFunction(node), node);
				}

				// Wähle das beste Individuum
				if (!ChooseNode())
				{
                    m_SearchProblem.OnFoundNoneDestination();
					return;
				}

				// Sende Event
				EmitSearchEvent(m_PriorityNodes.Count);

				// Wenn ein Abbruch erwünscht ist, wird die Suche abgebrochen
				if (m_Cancel)
				{
                    m_SearchProblem.OnFoundNoneDestination();
					return;
				}
			}	
		}

		protected void EmitSearchEvent(int countNodes)
		{
			if (m_EventHandler!=null)
			{
				m_EventHandler.SearchEvent(m_InspectedNodes, countNodes, ref m_Cancel);
			}
		}

        /// <summary>
        /// Gibt die Knotenmenge zurück oder setzt diese
        /// </summary>
        public IEnumerable<INode> NodeSet
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public int InspectedNodes
        {
            get { return m_InspectedNodes; }
        }
	}
}
