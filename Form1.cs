using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace regex
{
    public partial class Form1 : Form
    {
        MatchCollection match;
        int pos;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != "")
            {
                textBox2.Text = GetResponse(textBox4.Text);
                textBox4.Text = "";
            }

            if (textBox2.Text != "")
            {
                string source = textBox2.Text;
                string regex = textBox1.Text;
                pos = 1;

                match = Regex.Matches(source, regex, RegexOptions.IgnoreCase | RegexOptions.Singleline);

                if (match.Count > 0)
                {
                    label1.Text = "Match: " + match.Count;
                    label1.ForeColor = System.Drawing.Color.White;
                    button2.Enabled = true;

                    textBox3.Text = match[0].Value;
                    textBox2.Select(match[0].Index, match[0].Length);
                    textBox2.ScrollToCaret();
                }
                else
                {
                    label1.Text = "Match: Fail";
                    label1.ForeColor = System.Drawing.Color.Red;
                    textBox3.Text = "";
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) 
        { 
            if (keyData == (Keys.A | Keys.Control)) 
            {
                if (textBox2.Focused)
                {
                    textBox2.SelectAll();                    
                }
                else if (textBox1.Focused)
                {
                    textBox1.SelectAll();    
                }
                else if (textBox3.Focused)
                {
                    textBox3.SelectAll();    
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pos < match.Count)
            {
                textBox3.Text = match[pos].Value;
                textBox2.Select(match[pos].Index, match[pos].Length);
                textBox2.ScrollToCaret();                
                pos++;
                label1.Text = "Match: " + pos;
            }
        }

        public string GetResponse(string sURL)
        {
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(sURL);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();            
            string sourceCode = "";
            Stream receiveStream = myHttpWebResponse.GetResponseStream();
            using (StreamReader readStream = new StreamReader(receiveStream, true))
            {
                sourceCode = readStream.ReadToEnd();
            }

            myHttpWebResponse.Close();

            return sourceCode;
        }
    }
}