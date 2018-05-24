using System;
using System.Collections;
using OKSearchRoom;
using OKPriorityQueues;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für Permutation.
	/// </summary>
	public class Permutation
	{
		static private Random m_Random = new Random(unchecked((int)DateTime.Now.Ticks));

		public static void DoChange(ref IList list)
		{
			if (list.Count == 0)
				return;

			int index1 = m_Random.Next(0, list.Count);
			int index2 = m_Random.Next(0, list.Count);

			object helper = list[index1];
			list[index1] = list[index2];
			list[index2] = helper;
		}

		public static void DoShift(ref IList list)
		{
			if (list.Count == 0)
				return;

			int index1 = m_Random.Next(0, list.Count);
			int index2 = m_Random.Next(0, list.Count);

			if (index1 > index2)
			{
				object helper = list[index1];
				for (int i = index1; i < list.Count - 1; i++)
				{
					list[i] = list[i + 1];
				}
				list[list.Count - 1] = list[0]; 
				for (int i = 0; i < index2; i++)
				{
					list[i] = list[i + 1];
				}
				list[index2] = helper;
			}
			else
			{
				object helper = list[index1];
				for (int i = index1; i < index2; i++)
				{
					list[i] = list[i + 1];
				}
				list[index2] = helper;
			}
		}

		public static void DoMix(ref IList list)
		{
			if (list.Count == 0)
				return;

			int index1 = m_Random.Next(0, list.Count);
			int index2 = m_Random.Next(0, list.Count);

			if (index1 > index2)
			{
				PriorityQueue<double, object> prioQueue= new PriorityQueue<double, object>();
				
				for (int i = index1; i < list.Count; i++)
				{
					prioQueue.Push(m_Random.NextDouble(), list[i]);
				}
				for (int i = 0; i < index2; i++)
				{
					prioQueue.Push(m_Random.NextDouble(), list[i]);
				}
			
				// Mischen
				for (int i = index1; i < list.Count; i++)
				{
					list[i] = prioQueue.Pop();
				}
				for (int i = 0; i < index2; i++)
				{
					list[i] = prioQueue.Pop();
				}
			}
			else
			{
                PriorityQueue<double, object> prioQueue = new PriorityQueue<double, object>();
			
				for (int i = index1; i < index2; i++)
				{
					prioQueue.Push(m_Random.NextDouble(), list[i]);
				}
			
				// Mischen
				for (int i = index1; i < index2; i++)
				{
					list[i] = prioQueue.Pop();
				}
			}
		}

		private static void DoInvert(ref IList list, int startIndex, int endIndex)
		{
			int index1 = 0;
			int index2 = 0;
			if (startIndex <= endIndex)
			{
				index1 = m_Random.Next(startIndex, endIndex);
				//index2 = m_Random.Next(startIndex, endIndex);
				index2 = m_Random.Next(startIndex, list.Count);
			}
			else
			{
				int count = list.Count - startIndex + endIndex;
				index1 = m_Random.Next(0, count);
				if (index1 < list.Count - startIndex)
					index1 += startIndex;
				else
					index1 -= (list.Count - startIndex); 
				//index2 = m_Random.Next(0, count);
				index2 = m_Random.Next(0, list.Count);
				if (index2 < list.Count - startIndex)
					index2 += startIndex;
				else
					index2 -= (list.Count - startIndex);
			}
			
			DoInternalInvert(ref list, index1, index2);
		}

		private static void DoInternalInvert(ref IList list, int startIndex, int endIndex)
		{
			if (startIndex > endIndex)
			{
				// Sichern der ursprünglichen Reihenfolge, der zu invertierenden Kette
				ArrayList arrayTemp = new ArrayList(list.Count - startIndex + endIndex);
				for (int i = startIndex; i < list.Count; i++)
				{
					arrayTemp.Add(list[i]);
				}
				for (int i = 0; i < endIndex; i++)
				{
					arrayTemp.Add(list[i]);
				}
				
				// Invertieren
				for (int i = 0; i < list.Count - startIndex; i++)
				{
					list[i+ startIndex] = arrayTemp[arrayTemp.Count - i - 1];
				}
				for (int i = 0; i < endIndex; i++)
				{
					list[i] = arrayTemp[endIndex - i - 1];
				}
			}
			else
			{
				// Sichern der ursprünglichen Reihenfolge, der zu invertierenden Kette
				ArrayList arrayTemp = new ArrayList(endIndex - startIndex);
				for (int i = startIndex; i < endIndex; i++)
				{
					arrayTemp.Add(list[i]);
				}
			
				// Invertieren
				int count = endIndex - startIndex;
				for (int i = 0; i < count; i++)
				{
					list[endIndex - i - 1] = arrayTemp[i];
				}
			}
		}

		public static void DoInvert(ref IList list, int count)
		{
			if (list.Count == 0)
				return;

			int startIndex = m_Random.Next(0, list.Count);
			int endIndex = m_Random.Next(0, list.Count);

			DoInternalInvert(ref list, startIndex, endIndex);

			//int countInvert = m_Random.Next(1, 5);

			if (count > 1)
			{
				startIndex = m_Random.Next(0, list.Count);
				endIndex = m_Random.Next(0, list.Count);
				
				for (int i=0; i<count; i++)
				{
					DoInternalInvert(ref list, startIndex, endIndex);
				}
			}
		}
	}
}
