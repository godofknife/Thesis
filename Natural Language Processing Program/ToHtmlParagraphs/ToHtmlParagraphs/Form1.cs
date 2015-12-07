/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */ 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ToHtmlParagraphs
{
    public partial class Form1 : Form
    {
        const string PreDummy = "pre.block.erp";
        Regex preblockRegex = new Regex("<pre>.+?</pre>", RegexOptions.Singleline|RegexOptions.Compiled);
        Regex preDummyRegex = new Regex(string.Format("<p>{0}</p>", PreDummy), RegexOptions.Compiled);
        
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string input = textBoxInput.Text;
            if (input != string.Empty)
            {
                string output = ToParagraphs(input);
                textBoxOuput.Text = output;
            }
        }

        // Mark up natural paragraphs with HTML &lt;p&gt; and &lt;/p&gt;, with &lt;pre&gt;...&lt;/pre&gt; spared.
        private string ToParagraphs(string text)
        {
            // To host the extracted &lt;pre&gt;...&lt;/pre&gt; sections.
            List<string> preblocks = null;

            // If text has the any &lt;pre&gt;...&lt;/pre&gt; section
            if (preblockRegex.IsMatch(text))
            {
                // Extract the preblock strings and store them in preblocks
                preblocks = new List<string>();

                MatchCollection matches = preblockRegex.Matches(text);
                foreach (Match match in matches)
                    preblocks.Add(match.Value);

                // Replace all &lt;pre&gt;...&lt;/pre&gt; sections with a unique dummy string
                text = preblockRegex.Replace(text, PreDummy);
            }

            // Split text into natural paragraphs
            string[] paragraphs = text.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            // Mark up natural paragraphs with &lt;p&gt; and &lt;/p&gt; and concatenate them into text 
            text = string.Empty;

            foreach (string p in paragraphs)
            {
                text += string.Format("<p>{0}</p>", p.Trim());
            }

            // Restore the &lt;pre&gt;...&lt;/pre&gt; sections
            if (preblocks != null)
            {
                if (preDummyRegex.IsMatch(text))
                {
                    MatchCollection matches = preDummyRegex.Matches(text);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        // Each time replace only one &lt;p&gt;pre.block.erp&lt;/p&gt; with the formatted 
                        // &lt;pre&gt;content&lt;/pre&gt;to ensure correct restore
                        text = preDummyRegex.Replace(text, "\r\n\r\n" + preblocks[i] + "\r\n\r\n", 1, matches[i].Index);
                    }
                }
            }

            return text;
        }
    }
}
