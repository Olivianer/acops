using System;
using System.Collections;
using System.Collections.Generic;
using OKSearchRoom;

namespace OKSearchRoomTest
{
    /// <summary>
    /// Zusammendfassende Beschreibung für LloydPuzzleProblem.
    /// </summary>
    public class LloydPuzzleProblem : ISearchProblem
    {
        #region Private Member
        private INode m_Begin;
        private INode m_Destination;
        private bool m_FoundSolution = false;
        private List<LloydPuzzleSituation> m_CreatedSituations = new List<LloydPuzzleSituation>();
        private bool m_KeepCreatedSituationsinMemory;
        #endregion

        #region Construction
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="begin"></param>
        /// <param name="destination"></param>
        public LloydPuzzleProblem(Node begin, Node destination, bool keepCreatedSituationsinMemory)
        {
            m_Begin = begin;
            m_Destination = destination;
            m_KeepCreatedSituationsinMemory = keepCreatedSituationsinMemory;
        }
        #endregion

        #region Internal Member
        internal INode Begin
        {
            get
            {
                return m_Begin;
            }
        }

        internal INode Destination
        {
            get
            {
                return m_Destination;
            }
        }
        #endregion

        #region Public Member
        public bool FoundSolution
        {
            get
            {
                return m_FoundSolution;
            }
        }
        #endregion

        #region Private Functions
        private bool SituationAlreadyCreated(LloydPuzzleSituation situation)
        {
            foreach (LloydPuzzleSituation sit in m_CreatedSituations)
            {
                if (sit == situation)
                    return true;
            }
            return false;
        }
        #endregion

        #region ISearchProblem Members

        public INode[] GenerateChildren(INode node, int maxCount)
        {
            if (node == null)
            {
                throw new Exception("LLoydPuzzleProblem: The given node for GenerateChildren() is null.");
            }

            if (node is TreeNode == false)
            {
                throw new Exception("LLoydPuzzleProblem: The given node for GenerateChildren() has to be a tree node.");
            }

            TreeNode parentNode = node as TreeNode;

            List<INode> generatedNodes = new List<INode>();

            LloydPuzzleSituation situation = (LloydPuzzleSituation)node.Data;


            if (situation.TestDirection(LloydPuzzleSituation.EDirection.Up))
            {
                LloydPuzzleSituation sit = new LloydPuzzleSituation(situation);
                sit.DoMove(LloydPuzzleSituation.EDirection.Up);
                CheckOrAddCreatedSituation(parentNode, generatedNodes, sit);
            }
            if ((maxCount == 0 || (maxCount > 0 && generatedNodes.Count < maxCount))
                && situation.TestDirection(LloydPuzzleSituation.EDirection.Down))
            {
                LloydPuzzleSituation sit = new LloydPuzzleSituation(situation);
                sit.DoMove(LloydPuzzleSituation.EDirection.Down);
                CheckOrAddCreatedSituation(parentNode, generatedNodes, sit);
            }
            if ((maxCount == 0 || (maxCount > 0 && generatedNodes.Count < maxCount))
                && situation.TestDirection(LloydPuzzleSituation.EDirection.Right))
            {
                LloydPuzzleSituation sit = new LloydPuzzleSituation(situation);
                sit.DoMove(LloydPuzzleSituation.EDirection.Right);
                CheckOrAddCreatedSituation(parentNode, generatedNodes, sit);
            }
            if ((maxCount == 0 || (maxCount > 0 && generatedNodes.Count < maxCount))
                && situation.TestDirection(LloydPuzzleSituation.EDirection.Left))
            {
                LloydPuzzleSituation sit = new LloydPuzzleSituation(situation);
                sit.DoMove(LloydPuzzleSituation.EDirection.Left);
                CheckOrAddCreatedSituation(parentNode, generatedNodes, sit);
            }

            // Liste als Array zurückgeben
            INode[] result;
            result = new INode[generatedNodes.Count];
            generatedNodes.CopyTo(result);
            return result;
        }

        private void CheckOrAddCreatedSituation(TreeNode parentNode, List<INode> generatedNodes, LloydPuzzleSituation sit)
        {
            if (m_KeepCreatedSituationsinMemory)
            {
                // Schaue nach, ob diese Situation schon einmal generiert wurde
                // Dies verhindert bei der Tiefensuche eine unendliche Suche.
                if (!SituationAlreadyCreated(sit))
                {
                    TreeNode childNode = new TreeNode(parentNode, sit);
                    generatedNodes.Add(childNode);
                    m_CreatedSituations.Add(sit);
                }
            }
            else
            {
                 TreeNode childNode = new TreeNode(parentNode, sit);
                 generatedNodes.Add(childNode);
            }
        }

        #endregion

        #region IProblem Members

        public bool CompareNodes(INode node)
        {
            if (((LloydPuzzleSituation)node.Data) == (LloydPuzzleSituation)m_Destination.Data)
			{
				m_Destination = node;
				return true;
			}

			//((LloydPuzzleSituation)node.Data).Show();

			return false;
        }

        INode[] IProblem.FirstNodes
        {
            get
            {
                INode[] nodes = new TreeNode[1];
                nodes[0] = m_Begin;
                return nodes;
            }
        }

        public void OnFoundNoneDestination()
        {
        }

        public bool OnFoundDestination(INode destination, ISearchMethod searchMethod)
        {
            m_FoundSolution = true;
            return true;
        }

        public void OnStartSearch()
        {
            // Löschen der gemerkten Situationen
            m_CreatedSituations.Clear();
        }

        #endregion
    }
}
