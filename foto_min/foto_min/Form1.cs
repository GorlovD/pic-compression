using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Collections;

namespace foto_min
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        
        }

        int nom = 0;
        int kol = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            
          //  put = Environment.CurrentDirectory + @"\Фото";
            textBox1.Text = Environment.CurrentDirectory + @"\foto"; 
            comboBox1.SelectedIndex = 2;
            
          //  loadpicture();
        }


        private void SaveImage(Image image, string filename)
        {
            long zhatie=100L;

            if (comboBox1.Text=="2X")
                 {zhatie=50L;} else
               if (comboBox1.Text=="1X")
                   {zhatie=100L;} else
                     if (comboBox1.Text=="1.5X")
                       {zhatie=70L;} else
                        if (comboBox1.Text=="2.5X")
                          { zhatie=40L;} else
                            if (comboBox1.Text=="5X")
                              {zhatie=20L;} else
                                if (comboBox1.Text == "10X")
                                { zhatie = 10L; };
                                  if (comboBox1.Text == "XXL")
                                     { zhatie = 1000L; };
            var jgpEncoder = GetEncoder(ImageFormat.Jpeg);
            var myEncoder = System.Drawing.Imaging.Encoder.Quality;
            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, zhatie);
            myEncoderParameters.Param[0] = myEncoderParameter;
            image.Save(filename, jgpEncoder, myEncoderParameters);
        }
        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();
            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        static String BytesToString(long byteCount)
        {
            string[] suf = { "Byt", "KB", "MB", "GB", "TB", "PB", "**" }; //
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }



        private void loadpicture()
        {
          //  string[] files = Directory.GetFiles(textBox1.Text, "*.jpg", SearchOption.AllDirectories);
            pictureBox1.Image = null;
            string[] filters = { "*.jpg", "*.jpeg", "*.bmp", "*.png", "*.ico" };
            ArrayList fileEntries = new ArrayList();


            if (checkBox1.Checked == true)
            {
                foreach (string filter in filters)
                {
                    fileEntries.AddRange(Directory.GetFiles(textBox1.Text, filter, SearchOption.AllDirectories));
                }

            }
            else
            {
                // fileEntries = Directory.GetFiles(textBox1.Text); // получаешь список файлов в каталоге targetDirectory
                foreach (string filter in filters)
                {
                    fileEntries.AddRange(Directory.GetFiles(textBox1.Text, filter));
                }
            }
            
            //foreach (string filter in filters)
            //    {
            //        fileEntries.AddRange(Directory.GetFiles(textBox1.Text, filter, SearchOption.AllDirectories));
            //    }
            // foreach (string fn in fileEntries)
            //this.listBox1.Items.AddRange(files);
            //  MessageBox.Show(fileEntries[nom].ToString());

             kol = fileEntries.Count;
             label7.Text = "(найдено: " + kol + ")";
            if(kol==1)
            {
                button1.Enabled = false;
            };

            if (kol == 0)
            {
                MessageBox.Show("В указанной папке нет файлов с изображением");
                button1.Enabled = false;
                //break;
                return;
            }
            else button1.Enabled = true;

            Image image1 = new Bitmap(fileEntries[nom].ToString());
            int x = image1.Width * trackBar1.Value / 100;
            int y = image1.Height * trackBar1.Value / 100;
            // MessageBox.Show(image1.Width + "X" + image1.Height + "\r" + x + "x" + y);
            Image image2 = new Bitmap(image1, x, y);
            
            label3.Text = "Разрешение " + image1.Width + "X" + image1.Height;

            FileInfo file = new FileInfo(fileEntries[nom].ToString());

           label4.Text="Размер "+BytesToString(file.Length);
          groupBox2.Text ="Свойства файла "+ Path.GetFileName(fileEntries[nom].ToString())+":"; //имя файла
          SaveImage(image2, Environment.CurrentDirectory+@"\test.JPG");

          label6.Text = "Разрешение " + x + "X" + y;
          file = new FileInfo(Environment.CurrentDirectory + @"\test.JPG");

          label5.Text = "Размер " + BytesToString(file.Length);


          string fn = Environment.CurrentDirectory + @"\test.JPG";
          Image img = null;

          using (FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          {
              img = Image.FromStream(fs);
          }

          pictureBox1.Image = img;//.Load(Environment.CurrentDirectory+@"\test.JPG");
          File.Delete(fn);

        }

        private void trackBar1_Scroll_1(object sender, EventArgs e)
        {
            label1.Text = "Размер " + trackBar1.Value.ToString() + "% от оригинала";
            loadpicture();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
         //   string[] fileEntries = null;
            string[] filters = { "*.jpg", "*.jpeg", "*.bmp", "*.png", "*.ico" };
            ArrayList fileEntries = new ArrayList();

            if (checkBox1.Checked == true)
            {
               foreach(string filter in filters)
                {
                    fileEntries.AddRange(Directory.GetFiles(textBox1.Text, filter, SearchOption.AllDirectories));
                }

            }
            else
            {
               // fileEntries = Directory.GetFiles(textBox1.Text); // получаешь список файлов в каталоге targetDirectory
                foreach(string filter in filters)
                {
                  fileEntries.AddRange(Directory.GetFiles(textBox1.Text, filter));
                }
            }
           // MessageBox.Show(fileEntries.Length.ToString()); //количество файлов в папке;

            progressBar1.Visible = true;
            progressBar1.Maximum = fileEntries.Count;
      //      MessageBox.Show(fileEntries.Count.ToString()); //количество файлов
            progressBar1.Value = 0;


            

            foreach (string fn in fileEntries)
            {
                System.IO.FileInfo file = new System.IO.FileInfo(fn);
                long size = file.Length;
                
                if (checkBox4.Checked==true)
                {
                    if (size < Convert.ToDouble(textBox3.Text) * 1048576)         
                    {
                       continue;         //     Пропуск записей с помощью foreach
                    }
                  
                }

           //     MessageBox.Show(size.ToString());

                Image img = null;

                using (FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    img = Image.FromStream(fs);
                }

                int x = img.Width * trackBar1.Value / 100;
                int y = img.Height * trackBar1.Value / 100;
                // MessageBox.Show(image1.Width + "X" + image1.Height + "\r" + x + "x" + y);
                Image image1 = new Bitmap(img, x, y);

                string newfn = "";

                if (checkBox3.Checked == true)
                {
                    newfn = fn;
                }
                else
                {
                    newfn = textBox2.Text + @"\" + System.IO.Path.GetFileNameWithoutExtension(fn) + ".JPG";
                    // MessageBox.Show(newfn);
                }


                if (checkBox2.Checked == true)
                {
                    File.Delete(fn);
                    SaveImage(image1, newfn); // textBox1.Text+"\\"+fileName.Substring(fileName.Length-8, 4)+"1.JPG");
                }
                else
                {
                    //MessageBox.Show(newfn.Substring(0, newfn.Length - 4) + "_.JPG");
                    SaveImage(image1, newfn.Substring(0, newfn.Length - 4) + "_.JPG");

                }
                progressBar1.Value = progressBar1.Value + 1;
                Application.DoEvents();

                //if (checkBox2.Checked == true)
                //{
                //    File.Delete(fn);
                //    MessageBox.Show("Файл удалён " + fn);
                //}

            }
            progressBar1.Value = progressBar1.Maximum;
            progressBar1.Visible = false;
            MessageBox.Show("Готово! Обработано файлов: " + progressBar1.Maximum);

        }

        private void button3_Click(object sender, EventArgs e)
        {
           // string path = null;
            using (var dialog = new FolderBrowserDialog())
                if (dialog.ShowDialog() == DialogResult.OK)
                    textBox1.Text = dialog.SelectedPath;
            loadpicture();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadpicture();
        }


        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked==true)
            {
                groupBox5.Visible = false;
            }
            else
            {
                groupBox5.Visible = true;
                using (var dialog = new FolderBrowserDialog())
                    if (dialog.ShowDialog() == DialogResult.OK)
                        textBox2.Text = dialog.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button4.Enabled = true;
            if (nom!=kol) nom = ++nom;
            loadpicture();
            if (nom == kol-1) button1.Enabled = false;
          //  MessageBox.Show(nom+"   "+kol);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            if(nom!=0) nom = --nom;
            loadpicture(); 
            if(nom==0) button4.Enabled = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            loadpicture();
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (pictureBox1.SizeMode == PictureBoxSizeMode.AutoSize)
            {
                pictureBox1.Location = new Point(3, 3);
                pictureBox1.Width = panel1.Width-10;
                pictureBox1.Height = panel1.Height-10;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                hint.Show("Здесь показывается изображение после конвертации (масштабированный)", pictureBox1);
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                hint.Show("Здесь показывается изображение после конвертации (реальный размер)", pictureBox1);
            }
        }


        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            pictureBox1.Width = panel1.Width - 10;
            pictureBox1.Height = panel1.Height - 10;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44) //цифры, клавиша BackSpace и запятая а ASCII (44-запятая, 46-точка)
            {
                e.Handled = true;
            }
        }

      
    }
}
