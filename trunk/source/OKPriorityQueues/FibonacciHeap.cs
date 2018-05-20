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
    public class FibonacciHeap<TKey, TValue> : IPriorityQueue<TKey, TValue>
    {
        #region "class node"
        /// <summary>
        /// nodocu
        /// </summary>
        /// <typeparam name="TNodeKey"></typeparam>
        /// <typeparam name="TNodeValue"></typeparam>
        internal class Node<TNodeKey, TNodeValue>
        {
            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> _left;
            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> _right;
            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> _parent;
            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> _firstChild;
            /// <summary>
            /// nodocu
            /// </summary>
            private int _countChildren;
            /// <summary>
            /// nodocu
            /// </summary>
            private bool _mark;
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
                _left = this;
                _right = this;
                _parent = null;
                _mark = false;
                _countChildren = 0;
            }

            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> Left
            {
                get { return _left; }
                set { _left = value; }
            }

            /// <summary>
            /// nodocu
            /// </summary>
            public Node<TNodeKey, TNodeValue> Right
            {
                get { return _right; }
                set { _right = value; }
            }

            /// <summary>
            /// nodocu
            /// </summary>
            private Node<TNodeKey, TNodeValue> Parent
            {
                get { return _parent; }
                set { _parent = value; }
            }

            /// <summary>
            /// nodocu
            /// </summary>
            public Node<TNodeKey, TNodeValue> FirstChild
            {
                get { return _firstChild; }
                set { _firstChild = value; }
            }

            /// <summary>
            /// nodocu
            /// </summary>
            public int CountChildren
            {
                get { return _countChildren; }
            }

            /// <summary>
            /// Fügt einen Knoten als rechten Nachbarn ein.
            /// </summary>
            /// <param name="node"></param>
            public void AddNeighbour(Node<TNodeKey, TNodeValue> node)
            {
                node.Extract();
                Node<TNodeKey, TNodeValue> helper = Right;
                Right = node;
                node.Right = helper;
                node.Left = this;
                helper.Left = node;
                node.Parent = _parent;
                if (_parent != null)
                    _parent._countChildren++;
            }

            /// <summary>
            /// nodocu
            /// </summary>
            /// <param name="node"></param>
            public void AddChild(Node<TNodeKey, TNodeValue> node)
            {
                if (node == this)
                    throw new Exception("Child and parent are the same.");
                if (FirstChild != null)
                    FirstChild.AddNeighbour(node);
                else
                {
                    node.Extract();
                    FirstChild = node;
                    node.Parent = this;
                    _countChildren = 1;
                }
                //System.Console.WriteLine("#" + node.GetHashCode() + " " + node.Key.ToString() + " is child of " + "#" + this.GetHashCode() + " " + this.Key);
            }

            /// <summary>
            /// Löst den Knoten heraus.
            /// </summary>
            public void Extract()
            {
                if (_parent != null)
                {
                    _parent._countChildren--;
                    if (_parent._countChildren == 0)
                        _parent.FirstChild = null;
                    else if (_parent.FirstChild == this)
                        _parent.FirstChild = Right;
                    _parent = null;
                }
                Left.Right = Right;
                Right.Left = Left;
                Left = this;
                Right = this;
            }

            /// <summary>
            /// nodocu
            /// </summary>
            /// <param name="depth"></param>
            /// <returns></returns>
            public string GetNodeAsString(int depth)
            {
                if (depth == 10)
                    return "";
                string s = "";
                s += _key + "#" + this.GetHashCode();
                if (this == FirstChild)
                    return s;
                if (FirstChild != null)
                {
                    s += " -> ";
                    Node<TNodeKey, TNodeValue> child = FirstChild;
                    do
                    {
                        s += child.GetNodeAsString(depth + 1);
                        child = child.Right;
                        //s += ", ";
                    }
                    while (child != FirstChild);
                }
                s += "; ";
                return s;
            }

            /// <summary>
            /// Eine Markierung ist auf true, wenn:
            /// 1. Der Knoten war eine Wurzel in der Wurzelliste
            /// 2. Der Knoten wird zum Kind eines anderen Knoten gemacht
            /// 3. Der Knoten verliert ein Kind.
            /// </summary>
            public bool Mark
            {
                get { return _mark; }
                set { _mark = value; }
            }

            public TNodeValue Value
            {
                get { return _value; }
            }

            public TNodeKey Key
            {
                get { return _key; }
            }
        }

        #endregion

        /// <summary>
        /// nodocu
        /// </summary>
        private Node<TKey, TValue> _min;
        /// <summary>
        /// nodocu
        /// </summary>
        private int _countNodes;
        /// <summary>
        /// nodocu
        /// </summary>
        private IComparer<TKey> _comparer;

        /// <summary>
        /// Konstruktor
        /// </summary>
        public FibonacciHeap()
        {
            _comparer = null;
            _countNodes = 0;
        }

        /// <summary>
        /// nodocu
        /// </summary>
        /// <param name="comparer"></param>
        public FibonacciHeap(IComparer<TKey> comparer)
        {
            _comparer = comparer;
            _countNodes = 0;
        }

        /// <summary>
        /// Liefert die Anzahl der Elemente
        /// </summary>
        public int Count
        {
            get { return _countNodes; }
        }

        /// <summary>
        /// Löscht alle Elemente.
        /// </summary>
        public void Clear()
        {
            _min = null;
            _countNodes = 0;
        }

        /// <summary>
        /// Liefert den Wert, der den kleinsten Key enthält.
        /// Die Kosten betragen O(1).
        /// </summary>
        /// <returns></returns>
        public TValue Peek()
        {
            return _min.Value;
        }

        /// <summary>
        /// Liefert den obersten Schlüssel innerhalb der Queue.
        /// </summary>
        /// <returns></returns>
        public TKey PeekKey()
        {
            return _min.Key;
        }

        /// <summary>
        /// Das kleinste Element wird aus dem Heap entfernt und dessen Wert wird zurückgegeben. Existiert kein Element im Heap wird null zurückgegeben.
        /// </summary>
        /// <returns></returns>
        public TValue Pop()
        {
            if (_min == null)
                return default(TValue);
            Node<TKey, TValue> result = _min;
            if (_min.FirstChild == null)
            {
                if (_min == _min.Right)
                {
                    _min = null;
                }
                else
                {
                    Node<TKey, TValue> helper = _min.Right;
                    _min.Extract();

                    // Setze neues provisorisches m_Min
                    _min = helper;
                }
            }
            else
            {
                // Füge Kinder von m_Min zur Wurzelliste hinzu
                Node<TKey, TValue> child;
                while (_min.FirstChild != null)
                {
                    child = _min.FirstChild;
                    _min.AddNeighbour(child);
                }
                // Entferne m_Min und setze neues provisorische m_Min
                Node<TKey, TValue> helper2 = _min.Right;
                _min.Extract();
                _min = helper2;
            }
            _countNodes--;

            //System.Console.WriteLine(ToString());

            // Räume auf, und bestimme wahres m_Min
            CleanUp();

            //			if (m_Min != null)
            //				System.Console.WriteLine("After Pop: "+ m_Min.Key.ToString());
            //			else
            //				System.Console.WriteLine("After Pop: leer");
            return result.Value;
        }

        /// <summary>
        /// Ein Element mit Key wird als Knoten dem Heap hinzugefügt. Dabei geschieht nichtsweiter, als das
        /// der Knoten in der Wurzelliste eingefügt wird. Die Kosten betragen O(1).
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Push(TKey key, TValue value)
        {
            //			System.Console.WriteLine("Push "+ key.ToString());
            if (_comparer == null && key.GetType().GetInterface("IComparable", false) == null)
                throw new Exception("The Key has to implement the IComparable interface");

            Node<TKey, TValue> node = new Node<TKey, TValue>(key, value);
            if (_min == null)
            {
                _min = node;
            }
            else
            {
                // Einfügen des Knotens in die Wurzelliste rechts neben den minimalen Knoten
                _min.AddNeighbour(node);

                if (Compare(node, _min) < 0)
                    _min = node;
            }
            _countNodes++;
            //			System.Console.WriteLine("After Push: "+ m_Min.Key.ToString());
        }

        /// <summary>
        /// Vereinigt zwei FibonacciHeaps zu einem neuen FibonacciHeap. Dabei werden die übergebenen Heaps
        /// danach geleert. Die Kosten betragen O(1).
        /// </summary>
        /// <param name="heap1"></param>
        /// <param name="heap2"></param>
        /// <returns></returns>
        public static FibonacciHeap<TKey, TValue> Union(FibonacciHeap<TKey, TValue> heap1, FibonacciHeap<TKey, TValue> heap2)
        {
            FibonacciHeap<TKey, TValue> result;

            if (heap1._comparer != null)
                result = new FibonacciHeap<TKey, TValue>(heap1._comparer);
            else if (heap2._comparer != null)
                result = new FibonacciHeap<TKey, TValue>(heap2._comparer);
            else
                result = new FibonacciHeap<TKey, TValue>();

            // Festlegen des neuen minimalen Knotens
            if (heap1._min == null)
            {
                if (heap2._min != null)
                {
                    result._min = heap2._min;
                    result._countNodes = heap2._countNodes;
                }
            }
            else if (heap2._min == null)
            {
                if (heap1._min != null)
                {
                    result._min = heap1._min;
                    result._countNodes = heap1._countNodes;
                }
            }
            else
            {
                if (heap1.Compare(heap1._min, heap2._min) < 0)
                    result._min = heap1._min;
                else
                    result._min = heap2._min;

                // Verketten beider Wurzellisten
                heap1._min.AddNeighbour(heap2._min);
                result._countNodes = heap1._countNodes + heap2._countNodes;
            }



            heap1.Clear();
            heap2.Clear();

            return result;
        }

        private int Compare(Node<TKey, TValue> node1, Node<TKey, TValue> node2)
        {
            if (_comparer != null)
                return _comparer.Compare(node1.Key, node2.Key);
            else
                return ((IComparable)node1.Key).CompareTo(((IComparable)node2.Key));
        }

        /// <summary>
        /// Aufräumen nach Extrahieren des Minimums aus dem Fibonacci-Heap
        /// </summary>
        private void CleanUp()
        {
            if (_min == null)
                return;
            if (_min == _min.Right)
                return;
            List<Node<TKey, TValue>> A = new List<Node<TKey, TValue>>();
            Node<TKey, TValue> node = _min.Right;

            Node<TKey, TValue> y;
            Node<TKey, TValue> x;
            int maxDegree = 0;

            do
            {
                x = node;
                node = node.Right;
                int degree = x.CountChildren;

                // Erhöhe die ArrayList, falls notwendig
                while (degree >= A.Count)
                    A.Add(null);

                while (degree < A.Count && A[degree] != null)
                {
                    y = A[degree];
                    if (x == y)
                        throw new Exception("x is equal to y");
                    if (Compare(y, x) < 0)
                    {
                        Node<TKey, TValue> helper = x;
                        x = y;
                        y = helper;
                    }
                    // Entferne y aus der Wurzelliste und mache y zu einem Sohn von x
                    x.AddChild(y);
                    // Entferne die Markierung von y
                    y.Mark = false;
                    A[degree] = null;
                    degree++;
                }
                // Erhöhe die ArrayList, falls notwendig
                if (degree >= A.Count)
                    A.Add(null);

                A[degree] = x;
                if (maxDegree < degree)
                    maxDegree = degree;
            }
            while (node != _min);

            node = null;
            for (int i = 0; i <= maxDegree; i++)
            {
                if (A[i] == null)
                    continue;
                if (node == null)
                    node = A[i];
                else
                    node.AddNeighbour(A[i]);
                // Ist node neuer minimaler Knoten des Baumes?
                if (Compare(A[i], _min) <= 0)
                    _min = A[i];
            }
        }

        /// <summary>
        /// Anzeige des FibonacciHeaps als String
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string s = "";
            s += "Count: " + _countNodes + " ";
            if (_min == null)
                return s;
            Node<TKey, TValue> node = _min;
            do
            {
                s += node.GetNodeAsString(0);
                node = node.Right;
            }
            while (_min != node);
            return s;
        }

        /// <summary>
        /// nodocu
        /// </summary>
        public IEnumerable<TValue> Nodes
        {
            get
            {
                TValue[] valueList = new TValue[_countNodes];
                Node<TKey, TValue> node = _min;
                int counter=0;
                do
                {
                    FillNode(node, valueList, ref counter);
                    node = node.Right;
                }
                while (_min != node);
                return valueList;
            }
        }

        internal void FillNode(Node<TKey, TValue> node, TValue[] valueList, ref int counter)
        {
            valueList[counter] = node.Value;
            counter++;
            if (node == node.FirstChild)
                return;
            if (node.FirstChild != null)
            {
                Node<TKey, TValue> child = node.FirstChild;
                do
                {
                    FillNode(child, valueList, ref counter);
                    child = child.Right;
                }
                while (child != node.FirstChild);
            }
        }
    }
}
