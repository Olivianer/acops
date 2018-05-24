using System;
using OKSearchRoom;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für TravelingSalesmanProblem.
	/// </summary>
	public class TravelingSalesmanProblem : IEvolutionaryProblem
	{
		protected int m_ChildrenPopulation;
		protected int m_ParentPopulation;
		protected int m_Counter;
		protected int m_CounterSameTss;
		static private Random m_Random;
        protected bool m_FoundSolution;

		public TravelingSalesmanProblem(int parentPopulation, int childrenPopulation)
		{
			if (m_Random == null)
				m_Random = new Random(unchecked((int)DateTime.Now.Ticks));

			m_Counter = 0;
			m_CounterSameTss = 0;
			m_ParentPopulation = parentPopulation;
			m_ChildrenPopulation = childrenPopulation;
		}

		public int ChildrenPopulation
		{
			get
			{
				return m_ChildrenPopulation;
			}
			set
			{
				m_ChildrenPopulation = value;
			}
		}

		public int ParentPopulation
		{
			get
			{
				return m_ParentPopulation;
			}
			set
			{
				m_ParentPopulation = value;
			}
		} 

		public void DoMutation(ref Node individual)
		{
            TravelingSalesmanSolution tss = (TravelingSalesmanSolution)individual.Data;

			/*if (m_Random.NextDouble() < 0.01)
			{
				tssMut = tss.DoChangeMutation();
			}
			else*/
			{
				tss.DoInvertMutation();
			}
		}

		public Node DoRecombination(Node father, Node mother)
		{
			TravelingSalesmanSolution tssFather = (TravelingSalesmanSolution)father.Data;
			TravelingSalesmanSolution tssMother = (TravelingSalesmanSolution)mother.Data;
			TravelingSalesmanSolution tssChild = tssFather.DoEdgesRecombination(tssMother);
			Node node = new Node(tssChild);
			return node;
		}

		public double GetObjectiveFunction(Node node)
		{
			TravelingSalesmanSolution tss = (TravelingSalesmanSolution)node.Data;
			return tss.CalculateDistance();
		}

		// Todo
		public bool CompareNodes(INode node)
		{
			TravelingSalesmanSolution tss = (TravelingSalesmanSolution)node.Data;

			System.Console.WriteLine("Distance: " + tss.ToString());

			m_Counter++;

			//if (m_Counter == 2000)
			//	return true;

			return false;
		}

		public INode[] FirstNodes 
		{ 
			get
			{
				TravelingSalesmanSolution[] tssList = new TravelingSalesmanSolution[m_ParentPopulation];
				Node[] nodes = new Node[m_ParentPopulation];

				for (int i=0; i<nodes.Length; i++)
				{
					tssList[i] = new TravelingSalesmanSolution();
					tssList[i].RandomizePlaceList();
					nodes[i] = new Node(tssList[i]);
				}

				return nodes;
			}
		}

		public void OnFoundNoneDestination()
		{
		}

        public bool OnFoundDestination(INode destination, ISearchMethod searchMethod)
        {
            m_FoundSolution = true;
            return true;
        }

        public bool FoundSolution
        {
            get
            {
                return m_FoundSolution;
            }
        }

        public void OnStartSearch()
        {
        }
	}
}
