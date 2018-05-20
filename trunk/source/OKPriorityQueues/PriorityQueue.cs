using System;
using System.Collections.Generic;
using System.Text;

namespace OKPriorityQueues
{
    /// <summary>
    /// nodocu
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class PriorityQueue<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        #region class node
        /// <summary>
        /// nodocu
        /// </summary>
        /// <typeparam name="TNodeKey"></typeparam>
        /// <typeparam name="TNodeValue"></typeparam>
        protected class Node<TNodeKey, TNodeValue>
        {
            /// <summary>
            /// nodocu
            /// </summary>
            private TNodeKey _key;
            /// <summary>
            /// nodocu
            /// </summary>
            private TNodeValue _value;

            /// <summary>
            /// nodocu
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            public Node(TNodeKey key, TNodeValue value)
            {
                _key = key;
                _value = value;
            }

            /// <summary>
            /// nodocu
            /// </summary>
            public TNodeKey Key
            {
                get
                {
                    return _key;
                }
            }

            /// <summary>
            /// nodocu
            /// </summary>
            public TNodeValue Value
            {
                get
                {
                    return _value;
                }
            }
        }
        #endregion

        /// <summary>
        /// nodocu
        /// </summary>
        private List<Node<TKey, TValue>> _array;
        /// <summary>
        /// nodocu
        /// </summary>
        private IComparer<TKey> _comparer;

        /// <summary>
        /// nodocu
        /// </summary>
        public PriorityQueue()
        {
            _array = new List<Node<TKey, TValue>>();
            _comparer = null;
        }

        /// <summary>
        /// nodocu
        /// </summary>
        public IEnumerable<TValue> Nodes
        {
            get
            {
                TValue[] valueList = new TValue[_array.Count];
                for (int i = 0; i < _array.Count; i++ )
                {
                    valueList[i] = _array[i].Value;
                }
                return valueList;
            }
        }

        /// <summary>
        /// nodocu
        /// </summary>
        /// <param name="comparer"></param>
        public PriorityQueue(IComparer<TKey> comparer)
        {
            _array = new List<Node<TKey, TValue>>();
            _comparer = comparer;
        }

        /// <summary>
        /// Fügt ein neues Element ein.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Push(TKey key, TValue value)
        {
            if (value == null)
                throw new Exception("The given value is null");
            if (_comparer == null && key.GetType().GetInterface("IComparable", false) == null)
                throw new Exception("The Key has to implement the IComparable interface");

            Node<TKey, TValue> node = new Node<TKey, TValue>(key, value);

            int p, p2;
            p = _array.Count;
            _array.Add(node);
            Node<TKey, TValue> helper;

            do
            {
                if (p == 0)
                    break;
                p2 = (p - 1) / 2;
                if (Compare(_array[p], _array[p2]) < 0)
                {
                    helper = _array[p];
                    _array[p] = _array[p2];
                    _array[p2] = helper;
                    p = p2;
                }
                else
                    break;
            } while (true);
        }

        /// <summary>
        /// nodocu
        /// </summary>
        /// <param name="node1"></param>
        /// <param name="node2"></param>
        /// <returns></returns>
        private int Compare(Node<TKey, TValue> node1, Node<TKey, TValue> node2)
        {
            if (_comparer != null)
                return _comparer.Compare(node1.Key, node2.Key);
            else
                return ((IComparable)node1.Key).CompareTo(((IComparable)node2.Key));
        }

        /// <summary>
        /// Entfernt das oberste Element und gibt es zurück. 
        /// </summary>
        /// <returns></returns>
        public TValue Pop()
        {
            if (_array.Count == 0)
                return default(TValue);

            int p, p1, p2, pn;
            p = 0;
            Node<TKey, TValue> helper;
            Node<TKey, TValue> result = _array[0];
            _array[0] = _array[_array.Count - 1];
            _array.RemoveAt(_array.Count - 1);

            do
            {
                pn = p;
                p1 = 2 * p + 1;
                p2 = 2 * p + 2;
                if (_array.Count > p1 && (Compare(_array[p], _array[p1]) > 0)) // links kleiner
                    p = p1;
                if (_array.Count > p2 && (Compare(_array[p], _array[p2]) > 0)) // rechts noch kleiner
                    p = p2;

                if (p == pn)
                    break;

                helper = _array[p];
                _array[p] = _array[pn];
                _array[pn] = helper;
            } while (true);

            return result.Value;
        }

        /// <summary>
        /// Liefert den obersten Wert innerhalb der Queue.
        /// </summary>
        /// <returns></returns>
        public TValue Peek()
        {
            if (_array.Count == 0)
                return default(TValue);

            return _array[0].Value;
        }

        /// <summary>
        /// Liefert den obersten Schlüssel innerhalb der Queue.
        /// </summary>
        /// <returns></returns>
        public TKey PeekKey()
        {
            if (_array.Count == 0)
                return default(TKey);

            return _array[0].Key;
        }

        /// <summary>
        /// Löscht alle Elemente.
        /// </summary>
        public void Clear()
        {
            _array.Clear();
        }

        /// <summary>
        /// Liefert die Anzahl der Elemente
        /// </summary>
        public int Count
        {
            get
            {
                return _array.Count;
            }
        }
    }
}
