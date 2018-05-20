using System;

namespace OKHeuristicSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für LloydPuzzleSituation.
	/// </summary>
	public class LloydPuzzleSituation
	{
		public enum EDirection{ Left, Right, Up, Down };
		private int[]	m_Situation;
		int				m_IndexHole = int.MinValue;
		
        public LloydPuzzleSituation(int[] situation)
		{
			m_Situation = (int[])situation.Clone();
            for (int i = 0; i < m_Situation.Length; i++)
            {
                if (m_Situation[i] == 0)
                {
                    m_IndexHole = i;
                    break;
                }
            }

            if (m_IndexHole == int.MinValue)
                throw new Exception("There is no hole into the puzzle.");
		}

		public LloydPuzzleSituation(int[] situation, int indexHole)
		{
			m_Situation = (int[])situation.Clone();
			if (m_Situation[indexHole] != 0)
				throw new Exception("The index of the hole is wrong");
			m_IndexHole = indexHole;
		}

		public LloydPuzzleSituation(LloydPuzzleSituation Operand)
		{
			m_Situation = (int[])Operand.m_Situation.Clone();
			m_IndexHole = Operand.m_IndexHole;
		}

		public int this[int index]
		{
			get
			{
				return m_Situation[index];
			}
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public override bool Equals(object o)
		{
			if (o.GetType()!=this.GetType())
				return false;
			return ((LloydPuzzleSituation)o)==this;
		}

		public static bool operator==(LloydPuzzleSituation situation1, LloydPuzzleSituation situation2)
		{
			if (situation1.m_Situation.GetLength(0) != situation2.m_Situation.GetLength(0))
				return false;

			for(int i=0; i<situation1.m_Situation.GetLength(0); i++)
			{
				if (situation1.m_Situation[i] != situation2.m_Situation[i])
					return false;
			}
			return true;
		}

		public static bool operator!=(LloydPuzzleSituation situation1, LloydPuzzleSituation situation2)
		{
			return !(situation1 == situation2);
		}

		public int Dimension
		{
			get
			{
				return (int)Math.Sqrt(m_Situation.GetLength(0));
			}
		}

		public bool TestDirection(EDirection direction)
		{
			bool test=false;
			int dimension = Dimension;

			switch(direction)
			{
				case EDirection.Left:
					if (m_IndexHole%dimension == 0)
						test = false;
					else
						test = true;
					break;
				case EDirection.Right:
					if (m_IndexHole%dimension == (dimension - 1))
						test = false;
					else
						test = true;
					break;
				case EDirection.Up:
					if (m_IndexHole/dimension == 0)
						test = false;
					else
						test = true;
					break;
				case EDirection.Down:
					if (m_IndexHole/dimension == (dimension - 1))
						test = false;
					else
						test = true;
					break;
			}

			return test; 
		}

		public void DoMove(EDirection direction)
		{
			int dimension = Dimension;

			switch(direction)
			{
				case EDirection.Left:
					m_Situation[m_IndexHole] = m_Situation[m_IndexHole - 1];
					m_Situation[m_IndexHole - 1] = 0;
					m_IndexHole--;
					break;
				case EDirection.Right:
					m_Situation[m_IndexHole] = m_Situation[m_IndexHole + 1];
					m_Situation[m_IndexHole + 1] = 0;
					m_IndexHole++;
					break;
				case EDirection.Up:
					m_Situation[m_IndexHole] = m_Situation[m_IndexHole - dimension];
					m_Situation[m_IndexHole - dimension] = 0;
					m_IndexHole = m_IndexHole - dimension;
					break;
				case EDirection.Down:
					m_Situation[m_IndexHole] = m_Situation[m_IndexHole + dimension];
					m_Situation[m_IndexHole + dimension] = 0;
					m_IndexHole = m_IndexHole + dimension;
					break;
			}
		}

		public int GetDistance(int index1, int index2)
		{
			int distance = 0;
			int dimension = Dimension;

			if (index1 < 0 || index1 >= m_Situation.GetLength(0) || index2 < 0 || index2 >= m_Situation.GetLength(0))
			{
				return -1;
				//throw
			}
	
			distance += Math.Abs(index1/dimension - index2/dimension);
			distance += Math.Abs(index1%dimension - index2%dimension);
			return distance;
		}

		public int IndexOf(int number)
		{
			for (int i=0; i<m_Situation.GetLength(0); i++)
			{
				if (m_Situation[i] == number)
					return i;
			}

			//throw
			return -1;
		}

		public override string ToString()
		{
			string output = "";
			foreach(int element in m_Situation)
			{
				output += element.ToString();
			}
			return output;
		}

		public void Show()
		{
			System.Console.WriteLine(ToString());
		}
	}
}
