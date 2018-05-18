///////////////////////////////////////////////////////////
//  INode.cs
//  Implementation of the Interface INode
//  Generated by Enterprise Architect
//  Created on:      05-Nov-2006 11:44:49
//  Original author: Oliver Kuehne
///////////////////////////////////////////////////////////


namespace OKSearchRoom {
    /// <summary>
    /// Beschreibt ein Interface, das einen Suchbaumknoten darstellt
    /// </summary>
	public interface INode 
	{
        /// <summary>
        /// Liefert oder setzt das vom Knoten eingebettete Objekt.
        /// </summary>
        object Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gibt die Suchtiefe des Knotens zur�ck.
        /// </summary>
        int Depth
        {
            get;
        }

        /// <summary>
        /// Renitialisiert den Knoten
        /// </summary>
        void Clear();


	}//end INode

}//end namespace OKSearchRoom