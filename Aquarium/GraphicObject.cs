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
            BackgroundImage = new Bitmap(BackgroundImage, Math.Abs(width), Math.Abs(height) );
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

            //Ограничение размера Bitmap = ushort.MaxValue
            if ( (w < ushort.MaxValue) && (h < ushort.MaxValue) )
            {
                try
                {
                    BackgroundImage = new Bitmap(BackgroundImage, w, h);
                    this.Size = BackgroundImage.Size;
                }
                catch
                {
                    MessageBox.Show("Ошибка при масштабировании изображения\n"
                        + "Множитель: " + scale + "; Итоговый размер: " + w + " x " + h);
                }
            }


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
        protected double rx, ry;
        /// <summary>
        /// Ускорение по оси Y. (отрицательные значения для всплытия)
        /// </summary>
        public double Fa = 0;

        public double SpeedY = 0;
        //TODO Coliding
        public bool IsCollidingWith(GameObject target)
        {
            //Rectangle Rec1 = new Rectangle(this.Location, this.Size);
            //Rectangle Rec2 = new Rectangle(target.Location, target.Size);

            //if ( Rec1.IntersectsWith(Rec2) )

            //Экономим на переменных
            if ( new Rectangle(this.Location, this.Size).IntersectsWith(new Rectangle(target.Location, target.Size)))
            {
                return true;
            }
            //else
            return false;
        }
        //TODO Coliding
        public double GetCollidingForce(GameObject target)
        {
            Rectangle intersection = new Rectangle(this.Location, this.Size);
            intersection.Intersect(new Rectangle(target.Location, target.Size));
            //По теореме Пифагора диагональ d = sqrt( a^2 + b^2 )
            return Math.Sqrt( Math.Pow( intersection.Width, 2) + Math.Pow( intersection.Height, 2 ) );
        }

        public virtual void gMoveTo(double rx, double ry)
        {
            //Высота картинки пробивает пол
            if ( (ry + Height) > floorY)
            {
                ry = floorY - Height;
            }
            //Картинка пробивает потолок
            else if (ry < topY)
            {
                ry = topY;
            }

            Location = new Point((int) rx, (int) ry);
        }
        public virtual void gMoveOn(double dRx, double dRy)
        {
            rx += dRx;
            ry += dRy;
            Location = new Point((int) rx, (int) ry);
        }
        public virtual void Update(int dt)
        {
            SpeedY += Fa;
            //TODO Coliding

            //Собирается ниже пола
            if (Location.Y + Height + SpeedY > floorY)
            {
                gMoveTo(Location.X, floorY - Height);
                Fa = 0;
                SpeedY = 0;
            }
            else
            {
                //Выше потолка
                if (Location.Y + SpeedY < topY)
                {
                    gMoveTo(Location.X, topY);
                    Fa = 0;
                    SpeedY = 0;
                }
            //Норм
                else
                {
                    gMoveOn(0, SpeedY);
                }

            }



        }
        public GameObject()
        {
            rx = Location.X;
            ry = Location.Y;

            Fa = 500;
        }
        public GameObject(string path) : base(path)
        {
            rx = Location.X;
            ry = Location.Y;

            Fa = 500;
        }
        public GameObject(string path, double scale) : base(path, scale)
        {
            rx = Location.X;
            ry = Location.Y;

            Fa = 500;
        }
        public GameObject(string path, int width, int height) : base(path, width, height)
        {
            rx = Location.X;
            ry = Location.Y;

            Fa = 500;
        }
        public GameObject(string path, double scale, double pFa, int x, int y,  bool pColidingEnabled) : base(path, scale, x, y)
        {
            gMoveTo(x, y);
            rx = Location.X;
            ry = Location.Y;
            ColidingEnabled = pColidingEnabled;


            Fa = pFa;
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
