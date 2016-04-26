/**
 * Find the average type token ratio of the text entered by user
 * or read from a selected file.
 * 
 * Author: Jiayun Han
 * http://www.@nlpdotnet.com
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AverageTypeTokenRatio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
