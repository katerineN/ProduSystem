using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

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

        public bool ComparePr(List<string> pr)
        {
            return preconditions.All(f => pr.Contains(f));
        }

        //Вывод результата
        public string Print()
        {
            string result = "";
            Form1 form1 = new Form1();
            //проходимся по всем предпосылкам
            foreach (var p in preconditions)
            {
                result += form1.facts[p].Text;
                result += (p != preconditions.Last()) ? " , " : "";
            }

            result += "->" + form1.facts[consequence].Text;
            return result;
        }

       
        
        //Вывод результата
        public string PrintBackward(List<string> res, string r)
        {
            string result = "";
            Form1 form1 = new Form1();
            //проходимся по всем предпосылкам
            foreach (var p in preconditions)
            {
                if (res.Contains(p))
                {
                    result += form1.facts[p].Text;
                    result += (p != preconditions.Last()) ? " , " : "";
                }
            }
            if (form1.facts[consequence].Text == r || ComparePr(res))
            {
                result += "->" + form1.facts[consequence].Text;
                res.Add(form1.facts[consequence].Text);
            }

            result = null;
            return result;
        }

        /// <summary>
        /// проверяем правильный ли вывод
        /// </summary>
        /// <returns></returns>
        public string IsWeatherOrCutePhrase()
        {
            Form1 form1 = new Form1();
            string res = "";
            if (consequence[0] == 'F' || consequence[0] == 'Z')
                res = form1.facts[consequence].Text;
            return res;
        }
    }
}