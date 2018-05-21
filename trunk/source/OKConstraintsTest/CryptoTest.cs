using System;
using Xunit;
using OKHeuristicSearchRoom;
using OKConstraints;

namespace OKConstraintsTest
{
    public class CryptoTest
    {
        [Fact]
        public void SoftHillClimbingCrypto()
        {
            CryptoArithmProblem problem = new CryptoArithmProblem();
            var searchMethod = new SoftHillClimbingFirstSearch(problem);
            DateTime date = DateTime.Now;
            ConstraintConfiguration startConfiguration = (ConstraintConfiguration)problem.FirstNodes[0].Data;
            searchMethod.Run();
            Assert.Equal(1, problem.SolutionList.Count);
            ConstraintConfiguration endConfiguration = (ConstraintConfiguration)problem.SolutionList[0].Data;
            var variables = endConfiguration.Variables;
            Assert.Equal(1, variables[0].Value);
            Assert.Equal(0, variables[1].Value);
            Assert.Equal(6, variables[2].Value);
            Assert.Equal(2, variables[3].Value);
            Assert.Equal(1, variables[4].Value);
            Assert.Equal(5, variables[5].Value);
            Assert.Equal(1, variables[6].Value);
            Assert.Equal(2, variables[7].Value);
            Assert.Equal(1, variables[8].Value);
        }
    }
}
