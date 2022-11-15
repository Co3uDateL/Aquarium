using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aquarium
{

    public partial class Aquarium : Form
    {
        List<Fish> Fishies = new List<Fish>();
        Fish tempFish = new Fish();

        private void InitFish(string pName)
        {
            tempFish = new Fish(pName);
            Fishies.Add(tempFish);
            Fishies[Fishies.Count - 1].Show();
        }
        private void InitFish(string pName, int pMaxSpeedConst, uint pMemoryLasts, int pFov, double pTriggeredMultiplier)
        {
            tempFish = new Fish(pName, pMaxSpeedConst, pMemoryLasts, pFov, pTriggeredMultiplier);
            Fishies.Add(tempFish);
            Fishies[Fishies.Count - 1].Show();
        }

        private void cMenuNewFish(object sender, EventArgs e)
        {
            f1(sender, e);
            f2(sender, e);
            f3(sender, e);
            f4(sender, e);
            f5(sender, e);
        }

        private void cMenuEnableFeeding(object sender, EventArgs e)
        {
            if (cMenuFeedEnable.CheckState == CheckState.Checked)
            {
                foreach (Fish x in Fishies)
                {
                    x.cursorFear = true;
                }
            }
                else
                {
                    foreach (Fish x in Fishies)
                    {
                        x.cursorFear = false;
                    }
                }


            cMenuFeedEnable.Checked = !cMenuFeedEnable.Checked;
        }

        private void cMenuExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }

        public Aquarium()
        {
            InitializeComponent();
            Hide();

            trayIcon.Icon = new Icon("../../data/clownfish.ico");
            trayIcon.ContextMenuStrip = cMenu;

            TransparencyKey = BackColor;

            timer.Start();

            f1(this, EventArgs.Empty);
        }

        private void UpdateFishies(object sender, EventArgs e)
        {
            foreach (Fish x in Fishies)
            {
                x.Update(timer.Interval);
            }
        }

        private void f1(object sender, EventArgs e)
        {
            //        Название файла текстуры рыбы
            //        |   Скорость в обычном состоянии
            //        |   |    Как долго будет напугана
            //        |   |    |     Как далеко видит опасность
            //        |   |    |     |   Множитель ускорения во время испуга
            InitFish("1", 200, 500, 250, 4);
        }
        private void f2(object sender, EventArgs e)
        {
            //        Название файла текстуры рыбы
            //        |   Скорость в обычном состоянии
            //        |   |    Как долго будет напугана
            //        |   |    |     Как далеко видит опасность
            //        |   |    |     |   Множитель ускорения во время испуга
            InitFish("2", 10, 100, 100, 10);

        }
        private void f3(object sender, EventArgs e)
        {
            //        Название файла текстуры рыбы
            //        |   Скорость в обычном состоянии
            //        |   |    Как долго будет напугана
            //        |   |    |     Как далеко видит опасность
            //        |   |    |     |   Множитель ускорения во время испуга
            InitFish("3", 100, 100, 100, 20);

        }
        private void f4(object sender, EventArgs e)
        {
            //        Название файла текстуры рыбы
            //        |   Скорость в обычном состоянии
            //        |   |    Как долго будет напугана
            //        |   |    |     Как далеко видит опасность
            //        |   |    |     |   Множитель ускорения во время испуга
            InitFish("4", 100, 100, 100, 0.01F);
        }
        private void f5(object sender, EventArgs e)
        {
            //        Название файла текстуры рыбы
            //        |   Скорость в обычном состоянии
            //        |   |    Как долго будет напугана
            //        |   |    |     Как далеко видит опасность
            //        |   |    |     |   Множитель ускорения во время испуга
            InitFish("5", 100, 100, 100, 2);
        }

        private void Aquarium_FormClosed(object sender, FormClosedEventArgs e)
        {
            trayIcon.Visible = false;
        }
    }
}
