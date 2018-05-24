using System;
using System.Collections;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für TravelingSalesmanSolution.
	/// </summary>
	public class TravelingSalesmanSolution
	{
		static private Random m_Random;
		static private Matrix m_DistanceMatrix;	// Referenz auf die  Matrix die die Entfernungen zwischen den Orten angibt
		private ArrayList m_PlaceList;			// Stellt die Liste mit allen Orten dar, die hintereinander erreicht werden

		public TravelingSalesmanSolution()
		{
			if (m_Random == null)
				m_Random = new Random(unchecked((int)DateTime.Now.Ticks));

			if (m_DistanceMatrix == null)
				throw new Exception("The distance matrix is null");
			
			// Der Standardweg geht von 0 zu 1 zu 2 ... zu 99
			m_PlaceList = new ArrayList(m_DistanceMatrix.RowCount);
			for (int i=0; i<m_DistanceMatrix.RowCount; i++)
			{
				m_PlaceList.Add(i);
			}
		}

		public TravelingSalesmanSolution(TravelingSalesmanSolution solution)
		{
			if (m_Random == null)
				m_Random = new Random(unchecked((int)DateTime.Now.Ticks));

			m_PlaceList = new ArrayList(solution.m_PlaceList.Count);
			foreach(int place in solution.m_PlaceList)
			{
				m_PlaceList.Add(place);
			}
		}
		
		static public Matrix DistanceMatrix
		{
			get
			{
				return m_DistanceMatrix;
			}
			set
			{
				m_DistanceMatrix = value;
			}
		}

		public double CalculateDistance()
		{
			double distance = 0;

			if (m_PlaceList.Count == 0)
				return distance;

			// Alle Distanzen zwischen den Orten aufaddieren
			for (int i=0; i<m_PlaceList.Count-1; i++)
			{
				distance += (double) m_DistanceMatrix.GetValue((int)m_PlaceList[i], (int)m_PlaceList[i+1]);
			}

			// Der Weg vom letzten Ort zum ersten Ort muss noch aufaddiert werden
			distance += (double) m_DistanceMatrix.GetValue((int)m_PlaceList[m_PlaceList.Count-1], (int)m_PlaceList[0]);

			return distance;
		}

		public TravelingSalesmanSolution DoChangeMutation()
		{
			TravelingSalesmanSolution tss = new TravelingSalesmanSolution(this);
			IList list = tss.m_PlaceList;
			Permutation.DoChange( ref list);
			return tss;
		}

        public void DoInvertMutation()
		{
			IList list = this.m_PlaceList;
			
			/*if (tss.CalculateDistance() > 700)
				Permutation.DoInvert( ref list, 1);
			else if (tss.CalculateDistance() > 680)
			{
				Permutation.DoInvert( ref list, 3);
			}
			else
			{
				Permutation.DoInvert( ref list, 6);
			}
			Permutation.DoInvert( ref list, 6);*/
			switch(m_Random.Next( 0, 2))
			{
				case 0:
					//if (tss.CalculateDistance() > 700)
						Permutation.DoInvert( ref list, m_Random.Next( 1, 30));
					//else
					//{
					//	Permutation.DoInvert( ref list, 6);
					//}
					break;
				case 1:
					Permutation.DoShift( ref list);
					break;
				case 2:
					Permutation.DoChange( ref list);
					break;
				case 3:
					Permutation.DoMix( ref list);
					break;
			}
		}

		public TravelingSalesmanSolution DoEdgesRecombination(TravelingSalesmanSolution tssPartner)
		{
			// Das zu erzeugende Kind
			TravelingSalesmanSolution tssChild = new TravelingSalesmanSolution();

			// Eine Liste der Orte, die noch zur Verfügung stehen, wird gebraucht, wenn ein zufälliger rechtsseitiger Ort generiert werden muss 
			ArrayList tempPlaceList = new ArrayList(m_PlaceList.Count);
			
			// Mit allen Orten füllen
			for (int i=0; i<m_PlaceList.Count; i++)
			{
				tempPlaceList.Add(i);
			}

			// Die Hashtable, die alle AdjazenzInformationen in Form eine ArrayList von höchstens 4 Werten hält 
			Hashtable adjList = new Hashtable(); 

			for (int i=0; i<m_PlaceList.Count; i++)
			{
				int indexFather = i;
				int indexMother = tssPartner.GetPlaceIndex((int)m_PlaceList[i]);
				
				ArrayList adj = new ArrayList(4);
			
				// der linksseitige Ort vom Vater
				if (indexFather > 0)
					adj.Add(m_PlaceList[indexFather-1]);
				else
					adj.Add(m_PlaceList[m_PlaceList.Count-1]);

				// der rechtsseitige Ort vom Vater
				if (indexFather < m_PlaceList.Count-1)
					adj.Add(m_PlaceList[indexFather+1]);
				else
					adj.Add(m_PlaceList[0]);

				// der linksseitige Ort der Mutter, wenn noch nicht enthalten
				if (indexMother > 0)
				{
					if (!adj.Contains(tssPartner.m_PlaceList[indexMother-1]))
						adj.Add(tssPartner.m_PlaceList[indexMother-1]);
				}
				else
				{
					if (!adj.Contains(tssPartner.m_PlaceList[m_PlaceList.Count-1]))
						adj.Add(tssPartner.m_PlaceList[m_PlaceList.Count-1]);
				}

				// der rechtsseitige Ort der Mutter, wenn noch nicht enthalten
				if (indexMother < m_PlaceList.Count-1)
				{
					if (!adj.Contains(tssPartner.m_PlaceList[indexMother+1]))
						adj.Add(tssPartner.m_PlaceList[indexMother+1]);
				}
				else
				{
					if (!adj.Contains(tssPartner.m_PlaceList[0]))
						adj.Add(tssPartner.m_PlaceList[0]);
				}

				adjList.Add(m_PlaceList[i], adj);
			}

			// Wähle zufällig eine AnfangsAdjazenzInformation		
			int adjIndex = Convert.ToInt32(Math.Round(m_Random.NextDouble()*(m_PlaceList.Count-1)));
			
			// Belege den ersten Ort des Kindes
			tssChild.m_PlaceList[0] = adjIndex;

			for (int i=1; i<m_PlaceList.Count; i++)
			{
				// Entferne den aktuellen AdjazenzIndex (Ort) aus der Adjazenzliste und aus der temporären Ortliste
				RemovePlaceFromAdjacent(adjList.Values, adjIndex);
				tempPlaceList.Remove(adjIndex);

				int count = 0;
				int countMin = 5;
				ArrayList adjCurrent = (ArrayList)adjList[adjIndex];
				ArrayList nextAdjs = new ArrayList();
				
				foreach (int place in adjCurrent)
				{
					ArrayList adjTemp = (ArrayList)adjList[place];
					count = adjTemp.Count;
					if (count < countMin)
					{
						nextAdjs.Clear();
						countMin = count;
						nextAdjs.Add(place);
					}
					else if (count == countMin)
					{
						nextAdjs.Add(place);
					}
				}

				// Wurden keine möglichen Kanten gefunden, muss ein zufällig ausgewählter Ort genutzt werden
				if (nextAdjs.Count == 0)
				{
					int tempIndex = Convert.ToInt32(Math.Round(m_Random.NextDouble()*(tempPlaceList.Count-1)));
					adjIndex = (int)tempPlaceList[tempIndex];
				}
				else if (nextAdjs.Count == 1)
				{
					adjIndex = (int)nextAdjs[0];
				}
				else
				{
					// Bei mehreren guten nächsten Orten, wähle zufällig einen aus
					int indexMinPlace = Convert.ToInt32(Math.Round(m_Random.NextDouble()*(nextAdjs.Count-1)));
					adjIndex = (int)nextAdjs[indexMinPlace];
				}

				// Nächsten Ort setzen
				tssChild.m_PlaceList[i] = adjIndex;
			}

			return tssChild;
		}

		public void RandomizePlaceList()
		{
			SortedList sortedList = new SortedList(m_PlaceList.Count);
			
			foreach (int placeIndex in m_PlaceList)
			{
				sortedList.Add(m_Random.NextDouble(), placeIndex);
			}

			IList valueList = sortedList.GetValueList();
			
			for (int i=0; i<valueList.Count; i++)
			{
				m_PlaceList[i] = valueList[i];
			}
		}

		public int GetPlaceIndex(int place)
		{
			for (int i=0; i<m_PlaceList.Count; i++)
			{
				if ((int)m_PlaceList[i] == place)
					return i;
			}
			
			throw new Exception("The place " + place + " could not be found.");
		}

		public override string ToString()
		{
			return CalculateDistance().ToString();
		}

		public static void NewRandom()
		{
			m_Random = new Random(unchecked((int)DateTime.Now.Ticks));
		}

		private void RemovePlaceFromAdjacent(ICollection adjValueList, int place)
		{
			int counter = 0;
			foreach (ArrayList adj in adjValueList)
			{
				if (adj.Contains(place))
				{
					adj.Remove(place);

					// Ein Ort kann höchstens 4 mal vorkommen
					if (counter == 3)
						return;
					else
						counter++;
				}
			}
		}
	}
}