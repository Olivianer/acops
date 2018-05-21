using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraints
{
    /// <summary>
    /// Gibt den Typ der verwendeten Domäne an.
    /// </summary>
    public enum DomainType 
    {
        /// <summary>
        /// Die Werte werden in einer Liste von Intervallen gehalten.
        /// </summary>
        /// <remarks>
        /// So ist es z.B. möglich die Werte 2,3,4,5,6 als Interval von 2-6 zu halten.
        /// Der Vorteil liegt darin, dass dadurch Speicheplatz eingespart wird. Der Zugriff wird aber langsamer
        /// </remarks>
        IntervalDomain = 1,
        /// <summary>
        /// Alle Werte werden in einer Liste gehalten.
        /// </summary>
        /// <remarks>
        /// Der Zugriff ist schnell, aber der Speicherbedarf ist hoch
        /// </remarks>
        ValueDomain,
        /// <summary>
        /// Diese Domain kann ausschließlich für Boolsche Probleme verwendet werden.
        /// </summary>
        BoolDomain 
    };

    /// <summary>
    /// Gibt den verwendeten Typ des Knotens zurück.
    /// </summary>
    public enum NodeType 
    {
        /// <summary>
        /// Der normale Knotentyp
        /// </summary>
        Node = 1,
        /// <summary>
        /// Bei diesem Knotentyp werden die Eltern-Kind-Beziehungen beibehalten.
        /// </summary>
        /// <remarks>
        /// Dies ermöglicht eine Navigation vom Start- zum Zielknoten, oder umgekehrt.
        /// </remarks>
        TreeNode 
    };

    /// <summary>
    /// Gibt an welche Suchmethode verwendet wird.
    /// </summary>
    public enum SearchMethod 
    {
        /// <summary>
        /// Die Breitensuche.
        /// </summary>
        BreadthFirstSearch = 1,
        /// <summary>
        /// Die Tiefensuche. 
        /// </summary>
        DepthFirstSearch,
        /// <summary>
        /// Die iterative Tiefensuche.
        /// </summary>
        IterativeDepthFirstSearch,
        /// <summary>
        /// Die Suchmethode nach dem Bergsteigerprinzip
        /// </summary>
        HillClimbing,
        /// <summary>
        /// Die Suchmethode nach dem Bergsteigerprinzip, die ab und zu nicht nur gierig agiert.
        /// </summary>
        SoftHillClimbing 
    };

    /// <summary>
    /// Gibt an, welche Konsistenzprüfung scheiterte, oder ob die Konsistenzprüfung erfolgreich war.
    /// </summary>
    public enum ConsistencyCheck 
    {
        /// <summary>
        /// Die Konsistenzprüfung war erfolgreich.
        /// </summary>
        OK = 1,
        /// <summary>
        /// Die Knotenkonsistenzprüfung ist gescheitert.
        /// </summary>
        NoNodeConsistency,
        /// <summary>
        /// Die Kantenkonsistenzprüfung ist gescheitert.
        /// </summary>
        NoArcConistency,
        /// <summary>
        /// Die Grenzenkonsistenzprüfung ist gescheitert.
        /// </summary>
        NoBoundsConsistency 
    };

    /// <summary>
    /// Gibt an, an welcher Stelle eine Konsistenzprüfung stattfindet
    /// </summary>
    public enum ConsistencyCheckRegion 
    {
        /// <summary>
        /// Die Konsistenzprüfung findet bei Start der Suche statt.
        /// </summary>
        Start = 1,
        /// <summary>
        /// Die Konsistenzprüfung findet bei jeder Erzeugung eines neuen Knotens statt.
        /// </summary>
        EachNodeGenerating,
        /// <summary>
        /// Die Konsistenzprüfung findet nach jeder gefundenen Lösung statt.
        /// </summary>
        EachSolution 
    };

    /// <summary>
    /// Beschreibt den Typ der Konsistenzprüfung
    /// </summary>
    public enum ConsistencyType 
    {
        /// <summary>
        /// Knotenkonsistenz
        /// </summary>
        Node = 1,
        /// <summary>
        /// Kantenkonsistenz
        /// </summary>
        Arc,
        /// <summary>
        /// Grenzenkonsistenz
        /// </summary>
        Bounds 
    };

    /// <summary>
    /// Gibt an welches Optimierungsverfahren verwendet wird.
    /// </summary>
    public enum OptimizationMethod
    {
        /// <summary>
        /// BranchAndBound-Verfahren
        /// </summary>
        BranchAndBound = 1,
        /// <summary>
        /// Simplex-Verfahren für lineare Probleme
        /// </summary>
        Simplex
    };
}
