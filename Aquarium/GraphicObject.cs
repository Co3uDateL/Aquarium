﻿using System;
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
    /// <summary>
    /// Невидимая форма с картинкой
    /// Служит для рисования объектов без логики на экране
    /// </summary>
    public partial class GraphicObject : Form
    {
        // запуск в Aquarium/bin/Debug
        // текстуры в  Aquarium/data/textures/
        //public const string TexturePath = "../Aquarium/data/textures/";

        /* readonly - поля и статичные методы */
        public static readonly int ScrW = Screen.PrimaryScreen.Bounds.Width;
        public static readonly int ScrH = Screen.PrimaryScreen.Bounds.Height;
        public static readonly Point ScrBounds = new Point(ScrW, ScrH);

        /// <summary>
        /// Определяет Y границу симуляции - по панели задач (В нестандартных случаях закрепления панели - нижняя граница экрана)
        /// </summary>
        public static readonly int floorY = Screen.PrimaryScreen.WorkingArea.Bottom;
        public static readonly int topY = Screen.PrimaryScreen.WorkingArea.Top + 200;

        protected bool IsDragged = false;

        //Свойство "Перетаскивается". Когда активируется должно запомнить офсеты
        public bool _IsDragged_
        {
            get
            {
                return IsDragged;
            }

            set
            {
                if (value)
                {
                    //Начинаем перетаскивание
                    IsDragged = value;
                    DragAndDropOffestX = MousePosition.X - Location.X;
                    DragAndDropOffestY = MousePosition.Y - Location.Y;
                }
                else IsDragged = value;
            }
        }

        protected int DragAndDropOffestX = 0;
        protected int DragAndDropOffestY = 0;
        public static bool InScrBounds(Point p)
        {
            //&& = если первый результат false, не сравнивать дальше
            if
            (
                ( (p.X >= 0) && (p.X <= ScrBounds.X) )
                &&
                ( (p.Y >= 0 ) && (p.Y <= ScrBounds.Y) )
            )
            {
                return true;
            }
            //else
            return false;
        }
        public static bool InScrBounds(int x, int y)
        {
           return InScrBounds(new Point(x, y));
        }

        //TODO Всё сверху было бы неплохо перенести в библиотеку или program,
        //Для удобства отладки временно оставлено тут

        /// <summary>
        /// Инициализирует Bitmap по пути path с защитами (сообщение пользователю в случае ошибки)
        /// </summary>
        /// <param name="path">Полный путь до файла</param>
        /// <returns>Возвращает null, если картинку не удалось загрузить</returns>
        public static Bitmap GetBitmap(string path)
        {
            Bitmap temp = null;

            //Если файл не обнаружится, по какой-то причине вызывается исключение неправильного аргумента

            try
            {
                temp = new Bitmap(path);
            }
            catch (System.IO.FileNotFoundException)
            {
                MessageBox.Show("Произошла ошибка!\n" +
                    "Не удалось обнаружить картинку\n" +
                    "По пути: " +path);
            }
            catch (System.IO.FileLoadException)
            {
                MessageBox.Show("Произошла ошибка!" +
                    "Не удалось загрузить картинку");
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Произошла ошбика!\n"+
                    ""+path+"\n"+
                    ae.Message); 
            }

            return temp;
        }
        //static public Bitmap GetRandomBitmapFromDir(string path)
        //{
        //    string way = "../../textures/";
        //    string[] files = Directory.GetFiles(way + path, "*.png", SearchOption.TopDirectoryOnly);
        //    return new Bitmap(files[random.Next(files.Length)]);
        //}
        //static public Bitmap GetRandomBitmapFromDir(string path, out int id)
        //{
        //    string way = "../../textures/";
        //    string[] files = Directory.GetFiles(way + path, "*.png", SearchOption.TopDirectoryOnly);
        //    id = random.Next(files.Length);
        //    return new Bitmap(files[id]);
        //}
        public static Bitmap ResizeBitmap(Bitmap original, int width, int height)
        {
            width = Math.Abs(width);
            height = Math.Abs(height);

            if ( (width + height) < 2) //Очень малая площадь около нуля
            {
                width = (int) ( (original.Width+1) * 10);
                height = (int) ( (original.Height+1) * 10);
            }
            try
            {
                //a holder for the result
                Bitmap result = new Bitmap(width, height);

                //use a graphics object to draw the resized image into the bitmap
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    //set the resize quality modes to high quality
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                    //draw the image into the target bitmap
                    graphics.DrawImage(original, 0, 0, result.Width, result.Height);
                }

                //return the resulting bitmap
                return result;
            }
            catch (Exception exc)
            {
                MessageBox.Show(
                    width + " " + height +"\n" + 
                    "Ошибка масштабирования\n" +
                    exc.Message);
                return original;
            }
        }

        /* Методы класса 
           Многие методы чрезмерно раздробленны, (функции состоят из одной строчки - вызова другой)
           
        Против
             - Плохая оптимизация памяти
        За
             + Позволяет легко изменять ключевые механики, что важно для родителя
               Например, если вместо BackgroundImage начать использовать PictureBox
             + Устраняет дублирование
            (+) Учебные цели
         */

        /// <summary>
        /// Устанавливает изображение формы на переданное
        /// </summary>
        /// <param name="sample"></param>
        public void SetBitmap(Bitmap sample)
        {
            //Если есть старый объект - закроем его картинку, освободим ресурсы
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
            }

            //Если по какой-то причине в функцию не передана картинка, нарисуем ошибку
            if (sample == null)
            {
                //TODO Защита в случае отсутствия картинки
                //sample = new Bitmap( "../../data/textures/error.png" );
                sample = GetBitmap("../../data/textures/error.png");
            }

            this.Size = sample.Size;
            BackgroundImage = sample;
        }
        /// <summary>
        /// Устанавливает изображение формы на загруженное по переданному пути
        /// </summary>
        /// <param name="path"></param>
        public void SetBitmap(string path)
        {
            Bitmap bitmap = GetBitmap(path);
            SetBitmap( bitmap );
        }
        /// <summary>
        /// Устанавливает изображение формы на изображение донора
        /// </summary>
        /// <param name="donor"></param>
        public void SetBitmap(GraphicObject donor)
        {
            SetBitmap((Bitmap)donor.BackgroundImage);
        }

        /// <summary>
        /// Установить размеры графического объекта на модуль переданных
        /// int для лучшей совместимости
        /// </summary>
        public void ResizeBitmap(int width, int height)
        {
            //Этот грустный метод оставляет большую белую некрасивую обводку
            //BackgroundImage = new Bitmap(BackgroundImage, Math.Abs(width), Math.Abs(height) );
            //this.Size = BackgroundImage.Size;

            //Метод ближайшего соседа
            BackgroundImage = ResizeBitmap((Bitmap)BackgroundImage, width, height);
            this.Size = BackgroundImage.Size;

        }

        /// <summary>
        /// Умножить размеры объекта на переданный множитель
        /// </summary>
        /// <param name="scale"></param>
        public void ResizeBitmap(double scale)
        {
            int w, h;
            w = (int)(BackgroundImage.Width * Math.Abs(scale));
            h = (int)(BackgroundImage.Height * Math.Abs(scale));

            ResizeBitmap(w, h);
        }
        //Чтобы избежать переопределения наследуемого события Move, будет использовано название gMove (general Move)
        /// <summary>
        /// Мгновенно передвинет левый верхний угол формы в заданные координаты
        /// </summary>
        public virtual void gMoveTo(int x, int y)
        {
            Location = new Point(x, y);
        }
        /// <summary>
        /// Мгновенно передвинет левый верхний угол формы в заданную точку
        /// </summary>
        public virtual void gMoveTo(Point p)
        {
            Location = p;
        }
        //Чтобы избежать переопределения наследуемого события Move, будет использовано название gMove (general Move)
        /// <summary>
        /// Мгновенно передвинет форму на переданный вектор
        /// </summary>
        public virtual void gMoveOn(int dx, int dy)
        {
            Location = new Point(Location.X + dx, Location.Y + dy);
        }

        /* Конструкторы класса */
        /// <summary>
        /// Создаёт графический объект без изображения (невидимая форма)
        /// </summary>
        public GraphicObject()
        {
            InitializeComponent();
            this.Hide();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            TransparencyKey = BackColor; //White

            //GO.ActiveForm.BackgroundImage = GetImage(TexturePath + "error.png");
            this.Show();
        }
        
        //SetImage

        /// <summary>
        /// Создаёт графический объект из файла по пути path
        /// </summary>
        /// <param name="path"></param>
        public GraphicObject(string path)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White
            SetBitmap(path);
            this.Show();
        }

        /// <summary>
        /// Создаёт графический объект из объекта Image
        /// </summary>
        /// <param name="path"></param>
        public GraphicObject(Bitmap sample)
        {
            InitializeComponent();
            this.Hide();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            TransparencyKey = BackColor; //White
            SetBitmap(sample);
            this.Show();
        }

        /// <summary>
        /// Создаёт графический объект из объекта Image
        /// </summary>
        /// <param name="path"></param>
        public GraphicObject(GraphicObject sample)
        {
            InitializeComponent();

            this.Hide();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            TransparencyKey = BackColor; //White
            SetBitmap((Bitmap)sample.BackgroundImage);
            Location = sample.Location;
            Size = sample.Size;
            AutoSizeMode = sample.AutoSizeMode;
            this.Show();

        }

        //SetImage(path) + Resize()

        /// <summary>
        /// Создаёт графический объект из файла по пути path и устанавливает размеры на модуль переданных
        /// </summary>
        public GraphicObject(string path, int width, int height)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White
            SetBitmap(path);
            ResizeBitmap(width, height);
            this.Show();
        }

        /// <summary>
        /// Создаёт графический объект из файла по пути path и умножает размеры на модуль переданного множителя
        /// </summary>
        public GraphicObject(string path, double scale)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White
            SetBitmap(path);
            ResizeBitmap(scale);
            this.Show();
        }

        //TODO
        //SetImage(image) + Resize()
        //SetImage(GraphicObject) + Resize()

        //SetImage(path) + Resize() + gMove()
        /// <summary>
        /// Создаёт графический объект из файла по пути path
        /// с указанными параметрами
        /// </summary>
        public GraphicObject(string path, double scale, Point to)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White

            SetBitmap(path);
            ResizeBitmap(scale);
            gMoveTo(to);

            this.Show();
        }
        /// <summary>
        /// Создаёт графический объект из файла по пути path
        /// с указанными параметрами
        /// </summary>
        public GraphicObject(string path, double scale, int toX, int toY)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White

            SetBitmap(path);
            ResizeBitmap(scale);
            gMoveTo(toX, toY);

            this.Show();
        }

        /// <summary>
        /// Создаёт графический объект из файла по пути path
        /// с указанными параметрами
        /// </summary>
        public GraphicObject(Bitmap sample, double scale, Point to)
        {
            //TODO Кажется, дублирование неизбежно. Конструкторы повторяют друг друга и отличаются разве что парой строк
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
            TransparencyKey = BackColor; //White

            SetBitmap(sample);
            ResizeBitmap(scale);
            gMoveTo(to);

            this.Show();
        }

        private void eDragAndDropStart(object sender, MouseEventArgs e)
        {
            DragAndDropStart();
        }

        private void eDragAndDropMove(object sender, MouseEventArgs e)
        {
            DragAndDropMove();
        }

        private void eDragAndDropEnd(object sender, MouseEventArgs e)
        {
            DragAndDropEnd();
        }

        //Вызывается по событию eDragAndDropStart! 
        public virtual void DragAndDropStart()
        {
            //Не предусмотрено для GraphicObject
        }

        //Вызывается по событию eDragAndDropMove! 
        public virtual void DragAndDropMove()
        {
            //Не предусмотрено для GraphicObject
        }

        //Вызывается по событию eDragAndDropEnd! 
        public virtual void DragAndDropEnd()
        {
            //Не предусмотрено для GraphicObject
        }
    }
    /// <summary>
    /// Игровой объект
    /// "Более физический" объект, имеет границы, не может пересекаться с д
    /// </summary>
    public class GameObject : GraphicObject
    {
        //TODO
        /// <summary>
        ///Приклеен ли объект к заднему фону (отключает гравитацию и перемещение)
        /// </summary>
        //bool IsClued = false;
        /// <summary>
        /// Если включено, разрешает обработку столкновений с этим объектом
        /// </summary>
        public bool ColidingEnabled = true;
        /// <summary>
        /// Реальные коордианты объекта
        /// </summary>
        protected double rx;
        protected double ry;

        /// <summary>
        /// Ускорение по оси Y. (отрицательные значения для всплытия)
        /// </summary>
        protected double Acceleration = 1;
        protected double LastAcceleration = 0;
        protected double SpeedY = 0;

        //TODO Colidings
        public bool IsCollidingWith(Rectangle target)
        {
             return (new Rectangle(this.Location, this.Size).IntersectsWith(target));
        }
        public bool IsCollidingWith(GameObject target)
        {
            return IsCollidingWith(new Rectangle(target.Location, target.Size));
        }
        public bool IsCollidingWith(Point p)
        {
            return IsCollidingWith(new Rectangle(p, new Size(1, 1)));
        }

        ////TODO Coliding
        //public double GetCollidingForce(GameObject target)
        //{
        //    Rectangle intersection = new Rectangle(this.Location, this.Size);
        //    intersection.Intersect(new Rectangle(target.Location, target.Size));
        //    //По теореме Пифагора диагональ d = sqrt( a^2 + b^2 )
        //    return Math.Sqrt( Math.Pow( intersection.Width, 2) + Math.Pow( intersection.Height, 2 ) );
        //}
        public virtual void gMoveTo(double tx, double ty)
        {
            rx = tx;
            ry = ty;
            Location = new Point((int) tx, (int) ty);
        }
        public virtual void gMoveOn(double dRx, double dRy)
        {
            rx += dRx;
            ry += dRy;
            Location = new Point((int) rx, (int) ry);
        }


        public override void DragAndDropStart()
        {
            //no Drag&Drop for GraphicObject
            //Взять за центр
            IsDragged = true;
            DragAndDropOffestX = MousePosition.X - Location.X;
            DragAndDropOffestY = MousePosition.Y - Location.Y;
        }
        public override void DragAndDropMove()
        {
            if (IsDragged) { gMoveTo(MousePosition.X - DragAndDropOffestX, MousePosition.Y - DragAndDropOffestY); } 
        }
        public override void DragAndDropEnd()
        {
            //no Drag&Drop for GraphicObject
            IsDragged = false;
        }
        public virtual void Update(int dt)
        {
            /// <summary>
            /// Подвергнуться действию гравитации
            /// </summary>

            //В конце подвергнуться действию гравитцаии
            if ( (!IsDragged) || (Visible) )
            {
                //if (Acceleration != LastAcceleration)
                //{
                //    Acceleration = LastAcceleration;
                //}

                //TODO Переделать
                Acceleration = LastAcceleration;
                SpeedY += Acceleration;

                //TODO Coliding

                //Низ картинки будет ниже пола?
                if (Location.Y + BackgroundImage.Height + SpeedY + Acceleration > floorY)
                {
                    gMoveTo(Location.X, floorY - BackgroundImage.Height);
                    LastAcceleration = Acceleration;
                    Acceleration = 0;
                    SpeedY = 0;
                }
                else
                {
                    //Верх - выше потолка?
                    if (Location.Y + SpeedY <= topY)
                    {
                        gMoveTo(Location.X, topY + 1);
                        Acceleration = 0;
                        SpeedY = 0;
                    }
                    //Норм
                    else
                    {
                        gMoveOn(0, SpeedY);
                    }
                }
            }
        }

        public GameObject(string path) : base(path)
        {
            rx = Location.X;
            ry = Location.Y;

            Acceleration = 0;
        }
        public GameObject(string path, double scale) : base(path, scale)
        {
            rx = Location.X;
            ry = Location.Y;

            Acceleration = 0;
        }
        public GameObject(string path, int width, int height) : base(path, width, height)
        {
            rx = Location.X;
            ry = Location.Y;

            Acceleration = 0;
        }
        public GameObject(string path, double scale, double pFa, int x, int y,  bool pColidingEnabled) : base(path, scale, x, y)
        {
            gMoveTo(x, y);
            rx = Location.X;
            ry = Location.Y;
            ColidingEnabled = pColidingEnabled;

            Acceleration = pFa;
            LastAcceleration = Acceleration;
        }

    }
    public class Food : GameObject
    {
        private double MaxCalories;
        private double _calories;
        public bool hidden = false;
        public double calories
        {
            get
            {
                return _calories;
            }

            set
            {
                if (value < 0)
                {
                    consumed();
                }
                else
                {
                    _calories = value;
                }
            }
        }
        public int smell;

        public void consumed()
        {
            _calories = 0;
            hidden = true;

            this.Visible = false;

        }
        //TODO type enumerator

        /// <summary>
        /// Создаст еду
        /// </summary>
        /// <param name="type"></param>
        public Food(string path, int px, int py, int pCalories, double pscale, double pFa, int pSmell, bool pColidingEnabled) 
             : base(path, pscale, pFa, px, py, pColidingEnabled)
        {
            calories = pCalories;
            smell = pSmell;

            hidden = false;
        }
    }

    public class Fish : GameObject
    {
        //--- --- ---
        /// <summary>
        /// Стандартная максимальная скорость рыбы, не изменяется
        /// </summary>
        public double MaxSpeedConst;

        /// <summary>
        /// Боится ли рыба курсор
        /// </summary>
        public bool cursorFear = true;

        /// <summary>
        /// Время в милисекундах до выхода из страха
        /// </summary>
        public uint memoryLasts;

        /// <summary>
        ///Поле зрения рыбы
        /// </summary>
        public int fov;

        /// <summary>
        /// Множитель ускорения во время испуга
        /// </summary>
        public double TriggeredSpeedMultiplier = 4;

        /// <summary>
        /// Задержка поворота рыбы 
        /// </summary>
        public uint RotationDelay;

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
        /// Замораживает update - функцию рыбы.
        /// </summary>

        //TODO Система питания и хищники
        /// <summary>
        /// Насколько рыба сыта
        /// </summary>
        //private double Saturation = 250;
        ///// <summary>
        ///// Максимум насыщения
        ///// </summary>
        //private double SaturationMax = 250;
        ///// <summary>
        ///// Сколько корма рыба есть за секунду жизни
        ///// </summary>
        //private double SaturationDrainMultiplier = 1;


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
        /// Координаты текущего вейпоинта
        /// </summary>
        private int goX, goY;

        /// <summary>
        /// Координаты прошлого вейпоинта
        /// </summary>
        private int goTX, goTY;

        /// <summary>
        /// Скорость рыбы по осям Х (по Y унаследована от GameObject)
        /// </summary>
        private double SpeedX;

        /// <summary>
        /// Прошедшие тики с момента испуга
        /// </summary>
        private uint memoryTicks;

        /// <summary>
        /// Сколько тиков прошло с момента начала поворота
        /// </summary>
        private uint rotationTicks;

        /// Еда которую мы хотим съесть
        private Food TargetFood = null;

        private uint foodSize = 1000;

        private int Width, Height;

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

        //TODO избавиться от переменных
        /// <summary>
        ///  Вектора для управления рыбой, вынесены в общую часть, чтобы не персоздавать их в коде
        /// </summary>
        /// <param name="dx"> По X </param>
        /// <param name="dy"> По Y </param>
        /// <param name="g"> Гипотенуза (сумма векторов) = расстояние до мышки </param>
        /// 
        private double dx, dy, g;
        //}
        public Fish(string path, int pMaxSpeedConst, bool pcursorFear, uint pMemoryLasts, int pFov, double pTriggeredMultiplier, uint pRotationDelay)
             : base(path)
        {
            //InitializeComponent();
            //InitializeBitmap("fish/"+name);
            TransparencyKey = BackColor;

            cursorFear = pcursorFear;
            MaxSpeedConst = pMaxSpeedConst;
            memoryLasts = pMemoryLasts;
            fov = pFov;

            CurSpeed = MaxSpeedConst;
            RotationDelay = pRotationDelay;
            TriggeredSpeedMultiplier = pTriggeredMultiplier;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;

            rx = Aquarium.random.Next(0, ScrW);
            ry = Aquarium.random.Next(topY, floorY);

            Width = BackgroundImage.Width;
            Height = BackgroundImage.Height;

            //SaturationDrainMultiplier = Math.Sqrt(Math.Pow(BackgroundImage.Width, 2) + Math.Pow(BackgroundImage.Height, 2)); //Около 90.5
            //SaturationMax = SaturationDrainMultiplier * 25; //Проживёт 25 секунд без еды
            //Saturation = 0.8 * SaturationMax;

            gMoveTo(rx, ry);
        }

        public override void DragAndDropEnd()
        {
            IsDragged = false;

            //Вспомнить путь до точки
            StateMachine(11, 0);
            //Переключиться в то состояние в котором мы были
            state = 0;
            StateMachine(state, 0);
        }

        public void CalculateSpeedXY()
        {
            //State 11 replacement

            //UNHACK    
            //this.BackColor = Color.Gray;
            //Пересчитаем новый путь до старой точки

            //чтобы всегда видеть картинку на экране
            //Х должен быть Больше Ширины картинки,
            //goX = Math.Max(goX, BackgroundImage.Width + 110);
            ////X должен быть Меньше Ширина экрана - Ширина картинки
            //goX = Math.Min(goX, ScrW - BackgroundImage.Width);

            ////Так как ось Y направлена вниз
            ////Y должен быть Больше ПотолокY + Высота картинки
            //goY = Math.Max(goY, topY + BackgroundImage.Height);
            ////Y должен быть Меньше Высоты экрана - Высота картинки
            //goY = Math.Min(goY, floorY - BackgroundImage.Height);


            //Вектор Х
            dx = goX - (Location.X + BackgroundImage.Width / 2);
            //Вектор У
            dy = goY - (Location.Y + BackgroundImage.Height / 2);
            //Гипотенуза G (приближенная траектория)
            g = Math.Sqrt(dx * dx + dy * dy);

            //Установка значений, "план" достижения области точки
            SpeedX = CurSpeed * dx / g;
            SpeedY = CurSpeed * dy / g;

            //Так как вызывается из разных состояний, для универсальности не будем переходить в состояние из 11-го состояния.
            //Укажите переход в другое состояние после вызова 11.
        }


        //private double FoodDrain()
        //{
        //    return (CurSpeed / (double)1000 * SaturationDrainMultiplier / (double)1000 * dt/(double) 1000);
        //}

        public void StateMachine(byte inpState, int dt)
        {
            //Обработки состояния.
            //В зависимости от номера состояния
            //Воспроизводит то или иное поведение рыбы.
            switch (inpState)
            {
                //S0 Проверить условия - Выбрать состояние
                //
                case 0:
                    {
                        //UNHACK    
                        //this.BackColor = Color.Green;
                        // S0 -> S15
                        //Если рыба боиться курсора 
                        if (cursorFear)
                        {
                            //S 15: видит курсор 
                            dx = Control.MousePosition.X - (Location.X + BackgroundImage.Width / 2);
                            dy = Control.MousePosition.Y - (Location.Y + BackgroundImage.Height / 2);
                            // g = sqrt(dx^2 + dy^2) < fov
                            if ((Math.Sqrt(dx * dx + dy * dy) < fov))
                            {
                                state = 15;
                                StateMachine(state, 0);
                                break;
                            }

                            //else g >= FOV -> Рыба успешно оторвалась и больше не напугана
                        }

                        //Рыба увидела еду
                        //TODO Исправить плохой код
                        //Переход - перенайти еду
                        TargetFood = Program.MainForm.FindClosestFood(this);
                        if (TargetFood != null)
                        {
                            state = 2;
                            break;
                        }

                        //S0 -> S 10: Если точка рандеву не достигнута или путь ещё не построен

                        //По Х: Левый угол формы --- goX --- Правый угол формы
                        //TODO Добавить переменную разброса int spread = 50;
                        //Разброс в 50 с каждой стороны для исключения ситуаций когда рыба не попадает в точку из-за неточности рассчёта
                        if ((Location.X - 50 < goX) && (goX < (Location.X + BackgroundImage.Width + 50)) || !isPathfinded)
                        {
                            //По Y: верхний угол формы - - - goY - - - Нижний угол изображения
                            if ((Location.Y - 50 < goY) && (goY < (Location.Y + BackgroundImage.Height + 50)) || !isPathfinded)
                            {
                                state = 10;
                                StateMachine(state, 0);
                                break;
                            }

                        }


                        //S0 - Поворот
                        //Если в другую сторону - повернуть
                        if (((SpeedX < 0) && (!isFlipped)) || ((SpeedX > 0) && (isFlipped)))
                        {
                                //state = 5; Стоит ли это выносить в отдельное состояние?
                                //Время поворота закончилось
                                if (rotationTicks >= RotationDelay)
                                {
                                    Bitmap rotatedBI = new Bitmap(BackgroundImage);
                                    rotatedBI.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                    BackgroundImage = rotatedBI;
                                    isFlipped = !isFlipped;
                                    rotationTicks = 0;

                                    state = 0;
                                    //Не выполняем мгновенный переход в цикличное состояние движения
                                    break;
                                }
                                else
                                {//Продолжаем поворот
                                    rotationTicks += (uint)dt;
                                    gMoveOn(SpeedX / 10 * dt / 1000, SpeedY / 10 * dt / 1000);
                                    break;
                                }
                        }

                        //S0 - Движение
                        //Если всё ок - плыть дальше
                        //Если выполнилось хоть одно условие, отработает встроенный в них break и сюда не дойдёт

                        //TODO Изменить формулу
                        //Число пройденных пикселей / 100 000 (На полном желудке в 100
                        //double check = FoodDrain();
                        //Saturation -= FoodDrain();
                        gMoveOn(SpeedX * dt / 1000, SpeedY * dt / 1000);
                        break;
                    }

                case 5:
                    {
                        //UNHACK    
                        //this.BackColor = Color.Red;
                        //S 5 <Time Out> --> S 10: Если рыба напугана и время испуга истекло
                        //Переключатель "Успокоиться и вспомнить точку"
                        //!isTriggered || g >= FOV (Следует из предыдущего выражения.) If нужен, так как может возникнуть при g >= FOV
                        if (isTriggered) //Всё ещё напугана
                        {
                            if (memoryTicks < memoryLasts)
                            {
                                memoryTicks += (uint)dt;
                                //S5 - Поворот
                                //S5 - Движение
                                //Если в другую сторону - повернуть

                                if (((SpeedX < 0) && (!isFlipped)) || ((SpeedX > 0) && (isFlipped)))
                                {
                                    //state = 5; Стоит ли это выносить в отдельное состояние?
                                    //Время поворота закончилось
                                    if (rotationTicks >= RotationDelay)
                                    {
                                        Bitmap rotatedBI = new Bitmap(BackgroundImage);
                                        rotatedBI.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                        BackgroundImage = rotatedBI;
                                        isFlipped = !isFlipped;
                                        rotationTicks = 0;

                                        state = 0;
                                        //Не выполняем мгновенный переход в цикличное состояние движения
                                    }
                                    else
                                    {//Продолжаем поворот
                                        rotationTicks += (uint)dt;
                                        gMoveOn(SpeedX / 10 * dt / 1000, SpeedY / 10 * dt / 1000);
                                    }
                                }
                                else
                                {
                                    //В испуге тратим больше еды из-за скорости
                                    //Saturation -= FoodDrain();
                                    gMoveOn(SpeedX * dt / 1000, SpeedY * dt / 1000);
                                }

                            }
                            else //S5 -> S11 -> S0 
                            {
                                memoryTicks = 0;
                                isTriggered = false;
                                goX = goTX;
                                goY = goTY;
                                
                                CurSpeed = MaxSpeedConst;

                                CalculateSpeedXY();

                                //S5 -> S0
                                state = 0;
                                StateMachine(state, 0);
                                break; //TODO Нужен ли тут брейк?
                            }
                        }
                        break;
                    }

                case 10:
                    {
                        //UNHACK    
                        //this.BackColor = Color.Wheat;
                        //Генерация новой точки
                        goX = Aquarium.random.Next(ScrW) - Width;
                        goY = Aquarium.random.Next(ScrH) - Height;

                        //Рассчёт пути до неё
                        CalculateSpeedXY();

                        //Флаг "Путь расчитан"
                        isPathfinded = true;

                        //S10 - S0
                        state = 0;
                        StateMachine(state, dt);
                        break;
                    }

                //Найти еду
                //case 12:
                //    {
                //        TargetFood = Program.MainForm.FindClosestFood(this);
                //        if ( TargetFood == null )
                //        {
                //            state = 0;
                //            break;
                //        }

                //        goX = TargetFood.Location.X;
                //        goY = TargetFood.Location.Y;
                //        CalculateSpeedXY(); 

                //        if ( Aquarium.random.Next(2) == 1)
                //        {
                //            this.BackColor = Color.Red;
                //        }
                //        else this.BackColor = Color.Orange;

                //        state = 2;
                //        Не выполняем мгновенный переход в цикличное состояние движения
                //        break;
                //    }

                //Плыть к еде
                case 2:
                    {
                        //Переход 1 "Напугана"
                        //Если рыба боиться курсора 
                        if (cursorFear)
                        {
                            //S 15: видит курсор
                            //STACKOVERFLOW?
                            dx = Control.MousePosition.X - (Location.X + BackgroundImage.Width / 2);
                            dy = Control.MousePosition.Y - (Location.Y + BackgroundImage.Height / 2);
                            // g = sqrt(dx^2 + dy^2) < fov
                            if ((Math.Sqrt(dx * dx + dy * dy) < fov))
                            {
                                state = 15;
                                StateMachine(state, 0);
                                break;
                            }

                            //else g >= FOV -> Рыба успешно оторвалась и больше не напугана
                        }

                        //Переход 2 - перенайти еду
                        TargetFood = Program.MainForm.FindClosestFood(this);
                        if (TargetFood != null)
                        {
                            goX = TargetFood.Location.X;
                            goY = TargetFood.Location.Y;
                            CalculateSpeedXY();
                        }
                        else
                        {
                            state = 0;
                            break;
                        }

                        //Переход 3 "Достигли еды"
                        if ((Location.X-50  < TargetFood.Location.X) && (TargetFood.Location.X < (Location.X+50 + BackgroundImage.Width)))
                        {
                            //По Y: верхний угол формы - - - goY - - - Нижний угол изображения
                            if ((Location.Y < TargetFood.Location.Y) && (TargetFood.Location.Y < (Location.Y + BackgroundImage.Height)))
                            {
                                state = 52;
                                //Не выполняем мгновенный переход в цикличное состояние движения
                                break;
                            }

                        }

                        //S2 - Поворот
                        //Если в другую сторону - повернуть
                        if (((SpeedX < 0) && (!isFlipped)) || ((SpeedX > 0) && (isFlipped)))
                        {
                            //state = 5; Стоит ли это выносить в отдельное состояние?
                            //Время поворота закончилось
                            if (rotationTicks >= RotationDelay)
                            {
                                Bitmap rotatedBI = new Bitmap(BackgroundImage);
                                rotatedBI.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                BackgroundImage = rotatedBI;
                                isFlipped = !isFlipped;
                                rotationTicks = 0;

                                state = 0;
                                //Не выполняем мгновенный переход в цикличное состояние движения
                                break;
                            }
                            else
                            {//Продолжаем поворот
                                rotationTicks += (uint)dt;
                                gMoveOn(SpeedX / 10 * dt / 1000, SpeedY / 10 * dt / 1000);
                                break;
                            }
                        }


                        gMoveOn(SpeedX * dt / 1000, SpeedY * dt / 1000);
                        break;
                    }

                //Есть еду
                case 52:
                    {
                        if ((TargetFood.calories >= foodSize* dt / (double) 1000 ))
                        {
                            TargetFood.calories = (TargetFood.calories - foodSize * dt / (double)1000);
                            //Fish.saturation += foodSize;
                            break;
                        }
                        else
                        {
                            //TODO Перееделать под трату калорий
                            //saturation += TargetFood.calories
                            TargetFood.calories = 0;

                            TargetFood.consumed();

                            state = 0;
                            //Не выполняем мгновенный переход в цикличное состояние движения
                            break;
                        }
                    }


                case 15:
                    {
                        //UNHACK    
                        //this.BackColor = Color.Orange;
                        //Поиск точки побега при испуге
                        goTX = goX;
                        goTY = goY;

                        CurSpeed = TriggeredSpeed;
                        isTriggered = true;

                        //TODO case
                        //Курсор в поле зрения!
                        
                        //I Приближается справа, справа-сверху, сверху
                        if ((dx >= 0) & (dy >= 0))
                        {
                            //Сгенерируем новую точку и придумаем как её достичь
                            goX = Math.Max(BackgroundImage.Width, Aquarium.random.Next(0, Control.MousePosition.X) - BackgroundImage.Width);
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }
                        //II Приближается справа - снизу 
                        if ((dx > 0) & (dy < 0))
                        {
                            goX = Aquarium.random.Next(0, Control.MousePosition.X) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }
                        //III Приближается слева, слева - снизу, снизу
                        if ((dx <= 0) & (dy >= 0))
                        {
                            goX = Aquarium.random.Next(Control.MousePosition.X, ScrW) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(0, Control.MousePosition.Y) - BackgroundImage.Height;
                        }
                        //IV Приближается слева - сверху
                        if ((dx < 0) & (dy < 0))
                        {
                            goX = Aquarium.random.Next(Control.MousePosition.X, ScrW) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }

                        //S15 - S11 - S5
                        CalculateSpeedXY();

                        //S15 - S5
                        state = 5;
                        StateMachine(state, dt); //Сразу продолжим движение в состоянии 5 за 10 мс (минимальный тик)
                        break;
                    }


                case 51:
                    {
                        //StartEatingFood
                        break;
                    }
            }
        }


        public override void Update(int dt)
        {
            //Активировать переключатель состояний если рыбу не перетягивают
            if (!IsDragged)
            {
                StateMachine(state, dt);
            }

        }
    }
}
