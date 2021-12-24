using System.Collections.Generic;
using System.Linq;

namespace ProduSystem
{
    public class Rules
    {
        // Посылки правила
        public List<string> preconditions = new List<string>();
        // Заключение правила
        public string consequence;

        public Rules(List<string> pr, string cons)
        {
            preconditions = pr;
            consequence = cons;
        }

        public Rules(string line)
        {
            string[] temp = line.Split('-');
            consequence = temp[1].Trim(' ');
            preconditions = temp[0].Split(',').Select(f => f.Trim(' ')).ToList();
        }

    }
}