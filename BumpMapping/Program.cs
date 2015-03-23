using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BumpMapping
{
    static class Program
    {
        //main method in Program.cs
        [STAThread]
        static void Main()
        {
            Form1 app = new Form1();
            app.InitializeGraphics();
            app.Show();
            while (app.Created)
            {
                app.Render();
                Application.DoEvents();
            }
            //app.DisposeGraphics();
        }
    }
}
