using System;
using System.Collections;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für PlaceList.
	/// </summary>
	public class PlaceList : IList, IEnumerator
	{
		private ArrayList m_Places;
		private int m_Index = -1;

		public PlaceList()
		{
			m_Places = new ArrayList();
		}

		#region "IEnumerator Implemantation"

		object IEnumerator.Current
		{
			get
			{
				return m_Places[m_Index];
			}
		}

		public Place Current
		{
			get
			{
				return (Place)m_Places[m_Index];
			}
		}

		public bool MoveNext()
		{
			m_Index++;
			return m_Index < m_Places.Count;
		}

		public void Reset()
		{
			m_Index = -1;
		}  

		public IEnumerator GetEnumerator()
		{
			return this;
		}
        
		#endregion //IEnumerator Implemantation

		#region "ICollection Implemantation"

		public int Count
		{
			get
			{
				return m_Places.Count;
			}
		}

		public object SyncRoot
		{
			get{return this;}
		}         

		public bool IsSynchronized
		{
			get{return false;}
		}                         

		public void CopyTo(Array a, int index)
		{
			m_Places.CopyTo(a, index);
		}

		#endregion //"ICollection Implemantation"

		#region "IList Implementation"

		object IList.this[int index]
		{
			get
			{
				return m_Places[index];
			}
			set
			{
				m_Places[index] = value;
			}
		}

		public Place this[int index]
		{
			get
			{
				return (Place) m_Places[index];
			}
			set
			{
				m_Places[index] = value;
			}
		}

		public void Clear()
		{
			m_Places.Clear();
		}

		bool IList.Contains(object o)
		{
			return m_Places.Contains(o);
		}

		public bool Contains(Place place)
		{
			return m_Places.Contains(place);
		}

		int IList.IndexOf(object o)
		{
			return m_Places.IndexOf(o);
		}

		public int IndexOf(Place place)
		{
			return m_Places.IndexOf(place);
		}

		int IList.Add( object o)
		{
			return m_Places.Add(o);
		}

		public int Add( Place place)
		{
			return m_Places.Add(place);
		}

		void IList.Insert(int index, object o)
		{
			m_Places.Insert(index, o);
		}

		public void Insert(int index, Place place)
		{
			m_Places.Insert(index, place);
		}

		public void RemoveAt(int index)
		{
			m_Places.RemoveAt(index);
		}

		void IList.Remove(object o)
		{
			m_Places.Remove(o);
		}

		public void Remove(Place place)
		{
			m_Places.Remove(place);
		}

		public bool IsFixedSize
		{
			get{return false;}
		}

		public bool IsReadOnly
		{
			get{return false;}
		}

			#endregion //"IList Implemantation"

	}
}
