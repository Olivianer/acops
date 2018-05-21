using System;
using System.Collections.Generic;
using System.Text;
using OKConstraintVariables;

namespace OKConstraints
{
    /// <summary>
    /// todo: Überdenken dieser Idee
    /// </summary>
    /// <param name="varList"></param>
    /// <param name="numberList"></param>
    internal delegate void boundsConsistency(List<Variable> varList, List<double> numberList);

    /// <summary>
    /// todo
    /// </summary>
    public class ConstraintForms
    {
        /// <summary>
        /// todo
        /// </summary>
        private static Dictionary<string, boundsConsistency> _Forms = new Dictionary<string,boundsConsistency>();

        /// <summary>
        /// todo
        /// </summary>
        public static void Init()
        {
            _Forms.Clear();
            _Forms.Add("<=(+(v,n),v)", AddVar1Num1LessOrEqualThanNumber);
            _Forms.Add("<=(+(v,v),n)", AddVar2LessOrEqualThanNumber);
            _Forms.Add("<=(+(v,+(v,v)),n)", AddVar3LessOrEqualThanNumber);
            //_Forms.Add("<=(+(+(v,v),v),n)", AddVar3LessOrEqualThanNumber);
            _Forms.Add("<=(+(+(*(n,v),*(n,v)),*(n,v)),n)", AddMulVar3LessOrEqualThanNumber);
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="constraint"></param>
        /// <param name="changing"></param>
        /// <returns></returns>
        public static bool CheckBoundsConsistency(Constraint constraint, ref bool changing)
        {
            changing = false;
            // Wenn für dieses Constraint keine geeignete Form existiert gib true zurück
            if (constraint.BoundsConsistencyPossible == false)
                return true;

            StringBuilder strBuilder = new StringBuilder();
            List<Variable> varList = new List<Variable>();
            List<double> numberList = new List<double>();
            constraint.GetForm(ref strBuilder, ref varList, ref numberList);

            int countBefore = 0;
            // Merken der Anzahl an freien Elementen der Domain
            foreach (Variable var in varList)
            {
                if (var.Domain.Count == 0)
                    return false;
                //{
                //    // Wenn die Variable schon zugewiesen wurde, ist hier Schluss
                //    if (var.HasAsignedValue == true)
                //        return true;
                //}
                countBefore += var.Domain.Count;
            }

            string form = strBuilder.ToString();
            boundsConsistency boundsCons;
            
            // Kennzeichne einen Constraint, dessen Form nicht bekannt ist
            if (_Forms.TryGetValue(form, out boundsCons) == false)
            {
                constraint.forbidBoundsConsistency();
                return true;
            }

            boundsCons(varList, numberList);

            int countAfter = 0;
            // Prüfe, ob eine Domain leer ist
            foreach (Variable var in varList)
            {
                if (var.Domain.Count == 0)
                    return false;
                countAfter += var.Domain.Count;
            }

            if (countAfter != countBefore)
                changing = true;

            return true;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="varList"></param>
        /// <param name="numberList"></param>
        private static void AddVar2LessOrEqualThanNumber(List<Variable> varList, List<double> numberList)
        {
            if (varList == null || varList.Count != 2)
                throw new Exception("boundsConsistency for AddVar2LessOrEqualThanNumber() failed: wrong count of variables");
            if (numberList == null || numberList.Count != 1)
                throw new Exception("boundsConsistency for AddVar2LessOrEqualThanNumber() failed: wrong count of numbers");
            // Beispiel: Aus x1 + x2 <= 7 wird
            // x1Max = 7 - x2Min und
            // x2Max = 7 - X1Min
            varList[0].Domain.Max = numberList[0] - varList[1].Domain.Min;
            varList[1].Domain.Max = numberList[0] - varList[0].Domain.Min;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="varList"></param>
        /// <param name="numberList"></param>
        private static void AddVar3LessOrEqualThanNumber(List<Variable> varList, List<double> numberList)
        {
            if (varList == null || varList.Count != 3)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of variables");
            if (numberList == null || numberList.Count != 1)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of numbers");
            // Beispiel: Aus x1 + x2 + x3 <= 7 wird
            // x1Max = 7 - x2Min - x3Min
            // x2Max = 7 - X1Min - x3Min
            // x3Max = 7 - X1Min - x2Min
            varList[0].Domain.Max = numberList[0] - varList[1].Domain.Min - varList[2].Domain.Min;
            varList[1].Domain.Max = numberList[0] - varList[0].Domain.Min - varList[2].Domain.Min;
            varList[2].Domain.Max = numberList[0] - varList[0].Domain.Min - varList[1].Domain.Min;
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="varList"></param>
        /// <param name="numberList"></param>
        private static void AddMulVar3LessOrEqualThanNumber(List<Variable> varList, List<double> numberList)
        {
            if (varList == null || varList.Count != 3)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of variables");
            if (numberList == null || numberList.Count != 4)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of numbers");
            // Beispiel: Aus 3*x1 + 4*x2 + 2*x3 <= 7 wird
            // x1Max = (7 - 4*x2Min - 2*x3Min)/3
            // x2Max = (7 - 3*X1Min - 2*x3Min)/4
            // x3Max = (7 - 3*X1Min - 4*x2Min)/2
            varList[0].Domain.Max = (numberList[3] - numberList[1] * varList[1].Domain.Min - numberList[2] * varList[2].Domain.Min) / numberList[0];
            varList[1].Domain.Max = (numberList[3] - numberList[0] * varList[0].Domain.Min - numberList[2] * varList[2].Domain.Min) / numberList[1];
            varList[2].Domain.Max = (numberList[3] - numberList[0] * varList[0].Domain.Min - numberList[1] * varList[1].Domain.Min) / numberList[2];
        }

        /// <summary>
        /// todo
        /// </summary>
        /// <param name="varList"></param>
        /// <param name="numberList"></param>
        private static void AddVar1Num1LessOrEqualThanNumber(List<Variable> varList, List<double> numberList)
        {
            if (varList == null || varList.Count != 2)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of variables");
            if (numberList == null || numberList.Count != 1)
                throw new Exception("boundsConsistency for AddVar3LessOrEqualThanNumber() failed: wrong count of numbers");
            // Beispiel: Aus x1 + 1 <= x2 wird
            // x1Max = x2Max - 1
            // x2Min = x1Min + 1
            varList[0].Domain.Max = varList[1].Domain.Max - numberList[0];
            varList[1].Domain.Min = varList[0].Domain.Min + numberList[0];
        }
    }
}
