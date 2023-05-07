using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Aquarium
{
    public partial class Aquarium : Form
    {
        public List<GameObject> GameObjects = new List<GameObject>();

        public static Random random = new Random();
        public static readonly string TexturePath = "../../data/textures/";

        /// <summary>
        /// Создаёт экземляр класса "Рыба" с заданными свойствами
        /// </summary>
        /// <param name="pName">Название текстуры</param>
        /// <param name="pMaxSpeedConst">Стандартная скорость рыбы</param>
        /// <param name="pcursorFear">Боиться ли курсора</param>
        /// <param name="pMemoryLasts">Сколько длиться испуг</param>
        /// <param name="pFov">Радиус в котором рыба видит угрозу</param>
        /// <param name="pTriggeredMultiplier">Множитель ускорения рыбы во время испуга</param>
        public void InitFish(string pPath, int pMaxSpeedConst, bool pcursorFear, uint pMemoryLasts, int pFov, double pTriggeredMultiplier, uint pRotationDelay)
        {
            GameObjects.Add( new Fish( pPath, pMaxSpeedConst, pcursorFear, pMemoryLasts, pFov, pTriggeredMultiplier, pRotationDelay) );
            GameObjects[GameObjects.Count - 1].Show();
        }
        public void InitFish(string name)
        {
            switch (name)
            {
                //Рыба клоун
                //Шустрая рыбка с средними параметрами
                case "1":
                    {
                        //string tempPath = System.IO.Path.GetFullPath(TexturePath + name + ".png");
                        InitFish(TexturePath + name + ".png", 250, true, 1000, 150, 3, 50);
                        break;
                    }
                //Карликовый удильщик
                //Медленная рыба, хищник, охотится из засады
                case "2":
                    {
                        InitFish(TexturePath + name + ".png", 100, true, 100, 300, 5, 250);
                        break;
                    }
                //Карликовая акула
                //Быстрый хищник, охотится на мелкую рыбу за счёт скорости, но плохо видит
                case "3":
                    {
                        InitFish(TexturePath + name + ".png", 500, false, 500, 125, 2, 75);
                        break;
                    }
                //Рыба клоун
                //Большая пугливая рыбка
                case "4":
                    {
                        InitFish(TexturePath + name + ".png", 300, true, 2500, 150, 0.01, 100);
                        break;
                    }
                //Рыба клоун
                //Маленькая рыбка с отличным зрением
                case "5":
                    {
                        InitFish(TexturePath + name + ".png", 200, true, 300, 700, 4, 30);
                        break;
                    }
            }
        }

        public void InitGameObject(string path, double scale, double Fa, int x, int y,  bool colidingEnabled)
        {
            GameObjects.Add( new GameObject(path, scale, Fa, x, y,  colidingEnabled) );
        }

        public void InitGameObject(string name, int x, int y)
        {
            //По какой-то причине конкатенация строк вызывает исключение неправильного аргумента
            switch (name)
            {
                case "castle":
                    {
                        InitGameObject("../../data/textures/object/" + name + ".png", random.Next(80, 200) / 100, 10, x, y, true) ;
                        break;
                    }
                case "shell":
                    {
                        InitGameObject("../../data/textures/object/" + name + ".png", random.Next(50, 150) / 100, 0.05, x, y, true);
                        break;
                    }
                case "rock":
                    {
                        InitGameObject("../../data/textures/object/" + name + ".png", random.Next(50, 200) / 100, 2, x, y, true);
                        break;
                    }
                case "weed":
                    {
                        InitGameObject("../../data/textures/object/" + name + ".png", random.Next(150, 300) / 100, 0.6, x, y, true);
                        break;
                    }
            }
        }
        public void InitFood(string path, int x, int y, int calories, double scale, double Fa, int smell,  bool colidingEnabled)
        {
            //string path, int x, int y, int pCalories, double scale, int weight, int pSmell, bool pColidingEnabled 
            GameObjects.Add( new Food(path, x, y, calories, scale, Fa, smell, colidingEnabled) );
        }
        public void InitFood(string name, int x, int y)
        {
            //По какой-то причине конкатенация строк вызывает исключение неправильного аргумента
            switch (name)
            {
                case "pill":
                    {
                        InitFood
                            (
                                "../../data/textures/food/" + name + "/"+(random.Next(0,2).ToString())+".png",
                                x,
                                y,
                                500, //calories
                                5,   //scale
                                (2/(double)10),//Fa
                                10,  //Smell
                                true
                            );
                        break;
                    }
                case "granule":
                    {
                        InitFood
                            (
                                "../../data/textures/food/" + name + "/" + (random.Next(0, 2).ToString()) + ".png",
                                x,
                                y,
                                250,//calories
                                4,//scale
                                (1 / (double)10),//Fa
                                200,//Smell
                                true
                            );
                        break;
                    }
                case "flake":
                    {
                        InitFood
                            (
                                "../../data/textures/food/" + name + "/" + (random.Next(0, 2).ToString()) + ".png",
                                x,
                                y,
                                random.Next(50, 300),//calories
                                3,//scale
                                (random.Next(-100,100)/ (double)100),//Fa
                                400,//Smell
                                true
                            );
                        break;
                    }
            }
        }

        public List<Food> GetFoodList()
        {
            List<Food> foods = new List<Food>();
            for (int i = 0; i < GameObjects.Count; i++)
            {
                if (GameObjects[i].GetType() == typeof(Food))
                {
                    //По идее преобразование не должно менять объект, так как есть фильтр по Food
                    foods.Add((Food)GameObjects[i]);
                }
            }

            return foods;
        }
        public Food FindClosestFood(Fish fish)
        {
            List<Food> foods = GetFoodList();
            Food closest = null;

            double g, gClosest;

            foreach (Food f in foods)
            {
                if (f.hidden == false)
                {
                    //Гипотенуза G у f (приближенная траектория)
                    g = Math.Sqrt(
                                    f.Location.X - (fish.Location.X + fish.BackgroundImage.Width / 2) +
                                    f.Location.Y - (fish.Location.Y + fish.BackgroundImage.Height / 2)
                                 );

                    //f в зоне обзора рыбы
                    if (g < fish.fov)
                    {
                        //Если ближайшая ещё не найдена -> нам не с чем сравнивать -> f - ближайшая
                        if (closest != null)
                        {
                            //Есть другая еда, которая может быть ближе.
                            //Посчитаем путь до неё
                            gClosest = Math.Sqrt(
                                                   Math.Pow(closest.Location.X - (fish.Location.X + fish.BackgroundImage.Width / 2), 2) +
                                                   Math.Pow(closest.Location.Y - (fish.Location.Y + fish.BackgroundImage.Height / 2), 2)
                                                );
                            //Путь до closest больше чем до f -> f - ближайшая
                            if (g > gClosest)
                            {
                                closest = f;
                            }
                            //else closest = closest
                        }
                        else
                        {
                            closest = f;
                        }

                    }
                }
                
            }

            return closest;
        }



        private void cMenuNewFish(object sender, EventArgs e)
        {
            f1(sender, e);
            f2(sender, e);
            f3(sender, e);
            f4(sender, e);
            f5(sender, e);
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
        }
        private void UpdateAll(object sender, EventArgs e)
        {
            foreach (GameObject g in GameObjects)
            {
                g.Update(timer.Interval);
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

        private void PlaceCastle(object sender, EventArgs e)
        {
            GraphicObject temp_grO = new GraphicObject("../../data/textures/object/castle.png", 1, 500, 100);
        }

        private void NewGameO(object sender, EventArgs e)
        {
            PlaceShell(sender, e);
            PlaceWeed(sender, e);
            PlaceRock(sender, e);
            PlaceCastle(sender, e);
        }

        private void PlaceShell(object sender, EventArgs e)
        {
            InitGameObject("shell", random.Next(10, GraphicObject.ScrW-10), random.Next(0, GraphicObject.ScrH));
        }

        private void PlaceWeed(object sender, EventArgs e)
        {
            InitGameObject("weed", random.Next(10, GraphicObject.ScrW - 10), random.Next(0, GraphicObject.ScrH));
        }

        private void PlaceRock(object sender, EventArgs e)
        {
            InitGameObject("rock", random.Next(10, GraphicObject.ScrW - 10), random.Next(0, GraphicObject.ScrH));
        }
        private void PlaceCastleGO(object sender, EventArgs e)
        {
            InitGameObject("castle", random.Next(10, GraphicObject.ScrW) - 10, random.Next(0, GraphicObject.ScrH));
        }



        private void Aquarium_Load(object sender, EventArgs e)
        {
            trayIcon.ShowBalloonTip(
                5000,
                "Буль-Буль!\n",
                "Аквариум запущен!\n" +
                "Нажмите правой кнопкой по иконке для взаимодействий!",
                ToolTipIcon.Info
            );
        }

        private void PlaceGranule(object sender, EventArgs e)
        {
            int rnx = random.Next(10, GraphicObject.ScrW - 10);
            int rny = random.Next(0, GraphicObject.ScrH);
            InitFood("granule", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("granule", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("granule", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
        }

        private void PlacePill(object sender, EventArgs e)
        {
            int rnx = random.Next(10, GraphicObject.ScrW - 10);
            int rny = random.Next(0, GraphicObject.ScrH);
            InitFood("pill", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("pill", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("pill", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
        }

        private void PlaceFlake(object sender, EventArgs e)
        {
            int rnx = random.Next(10, GraphicObject.ScrW - 10);
            int rny = random.Next(0, GraphicObject.ScrH);
            InitFood("flake", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("flake", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
            InitFood("flake", rnx + random.Next(-150, 150), rny + random.Next(-50, 50));
        }

        private void PlaceFood(object sender, EventArgs e)
        {
            PlaceGranule(sender, e);
            PlacePill(sender, e);
            PlaceFlake(sender, e);
        }
    }

}
