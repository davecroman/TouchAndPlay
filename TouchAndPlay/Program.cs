using System;
using System.Windows.Forms;

namespace TouchAndPlay
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            using (Main game = new Main())
            {
                Form frm = (Form)Form.FromHandle(game.Window.Handle);
                frm.FormBorderStyle = FormBorderStyle.None; 
                game.Run();
            }
        }
    }
#endif
}

