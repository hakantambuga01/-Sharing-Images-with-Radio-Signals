using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace Pixel_detection
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Bitmap resim;
        string[] portlar = SerialPort.GetPortNames(); 
        //_________________________________FORM1 ACILDIGINDA________________________
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string port in portlar)
            {
                comboBox1.Items.Add(port);
            }
            comboBox2.Items.Add("4800");
            comboBox2.Items.Add("9600");
            comboBox2.Items.Add("115200");
            comboBox2.Items.Add("250000");
        }

        //__________________________PORT AC_____________________________________
        private void button3_Click(object sender, EventArgs e)
        {      
            if (serialPort1.IsOpen == false)
            {
                if (comboBox1.Text == "")
                    return;
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                try
                {
                    serialPort1.Open();
                    button3.BackColor = Color.Green;
                    button4.BackColor = Color.WhiteSmoke;                                     
                }
                catch (Exception hata)
                { MessageBox.Show("Hata:" + hata.Message); }
            }

        }
        //_________________________PORT KAPAT______________________________________
        private void button4_Click(object sender, EventArgs e)
        {         
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                button4.BackColor = Color.Red;
                button3.BackColor = Color.WhiteSmoke;
            }
        }

        //____________________________RESMİ AL__________________________________

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog resimOku = new OpenFileDialog();
            resimOku.ShowDialog();
            string dosyaYolu = resimOku.FileName;
            resim = new Bitmap(dosyaYolu);
            resim = new Bitmap(resim, new Size(64, 64));

            pictureBox1.Image = resim;
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        //__________________________RESMİ PAYLAŞ______________________________________
        private void button2_Click(object sender, EventArgs e)
        {
            int yukseklik = resim.Height; int genislik = resim.Width;

            serialPort1.Write(genislik.ToString() + "," + yukseklik.ToString());
            Thread.Sleep(100);

            for(int i = 0; i < yukseklik; i++)
            {

                for(int j = 0; j < genislik; j++)
                {
                    Color renk = resim.GetPixel(j,i);
                    serialPort1.Write(renk.R.ToString() + "," + renk.G.ToString() + "," + renk.B.ToString());
                    Thread.Sleep(45);
                    serialPort1.DiscardOutBuffer();
                }
            }        
        }
    }  
}
