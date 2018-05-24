using System;

namespace OKSearchRoom
{
	/// <summary>
	/// Zusammendfassende Beschreibung f�r IEvolutionarySearchProblem.
	/// </summary>
	public interface IEvolutionaryProblem : IProblem
	{
		Node DoRecombination(Node father, Node mother);
		void DoMutation(ref Node individual);
		double GetObjectiveFunction(Node node);
		int ChildrenPopulation
		{
			get;
			set;
		}
		int ParentPopulation
		{
			get;
			set;
		} 
	}
}
