using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Aquarium
{

    public partial class Aquarium : Form
    {
        List<Fish> Fishies = new List<Fish>();
        Fish tempFish = new Fish();

        /// <summary>
        /// Создаёт экземляр класса "Рыба" с заданными свойствами
        /// </summary>
        /// <param name="pName">Название текстуры</param>
        /// <param name="pMaxSpeedConst">Стандартная скорость рыбы</param>
        /// <param name="pcursorFear">Боиться ли курсора</param>
        /// <param name="pMemoryLasts">Сколько длиться испуг</param>
        /// <param name="pFov">Радиус в котором рыба видит угрозу</param>
        /// <param name="pTriggeredMultiplier">Множитель ускорения рыбы во время испуга</param>
        public void InitFish(string pName, int pMaxSpeedConst, bool pcursorFear, uint pMemoryLasts, int pFov, double pTriggeredMultiplier, uint pRotationDelay)
        {
            tempFish = new Fish(pName, pMaxSpeedConst, pcursorFear, pMemoryLasts, pFov, pTriggeredMultiplier, pRotationDelay);
            Fishies.Add(tempFish);
            Fishies[Fishies.Count - 1].Show();
        }
        public void InitFish(string pName)
        {
            switch (pName)
            {
                //Рыба клоун
                //Шустрая рыбка с средними параметрами
                case "1":
                    {
                        InitFish(pName, 250, true, 1000, 150, 3, 50);
                        break;
                    }
                //Карликовый удильщик
                //Медленная рыба, хищник, охотится из засады
                case "2":
                    {
                        InitFish(pName, 100, true, 100, 300, 5, 250);
                        break;
                    }
                //Карликовая акула
                //Быстрый хищник, охотится на мелкую рыбу за счёт скорости, но плохо видит
                case "3":
                    {
                        InitFish(pName, 500, false, 500, 125, 2, 75);
                        break;
                    }
                //Рыба клоун
                //Большая пугливая рыбка
                case "4":
                    {
                        InitFish(pName, 300, true, 2500, 150, 0.01, 100);
                        break;
                    }
                //Рыба клоун
                //Маленькая рыбка с отличным зрением
                case "5":
                    {
                        InitFish(pName, 200, true, 300, 700, 4, 30);
                        break;
                    }
            }
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
            InitFish("1");
        }
        private void f2(object sender, EventArgs e)
        {
            InitFish("2");
        }
        private void f3(object sender, EventArgs e)
        {
            InitFish("3");
        }
        private void f4(object sender, EventArgs e)
        {
            InitFish("4");
        }
        private void f5(object sender, EventArgs e)
        {
            InitFish("5");
        }


        private void Aquarium_FormClosed(object sender, FormClosedEventArgs e)
        {
            trayIcon.Visible = false;
        }
    }

    public class GraphicObject : Form
    {
        protected Bitmap Texture;
        protected int x, y;

        public GraphicObject()
        {

        }

        public void LoadFromFile()
        {

        }
        public void Draw()
        {

        }
        public virtual void gMove()
        {

        }

    }
    public class Particle : GraphicObject
    {
        private int AnimationTicks;
        public bool deleted;

        public void Animate(int dt)
        {
            gMove();
        }

    }
    public class GameObject : GraphicObject
    {
        protected double rx, ry;

        public GameObject()
        {

        }
        public override void gMove()
        {

        }
        public bool IsCollidingWith(GameObject target)
        {
            return false;
        }

    }
    public class Food : GameObject
    {
        public int calories;
        public int size;
        public Color color;
        public int smell;
        public int disguised;
    }

}
