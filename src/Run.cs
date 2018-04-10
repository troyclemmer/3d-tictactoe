using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SeniorProject
{
    public class Run
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Display display_ = new Display();
            Application.Run(new Game(display_));
        }
    }
}
