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
    public partial class TestClasses : Form
    {
        public List<GameObject> GameObjects = new List<GameObject>();
        public static Random random = new Random();
        public static readonly string TexturePath = "../../data/textures/";
        public TestClasses()
        {
            InitializeComponent();
        }

        private void GetBitmap1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/"));
        }
        private void getBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Нужно занять картинку другим процессом чтобы тест отработал
            GraphicObject test = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/test.png"));
        }

        private void getBitmapToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject("WRONGPATH");
        }

        private void проверкаГраницfalseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/test.png"));
            test.gMoveTo(-200, -200);
            if (!GraphicObject.InScrBounds(test.Location)) { MessageBox.Show("Графический объект не в границах экрана."); }
            test.gMoveTo(1000, 500);
            if (GraphicObject.InScrBounds(test.Location)) { MessageBox.Show("Графический объект в границах экрана."); }
        }

        private void ResizeCheckClick(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/test.png"));
            test.ResizeBitmap(-250, -250);
            test.ResizeBitmap(70000, 70000);
        }

        private void setBitmapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/rock.png"));
            MessageBox.Show("Графический объект меняет картинку.");
            GraphicObject testobj = new GraphicObject(GraphicObject.GetBitmap(TexturePath + "object/test.png"));
            test.SetBitmap(testobj);
        }

        private void isDraggedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GraphicObject test = new GraphicObject(TexturePath + "object/rock.png");
            test._IsDragged_ = true;
                
            MessageBox.Show("Графический объект начал перетаскивание");
            test.DragAndDropEnd();
            MessageBox.Show("Графический объект завершил перетаскивание");
        }
        private void isCollidingWithToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameObject test = new GameObject(TexturePath + "object/rock.png", 100, 100);
            test.gMoveTo(50, 50);
            Point p = new Point(100, 100);

            if (test.IsCollidingWith(p)) { test.BackColor = Color.Red; }
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GameObject test = new GameObject(TexturePath + "object/rock.png", 1, 10, 500, 100, true);
            MessageBox.Show("Начинаем обновления");
            for (int i = 0; i < 100; i++) { test.Update(15); }

        }
        private void состояние0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);

            test.StateMachine(0, 10);
            test.BackColor = Color.Blue;
        }

        private void состояние1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);

            test.StateMachine(1, 10);
            test.BackColor = Color.Yellow;

        }

        private void состояние5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);

            test.StateMachine(5, 10);
            test.BackColor = Color.Green;

        }

        private void состояние5ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);

            test.StateMachine(10, 10);
            test.BackColor = Color.Red;
        }

        private void состояние5ToolStripMenuItem2_Click(object sender, EventArgs e)
        {

            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);
            test.StateMachine(11, 10);
            test.BackColor = Color.Gray;
        }

        private void состояние5ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);
            test.StateMachine(15, 10);
            test.BackColor = Color.Black;

        }

        private void updateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Fish test = new Fish(TexturePath + "1.png", 300, true, 100, 200, 4, 50);
            for (int i = 0; i < 100; i++) { test.Update(10); }

        }

        private void dragAndDropToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    //GameObject

    }
}

