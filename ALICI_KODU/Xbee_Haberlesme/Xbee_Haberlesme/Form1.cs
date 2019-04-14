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

namespace Xbee_Haberlesme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        string[] portlar = SerialPort.GetPortNames();
        bool first = false;
        Bitmap bmp;
        Color renk;
        int genislik; int yukseklik;
        int yukseklikSayac = 0; int genislikSayac = 0;

        //____________________FORM YÜKLENDİĞİNDE___________________
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
        //______________________PORT AÇ______________________________
        private void button1_Click(object sender, EventArgs e)
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
                    button1.BackColor = Color.Green;
                    button2.BackColor = Color.WhiteSmoke;
                }
                catch (Exception hata)
                {
                    MessageBox.Show("Hata:" + hata.Message);
                }
            }
        }

 
        //_______________________PORT KAPAT____________________
        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
                button2.BackColor = Color.Red;
                button1.BackColor = Color.WhiteSmoke;
            }
        }


       
        //____________________VERİLERİ AL___________________________
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            string[] gelen = serialPort1.ReadExisting().Split(',');

            if (first == false)
            {
                genislik = Convert.ToInt32(gelen[0]); yukseklik = Convert.ToInt32(gelen[1]);
                bmp = new Bitmap(genislik,yukseklik);
               // pictureBox2.Size = new Size(genislik, yukseklik);
                first = true;
            }

            else
            {                
                bmp.SetPixel(genislikSayac, yukseklikSayac, Color.FromArgb(Convert.ToInt32(gelen[0]), Convert.ToInt32(gelen[1]),
                Convert.ToInt32(gelen[2])));
                pictureBox2.Image = bmp;
                genislikSayac++;

                if(genislikSayac>= genislik)
                {
                    genislikSayac = 0;
                    yukseklikSayac++;
                }
            }
            serialPort1.DiscardInBuffer();             
        }
    }
}
