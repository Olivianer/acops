using System;
using System.Collections.Generic;
using System.Text;

namespace OKSearchRoomTest
{
    public class QueenConstellation
    {
        int[] _board;
        bool[,] _occupiedPositions;
        int _lastQueenOccupations;

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="boardDimension"></param>
        public QueenConstellation(int boardDimension)
        {
            _lastQueenOccupations = 0;
            _board = new int[boardDimension];
            for (int i = 0; i < _board.GetLength(0); i++)
            {
                _board[i] = -1;
            }
            _occupiedPositions = new bool[boardDimension, boardDimension];
        }

        /// <summary>
        /// Kopierkonstruktor
        /// </summary>
        /// <param name="Operand"></param>
        public QueenConstellation(QueenConstellation operand)
        {
            _lastQueenOccupations = 0;
            _board = new int[operand._board.GetLength(0)];
            operand._board.CopyTo(_board, 0);
            _occupiedPositions = new bool[operand._board.GetLength(0), operand._board.GetLength(0)];
            for (int i = 0; i < _occupiedPositions.GetLength(0); i++)
            {
                for (int j = 0; j < _occupiedPositions.GetLength(1); j++)
                {
                    _occupiedPositions[i, j] = operand._occupiedPositions[i, j];
                }
            }
        }

        public void Show()
        {
            System.Console.WriteLine(ToString());
        }

        public int BoardDimension
        {
            get
            {
                return _board.GetLength(0);
            }
        }

        public int LastQueenOccupations
        {
            get
            {
                return _lastQueenOccupations;
            }
        }

        public bool AllQueensOnTheBoard
        {
            get
            {
                foreach (int position in _board)
                {
                    if (position == -1)
                        return false;
                }
                return true;
            }
        }

        public int FreeColumn
        {
            get
            {
                for (int i = 0; i < _board.GetLength(0); i++)
                {
                    if (_board[i] == -1)
                        return i;
                }
                return -1;
            }
        }

        public override string ToString()
        {
            string result = String.Empty;
            foreach (int position in _board)
            {
                result += position.ToString();
                result += ",";
            }
            return result;
        }

        public bool TestQueenPosition(int column, int row)
        {
            // Pr¸fe, ob eine Dame diese Stelle schon abdeckt
            if (_occupiedPositions[column, row] == true)
                return false;
            return true;
        }

        public void SetQueen(int column, int row)
        {
            int i;
            int j;
            int sumNewOccupiedPositions = 0;
            // Set the queen position
            _board[column] = row;

            // Set the occupied position for the queen position
            _occupiedPositions[column, row] = true;

            // Set occupied positions from the queen and keep in mind the count of occupied positions

            // row
            // left
            i = column - 1;
            while (i >= 0)
            {
                if (_occupiedPositions[i, row] == false)
                {
                    _occupiedPositions[i, row] = true;
                    sumNewOccupiedPositions++;
                }
                i--;
            }

            // right
            i = column + 1;
            while (i < _board.GetLength(0))
            {
                if (_occupiedPositions[i, row] == false)
                {
                    _occupiedPositions[i, row] = true;
                    sumNewOccupiedPositions++;
                }
                i++;
            }

            // diagonal
            // top left
            i = column - 1;
            j = row - 1;
            while (i >= 0 && j >= 0)
            {
                if (_occupiedPositions[i, j] == false)
                {
                    _occupiedPositions[i, j] = true;
                    sumNewOccupiedPositions++;
                }
                i--;
                j--;
            }

            // down left
            i = column - 1;
            j = row + 1;
            while (i >= 0 && j < _board.GetLength(0))
            {
                if (_occupiedPositions[i, j] == false)
                {
                    _occupiedPositions[i, j] = true;
                    sumNewOccupiedPositions++;
                }
                i--;
                j++;
            }

            // top right
            i = column + 1;
            j = row - 1;
            while (i < _board.GetLength(0) && j >= 0)
            {
                if (_occupiedPositions[i, j] == false)
                {
                    _occupiedPositions[i, j] = true;
                    sumNewOccupiedPositions++;
                }
                i++;
                j--;
            }

            // down right
            i = column + 1;
            j = row + 1;
            while (i < _board.GetLength(0) && j < _board.GetLength(0))
            {
                if (_occupiedPositions[i, j] == false)
                {
                    _occupiedPositions[i, j] = true;
                    sumNewOccupiedPositions++;
                }
                i++;
                j++;
            }

            _lastQueenOccupations = sumNewOccupiedPositions;
        }
    }
}
