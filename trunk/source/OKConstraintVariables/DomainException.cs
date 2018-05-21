using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraintVariables
{
    /// <summary>
    /// Wenn etwas innerhalb der Domäne schief geht, wird diese Exception geworfen
    /// </summary>
    public class DomainException : Exception
    {
        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="message">Nachricht der Exception</param>
        public DomainException(string message)
            : base(message)
        { }
    }
}
