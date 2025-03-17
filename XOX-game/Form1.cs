using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XOX_game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tahtayiTemizle();
        }

        private int baslayan;
        private bool oyuncuTiklayabilirMi;
        private int siraKimde; // 0 / 1
        private bool secimSirasiMi = false;
        
        private int baslayaniSec() // 0 Yada 1 döndürüyor | 0 Bilgisayar 1 Oyuncudur
        {
            int baslayanSecimi;
            
            Random atakan = new Random();
            baslayanSecimi = atakan.Next(0, 2);
            
            if(baslayanSecimi == 0)
            {
                MessageBox.Show("Başlayan Bilgisayar Seçildi");
            } else if(baslayanSecimi == 1)
            {
                MessageBox.Show("Başlayan Oyuncu Seçildi");
                MessageBox.Show("Lütfen Taş Türünü Seçiniz");
                secimSirasiMi = true;
            }
            
            siraKimde = baslayanSecimi;

            return baslayanSecimi;
        }



        int[,] atakan = new int[3, 3];
        
        private void tahtayiTemizle()
        {
            for(int i = 0; i < 3; i++)
            {
                for (int a = 0; a < 3; a++)
                {
                    atakan[a,i] = 5;
                }
            }
        }


        //    1 = X   0 = O   \\
        private bool yatayTespit;
        private bool dikeyTespit;
        private bool caprazTespit;

        private int yatayTespitSonucu;
        private int dikeyTespitSonucu;
        private int caprazTespitSonucu;

        private void yatayTarama()
        {
            for (int row = 0; row < 3; row++)
            {
                int a = atakan[row, 0];
                int b = atakan[row, 1];
                int c = atakan[row, 2];

                if (a == b && b == c && a != 5)
                {
                    yatayTespit = true;
                    yatayTespitSonucu = a;
                    return; // Tespit edildi, çık
                }
            }
            yatayTespit = false;
        }


        // Çapraz Kontrol Mekanizmasi

        private void caprazTarama()
        {
            int a = atakan[0, 0];
            int b = atakan[1, 1];
            int c = atakan[2, 2];

            if (a == b && b == c && a != 5)
            {
                caprazTespit = true;
                caprazTespitSonucu = a;
                return;
            }

            a = atakan[0, 2];
            b = atakan[1, 1];
            c = atakan[2, 0];

            if (a == b && b == c && a != 5)
            {
                caprazTespit = true;
                caprazTespitSonucu = a;
            }
            else
            {
                caprazTespit = false;
            }
        }



        private void dikeyTarama()
        {
            for (int col = 0; col < 3; col++)
            {
                int a = atakan[0, col];
                int b = atakan[1, col];
                int c = atakan[2, col];

                if (a == b && b == c && a != 5)
                {
                    dikeyTespit = true;
                    dikeyTespitSonucu = a;
                    return;
                }
            }
            dikeyTespit = false;
        }






        // OYUNUN BEL KEMİGİ <3 \\


        private int oynanmaSayisi = 0; // Lütfen oynanma sayisini sürekli sıfırlamayı unutmayınız <3 XD

        //    1 = X   0 = O   \\

        private int secilenKarakter = 2; // Oyuncunun Oynadığı taş


        private void clooner(PictureBox atakan, Point lokasyon)
        {
            PictureBox kopyalaniyor = new PictureBox();

            kopyalaniyor.Image = atakan.Image;
            kopyalaniyor.Location = lokasyon;
            kopyalaniyor.Size = atakan.Size;
            kopyalaniyor.SizeMode = atakan.SizeMode;
            kopyalaniyor.Visible = true;

            this.Controls.Add(kopyalaniyor);
            kopyalaniyor.BringToFront();
            }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(secimSirasiMi == true)
            {
                return;
            } else
            {
                if (siraKimde == 0 && oynanmaSayisi == 0)
                {
                    // Sıra Bilgisayarda

                    siraKimde = 1; // sıra oyuncuya geçti
                    oynanmaSayisi = 1;
                    MessageBox.Show("Hamle Sırası Oyuncuya Devrediliyor..");

                }
                else if (siraKimde == 1 && oynanmaSayisi == 0)
                {
                    // Sıra Oyuncuda
                    

                    oynanmaSayisi = 1;
                    MessageBox.Show("Sira Sizde");
                    //  siraKimde = 0;
                }




            }


            // SÜREKLİ ÇALIŞACAK OLAN TARAMA İŞLEMLERİ \\

            dikeyTarama();
            yatayTarama();
            caprazTarama();

            if(yatayTespit == true)
            {
                Console.WriteLine("Yatay TESPİT EDİLDİ");
            }
            if(dikeyTespit  == true)
            {
                Console.WriteLine("Dikey TESPİT EDİLDİ");
            }
            if (caprazTespit  == true)
            {
                Console.WriteLine("Capraz TESPİT EDİLDİ");
            }

        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            baslayaniSec();
            timer1.Start();
        }

        private void x_pct_Click(object sender, EventArgs e)
        {
            if(secimSirasiMi == true)
            {
                secilenKarakter = 1; // X harfi
                x_pct.Visible = false;
                o_pct.Visible = false;
                secimSirasiMi = false;
            }
        }

        private void o_pct_Click(object sender, EventArgs e)
        {
            if (secimSirasiMi == true)
            {
                secilenKarakter = 0; // Y harfi
                o_pct.Visible = false;
                x_pct.Visible = false;
                secimSirasiMi = false;
            }
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if(secilenKarakter == 1)
                {

                    clooner(x_, new Point(panel1.Location.X, panel1.Location.Y));
                    atakan[0,0] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel1.Location.X, panel1.Location.Y));
                    atakan[0, 0] = 0;
                }   
                


                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel2.Location.X, panel2.Location.Y));
                    atakan[0,1] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel2.Location.X, panel2.Location.Y));
                    atakan[0, 1] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {

                    clooner(x_, new Point(panel3.Location.X, panel3.Location.Y));
                    atakan[0,2] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel3.Location.X, panel3.Location.Y));
                    atakan[0, 2] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }





        private void panel6_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {

                    clooner(x_, new Point(panel6.Location.X, panel6.Location.Y));
                    atakan[1, 0] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel6.Location.X, panel6.Location.Y));
                    atakan[1, 0] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel5_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel5.Location.X, panel5.Location.Y));
                    atakan[1, 1]=1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel5.Location.X, panel5.Location.Y));
                    atakan[1, 1] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel4_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel4.Location.X, panel4.Location.Y));
                    atakan[1, 2] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel4.Location.X, panel4.Location.Y));
                    atakan[1, 2] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }







        private void panel7_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel7.Location.X, panel7.Location.Y));
                    atakan[2, 0] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel7.Location.X, panel7.Location.Y));
                    atakan[2, 0] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel8_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel8.Location.X, panel8.Location.Y));
                    atakan[2, 1] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel8.Location.X, panel8.Location.Y));
                    atakan[2, 1] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

        private void panel9_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {
                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel9.Location.X, panel9.Location.Y));
                    atakan[2, 2] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel9.Location.X, panel9.Location.Y));
                    atakan[2, 2] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }
    }
}
