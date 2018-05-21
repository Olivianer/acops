using System;
using System.Collections.Generic;
using System.Text;
using OKConstraints;
using OKConstraintOperations;
using OKConstraintVariables;

namespace OKConstraintsTest
{
    
    public class CryptoArithmProblem : ConstraintProblem
    {
        
        public CryptoArithmProblem()
        {
            BuildConstraints();
        }

        protected void BuildConstraints()
        {
            // Variablen
            VariablesOperator o = new VariablesOperator("O");
            VariablesOperator x1 = new VariablesOperator("X1");
            VariablesOperator w = new VariablesOperator("W");
            VariablesOperator u = new VariablesOperator("U");
            VariablesOperator x2 = new VariablesOperator("X2");
            VariablesOperator t = new VariablesOperator("T");
            VariablesOperator f = new VariablesOperator("F");
            VariablesOperator r = new VariablesOperator("R");
            VariablesOperator x3 = new VariablesOperator("X3");

            // Konstanten
            DoubleOperator ten = new DoubleOperator(10);

            // O + O = R + 10 * X1
            Addition firstAddition = new Addition(o, o);
            Multiply multiply = new Multiply(ten, x1);
            Addition secondAddition = new Addition(r, multiply);
            Equal equal = new Equal(firstAddition, secondAddition);
            Constraint constraint = new Constraint(equal);

            _constraintList.Add("O", constraint);
            _constraintList.Add("R", constraint);
            _constraintList.Add("X1", constraint);

            // X1 + W + W = U + 10 * X2
            secondAddition = new Addition(w, w);
            firstAddition = new Addition(x1,secondAddition);
            multiply = new Multiply(ten, x2);
            secondAddition = new Addition(u, multiply);
            equal = new Equal(firstAddition, secondAddition);
            constraint = new Constraint(equal);

            _constraintList.Add("X1", constraint);
            _constraintList.Add("W", constraint);
            _constraintList.Add("U", constraint);
            _constraintList.Add("X2", constraint);

            // X2 + T + T = O + 10 * X3
            secondAddition = new Addition(t,t);
            firstAddition = new Addition(x2, secondAddition);
            multiply = new Multiply(ten, x3);
            secondAddition = new Addition(o, multiply);
            equal = new Equal(firstAddition, secondAddition);
            constraint = new Constraint(equal);

            _constraintList.Add("X2", constraint);
            _constraintList.Add("T", constraint);
            _constraintList.Add("O", constraint);
            _constraintList.Add("X3", constraint);

            // X3 = F
            equal = new Equal(x3, f);
            constraint = new Constraint(equal);

            _constraintList.Add("X3", constraint);
            _constraintList.Add("F", constraint);

            ConstraintConfiguration configuration = new ConstraintConfiguration();
            // Variablen
            AddVariable(configuration, "O");
            AddVariable(configuration, "X1");
            AddVariable(configuration, "W");
            AddVariable(configuration, "U");
            AddVariable(configuration, "X2");
            AddVariable(configuration, "T");
            AddVariable(configuration, "F");
            AddVariable(configuration, "R");
            AddVariable(configuration, "X3");

            _begin = CreateNode(null, configuration);
        }

        void AddVariable(ConstraintConfiguration configuration, string name)
        {
            Interval interval = new Interval(0, 9, 1);
            List<Interval> intervals = new List<Interval>();
            intervals.Add(interval);
            var domain = new Domain(intervals);
            var variable = new Variable(name, domain);
            configuration.AddVariable(variable);
        }
    }
}
