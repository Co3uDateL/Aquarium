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
        //const не подходит, так как часть пути ../../ берётся из консоли уже после запуска приложения
        // из Aquarium/bin/Debug
        // в  Aquarium/data/textures/
        public static readonly string TexturePath = "../Aquarium/data/textures/";

        /* readonly - поля и статичные методы */
        public static readonly int ScrW = Screen.PrimaryScreen.Bounds.Width;
        public static readonly int ScrH = Screen.PrimaryScreen.Bounds.Height;
        public static readonly Point ScrBounds = new Point(ScrW, ScrH);
        //Определяет "пол" симуляции - на панели задач
        public static readonly int floorY = Screen.PrimaryScreen.WorkingArea.Bottom;

        /// <summary>
        /// Определяет ближе ли точка p к 0; 0; чем bounds
        /// </summary>
        /// <param name="p">точка для которой выполняется проверка</param>
        /// <param name="bounds">правая нижняя точка границы (начало в 0; 0)</param>
        /// <returns></returns>
        public static bool Intersects(Rectangle Lrec, Rectangle Rrec)
        {
            return Lrec.IntersectsWith(Rrec);
        }
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
        public static Bitmap GetImage(string path)
        {
            Bitmap temp = null;

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
                    ""+path); 
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
        public void SetImage(Bitmap sample)
        {
            //Если есть старый объект - закроем его картинку, освободим ресурсы
            if (BackgroundImage != null)
            {
                BackgroundImage.Dispose();
            }

            //Если по какой-то причине в функцию не передана картинка, нарисуем ошибку
            if (sample == null)
            {
                sample = new Bitmap( "../../data/textures/error.png" );
            }

            this.Size = sample.Size;
            BackgroundImage = sample;
        }
        /// <summary>
        /// Устанавливает изображение формы на загруженное по переданному пути
        /// </summary>
        /// <param name="path"></param>
        public void SetImage(string path)
        {
            Bitmap bitmap = GetImage(path);
            SetImage( bitmap );
        }
        /// <summary>
        /// Устанавливает изображение формы на изображение донора
        /// </summary>
        /// <param name="donor"></param>
        public void SetImage(GraphicObject donor)
        {
            SetImage((Bitmap)donor.BackgroundImage);
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

        /// <summary>
        /// Создаёт графический объект из файла по пути path
        /// </summary>
        /// <param name="path"></param>
        public GraphicObject(string path)
        {
            InitializeComponent();
            //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
            this.Hide();
                TransparencyKey = BackColor; //White
                SetImage(path);
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
                SetImage(sample);
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
                SetImage((Bitmap)sample.BackgroundImage);
                Location = sample.Location;
                Size = sample.Size;
                AutoSizeMode = sample.AutoSizeMode;
            this.Show();

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
        /// Мгновенно передвинет левый верхний угол формы в заданные координаты
        /// </summary>
        public virtual void gMoveOn(int dx, int dy)
        {
            Location = new Point(Location.X + dx, Location.Y + dy);
        }
    }
    /// <summary>
    /// Игровой объект
    /// "Более физический" объект, имеет границы, не может пересекаться с д
    /// </summary>
    public class GameObject : GraphicObject
    {
        protected double rx, ry;
        public GameObject()
        {
            rx = Location.X;
            ry = Location.Y;
        }
        public bool IsCollidingWith(GameObject target)
        {
            
            return false;
        }
        public double GetCollidingForce(GameObject target)
        {
            
            return 0;
        }
        public virtual void Update(int dt)
        {
            //Гравитация и просчёт столкновений с другими объектами
            if ( (this.Location.Y + this.Height) <= floorY )
            {
                gMoveOn(0, 1);
            }
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
