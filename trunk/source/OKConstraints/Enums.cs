using System;
using System.Collections.Generic;
using System.Text;

namespace OKConstraints
{
    /// <summary>
    /// Gibt den Typ der verwendeten Dom�ne an.
    /// </summary>
    public enum DomainType 
    {
        /// <summary>
        /// Die Werte werden in einer Liste von Intervallen gehalten.
        /// </summary>
        /// <remarks>
        /// So ist es z.B. m�glich die Werte 2,3,4,5,6 als Interval von 2-6 zu halten.
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
        /// Diese Domain kann ausschlie�lich f�r Boolsche Probleme verwendet werden.
        /// </summary>
        BoolDomain 
    };

    /// <summary>
    /// Gibt den verwendeten Typ des Knotens zur�ck.
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
        /// Dies erm�glicht eine Navigation vom Start- zum Zielknoten, oder umgekehrt.
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
    /// Gibt an, welche Konsistenzpr�fung scheiterte, oder ob die Konsistenzpr�fung erfolgreich war.
    /// </summary>
    public enum ConsistencyCheck 
    {
        /// <summary>
        /// Die Konsistenzpr�fung war erfolgreich.
        /// </summary>
        OK = 1,
        /// <summary>
        /// Die Knotenkonsistenzpr�fung ist gescheitert.
        /// </summary>
        NoNodeConsistency,
        /// <summary>
        /// Die Kantenkonsistenzpr�fung ist gescheitert.
        /// </summary>
        NoArcConistency,
        /// <summary>
        /// Die Grenzenkonsistenzpr�fung ist gescheitert.
        /// </summary>
        NoBoundsConsistency 
    };

    /// <summary>
    /// Gibt an, an welcher Stelle eine Konsistenzpr�fung stattfindet
    /// </summary>
    public enum ConsistencyCheckRegion 
    {
        /// <summary>
        /// Die Konsistenzpr�fung findet bei Start der Suche statt.
        /// </summary>
        Start = 1,
        /// <summary>
        /// Die Konsistenzpr�fung findet bei jeder Erzeugung eines neuen Knotens statt.
        /// </summary>
        EachNodeGenerating,
        /// <summary>
        /// Die Konsistenzpr�fung findet nach jeder gefundenen L�sung statt.
        /// </summary>
        EachSolution 
    };

    /// <summary>
    /// Beschreibt den Typ der Konsistenzpr�fung
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
        /// Simplex-Verfahren f�r lineare Probleme
        /// </summary>
        Simplex
    };
}
