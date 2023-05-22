using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;


namespace лаба_ооп6
{
    public partial class Form1 : Form 
    {
        Storage<Shape> MyStorage = new Storage<Shape>(); //хранилище фигурок
        Tree tree;//дерево самих фигур
        public Form1()
        {
            InitializeComponent();
            (pictureBox1 as Control).KeyPress += new KeyPressEventHandler(PressEventHandler);// считывание нажатия мышки и выделение после
            tree = new Tree(MyStorage, treeView1);
            MyStorage.AddObserver(tree);//связь между хранилищем объектов и деревом
            treeView1.CheckBoxes = true;//флаг для группировки

        }

        public void PressEventHandler(object sender, KeyPressEventArgs e) //считывание нажатия каждой клавиши
        {
            if (MyStorage.size() != 0)
            {
                if (e.KeyChar == 119) MyStorage.get().OffsetXY(0, -1);
                if (e.KeyChar == 115) MyStorage.get().OffsetXY(0, 1);
                if (e.KeyChar == 97) MyStorage.get().OffsetXY(-1, 0);
                if (e.KeyChar == 100) MyStorage.get().OffsetXY(1, 0);
                if (e.KeyChar == 98) MyStorage.get().Grow(1);
                if (e.KeyChar == 118) MyStorage.get().Grow(-1);
                if (e.KeyChar == 122) MyStorage.prevCur();
                if (e.KeyChar == 120) MyStorage.nextCur();
                if (e.KeyChar == 8) MyStorage.del();
                if (e.KeyChar == 32)
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }

                if (e.KeyChar == 111)//добавление дополнительных углов у фигуры
                {
                    if (MyStorage.get() is Rhombus) ((Rhombus)MyStorage.get()).growN(1);
                    if (MyStorage.get() is Triangle) ((Triangle)MyStorage.get()).growN(1);
                }
                if (e.KeyChar == 112)//уменьшение дополнительных углов у фигуры
                {
                    if (MyStorage.get() is Rhombus) ((Rhombus)MyStorage.get()).growN(-1);
                    if (MyStorage.get() is Triangle) ((Triangle)MyStorage.get()).growN(-1);
                }



                if (e.KeyChar == 110)
                {
                    while (MyStorage.size() != 0) MyStorage.del();
                    pictureBox1.Invalidate();
                }
                if (e.KeyChar == 99)
                {

                    if (colorDialog1.ShowDialog() == DialogResult.OK) MyStorage.get().SetColor(colorDialog1.Color);
                }
                if (e.KeyChar == 107)
                {

                    if (colorDialog1.ShowDialog() == DialogResult.OK) MyStorage.get().SetColor(colorDialog1.Color);
                }

                Random rnd = new Random();
                int obj = rnd.Next(1, 3);
                if (e.KeyChar == 49)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new CCircle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 51)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Rhombus(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, 4, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }
                if (e.KeyChar == 50)
                {
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Triangle(rnd.Next(4, pictureBox1.Width - 50), rnd.Next(4, pictureBox1.Height - 50), rad, 3, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width - 50, pictureBox1.Height - 50));
                }

            }
            tree.Print();//вывод свойств фигур в окно
            pictureBox1.Invalidate();
        }



        private void pictureBox1_Paint_1(object sender, PaintEventArgs e) //создание самого окна для фигур
        {
            if (MyStorage.size() != 0)
            {
                MyStorage.toFirst();
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())
                {
                    MyStorage.getIterator().DrawObj(e.Graphics);
                    if (MyStorage.isChecked() == true) MyStorage.getIterator().DrawRectangle(e.Graphics, new Pen(Color.Gray, 2));
                }
                MyStorage.get().DrawRectangle(e.Graphics, new Pen(Color.Red, 1));
            }

        }

        private void pictureBox1_MouseDown_1(object sender, MouseEventArgs e)//считывание и отслеживание на создание и выделение фигур
        {
            bool isFinded = false;
            if (MyStorage.size() != 0)
            {
                MyStorage.toFirst();
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next())
                {
                    if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Left)
                    {
                        isFinded = true;
                        MyStorage.setCurPTR();
                        break;
                    }
                    if (MyStorage.getIterator().Find(e.X, e.Y) == true && e.Button == MouseButtons.Right)
                    {//
                        isFinded = true;
                        MyStorage.check();
                        break;
                    }
                }
            }
            if (isFinded == false)//считывается какая из фигур должна отрисовываться относительно того какая выбрана
            {
                if (radioButton1.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new CCircle(e.X, e.Y, rad, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton2.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Rhombus(e.X, e.Y, rad, 4, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                }
                else
                if (radioButton3.Checked == true)
                {
                    Random rnd = new Random();
                    int rad = rnd.Next(10, 100);
                    MyStorage.add(new Triangle(e.X, e.Y, rad, 3, Color.FromArgb(rnd.Next(0, 256), rnd.Next(0, 256), rnd.Next(0, 256)), pictureBox1.Width, pictureBox1.Height));
                   

                }
                if (checkBox1.Checked)
                {
                    MyStorage.get().sticky = true;
                    MyStorage.get().AddStorage(MyStorage);
                }
            }
            if (isFinded == true && MyStorage.get().sticky == true) checkBox1.Checked = true; else checkBox1.Checked = false;
            pictureBox1.Invalidate();

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            pictureBox1.Focus();

        }

        private void button1_Click(object sender, EventArgs e)//кнопка сохранить объекты 
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream f = new FileStream(saveFileDialog1.FileName, FileMode.Create);//записывается в файл данные
                StreamWriter stream = new StreamWriter(f);//запись на данном этапе
                stream.WriteLine(MyStorage.size());//узнать размер хранилища
                if (MyStorage.size() != 0)
                {
                    MyStorage.toFirst();//перебираем все обьекты из MyStorage и все сохраняем 
                    for (int i = 0; i < MyStorage.size(); i++, MyStorage.next()) MyStorage.getIterator().Save(stream);
                    //сохраняем информацию и положение 
                }
                stream.Close();//закрыть закрываемый поток
                f.Close();//закрыть файл
            }

        }

        private void button4_Click(object sender, EventArgs e) //кнопка сгруппировать 
        {
            if (MyStorage.size() != 0)
            {
                SGroup group = new SGroup(pictureBox1.Width, pictureBox1.Height);//создаем новую группу для каких либо элементов
                MyStorage.toFirst();//переход к первому элементу
                int cnt = 0;
                for (int i = 0; i < MyStorage.size(); i++, MyStorage.next()) //считаем количество отмеченных элементов
                    if (MyStorage.isChecked() == true) cnt++;
                while (cnt != 0)
                {
                    if (MyStorage.isChecked() == true)
                    {
                        group.Add(MyStorage.getIterator());//добавляем в группу итератор если выбрана фигура 
                        MyStorage.delIterator();//удаляем из основного хранилища
                        cnt--;//пройти по всем элементам, которые были выбраны
                    }
                    if (MyStorage.size() != 0) MyStorage.next();//перейти к следуюзей группе
                }
                MyStorage.add(group);//добавление в группу
            }
            pictureBox1.Invalidate();//обновить основное окно

        }

        private void button3_Click(object sender, EventArgs e)//очистить 
        {
            while (MyStorage.size() != 0) MyStorage.del();//удалить все элементы из хранилища
            pictureBox1.Invalidate(); //обновляет (перерисовка)
        }

        private void button5_Click(object sender, EventArgs e)//разгруппировать
        {

                if (MyStorage.size() != 0 && MyStorage.get() is SGroup) //если это хранилище имеет связь с группой
                {
                    while (((SGroup)MyStorage.get()).size() != 0) // пока группа не станет пустой делать разгруппировку
                {
                    MyStorage.addAfter(((SGroup)MyStorage.get()).Out());
                    MyStorage.prevCur();
                    }
                MyStorage.del(); //удалить группу 
            }
                pictureBox1.Invalidate();
            

        }

        private void button2_Click(object sender, EventArgs e)//загрузить
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) //если нажата клавиша ОК
            {
                FileStream f = new FileStream(openFileDialog1.FileName, FileMode.Open); // открываем файл  
                StreamReader stream = new StreamReader(f);
                int i = Convert.ToInt32(stream.ReadLine()); //считываем 
                Factory shapeFactory = new ShapeFactory();//подключение шаблона проектирования
                for (; i > 0; i--)
                {//загружаем
                    string tmp = stream.ReadLine();
                    MyStorage.add(shapeFactory.createShape(tmp));//подключение шаблона проектирования к нашему хранилищу
                    MyStorage.get().Load(stream);
                }
                stream.Close();
                f.Close();
            }
            pictureBox1.Invalidate();//обновляем окно
            tree.Print();//отрисовываем дерево
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e) 
            //внизу окна панель которая отвечает за фигуры
        {
            if ((e.Action == TreeViewAction.ByKeyboard || e.Action == TreeViewAction.ByMouse) && e.Node.Text != "Фигуры")
            {


                TreeNode tmp = e.Node;

                while (tmp.Parent.Text != "Фигуры") tmp = tmp.Parent;
                treeView1.SelectedNode = tmp;
                MyStorage.toFirst();
                MyStorage.setCurPTR();
                for (int i = 0; i < tmp.Index; i++)
                {

                    MyStorage.nextCur();
                }
                MyStorage.setCurPTR();
            }


        }

        private void button6_Click(object sender, EventArgs e)//кнопка удалить на панеле справа
        {
            if (MyStorage.size() != 0)
                MyStorage.del();
            pictureBox1.Invalidate();

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {//Это событие возникает, когда пользователь щелкает мышью любую часть узла дерева, включая знак
         //"плюс" (+) или "минус" (-), который указывает, свернут или развернут узел.
            TreeNode tmp = e.Node;

            if (e.Button == MouseButtons.Right)//считывание на правую
            {
                MyStorage.toFirst();
                MyStorage.setCurPTR();

                for (int i = 0; i < tmp.Index; i++)
                {
                    MyStorage.nextCur();
                }
                if (MyStorage.size() != 0 && MyStorage.get() is SGroup)//если не пусто и есть группа то
                {//на пкм выделяется выбраный обьект крассным
                    while (((SGroup)MyStorage.get()).size() != 0)
                    {
                        MyStorage.addAfter(((SGroup)MyStorage.get()).Out());
                        MyStorage.prevCur();
                    }
                    MyStorage.del();
                }
            }
            if (e.Button == MouseButtons.Left)//на левую
            {//на лкм выбираются обьекты и серым

                treeView1.SelectedNode = tmp;

                MyStorage.toFirst();
                for (int i = 0; i <= treeView1.SelectedNode.Index; i++)
                {
                    MyStorage.next();
                }
                MyStorage.check();

            }
            pictureBox1.Invalidate();

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)//отрисовка после какой-то проверки
        {//обновление окна, для добавления красной или серой обводки
            TreeNode tmp = e.Node;
            treeView1.SelectedNode = tmp;

            MyStorage.toFirst();

            for (int i = 0; i < treeView1.SelectedNode.Index; i++)
            {
                MyStorage.next();
            }
            MyStorage.setCurPTR();
            MyStorage.del();


            pictureBox1.Invalidate();


        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {//в случае изменения кнопки "липкий"
            if (MyStorage.size() != 0 && !(MyStorage.get() is SGroup))
            {
                MyStorage.get().sticky = checkBox1.Checked;//если нажат липкий
                if (checkBox1.Checked == true) MyStorage.get().AddStorage(MyStorage);//проверка на нажатие
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

};
public abstract class Shape : ObjObserved
{
    public string name; //имена для всех объектов и групп
    protected Rectangle rect; //область объекта для его отрисовки и выделения
    protected int X, Y, width, height; //x, y для позиции объектов и групп, w и h для учитывания границ отрисовки
    public bool sticky = false;
    abstract public void Resize();
    abstract public void SetXY(int _x, int _y);
    abstract public void OffsetXY(int _x, int _y);
    abstract public void SetColor(Color c);
    abstract public void Grow(int gr);
    abstract public void DrawObj(System.Drawing.Graphics e);
    abstract public void DrawRectangle(System.Drawing.Graphics e, Pen pen);
    abstract public bool Find(int _x, int _y);
    abstract public bool Find(Shape obj);
    abstract public void Clone();
    abstract public Rectangle GetRectangle();  //получить границы фигуры для контроля выхода за пределы
    abstract public void Save(StreamWriter stream);
    abstract public void Load(StreamReader stream);
    abstract public string GetInfo();

}

public abstract class Factory //паттерн
{
    public abstract Shape createShape(string name);
}

public class ShapeFactory : Factory
{
    public override Shape createShape(string name) //создание фигуры самой с описанием 
    {
        Shape shape;
        switch (name)
        {
            case "Circle":
                shape = new CCircle();
                break;
            case "Group":
                shape = new SGroup();
                break;
            case "Rhombus":
                shape = new Rhombus();
                break;
            case "Triangle":
                shape = new Triangle();
                break;
            default:
                shape = null;
                break;
        }
        return shape;
    }
}

public class SGroup : Shape //класс группировок и добавлений
{
    public Storage<Shape> sto;

    public SGroup()//конструктор
    {
        sto = new Storage<Shape>();
        name = "Group";
    }

    public SGroup(int Width, int Height)//с параметрами группа 
    {
        width = Width;
        height = Height;
        sto = new Storage<Shape>();
        name = "Group";
    }

    public void Add(Shape s)//добавление фигур в группировки ( окно снизу ) 
    {
        sto.add(s);
        sto.get().sticky = false; //флаг для проверки на "липкость"
        if (sto.size() == 1) rect = new Rectangle(s.GetRectangle().X, s.GetRectangle().Y, s.GetRectangle().Width, s.GetRectangle().Height);
        else
        {//добавление фигуры в квадрат группировки
            if (s.GetRectangle().Left < rect.Left)
            {
                int tmp = rect.Right;
                rect.X = s.GetRectangle().Left;
                rect.Width = tmp - rect.X;
            }
            if (s.GetRectangle().Right > rect.Right) rect.Width = s.GetRectangle().Right - rect.X;
            if (s.GetRectangle().Top < rect.Top)
            {
                int tmp = rect.Bottom;
                rect.Y = s.GetRectangle().Top;
                rect.Height = tmp - rect.Y;
            }
            if (s.GetRectangle().Bottom > rect.Bottom) rect.Height = s.GetRectangle().Bottom - rect.Y;
        }
    }

    public Shape Out()//выход из группы
    {
        if (sto.size() != 0)
        {
            Shape tmp = sto.get();
            sto.del();
            Resize();
            return tmp;
        }
        return null;
    }

    public int size()//получение информации о размере группы
    {
        return sto.size();
    }

    public override void Resize()//перекрытый метод 
    {
        if (sto.size() != 0)//определение выхода границ для объектов в группе
        {
            sto.toFirst();
            rect = sto.getIterator().GetRectangle();
            for (int i = 0; i < sto.size(); i++, sto.next())
            {
                if (sto.getIterator().GetRectangle().Left < rect.Left)
                {
                    int tmp = rect.Right;
                    rect.X = sto.getIterator().GetRectangle().Left;
                    rect.Width = tmp - rect.X;
                }
                if (sto.getIterator().GetRectangle().Right > rect.Right) rect.Width = sto.getIterator().GetRectangle().Right - rect.X;
                if (sto.getIterator().GetRectangle().Top < rect.Top)
                {
                    int tmp = rect.Bottom;
                    rect.Y = sto.getIterator().GetRectangle().Top;
                    rect.Height = tmp - rect.Y;
                }
                if (sto.getIterator().GetRectangle().Bottom > rect.Bottom) rect.Height = sto.getIterator().GetRectangle().Bottom - rect.Y;
            }
        }
    }

    public override void Clone() //для обработки исключений
    {
        throw new NotImplementedException();
    }

    public override void DrawObj(Graphics e) //отрисовка обьекта с учетом итератора 
    {
        if (sto.size() != 0)
        {
            sto.toFirst();
            for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().DrawObj(e);
        }
    }

    public override void Grow(int gr)//для увеличесния и уменьшения фигуры 
    {
        if (sto.size() != 0)
        {
            if (gr > 0 && rect.X + gr > 1 && gr + rect.Right < width - 1 && rect.Y + gr > 1 && gr + rect.Bottom < height - 1)
            {
                rect.X -= gr;
                rect.Y -= gr;
                rect.Width += 2 * gr;
                rect.Height += 2 * gr;
                sto.toFirst();
                for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().Grow(gr);
            }
            if (gr < 0)
            {
                sto.toFirst();
                for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().Grow(gr);
                if (gr < 0) Resize();
            }
        }
    }

    public override void OffsetXY(int _x, int _y) //получение информации от координаты мышки
    {
        if (sto.size() != 0)
        {
            if (rect.X + _x > 0 && _x + rect.Right < width)
            {
                rect.X += _x;
                sto.toFirst();
                for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().OffsetXY(_x, 0);
            }
            if (rect.Y + _y > 0 && _y + rect.Bottom < height)
            {
                rect.Y += _y;
                sto.toFirst();
                for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().OffsetXY(0, _y);
            }
        }
    }

    public override void SetColor(Color c) //установка цвета 
    {
        if (sto.size() != 0)
        {
            sto.toFirst();
            for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().SetColor(c);
        }
    }

    public override void SetXY(int _x, int _y)  //связь с  координатами 
    {
        throw new NotImplementedException();
    }

    public override Rectangle GetRectangle() //получение информации ( геттер для фигур) 
    {
        return rect;
    }

    public override bool Find(int _x, int _y)  //поиск попала ли фигура под определенные параметры(либо липкость, либо попадание мышки на границы фигуры)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }

    public override void DrawRectangle(Graphics e, Pen pen)// отрисовка фигур 
    {
        e.DrawRectangle(pen, rect);
    }

    public override void Save(StreamWriter stream)//сохранение в группу
    {
        stream.WriteLine("Group");
        stream.WriteLine(sto.size() + " " + width + " " + height);
        if (sto.size() != 0)
        {
            sto.toFirst();
            for (int i = 0; i < sto.size(); i++, sto.next()) sto.getIterator().Save(stream);
        }
    }

    public override void Load(StreamReader stream)  //отгрузка, чтобы все корректно загружалось 
    {
        ShapeFactory factory = new ShapeFactory();
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int i = Convert.ToInt32(data[0]);
        width = Convert.ToInt32(data[1]);
        height = Convert.ToInt32(data[2]);
        for (; i > 0; i--)
        {
            Shape tmp = factory.createShape(stream.ReadLine());
            tmp.Load(stream);
            Add(tmp);
        }
    }

    public override string GetInfo()//получение информции об обьектах 
    {
        return name + "    Size : " + sto.size().ToString();
    }

    public override bool Find(Shape obj) //для поиска, кто выделен
    {
        if (sto.size() != 0)
        {
            sto.toFirst();
            for (int i = 0; i < sto.size(); i++, sto.next()) if (sto.getIterator().Find(obj) == true) return true;
        }
        return false;
    }
}

public class CCircle : Shape //создание кругов ( наследник класса фигур) 
{
    protected Color color;
    protected int R;
    public CCircle()
    {

        X = 0;
        Y = 0;
        R = 0;
        name = "Circle";
    }
    public CCircle(int x, int y, int r, Color c, int Width, int Height)//с параметром создание фигуры-круг
    {

        this.X = x;
        this.Y = y;
        width = Width;
        height = Height;
        name = "Circle";
        color = c;
        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        R = r;
        rect = new Rectangle(x - R, y - R, 2 * R, 2 * R);

    }
    public override void SetXY(int x, int y)
    {
        throw new NotImplementedException();
    }
    public override void Resize()
    {
        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void DrawObj(Graphics e)
    {
        e.DrawEllipse(new Pen(Color.Black, 2), rect);
        e.FillEllipse(new SolidBrush(color), rect);
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage != null && storage.size() != 0 && sticky == true)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (Find(storage.getIterator()) == true && storage.getIterator() != this)
                {
                    if (storage.getIterator().sticky == false)
                        storage.getIterator().OffsetXY(_x, _y);
                }
            }
        }

        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }
    public override Rectangle GetRectangle()
    {
        return rect;
    }
    public override void Clone()
    {
        throw new NotImplementedException();
    }
    public override void Save(StreamWriter stream)//сохранение информации о кружке(цвет, координаты, липкость...)
    {
        stream.WriteLine("Circle");
        stream.WriteLine((rect.X + R) + " " + (rect.Y + R) + " " + R + " " + color.R + " " + color.G + " " + color.B + " " + width + " " + height + " " + sticky);
    }

    public override bool Find(int _x, int _y)//для поиска кружков, чтобы внизу в параметрах находил её
    {
        if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= R * R) return true; else return false;
    }
    public override void Load(StreamReader stream) //загрузка откуда-то
    {
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        X = Convert.ToInt32(data[0]);
        Y = Convert.ToInt32(data[1]);
        R = Convert.ToInt32(data[2]);
        color = Color.FromArgb(Convert.ToInt32(data[3]), Convert.ToInt32(data[4]), Convert.ToInt32(data[5]));
        width = Convert.ToInt32(data[6]);
        height = Convert.ToInt32(data[7]);
        sticky = Convert.ToBoolean(data[8]);
        Resize();
    }
    public override string GetInfo()//вывод информации на экран(внизу) о кружочке 
    {
        return name + "  X: " + X + " Y: " + Y + " Rad: " + R + " " + color.ToString() + "Sticky: " + sticky;
    }


    public override bool Find(Shape obj)
    {
        string[] data = obj.GetInfo().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (data[0] != "Group")
        {
            int _x = Convert.ToInt32(data[2]);
            int _y = Convert.ToInt32(data[4]);
            int _rad = Convert.ToInt32(data[6]);
            if (Math.Pow(X - _x, 2) + Math.Pow(Y - _y, 2) <= Math.Pow(R + _rad, 2)) return true;
        }
        else return obj.Find(this); //просим группу поискать нас
        return false;
    }

}
public class Rhombus : CCircle //создание ромбов ( наследник класса фигур) 
{
    private int n;
    List<PointF> first;
    public Rhombus() : base()
    {
        name = "Rhombus";
    }


    public Rhombus(int x, int y, int r, int n, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {
        this.n = n;
        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        Resize();
        name = "Rhombus";

    }
    public override void Clone()
    {
        throw new NotImplementedException();
    }

    public override void DrawObj(Graphics e)
    {
        e.DrawPolygon(new Pen(Color.Black, 2), first.ToArray());
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }
    public override void Load(StreamReader stream)
    {
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        X = Convert.ToInt32(data[0]);
        Y = Convert.ToInt32(data[1]);
        R = Convert.ToInt32(data[2]);
        n = Convert.ToInt32(data[3]);
        color = Color.FromArgb(Convert.ToInt32(data[2]), Convert.ToInt32(data[5]), Convert.ToInt32(data[5]));
        width = Convert.ToInt32(data[8]);
        height = Convert.ToInt32(data[8]);
        sticky = Convert.ToBoolean(data[9]);
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage != null && storage.size() != 0 && sticky == true)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (Find(storage.getIterator()) == true && storage.getIterator() != this)
                {
                    if (storage.getIterator().sticky == false)
                        storage.getIterator().OffsetXY(_x, _y);
                }
            }
        }

        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = 0; i < 360; i += 360 / n)
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }

        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {
        
    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
    public override void Save(StreamWriter stream)
    {
        stream.WriteLine("Rhombus");
        stream.WriteLine(X + " " + Y + " " + R + " " + n + " " + color.R + " " + color.G + " " + color.B + " " + width + " " + height + " " + sticky);
    }


    public override string GetInfo()
    {
        return name + "  X: " + X + " Y: " + Y + " Rad: " + R + " " + color.ToString() + "Sticky: " + sticky;
    }
    public void growN(int gr)
    {
        if (n + gr > 2) n += gr;
        Resize();
    }

}

public class Triangle : CCircle //создание треугольников ( наследник класса фигур) 
{
    private int n;
    private int rotate = 0;
    List<PointF> first;
    public Triangle() : base()
    {
        name = "Triangle";
    }


    public Triangle(int x, int y, int r, int n, Color c, int Width, int Height) : base(x, y, r, c, Width, Height)
    {
        this.n = n;
        if (r > x) r = x;
        if (x + r > width) r = width - x;
        if (r > y) r = y;
        if (y + r > height) r = height - y;
        Resize();
        name = "Triangle";

    }
    public override void Clone()
    {
        throw new NotImplementedException();
    }

    public override void DrawObj(Graphics e)
    {
        e.DrawPolygon(new Pen(Color.Black, 2), first.ToArray());
        e.FillPolygon(new SolidBrush(color), first.ToArray());
    }

    public override void DrawRectangle(Graphics e, Pen pen)
    {
        e.DrawRectangle(pen, rect);
    }

    public override Rectangle GetRectangle()
    {
        return rect;
    }

    public override void Grow(int inc)
    {
        if (R + inc < X && X + R + inc < width && R + inc < Y && Y + R + inc < height && R + inc > 0) R += inc;
        Resize();
    }
    public override void Load(StreamReader stream)//отгрузка
    {
        string[] data = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        X = Convert.ToInt32(data[0]);
        Y = Convert.ToInt32(data[1]);
        R = Convert.ToInt32(data[2]);
        n = Convert.ToInt32(data[3]);
        rotate = Convert.ToInt32(data[4]);
        color = Color.FromArgb(Convert.ToInt32(data[5]), Convert.ToInt32(data[6]), Convert.ToInt32(data[7]));
        width = Convert.ToInt32(data[8]);
        height = Convert.ToInt32(data[9]);
        sticky = Convert.ToBoolean(data[10]);
        Resize();
    }

    public override void OffsetXY(int _x, int _y)
    {
        if (storage != null && storage.size() != 0 && sticky == true)
        {
            storage.toFirst();
            for (int i = 0; i < storage.size(); i++, storage.next())
            {
                if (Find(storage.getIterator()) == true && storage.getIterator() != this)
                {
                    if (storage.getIterator().sticky == false)
                        storage.getIterator().OffsetXY(_x, _y);
                }
            }
        }

        if (X + _x > R && X + _x + R < width) X += _x;
        if (Y + _y > R && Y + _y + R < height) Y += _y;
        Resize();
    }

    public override void Resize()
    {
        first = null;
        first = new List<PointF>();
        for (int i = 30; i < 360; i += 360 / n)
        {
            double radiani = (double)(i * 3.14) / 180;
            float xx = X + (int)(R * Math.Cos(radiani));
            float yy = Y + (int)(R * Math.Sin(radiani));
            first.Add(new PointF(xx, yy));
        }

        rect = new Rectangle(X - R, Y - R, 2 * R, 2 * R);
    }

    public override void SetColor(Color c)
    {
        color = c;
    }

    public override void SetXY(int _x, int _y)
    {

    }

    public override bool Find(int _x, int _y)
    {
        if (rect.X < _x && _x < rect.Right && rect.Y < _y && _y < rect.Bottom) return true; else return false;
    }
    public override void Save(StreamWriter stream)
    {
        stream.WriteLine("Triangle");
        stream.WriteLine(X + " " + Y + " " + R + " " + n + " " + rotate + " " + color.R + " " + color.G + " " + color.B + " " + width + " " + height + " " + sticky);
    }


    public override string GetInfo()
    {
        return name + "  X: " + X + " Y: " + Y + " Rad: " + R + " " + color.ToString() + "Sticky: " + sticky;
    }
    public void growN(int gr)
    {
        if (n + gr > 2) n += gr;
        Resize();
    }


}
class DPanel : Panel
{
    public DPanel()
    {
        this.DoubleBuffered = true;
        this.ResizeRedraw = true;
    }
}

public class Observer //паттерн 
{
    public virtual void SubjectChanged() { return; }
}

class Tree : Observer 
{
    private Storage<Shape> sto1; //набор обьектов
    private TreeView tree1; //некий обьект который отображает коллекцию помеченных элементов 
    public Tree(Storage<Shape> sto, TreeView tree)
    {
        this.sto1 = sto; //передаем помеченные и сами обьекты 
        this.tree1 = tree;
    }

    public void Print()
    {
        tree1.Nodes.Clear();//создаем таблицу отмеченных фигур внизу экрана
        if (sto1.size() != 0)
        {
            int SelectedIndex = 0;
            TreeNode start = new TreeNode("Фигуры");
            sto1.toFirst();
            for (int i = 0; i < sto1.size(); i++, sto1.next()) //создаем строку для каждого с нужным индексом
            {
                if (sto1.getCurPTR() == sto1.getIteratorPTR()) SelectedIndex = i;
                PrintNode(start, sto1.getIterator());
            }
            tree1.Nodes.Add(start);

            for (int i = 0; i < sto1.size(); i++)//выделенные обьекты помечать красным не выделенные черным
            {
                sto1.next();
                tree1.SelectedNode = tree1.Nodes[0].Nodes[i];
                if (sto1.isChecked() == true)
                    tree1.SelectedNode.ForeColor = Color.Red;
                else tree1.SelectedNode.ForeColor = Color.Black;
            }
        }
        tree1.ExpandAll();

    }

    private void PrintNode(TreeNode node, Shape shape)//нарисовка фигур с учётом паттерна 
    {//сам метод отрисовки более упрощен
        if (shape is SGroup)
        {
            TreeNode tn = new TreeNode(shape.GetInfo());
            if (((SGroup)shape).sto.size() != 0)
            {
                ((SGroup)shape).sto.toFirst();
                for (int i = 0; i < ((SGroup)shape).sto.size(); i++, ((SGroup)shape).sto.next())
                    PrintNode(tn, ((SGroup)shape).sto.getIterator());
            }
            node.Nodes.Add(tn);
        }
        else
        {

            node.Nodes.Add(shape.GetInfo());
        }
    }

    public override void SubjectChanged()
    {
        Print();
    }
}

public class ObjObserved
{
    public Storage<Shape> storage;
    public void AddStorage(Storage<Shape> MyStorage)
    {
        storage = MyStorage;
    }
}
public class Observed
{
    private List<Observer> observers;
    public Observed()
    {
        observers = new List<Observer>();
    }
    public void AddObserver(Observer o)
    {
        observers.Add(o);
    }
    public void Notify()
    {
        foreach (Observer observer in observers) observer.SubjectChanged();
    }
}


public class Storage<MStorage> : Observed //управление хранилищем 
{
    public class list
    {
        public MStorage data { get; set; }
        public list right { get; set; }
        public list left { get; set; }
        public bool isChecked = false;
    };
    private list first;
    private list last;
    private list current;
    private list iterator;

    private int rate;
    public Storage()
    {
        first = null;
        rate = 0;
    }
    public void add(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = last;
            last.right = tmp;
            last = tmp;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
        }
        last.right = first;
        current = tmp;
        first.left = last;
        rate++;
        Notify();
    }
    public void addBefore(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = (current.left);
            (current.left).right = tmp;
            current.left = tmp;
            tmp.right = current;
            if (current == first) first = current.left;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void addAfter(MStorage figure)
    {
        list tmp = new list();
        tmp.data = figure;
        if (first != null)
        {
            tmp.left = current;
            tmp.right = current.right;
            (current.right).left = tmp;
            current.right = tmp;
            if (current == last) last = current.right;
        }
        else
        {
            first = tmp;
            last = first;
            current = first;
            first.right = first;
            first.left = first;
        }
        current = tmp;
        rate++;
    }
    public void toFirst()
    {
        iterator = first;
    }
    public void toLast()
    {
        iterator = last;
    }
    public void next()
    {
        iterator = iterator.right;
    }
    public void prev()
    {
        iterator = iterator.left;
    }
    public void nextCur()
    {
        current = current.right;
        Notify();
    }
    public void prevCur()
    {
        current = current.left;
        Notify();
    }
    public void del() //удаление и сдвиг после этого
    {
        if (rate == 1)
        {
            first = null;
            last = null;
            current = null;
        }
        else
        {
            (current.left).right = current.right;
            (current.right).left = current.left;
            list tmp = current;
            if (current == last)
            {
                current = current.left;
                last = current;
            }
            else
            {
                if (current == first) first = current.right;
                current = current.right;
            }
        }
        rate--;
        Notify();
    }
    public void delIterator()
    {
        if (rate == 1)
        {
            first = null;
            last = null;
            iterator = null;
        }
        else
        {
            (iterator.left).right = iterator.right;
            (iterator.right).left = iterator.left;
            if (iterator == last)
            {
                iterator = iterator.left;
                last = iterator;
            }
            else
            {
                if (iterator == first) first = iterator.right;
                iterator = iterator.left;
            }
        }
        rate--;
        Notify();
    }
    public int size()
    {
        return rate;
    }
    public list getIteratorPTR()
    {
        return iterator;
    }
    public list getCurPTR()
    {
        return current;
    }
    public void setCurPTR()
    {
        current = iterator;
    }
    public bool isChecked()
    {
        if (iterator.isChecked == true) return true; else return false;
    }
    public void check()
    {
        iterator.isChecked = !iterator.isChecked;
    }
    public MStorage getIterator()
    {
        return (iterator.data);
    }
    public MStorage get()
    {
        return (current.data);
    }
}

