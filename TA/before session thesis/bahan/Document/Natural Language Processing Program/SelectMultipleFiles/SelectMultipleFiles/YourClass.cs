/**
 * Author: Jiayun Han
 * http://wwww.nlpdotnet.com
 */

using System.Collections.Generic;
using System.Windows.Forms;

namespace SelectMultipleFiles
{
    public class YourClass
    {

        /// <summary>
        ///  It captures the file names opened from a single or multiple folders.
        ///  User is given the chance to stop adding files by clicking Cancel.
        /// </summary>
        /// <returns> a set of file paths selected by user</returns>
        public static HashSet<string> RepetitiveOpenFiles()
        {
            HashSet<string> filesOpened = new HashSet<string>();
            DialogResult dr = new DialogResult();
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.Multiselect = true;

            do
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string[] files = ofd.FileNames;
                    foreach (var file in files)
                    {
                        filesOpened.Add(file);
                    }
                    //capture the confirmation box result 
                    dr = MessageBox.Show("More Files?", "Add Files", MessageBoxButtons.YesNo);
                }
                else
                {
                    dr = DialogResult.No;
                }
            } while (dr == DialogResult.Yes);

            return filesOpened;
        }
    }
}
