using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.TreeTraversal
{
    public class Traversal
    {       
        public static IEnumerable<T> GetBinaryTreeValues<T>(BinaryTree<T> tree)
            => GetElementsByTraverse(
                tree,
                t => new[] { t.Left, t.Right },
                t => new[] { t.Value });

        public static IEnumerable<Job> GetEndJobs(Job job)
            => GetElementsByTraverse(
                job,
                j => j.Subjobs,
                j => new[] { j.Subjobs.Count == 0 ? j : null });

        public static IEnumerable<Product> GetProducts(ProductCategory category)
            => GetElementsByTraverse(
                category,
                c => c.Categories,
                c => c.Products);

        static IEnumerable<TResult> GetElementsByTraverse<TNode, TResult>(TNode collection,
            Func<TNode, IEnumerable<TNode>> getNodes,
            Func<TNode, IEnumerable<TResult>> selector)
        {
            var list = new List<TResult>();
            var queue = new Queue<TNode>();
            queue.Enqueue(collection);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                var nodes = getNodes(node);
                foreach (var newNode in nodes)
                    if(newNode != null)
                        queue.Enqueue(newNode);                
                list.AddRange(selector(node).Where(x => x != null));
            }
            return list;
        }
    }
}