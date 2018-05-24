using System;
using System.Collections;

namespace OKSearchRoomTest
{
	/// <summary>
	/// Zusammendfassende Beschreibung für Matrix.
	/// </summary>
	public class Matrix
	{
		private ArrayList m_Rows;

		public Matrix()
		{
			m_Rows = new ArrayList();
		}

		public Matrix(Matrix matrix)
		{
			m_Rows = new ArrayList(matrix.RowCount);
			foreach(ArrayList arrayList in m_Rows)
			{
				ArrayList column = new ArrayList(arrayList);
				m_Rows.Add(column);
			}
		}

		public Matrix(int countRows, int countColumns)
		{
			m_Rows = new ArrayList(countRows);
			for(int i=0; i<countRows; i++)
			{
				ArrayList column = new ArrayList(countColumns);
				for (int j=0; j<countColumns; j++)
				{
					column.Add(null);
				}
				m_Rows.Add(column);
			}
		}

		public void AddRow( object[] row)
		{
			/*if (m_Rows == null)
				m_Rows = new ArrayList(1);

			// Wenn sich die Spaltenbreite erhöht, alle Zeilen auf gleiche Länge bringen
			if (row.GetLength(0) > m_CountColumns)
			{
				m_CountColumns = row.GetLength(0);
				for (int i=0; i<m_Rows.Count; i++)
				{
					while(((ArrayList)m_Rows[i]).Count < m_CountColumns)
					{
						((ArrayList)m_Rows[i]).Add(null);
					}
				}
			}

			ArrayList column = new ArrayList(row.GetLength(0));

			for (int i=0; i<row.GetLength(0); i++)
			{
				column.Add(row[i]);
			}

			// Wenn die übergebene Zeile eine kleine Spaltenbreite hat, wird der Rest mit null aufgefüllt
			while (column.Count < m_CountColumns)
			{
				column.Add(null);
			}

			m_CountColumns = column.Count;

			m_Rows.Add(column);*/
		}

		public void AddColumn( object[] column)
		{
			/*if (m_Rows == null)
				m_Rows = new ArrayList(column.GetLength(0));

			// Wenn sich die Zeilenhöhe erhöht, alle Spalten auf gleiche Zeilenhöhe bringen
			while (column.GetLength(0) > m_Rows.Count)
			{
				ArrayList column = new ArrayList(m_CountColumns);
				for (i=0; i<m_CountColumns; i++)
				{
					column.Add(null);
				}
				m_Rows.Add(column);
			}

			for (int i=0; i<column.GetLength(0); i++)
			{
				((ArrayList)m_Rows[i]).Add(column[i]);
			}

			// Wenn die übergeben Zeilenhöhe zu klein ist, wird der Rest mit null aufgefüllt
			for (int i=column.GetLength(0); i<m_Rows[i]; i++)
			{
				((ArrayList)m_Rows[i]).Add(null);
			}*/
		}

		public void SetValue(int row, int column, object value)
		{
			if (RowCount <= row || ColumnCount <= column)
				throw new Exception("Wrong index");
			((ArrayList)m_Rows[row])[column] = value;
		}

		public object GetValue(int row, int column)
		{
			if (RowCount <= row || ColumnCount <= column)
				throw new Exception("Wrong index");
			return ((ArrayList)m_Rows[row])[column];
		}

		public int RowCount
		{
			get
			{
				return m_Rows.Count;
			}
		}

		public int ColumnCount
		{
			get
			{
				if (m_Rows.Count>0)
					return ((ArrayList)m_Rows[0]).Count;
				else
					return 0;
			}
		}
	}
}
