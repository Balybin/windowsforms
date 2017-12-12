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

namespace zmeya
{
    public partial class Form1 : Form
    {
        int KeysBufMaxSize, SpeedOutput;
        List<int> KeysBuffer;
        Graphics draw;
        SolidBrush whiteBrush;
        SolidBrush orangeBrush;
        SolidBrush redBrush;
        Pen redPen;
        Pen blackPen;
        bool isFileLoad = false;
        bool reDrow = false;
        public Form1()
        {
            InitializeComponent();
            whiteBrush = new SolidBrush(Color.White);
            orangeBrush = new SolidBrush(Color.Orange);
            redBrush = new SolidBrush(Color.Red);
            redPen = new Pen(Color.Red);
            blackPen = new Pen(Color.Black);
            KeysBufMaxSize = 2;
            KeysBuffer = new List<int>();
            draw = pictureBox1.CreateGraphics();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (KeysBuffer.Count >= KeysBufMaxSize)
            {
                return;
            }
            int direction = -1;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    direction = 0;
                    break;
                case Keys.Right:
                    direction = 1;
                    break;
                case Keys.Down:
                    direction = 2;
                    break;
            }
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;
            result = MessageBox.Show("＜￣｀ヽ、　　　　　　　／￣＞\n　ゝ、　　＼　／⌒ヽ, ノ /´\n　　　ゝ、　` //＼° ͜ʖ ͡°/ ／\n　　 　　>　 　 　  , ノ\n　　　　　∠_,,,/´", "улетаешь без сохранения?", buttons);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = (TextBox)e.Control;
            tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);


        }


        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            string vlCell = ((TextBox)sender).Text;
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.')
                && (vlCell.IndexOf(".") == -1) && (vlCell.Length != 0)) 
                &&!((e.KeyChar == '-') && (vlCell.Length == 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(stream, System.Text.Encoding.Unicode);
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    if (dataGridView1[1,i].Value != null && dataGridView1[2,i].Value != null)
                        writer.WriteLine(dataGridView1[1,i].Value.ToString() + " " + dataGridView1[2,i].Value.ToString());
                }
                writer.Close();
                stream.Close();
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (reDrow)
            {
                e.Graphics.FillRectangle(whiteBrush, 0, 0, pictureBox1.Width, pictureBox1.Height);
                for (int i = 0; i < dataGridView1.Rows.Count - 2; ++i)
                {
                    e.Graphics.DrawLine(blackPen,
                        float.Parse(dataGridView1[1, i].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(dataGridView1[2, i].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(dataGridView1[1, i + 1].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture),
                        float.Parse(dataGridView1[2, i + 1].Value.ToString(), System.Globalization.CultureInfo.InvariantCulture));
                }
            }
        }

        private void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                if (dataGridView1[1, i] != null)
                    if(dataGridView1[1, i].Value.ToString().Length == 0)
                {
                    dataGridView1.Rows.RemoveAt(i);
                    for (int j = i; j < dataGridView1.Rows.Count; ++j)
                        dataGridView1[0, j].Value = (int.Parse(dataGridView1[0, j].Value.ToString()) - 1).ToString();

                    --i;
                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileLoad)
            {
                dataGridView1.Rows.Clear();
            }
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() != DialogResult.OK) return;
            StreamReader reader = new StreamReader(openFileDialog1.FileName);
            string buf = reader.ReadLine();
            string[] result;
            int point = 1;
            //try
            //{
            while (buf != null)
            {
                buf = point.ToString() + " " + buf;
                result = buf.Split(' ');
                dataGridView1.Rows.Add(result);
                buf = reader.ReadLine();
                ++point;
            }
            isFileLoad = true;
            reDrow = true;
                //}
                // catch()
             pictureBox1.Refresh();
        }
    }
   
}
