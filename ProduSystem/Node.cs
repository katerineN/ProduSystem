using System.Collections.Generic;
using System.Data;

namespace ProduSystem
{
    public class Node
    {
        public string Name;
        public List<Node> children = new List<Node>();
        public List<Node> parent = new List<Node>();
        public bool flag = false;

        public Node Clone()
        {
            Node res = new Node();
            res.parent = parent;
            res.children = children;
            return res;
        }
    }

    class OrNode : Node
    {
        public OrNode(string name)
        {
            this.Name = name;
        }
    }

    class AndNode : Node
    {

        public AndNode(string rule)
        {
            this.Name = rule;
        }
    }
    
}