using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraints
{
    /// <summary>
    /// Klasse zur Ausgabe einer Exception bei einer Verletzung der Konsistenz
    /// </summary>
    public class ConsistencyException : Exception
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="message">Nachricht der Exception</param>
        public ConsistencyException(string message)
            : base(message)
        { }
    }
}
