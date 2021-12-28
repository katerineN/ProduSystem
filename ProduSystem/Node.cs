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