using System;
using System.Windows.Forms;

namespace Aquarium
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm = new Aquarium() { };
            Application.Run(MainForm);

            //TEST PURPOSES ONLY
            //Application.Run(new GraphicObject("../../data/textures/object/castle.png", 0.3d) { });
            //Application.Run(new GameObject("../../data/textures/object/shell.png", 1, 10, 500, 100, true ) { });
        }

        public static Aquarium MainForm;
    }
}
