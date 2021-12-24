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
        SortedDictionary<string, Facts> facts = new SortedDictionary<string, Facts>();
        Dictionary<string, Rules> rules = new Dictionary<string, Rules>();
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
                    checkedListBoxF.Items.Add("" + item + ": " + facts[item].Text);
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
        
        //кнопка заново запускает
        private void button1_Click(object sender, EventArgs e)
        {
            ErrorLabel.Text = "";
            LoadProdSystem();
        }
        
    }
}