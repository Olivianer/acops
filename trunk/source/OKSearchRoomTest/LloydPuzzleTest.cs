using OKSearchRoom;
using System;
using Xunit;

namespace OKSearchRoomTest
{
    public class LloydPuzzleTest
	{
        [Fact]
        public void BreadthFirstSearchLP()
        {
			int[] puzzle_start = { 2, 3, 8, 1, 6, 7, 5, 4, 0 };
			LloydPuzzleSituation sitStart = new LloydPuzzleSituation(puzzle_start);

			int[] puzzle_ziel = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
			LloydPuzzleSituation sitEnd = new LloydPuzzleSituation(puzzle_ziel);
			TreeNode nodeStart = new TreeNode(sitStart);
			TreeNode nodeEnd = new TreeNode(sitEnd);
			LloydPuzzleProblem problem = new LloydPuzzleProblem(nodeStart, nodeEnd, false);

			var searchMethod = new BreadthFirstSearch(problem);
			searchMethod.Run();
			Assert.True(problem.FoundSolution);
			TreeNode currentNode = problem.Destination as TreeNode;

			int counter = 0;
			while ( currentNode != null )
			{
				((LloydPuzzleSituation)currentNode.Data).Show();
				currentNode = currentNode.Parent;
				counter++;
			}

			Assert.Equal(15, counter);

			Assert.Equal(1001468, searchMethod.InspectedNodes);
		}

        [Fact]
        public void IterativeDepthFirstSearchLP()
        {
            int[] puzzle_start = { 2, 3, 8, 1, 6, 7, 5, 4, 0 };
            LloydPuzzleSituation sitStart = new LloydPuzzleSituation(puzzle_start);

            int[] puzzle_ziel = { 1, 2, 3, 4, 5, 6, 7, 8, 0 };
            LloydPuzzleSituation sitEnd = new LloydPuzzleSituation(puzzle_ziel);
            TreeNode nodeStart = new TreeNode(sitStart);
            TreeNode nodeEnd = new TreeNode(sitEnd);
            LloydPuzzleProblem problem = new LloydPuzzleProblem(nodeStart, nodeEnd, false);

            var searchMethod = new IterativeDepthFirstSearch(problem, 15);
            searchMethod.Run();
            Assert.True(problem.FoundSolution);
            TreeNode currentNode = problem.Destination as TreeNode;

            int counter = 0;
            while (currentNode != null)
            {
                ((LloydPuzzleSituation)currentNode.Data).Show();
                currentNode = currentNode.Parent;
                counter++;
            }

            Assert.Equal(15, counter);

            Assert.Equal(3410112, searchMethod.InspectedNodes);
        }
    }
}
