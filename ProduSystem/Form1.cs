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
                    checkedListBoxS.Items.Add("" + item + ": " + facts[item].Text);
                if (item.First() == 'P')
                    checkedListBoxP.Items.Add("" + item + ": " + facts[item].Text);
                if (item.First() == 'Z')
                {
                    checkedListBoxZ.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                }

                if (item.First() == 'C')
                    checkedListBoxC.Items.Add("" + item + ": " + facts[item].Text);
                if (item.First() == 'F'){
                    //checkedListBoxF.Items.Add("" + item + ": " + facts[item].Text);
                    checkedListBoxF.Items.Add("" + item + ": " + facts[item].Text, CheckState.Indeterminate);
                    //comboBox1.Items.Add("" + item + ": " + facts[item]);
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
        
        //Вывод
        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            while(Forward()){}
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