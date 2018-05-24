using System;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für Place.
	/// </summary>
	public class Place
	{
		double m_X;
		double m_Y;

		public Place()
		{
			m_X = 0;
			m_Y = 0;
		}

		public Place(double x, double y)
		{
			m_X = x;
			m_Y = y;
		}

		public double X
		{
			get
			{
				return m_X;
			}
			set
			{
				m_X = value;
			}
		}

		public double Y
		{
			get
			{
				return m_Y;
			}
			set
			{
				m_Y = value;
			}
		}

		public double GetDistance(Place place)
		{
			double diffX = m_X - place.m_X;
			double diffY = m_Y - place.m_Y;
			return Math.Sqrt(diffX * diffX + diffY * diffY);
		}
	}
}
