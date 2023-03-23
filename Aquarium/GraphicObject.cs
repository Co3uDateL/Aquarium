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
                MessageBox.Show("Произошла ошибка!"  +
                    "Не удалось обнаружить картинку" +
                    "По пути: "+path);
            }
            catch (System.IO.FileLoadException)
            {
                MessageBox.Show("Произошла ошибка!" +
                    "Не удалось загрузить картинку");
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show("Произошло что-то непонятное\n" +
                    "Оглядывайтесь вверх и по сторонам\n" +
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
            if ( (width + height) < 2) //Очень малая площадь около нуля
            {
                width = (int) (original.Width * 0.3);
                height = (int) (original.Height * 0.3);
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
        bool ColidingEnabled = true;
        /// <summary>
        /// Реальные коордианты объекта
        /// </summary>
        protected double rx;
        protected double ry;

        /// <summary>
        /// Ускорение по оси Y. (отрицательные значения для всплытия)
        /// </summary>
        protected double Acceleration = 0;
        protected double LastAcceleration = 0;
        protected double SpeedY = 0;
        //TODO Colidings
        //public bool IsCollidingWith(GameObject target)
        //{
        //    //Rectangle Rec1 = new Rectangle(this.Location, this.Size);
        //    //Rectangle Rec2 = new Rectangle(target.Location, target.Size);

        //    //if ( Rec1.IntersectsWith(Rec2) )

        //    //Экономим на переменных
        //    if ( new Rectangle(this.Location, this.Size).IntersectsWith(new Rectangle(target.Location, target.Size)))
        //    {
        //        return true;
        //    }
        //    //else
        //    return false;
        //}
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
            if (!IsDragged)
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

        public GameObject()
        {
            rx = Location.X;
            ry = Location.Y;

            Acceleration = 500;
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
        public int calories;
        public int size;
        public Color color;
        public int smell;
        public int masking;

        //TODO type enumerator

        /// <summary>
        /// Создаст еду указанного типа с случайной картинкой
        /// </summary>
        /// <param name="type"></param>
        Food(string path, int pCalories, int pSize, Color pColor, int pSmell, int pMasking) : base(path)
        {
            calories = pCalories;
            size = pSize;
            color = pColor;
            smell = pSmell;
            masking = pMasking;


        }
    }
    public class Fish : GameObject
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
        double dx, dy, g;
        //}
        public Fish(string path) : base(path)
        {
            //InitializeComponent();
            //InitializeBitmap("fish/" + name);
            TransparencyKey = BackColor;

            CurSpeed = MaxSpeedConst;
            TriggeredSpeed = CurSpeed * TriggeredSpeedMultiplier;

            rx = Aquarium.random.Next(0, ScrW);
            ry = Aquarium.random.Next(topY, floorY);
            gMoveTo(rx, ry);
        }
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
            gMoveTo(rx, ry);
        }

        public override void Update(int dt)
        {
            //Активировать переключатель состояний если рыбу не перетягивают
            if (!IsDragged)
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
                        if ((Location.X - 100 < goX) && (goX < (Location.X + BackgroundImage.Width + 100)) || !isPathfinded)
                        {
                            //По Y: верхний угол формы - - - goY - - - Нижний угол изображения
                            if ((Location.Y - 100 < goY) && (goY < (Location.Y + BackgroundImage.Height + 100)) || !isPathfinded)
                            {
                                state = 10;
                            }

                        }

                        //Переключатель "Создать новую точку отступления от испуга"
                        if (cursorFear)
                        {
                            dx = Control.MousePosition.X - (Location.X + BackgroundImage.Width / 2);
                            dy = Control.MousePosition.Y - (Location.Y + BackgroundImage.Height / 2);
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
                        goX = Aquarium.random.Next(ScrW) - Width;
                        goY = Aquarium.random.Next(ScrH) - Height;
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
                            goX = Aquarium.random.Next(0, Control.MousePosition.X) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }
                        //Приближается справа - снизу 
                        if ((dx > 0) & (dy < 0))
                        {
                            goX = Aquarium.random.Next(0, Control.MousePosition.X) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }
                        //Приближается слева, слева - снизу, снизу
                        if ((dx <= 0) & (dy >= 0))
                        {
                            goX = Aquarium.random.Next(Control.MousePosition.X, ScrW) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(0, Control.MousePosition.Y) - BackgroundImage.Height;
                        }
                        //Приближается слева - сверху IV
                        if ((dx < 0) & (dy < 0))
                        {
                            goX = Aquarium.random.Next(Control.MousePosition.X, ScrW) - BackgroundImage.Width;
                            goY = Aquarium.random.Next(Control.MousePosition.Y, ScrH) - BackgroundImage.Height;
                        }
                    }

                    state = 0;
                }
                //И придумаем как её достичь

                //? Зачем это выделено скобками, если переключения логикой нет?
                {
                    //Вектор Х
                    dx = goX - (Location.X + BackgroundImage.Width / 2);
                    //Вектор У
                    dy = goY - (Location.Y + BackgroundImage.Height / 2);
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
                        Bitmap rotatedBI = new Bitmap(BackgroundImage);
                        rotatedBI.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        BackgroundImage = rotatedBI;
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

        }
    }
}
