using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aquarium
{
    public partial class Fish : GameObject
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
        public uint RotationDelay = 100;

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

        //TODO Перетаскивание рыбы
        /// <summary>
        /// Замораживает update - функцию рыбы.
        /// </summary>
        //private bool isDragged = false;

        //TODO Система питания и хищники
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
        private Bitmap Texture;

        //TODO избавиться от переменных 
        /// <summary>
        ///  Вектора для управления рыбой, вынесены в общую часть, чтобы не персоздавать их в коде
        /// </summary>
        /// <param name="dx"> По X </param>
        /// <param name="dy"> По Y </param>
        /// <param name="g"> Гипотенуза (сумма векторов) = расстояние до мышки </param>
        /// 
        double dx, dy, g;
        //--- --- ---
        //TODO Перенести в GraphOBJ
        private void InitializeBitmap(string name)
        {
            try
            {
                Texture = (Bitmap)Bitmap.FromFile("../../data/textures/" + name + ".png");
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = Texture;
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("There was an error." +
                    "Check the path to the bitmap.");
            }
        }

        //TODO Drag & Drop
        private void Fish_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Fish_MouseUp(object sender, MouseEventArgs e)
        {

        }

        public Fish()
        {
            InitializeComponent();
            InitializeBitmap("1");
            TransparencyKey = BackColor; //White

            CurSpeed = MaxSpeedConst;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;

            //Растянем/Сузим форму под изображение
            this.Width = Texture.Width;
            this.Height = Texture.Height;
        }
        public Fish(string name)
        {
            InitializeComponent();
            InitializeBitmap(name);
            TransparencyKey = BackColor;

            CurSpeed = MaxSpeedConst;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;
        }
        public Fish(string name, int pMaxSpeedConst, bool pcursorFear, uint pMemoryLasts, int pFov, double pTriggeredMultiplier, uint pRotationDelay)
        {
            InitializeComponent();
            InitializeBitmap(name);
            TransparencyKey = BackColor;

            cursorFear = pcursorFear;
            MaxSpeedConst = pMaxSpeedConst;
            memoryLasts = pMemoryLasts;
            fov = pFov;

            CurSpeed = MaxSpeedConst;
            RotationDelay = pRotationDelay;
            TriggeredSpeedMultiplier = pTriggeredMultiplier;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;
        }

        public virtual void Update(int dt)
        {
            //TODO switch
            //TODO Выделить состояния в отдельные методы с текстовыми названиями

            //Если Мы в районе точки рандеву или .не знаем куда плыть
            //По X: Левый угол формы - - - goX - - - Правый угол изображения

            //Анализируем ситуацию вокруг, если условие - меняем состояние
            //switch (state)
            //{
            if (state == 0)
            //0:
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

                    if ((g < fov) && (!isTriggered))
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
                //Генерация новой точки
                if (state == 10)
                {
                    goX = rnd.Next(maxx) - pictureBox1.Image.Width;
                    goY = rnd.Next(maxy) - pictureBox1.Image.Height;
                }

                //Поиск точки побега при испуге
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

            //? Зачем это выделено скобками, если переключения логикой нет?
            {
                //Вектор Х
                dx = goX - (Location.X + pictureBox1.Image.Width / 2);
                //Вектор У
                dy = goY - (Location.Y + pictureBox1.Image.Height / 2);
                g = Math.Sqrt(dx * dx + dy * dy);

                //Установка значений "план" достижения области точки
                SpeedX = CurSpeed * dx / g;
                SpeedY = CurSpeed * dy / g;

                isPathfinded = true;
            }

            //Перевернём если собираемся в другом направлении
            if (((SpeedX < 0) && (!isFlipped)) || ((SpeedX > 0) && (isFlipped)))
            {
                //state = 5;
                if (rotationTicks >= RotationDelay)
                {
                    Texture.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    pictureBox1.Image = Texture;
                    isFlipped = !isFlipped;
                    rotationTicks = 0;
                }
                else
                {
                    rotationTicks += (uint)dt;
                    //Надо как-то отключить перемещение у рыбы, чтобы это не вызвало проблем
                    //Состояние 5?
                    //Решено сильно замедлять рыбу
                    SpeedX /= 10;
                    SpeedY /= 10;

                }
            }
            //}
            rx += SpeedX * dt / 1000;
            ry += SpeedY * dt / 1000;

            Location = new Point((int)rx, (int)ry);

        }
        private void Fish_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }

    public class Predator : Fish
    {
        public int sharpvision;
        public override void Update(int dt)
        {

        }
        //public override void FindFood(int dt)
        //{

        //}

        //public override void Move(int dt)
        //{

        //}

        //public override void ChangeState(int dt)
        //{

        //}


    }
}
