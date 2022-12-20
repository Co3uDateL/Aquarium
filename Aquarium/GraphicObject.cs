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
        //const не подходит, так как часть пути /../../ берётся из консоли уже после запуска приложения
        public static readonly string TexturePath = "/../../data/textures/";

        /* readonly - поля и статичные методы */
        public static readonly int ScrW = Screen.PrimaryScreen.Bounds.Width;
        public static readonly int ScrH = Screen.PrimaryScreen.Bounds.Height;
        public static readonly Point ScrBounds = new Point(ScrW, ScrH);

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
            try
            {
                //return new Bitmap(path);
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
                    "Не удалось загрузить картинку" +
                    "По пути: " + path);
            }

            return new Bitmap(path);
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
        /// Устанавливает изображение формы на загруженное по переданному пути
        /// </summary>
        /// <param name="path"></param>
        public void SetImage(string path)
        {
            BackgroundImage = GetImage(TexturePath + path);
        }
        /// <summary>
        /// Устанавливает изображение формы на переданное
        /// </summary>
        /// <param name="sample"></param>
        public void SetImage(Image sample)
        {
            BackgroundImage = sample;
        }
        /// <summary>
        /// Устанавливает изображение формы на изображение донора
        /// </summary>
        /// <param name="donor"></param>
        public void SetImage(GraphicObject donor)
        {
            SetImage(BackgroundImage);
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
            this.Hide();
                //Чтобы избежать странных изменений формы при её создании, сделаем все присваивания в фоновом режиме
                TransparencyKey = BackColor; //White
                SetImage(TexturePath + path);
            this.Show();
        }

        /// <summary>
        /// Создаёт графический объект из объекта Image
        /// </summary>
        /// <param name="path"></param>
        public GraphicObject(Image sample)
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
                SetImage(sample.BackgroundImage);
                Location = sample.Location;
                Size = sample.Size;
                AutoSizeMode = sample.AutoSizeMode;
            this.Show();

        }

        //Чтобы избежать переопределения наследуемого события Move, будет использовано название gMove (general Move)
        /// <summary>
        /// Мгновенно передвинет левый верхний угол формы в заданные координаты
        /// </summary>
        public virtual void gMove(int x, int y)
        {
            Location = new Point(x, y);
        }
        /// <summary>
        /// Мгновенно передвинет левый верхний угол формы в заданную точку
        /// </summary>
        public virtual void gMove(Point p)
        {
            Location = p;
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
        public override void gMove(int x, int y)
        {
            Location = new Point(x, y);
        }
        public bool IsCollidingWith(GameObject target)
        {
            return false;
        }

        public double GetCollidingForce(GameObject target)
        {
            
            return 0;
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
