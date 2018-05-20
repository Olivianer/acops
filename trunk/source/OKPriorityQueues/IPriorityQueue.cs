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
    public interface IPriorityQueue<TKey, TValue>
    {
        /// <summary>
        /// Liefert die Anzahl der Elemente
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Löscht alle Elemente.
        /// </summary>
        void Clear();

        /// <summary>
        /// Liefert den obersten Wert innerhalb der Queue.
        /// </summary>
        /// <returns></returns>
        TValue Peek();

        /// <summary>
        /// Liefert den obersten Schlüssel innerhalb der Queue.
        /// </summary>
        /// <returns></returns>
        TKey PeekKey();

        /// <summary>
        /// Entfernt das oberste Element und gibt es zurück. 
        /// </summary>
        /// <returns></returns>
        TValue Pop();

        /// <summary>
        /// Fügt ein neues Element ein.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Push(TKey key, TValue value);

        /// <summary>
        /// nodocu
        /// </summary>
        IEnumerable<TValue> Nodes
        {
            get;
        }
    }
}
