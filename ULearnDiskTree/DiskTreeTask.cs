using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiskTree
{
    class DiskTreeTask
    {
        public static List<string> Solve(List<string> directories)
        {
            var diskTree = new DiskTree();
            diskTree.Add(directories);
            return diskTree.GetDiskTreeList();
        }
    }

    class DiskTree
    {
        public List<TreeNode> TopLevelDirectories { get; set; }

        public DiskTree()
        {
            TopLevelDirectories = new List<TreeNode>();
        }

        public void Add(IEnumerable<string> directories)
        {
            foreach (var dir in directories)
                this.Add(dir);
        }

        public void Add(string directory)
        {
            var root = TopLevelDirectories;
            var spl = directory.Split('\\');
            for (int i = 0; i < spl.Length; i++)
            {
                var name = spl[i];
                var treeNode = new TreeNode(name, i);
                if (!root.Any(x => x.Name == spl[i]))
                {
                    root.Add(treeNode);
                    root = treeNode.ChildrenDirectories;
                }
                else
                {
                    var existNode = root
                        .Where(x => x.Name == spl[i])
                        .FirstOrDefault();
                    root = existNode.ChildrenDirectories;
                }
            }
        }

        public List<string> GetDiskTreeList()
        {
            var result = new List<string>();
            var root = TopLevelDirectories;
            var queue = new Stack<TreeNode>();
            foreach (var item in root
                     .OrderByDescending(x => x.Name, StringComparer.Ordinal))
                queue.Push(item);
            while (queue.Count > 0)
            {
                var node = queue.Pop();
                foreach (var dir in node.ChildrenDirectories
                         .OrderByDescending(x => x.Name, StringComparer.Ordinal))
                    queue.Push(dir);
                result.Add(node.Name.PadLeft(node.Name.Length + node.Level));
            }
            return result;
        }
    }

    class TreeNode
    {
        public int Level { get; private set; }
        public string Name { get; private set; }
        public List<TreeNode> ChildrenDirectories { get; set; }

        public TreeNode(string name, int level)
        {
            Name = name;
            Level = level;
            ChildrenDirectories = new List<TreeNode>();
        }
    }
}