using System;
using System.Configuration;
using System.Collections.Generic;

// Каждый документ — это список токенов. То есть List<string>.
// Вместо этого будем использовать псевдоним DocumentTokens.
// Это поможет избежать сложных конструкций:
// вместо List<List<string>> будет List<DocumentTokens>
using DocumentTokens = System.Collections.Generic.List<string>;
using System.Linq;

namespace Antiplagiarism
{   
    public class LevenshteinCalculator
    {
        public List<ComparisonResult> CompareDocumentsPairwise(List<DocumentTokens> documents)
        {            
            var result = new List<ComparisonResult>();           
            for (int i = 0; i < documents.Count; i++)            
                for (int j = i + 1; j < documents.Count; j++)                                 
                    result.Add(GetLevensteinDistance(documents[i], documents[j]));                         
            return result;           
        }
       
        public ComparisonResult GetLevensteinDistance(DocumentTokens first, DocumentTokens second)
        {
            var opt = new double[first.Count + 1, second.Count + 1];
            for (int i = 0; i <= first.Count; i++)           
                opt[i, 0] = i;            
            for (int i = 0; i <= second.Count; i++)            
                opt[0, i] = i;            
            for (int i = 1; i <= first.Count; i++)            
                for (int j = 1; j <= second.Count; j++)
                {
                    var dist = TokenDistanceCalculator.GetTokenDistance(first[i - 1], second[j - 1]);
                    if (dist == 0)
                        opt[i, j] = opt[i - 1, j - 1];
                    else
                        opt[i, j] = new[]
                        {
                            opt[i - 1, j] + 1,
                            opt[i, j - 1] + 1,
                            opt[i - 1, j - 1] + dist
                        }.Min();                                           
                }            
            return new ComparisonResult(first, second, opt[first.Count, second.Count]);
        }
    }
}
