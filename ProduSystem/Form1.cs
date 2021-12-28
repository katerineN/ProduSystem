using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProduSystem
{
    public partial class Form1 : Form
    {
        public SortedDictionary<string, Facts> facts = new SortedDictionary<string, Facts>();
        public Dictionary<string, Rules> rules = new Dictionary<string, Rules>();
        private List<string> sumFacts = new List<string>();
        public Form1()
        {
            InitializeComponent();
            facts = FactsFromFile("facts.txt");
            rules = RulesFromFile("rules.txt");
            LoadProdSystem();
        }

        /// <summary>
        /// Выгружает факты из файла
        /// </summary>
        /// <param name="fname">Имя файла</param>
        /// <returns></returns>
        public SortedDictionary<string, Facts> FactsFromFile(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            SortedDictionary<string, Facts> res = new SortedDictionary<string, Facts>();
            foreach (var fact in lines)
            {
                string[] temp = fact.Split(':');
                if (temp.Length != 2)
                {
                    ErrorLabel.Text = "Error with parsing file of facts";
                    ErrorLabel.ForeColor = Color.Red;
                }
                res.Add(temp[0], new Facts(temp[0], temp[1]));
            }

            return res;
        }
        
        /// <summary>
        /// Выгружает правила из файла
        /// </summary>
        /// <param name="fname">Имя файла</param>
        /// <returns></returns>
        public Dictionary<string, Rules> RulesFromFile(string fname)
        {
            string[] lines = File.ReadAllLines(fname);
            Dictionary<string, Rules> res = new Dictionary<string, Rules>();
            foreach (var fact in lines)
            {
                string[] temp = fact.Split(':');
                if (temp.Length != 2)
                {
                    ErrorLabel.Text = "Error with parsing file of rules";
                    ErrorLabel.ForeColor = Color.Red;
                }
                res.Add(temp[0], new Rules(temp[1]));
            }

            return res;
        }
        
        /// <summary>
        /// Раскидывает факты по окошкам
        /// </summary>
        public void LoadProdSystem()
        {
            foreach (var item in facts.Keys)
            {
                if (item.First() == 'T')
                    checkedListBoxT.Items.Add("" + item + ": " + facts[item].Text);
                if (item.First() == 'S')
                {
                    if(item == "S42")
                        checkedListBoxS.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    else
                        checkedListBoxS.Items.Add("" + item + ": " + facts[item].Text);
                }

                if (item.First() == 'P')
                {
                    if(item == "P38" || item == "P9")
                        checkedListBoxP.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    else
                        checkedListBoxP.Items.Add("" + item + ": " + facts[item].Text);
                }

                if (item.First() == 'Z')
                {
                    checkedListBoxZ.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    comboBox1.Items.Add("" + item + ": " + facts[item].Text);
                }

                if (item.First() == 'C')
                {
                    if(item == "C21" || item == "C22")
                        checkedListBoxC.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    else
                        checkedListBoxC.Items.Add("" + item + ": " + facts[item].Text);
                }

                if (item.First() == 'F'){
                    checkedListBoxF.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    comboBox1.Items.Add("" + item + ": " + facts[item].Text);
                }

                if (item.First() == 'M')
                {
                    if(item == "M17")
                        checkedListBoxM.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    else
                        checkedListBoxM.Items.Add("" + item + ": " + facts[item].Text);
                }
            }
        
        }

        //ищет правила, заключениями которых является переданный факт
        public List<string> FindRules(string id, List<string> rep)
        {
            List<string> res = new List<string>();
            foreach (var i in rules){
                if (i.Value.consequence == id && !rep.Contains(i.Key))
                    res.Add(i.Key);
            }
            return res;
        }
        
        
        //Обратный вывод
        public Tuple<bool, List<string>> BackwardAlgorithm(string resFact)
        {
            //выбранные факты
            List<string> knownFacts = new List<string>(sumFacts);

            //id результатов
            List<string> resId = new List<string>();
            //проходим по айдишникам фактов
            foreach (var id in facts.Keys){
                //и узлы
                Dictionary<string, AndNode> andNodes = new Dictionary<string, AndNode>();
                //или узлы
                Dictionary<string, OrNode> orNodes = new Dictionary<string, OrNode>();
                //корень дерева, который добавляем к или узлам
                OrNode root = new OrNode(id);
                orNodes.Add(id, root);
                //дерево
                Stack<Node> tree = new Stack<Node>();
                tree.Push(root);
                //пока не просмотрим все дерево
                while (tree.Count != 0)
                {
                    Node current = tree.Pop();
                    //если текущий узел это правила
                    if (current is AndNode)
                    {
                        AndNode andNode = current as AndNode;
                        //просматриваем все посылки 
                        foreach (var f in rules[andNode.Name].preconditions)
                        {
                            //если в или узлах есть эта посылка, то добавляем к текущему узлу
                            //детей с этой посылкой
                            if (orNodes.ContainsKey(f))
                            {
                                current.children.Add(orNodes[f]);
                                orNodes[f].parent.Add(current);
                            }
                            //иначе добавляем нового ребенка с данной посылкой
                            else
                            {
                                orNodes.Add(f, new OrNode(f));
                                andNode.children.Add(orNodes[f]);
                                orNodes[f].parent.Add(andNode);
                                tree.Push(orNodes[f]);
                            }
                        }
                    }
                    //если текущий узел это факт
                    else
                    {
                        OrNode orNode = current as OrNode;
                        //перебираем все правила, заключениями которых является текущий узел
                        foreach (var rule in FindRules(orNode.Name, facts.Keys.ToList()))
                        {
                            //если существует и узел, который является правилом, то добавляем 
                            //текущему узлу детей
                            if (andNodes.ContainsKey(rule))
                            {
                                current.children.Add(andNodes[rule]);
                                andNodes[rule].parent.Add(current);
                            }
                            //иначе создаем такой узел
                            else
                            {
                                andNodes.Add(rule, new AndNode(rule));
                                orNode.children.Add(andNodes[rule]);
                                andNodes[rule].parent.Add(orNode);
                                resId.Add(rule);
                                tree.Push(andNodes[rule]);
                            }
                        }
                    }
                }

                //просматриваем список текущих или узлов
                foreach (var val in orNodes)
                {
                    //если в заданных фактах есть факт из узла
                    if (knownFacts.Contains(val.Key))
                    {
                        val.Value.flag = true;
                        foreach (Node p in val.Value.parent)
                            Helpers.resolve(p);
                        if (root.flag)
                        {
                            //resId.Add(root.Name);
                            //нашли вывод
                            if (root.Name == resFact)
                                return Tuple.Create(true, resId);
                        }
                    }
                }
            }

            return Tuple.Create(false, resId);
        }
        
        /// <summary>
        /// Прямой вывод
        /// </summary>
        /// <returns></returns>
        public bool Forward()
        {
            bool flag = false;
            //взяли факты из выбранных
            foreach (var fact in summary.Items)
            {
                sumFacts.Add(fact.ToString().Split(':')[0].Trim(' '));
            }

            foreach (var rule in rules)
            {
                //проверяем на наличие правила в посылке
                if (rule.Value.ComparePr(sumFacts))
                {
                    string res = rule.Value.consequence;
                    if (!sumFacts.Contains(res))
                    {
                        sumFacts.Add(rule.Value.consequence);
                        flag = true;
                        result.Text += rule.Value.Print();
                        string temp = rule.Value.IsWeatherOrCutePhrase();
                        if (temp != "")
                        {
                            listBox1.Items.Add(temp);
                        }
                    }
                }
            }

            return flag;
        }
        
        //Обратный вывод
        private Tuple<bool, List<string>> Backward()
        {
            string resFact = comboBox1.SelectedItem.ToString().Split(':')[0].Trim(' ');
            //взяли факты из выбранных
            foreach (var fact in summary.Items)
            {
                sumFacts.Add(fact.ToString().Split(':')[0].Trim(' '));
            }
            var res = BackwardAlgorithm(resFact);
            return res;
        }
        
        //Вывод
        private void button2_Click(object sender, EventArgs e)
        {
            if (!backward.Checked)
            {
                listBox1.Items.Clear();
                while (Forward())
                {
                }
            }
            else
            {
                var r = Backward();
                result.Text = "Существует ли погода для такой одежды? " + (r.Item1 ? "Да" : "Нет");

                if (r.Item1)
                    foreach (var id in r.Item2.Distinct().OrderByDescending(s => s).ToList())
                        //result.Text += Environment.NewLine + facts[id].Text;
                        result.Text += Environment.NewLine + rules[id].Print();
            }
        }
        
        //кнопка заново запускает
        private void button1_Click(object sender, EventArgs e)
        {
            checkedListBoxT.Items.Clear();
            checkedListBoxS.Items.Clear();
            checkedListBoxP.Items.Clear();
            checkedListBoxZ.Items.Clear();
            checkedListBoxC.Items.Clear();
            checkedListBoxF.Items.Clear();
            checkedListBoxM.Items.Clear();
            listBox1.Items.Clear();
            result.Text = "";
            summary.Items.Clear();
            ErrorLabel.Text = "";
            LoadProdSystem();
        }

        //Тут пошли методы, которые перекидывают выбранные факты в нужное окошко
        private void checkedListBoxT_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            summary.Items.Add(checkedListBoxT.SelectedItem);
            checkedListBoxT.Items.Remove(checkedListBoxT.SelectedItem);
        }

        private void checkedListBoxS_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            summary.Items.Add(checkedListBoxS.SelectedItem);
            checkedListBoxS.Items.Remove(checkedListBoxS.SelectedItem);
        }

        private void checkedListBoxP_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            summary.Items.Add(checkedListBoxP.SelectedItem);
            checkedListBoxP.Items.Remove(checkedListBoxP.SelectedItem);
        }

        private void checkedListBoxM_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            summary.Items.Add(checkedListBoxM.SelectedItem);
            checkedListBoxM.Items.Remove(checkedListBoxM.SelectedItem);
        }

        private void checkedListBoxC_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            summary.Items.Add(checkedListBoxC.SelectedItem);
            checkedListBoxC.Items.Remove(checkedListBoxC.SelectedItem);
        }

        
    }
}