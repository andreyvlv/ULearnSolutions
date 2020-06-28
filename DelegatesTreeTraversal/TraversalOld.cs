using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    public class TraversalOld
    {
        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> tree)
        {
            var result = new List<T>();
            FillList((queue) =>
            {
                var node = queue.Dequeue();
                if (node != null)
                {
                    queue.Enqueue(node.Left);
                    queue.Enqueue(node.Right);
                    result.Add(node.Value);
                }
            }, tree);            
            return result;            
        }

        public static IEnumerable<Job> GetEndJobs(Job job)
        {
            var result = new List<Job>();
            FillList((queue) =>
            {
                var node = queue.Dequeue();                
                if (node.Subjobs.Count != 0)
                    foreach (var subJob in node.Subjobs)
                        queue.Enqueue(subJob);
                else
                    result.Add(node);
            }, job);           
            return result;            
        }

        public static IEnumerable<Product> GetProducts(ProductCategory category)
        {
            var result = new List<Product>();
            FillList((queue) =>
            {
                var node = queue.Dequeue();                
                if (node.Categories.Count != 0)               
                    foreach (var prodCat in node.Categories)
                        queue.Enqueue(prodCat);                
                foreach (var prod in node.Products)                
                    result.Add(prod);                               
            }, category);            
            return result;
        }

        private static void FillList<T>(Action<Queue<T>> func, T collection)
        {           
            var queue = new Queue<T>();            
            queue.Enqueue(collection);
            while (queue.Count > 0)
                func(queue);            
        }
    }
}