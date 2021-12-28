using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ProduSystem
{
    public class Helpers
    {
        //устанавливаем разрешимость узлов
        public static void resolve(Node n)
        {
            if (n.flag)
                return;
            if (n is AndNode)
                n.flag = n.children.All(c => c.flag == true);

            if (n is OrNode)
                n.flag = n.children.Any(c => c.flag == true);

            if (n.flag)
                foreach (Node p in n.parent)
                    resolve(p);
        }
    }
}