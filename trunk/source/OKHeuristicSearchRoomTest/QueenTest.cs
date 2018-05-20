using System;
using Xunit;
using OKSearchRoom;
using OKHeuristicSearchRoom;

namespace OKHeuristicSearchRoomTest
{
    public class QueenTest
    {
        [Fact]
        public void AStarSearchQ()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new AStarFirstSearch(problem);
            DateTime date = DateTime.Now;
            searchMethod.Run();
            TreeNode currentNode = problem.Destination as TreeNode;

            int counter = 0;
            while (currentNode != null)
            {
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(9, counter);
            Assert.Equal(83, searchMethod.InspectedNodes);
        }

        [Fact]
        public void GreedySearchQ()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new GreedyFirstSearch(problem);
            DateTime date = DateTime.Now;
            searchMethod.Run();
            TreeNode currentNode = problem.Destination as TreeNode;

            int counter = 0;
            while (currentNode != null)
            {
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(9, counter);
            Assert.Equal(71, searchMethod.InspectedNodes);
        }

        [Fact]
        public void HillClimbingSearchQ()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new HillClimbingFirstSearch(problem);
            DateTime date = DateTime.Now;
            searchMethod.Run();
            TreeNode currentNode = problem.Destination as TreeNode;

            int counter = 0;
            while (currentNode != null)
            {
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(9, counter);
            Assert.Equal(41, searchMethod.InspectedNodes);
        }

        [Fact]
        public void SoftHillClimbingSearchQ()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new SoftHillClimbingFirstSearch(problem);
            DateTime date = DateTime.Now;
            searchMethod.Run();
            TreeNode currentNode = problem.Destination as TreeNode;

            int counter = 0;
            while (currentNode != null)
            {
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(9, counter);
            Assert.Equal(41, searchMethod.InspectedNodes);
        }
    }
}
