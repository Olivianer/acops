using OKSearchRoom;
using System;
using Xunit;

namespace OKSearchRoomTest
{
    public class QueenTest
    {
        [Fact]
        public void BreadthFirstSearchQueen()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new BreadthFirstSearch(problem);
            DateTime date = DateTime.Now;
            searchMethod.Run();
            TreeNode currentNode = problem.Destination as TreeNode;
            //Console.WriteLine(DateTime.Now.Subtract(date));
            //((QueenConstellation)currentNode.Data).Show();

            int counter = 0;
            while (currentNode != null)
            {
                //((QueenConstellation)currentNode.Data).Show();
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(9, counter);
            Assert.Equal(1966, searchMethod.InspectedNodes);

            //System.Console.WriteLine("Inspizierte Knoten: " + searchMethod.InspectedNodes.ToString());
        }

        [Fact]
        public void DepthFirstSearchQueen()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new DepthFirstSearch(problem);
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
            Assert.Equal(114, searchMethod.InspectedNodes);
        }

        [Fact]
        public void IterativeDepthFirstSearchQueen()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new IterativeDepthFirstSearch(problem);
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
            Assert.Equal(5621, searchMethod.InspectedNodes);
        }

        [Fact]
        public void IterativeBreadthFirstSearchQueen()
        {
            QueenProblem problem = new QueenProblem(8);
            var searchMethod = new IterativeBreadthFirstSearch(problem);
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
            Assert.Equal(100, searchMethod.InspectedNodes);
        }
    }
}
