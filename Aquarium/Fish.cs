using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Aquarium
{
    public partial class Fish : Form
    {
        //--- --- ---
        /// <summary>
        /// Стандартная максимальная скорость рыбы, не изменяется
        /// </summary>
        public double MaxSpeedConst = 200;

        /// <summary>
        /// Боится ли рыба курсор
        /// </summary>
        public bool cursorFear = true;

        /// <summary>
        /// Время в милисекундах до выхода из страха
        /// </summary>
        public uint memoryLasts = 100;

        /// <summary>
        ///Поле зрения рыбы
        /// </summary>
        public int fov = 400;

        /// <summary>
        /// Множитель ускорения во время испуга
        /// </summary>
        public double TriggeredSpeedMultiplier = 4;

        /// <summary>
        /// Задержка поворота рыбы 
        /// </summary>
        public double RotationSpeed = 50;

        //ushort oxygen = 100;
        //ushort saturation = 100;
        //--- --- ---

        private static Random rnd = new Random();

        static int maxx = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;
        static int maxy = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;

        /// <summary>
        /// Найден ли путь?
        /// </summary>
        private bool isPathfinded = false;

        /// <summary>
        /// Отражён ли спрайт рыбы (обычно смотрит вправо (+Х))
        /// </summary>
        private bool isFlipped = false;

        /// <summary>
        /// Испугана ли рыба?
        /// </summary>
        private bool isTriggered = false;

        /// <summary>
        /// Насколько рыба сыта
        /// </summary>
        //private double Saturation;

        /// <summary>
        /// Текущая скорость
        /// </summary>
        private double CurSpeed;

        /// <summary>
        /// Скорость с которой рыба уплывает во время испуга
        /// </summary>
        private double TriggeredSpeed;

        /// <summary>
        /// Количество милисекунд на которые обновляется рыба при вызове Update
        /// </summary>
        public int dt;

        /// <summary>
        /// Координаты точки рандеву
        /// </summary>
        private int goX, goY;

        /// <summary>
        /// Координаты точки побега от курсора
        /// </summary>
        private int goTX, goTY;

        /// <summary>
        /// Реальное положение рыбы
        /// </summary>
        private double rx = rnd.Next(0, maxx), ry = rnd.Next(0, maxy);

        /// <summary>
        /// Скорость рыбы по осям Х и У
        /// </summary>
        private double SpeedX, SpeedY;

        /// <summary>
        /// Прошедшие тики с момента испуга
        /// </summary>
        private uint memoryTicks;

        /// <summary>
        /// Сколько тиков прошло с момента начала поворота
        /// </summary>
        private uint rotationTicks;

        /// <summary>
        /// Состояние рыбы
        /// 0 - плыть с возможностью переключения;
        /// 10 - найти новую случайную точку
        /// 11 - найти новую точку ближе к поверхности
        /// 12 - найти точку за укрытием
        /// 15 - (испуг) найти точку подальше от курсора, не забыть старую;
        /// 21 - искать тень
        /// 30 - сон;
        /// </summary>
        private byte state = 0;

        /// <summary>
        /// Изображение рыбы
        /// </summary>
        private Bitmap FishTexture;

        /// <summary>
        ///  Управление поведением рыбы с каждым тиком dt
        /// </summary>
        /// <param name="dt"></param>
        /// 
        double dx, dy, g;
        //--- --- ---

        private void InitializeBitmap(string name)
        {
            try
            {
                FishTexture = (Bitmap)Bitmap.FromFile("../../data/textures/" + name + ".png");
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = FishTexture;
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error." +
                    "Check the path to the bitmap.");
            }
        }

        private void Fish_MouseClick(object sender, MouseEventArgs e)
        {
            if ( e.Button == MouseButtons.Left)
            {
                DoDragDrop(this, DragDropEffects.Move);
            }
        }


        public Fish()
        {
            InitializeComponent();
            InitializeBitmap("1");
            TransparencyKey = BackColor; //White

            CurSpeed = MaxSpeedConst;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;

            //Растянем/Сузим форму под изображение
            this.Width = FishTexture.Width;
            this.Height = FishTexture.Height;
    }
        public Fish(string name)
        {
            InitializeComponent();
            InitializeBitmap(name);
            TransparencyKey = BackColor;

            CurSpeed = MaxSpeedConst;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;
        }
        public Fish(string name, int pMaxSpeedConst, uint pMemoryLasts, int pFov, double pTriggeredMultiplier)
        {
            InitializeComponent();
            InitializeBitmap(name);
            TransparencyKey = BackColor;

            MaxSpeedConst = pMaxSpeedConst;
            memoryLasts = pMemoryLasts;
            fov = pFov;

            CurSpeed = MaxSpeedConst;
            TriggeredSpeedMultiplier = pTriggeredMultiplier;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;
        }

        private void MainForm_Click(object sender, EventArgs e)
        {
            //active = !active;

           CurSpeed += 1000;
        }
        public void Update(int dt)
        {
           //TODO switch

            //Если Мы в районе точки рандеву или .не знаем куда плыть
            //По X: Левый угол формы - - - goX - - - Правый угол изображения

            //Анализируем ситуацию вокруг, если условие - меняем состояние
            if (state == 0)
            {
                //Переключатель "Создать новую точку"
                if ((Location.X - 100 < goX) && (goX < (Location.X + pictureBox1.Image.Width + 100)) || !isPathfinded)
                {
                    //По Y: верхний угол формы - - - goY - - - Нижний угол изображения
                    if ((Location.Y - 100 < goY) && (goY < (Location.Y + pictureBox1.Image.Height + 100)) || !isPathfinded)
                    {
                        state = 10;
                    }

                }

                //Переключатель "Создать новую точку отступления от испуга"
                if (cursorFear)
                {
                    dx = Control.MousePosition.X - (Location.X + pictureBox1.Image.Width / 2);
                    dy = Control.MousePosition.Y - (Location.Y + pictureBox1.Image.Height / 2);
                    g = Math.Sqrt(dx * dx + dy * dy);

                    if ( (g < fov) && (!isTriggered))
                    {
                        state = 15;
                    }

                }

                //Переключатель "Успокоиться и вспомнить точку"
                if (isTriggered)
                {
                    {
                        if (memoryTicks < memoryLasts)
                        {
                            memoryTicks += (uint)dt;
                        }
                        else
                        {
                            memoryTicks = 0;
                            isTriggered = false;
                            goX = goTX;
                            goY = goTY;

                            CurSpeed = MaxSpeedConst; 

                            state = 0;
                        }
                        
                    }
                }
            }

            //Ищем новую точку если требуют состояния, с их целями
            if ((state >= 10) && (state < 20))
            {
                if (state == 10)
                {
                    goX = rnd.Next(maxx) - pictureBox1.Image.Width;
                    goY = rnd.Next(maxy) - pictureBox1.Image.Height;
                }

                if (state == 15)
                {
                    goTX = goX;
                    goTY = goY;

                    isTriggered = true;

                    CurSpeed = TriggeredSpeed;

                    //TODO case
                    //Курсор в поле зрения!
                    //Приближается справа, справа-сверху, сверху
                    if ((dx >= 0) & (dy >= 0))
                    {
                        //Сгенерируем новую точку и придумаем как её достичь
                        goX = rnd.Next(0, Control.MousePosition.X) - pictureBox1.Image.Width;
                        goY = rnd.Next(Control.MousePosition.Y, maxy) - pictureBox1.Image.Height;
                    }
                    //Приближается справа - снизу 
                    if ((dx > 0) & (dy < 0))
                    {
                        goX = rnd.Next(0, Control.MousePosition.X) - pictureBox1.Image.Width;
                        goY = rnd.Next(Control.MousePosition.Y, maxy) - pictureBox1.Image.Height;
                    }
                    //Приближается слева, слева - снизу, снизу
                    if ((dx <= 0) & (dy >= 0))
                    {
                        goX = rnd.Next(Control.MousePosition.X, maxx) - pictureBox1.Image.Width;
                        goY = rnd.Next(0, Control.MousePosition.Y) - pictureBox1.Image.Height;
                    }
                    //Приближается слева - сверху IV
                    if ((dx < 0) & (dy < 0))
                    {
                        goX = rnd.Next(Control.MousePosition.X, maxx) - pictureBox1.Image.Width;
                        goY = rnd.Next(Control.MousePosition.Y, maxy) - pictureBox1.Image.Height;
                    }
                }

                state = 0;
            }

            //И придумаем как её достичь
            {
                dx = goX - (Location.X + pictureBox1.Image.Width / 2);
                dy = goY - (Location.Y + pictureBox1.Image.Height / 2);
                g = Math.Sqrt(dx * dx + dy * dy);

                SpeedX = CurSpeed * dx / g;
                SpeedY = CurSpeed * dy / g;

                isPathfinded = true;
            }

            //Перевернём если собираемся в другом направлении
            if ( ((SpeedX < 0) && (!isFlipped)) || ((SpeedX > 0) && (isFlipped)) )
            {
                FishTexture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                pictureBox1.Image = FishTexture;
                isFlipped = !isFlipped;
            }

            rx += SpeedX * dt / 1000;
            ry += SpeedY * dt / 1000;

            Location = new Point((int)rx, (int)ry);

        }
        private void Fish_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
