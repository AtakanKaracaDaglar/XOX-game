using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
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

        private int siraKimde; // 0 / 1
        private bool secimSirasiMi = false;

        private int baslayaniSec() // 0 Yada 1 döndürüyor | 0 Bilgisayar 1 Oyuncudur
        {
            int baslayanSecimi;

            Random atakan = new Random();
            baslayanSecimi = atakan.Next(0, 2);

            if (baslayanSecimi == 0)
            {
                lbl_Gamer.Text = "Bilgisayar Seçildi";
                // bilgisayar rastgele karakter seçecek
                int bilgisayarKarakter = atakan.Next(0, 2);

                int oyuncuKarakter = (bilgisayarKarakter == 0) ? 1 : 0;

                secilenKarakter = oyuncuKarakter; // Oyuncuya kalan karakteri atıyoruz
                siraKimde = 0; // ilk hamle bilgisayarın olacak
                if(oyuncuKarakter == 1)
                {
                    x_pct.Visible = true;
                    o_pct.Visible = false;
                }
                else if (oyuncuKarakter == 0)
                {
                    o_pct.Visible = true;
                    x_pct.Visible = false;
                }

            }
            else if (baslayanSecimi == 1)
            {
                lbl_Gamer.Text = "Oyuncu Seçildi";
                if(oynanmaSayisi == 0 && secilenKarakter == 2)
                {
                    MessageBox.Show("Lütfen Taş Türünü Seçiniz");
                }
                
                secimSirasiMi = true;
            }

            siraKimde = baslayanSecimi;

            return baslayanSecimi;
        }



        int[,] atakan = new int[3, 3];

        private void tahtayiTemizle()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int a = 0; a < 3; a++)
                {
                    atakan[a, i] = 5;
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






        // OYUNUN BEL KEMİGİ \\


        private int oynanmaSayisi = 0; // Lütfen oynanma sayisini sürekli sıfırlamayı unutmayınız

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




        // yapay zeka yönetimi
        private void AImove()
        {
             //O = 0;
             //X = 1;

            int AIsymbol = (secilenKarakter == 1) ? 0 : 1;

            // kazanmayı dene
            if (TryWinningMove(AIsymbol))
            {
                Thread.Sleep(1500); // oyuncu nefes alsın az.
                sonrakiHamleGec();
                return;
            }
            // rakibin kazanmasını engelleme
            if (TryWinningMove(secilenKarakter, true))
            {
                Thread.Sleep(1500);
                sonrakiHamleGec();
                return;
            }
            // yoksa rastgele oyna
            Thread.Sleep(1500);
            RandomHareket(AIsymbol);
            sonrakiHamleGec();


        }


        private bool TryWinningMove(int symbol, bool engellemeModu = false)
        {
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (atakan[r, c] == 5) // boş hücre
                    {
                        atakan[r, c] = symbol;

                        if (CheckWinTemp(symbol))
                        {
                            atakan[r, c] = 5; // geri al
                            if (!engellemeModu)
                            {
                                // AI'nin kazanma hamlesi: yerleştir
                                atakan[r, c] = symbol;
                                SembolYerlestirme(r, c, symbol);
                            }
                            else
                            {
                                // Engellemek için, AI'nin taşını koy
                                int aiSymbol = (secilenKarakter == 1) ? 0 : 1;
                                atakan[r, c] = aiSymbol;
                                SembolYerlestirme(r, c, aiSymbol);
                            }
                            return true;
                        }

                        atakan[r, c] = 5; // geri al
                    }
                }
            }
            return false;
        }





        private bool CheckWinTemp(int symbol)
        {
            // Geçici tarama bayrakları
            bool tempYatay = false, tempDikey = false, tempCapraz = false;

            // Yatay
            for (int row = 0; row < 3; row++)
            {
                if (atakan[row, 0] == symbol && atakan[row, 1] == symbol && atakan[row, 2] == symbol)
                {
                    tempYatay = true;
                    break;
                }
            }

            // Dikey
            for (int col = 0; col < 3; col++)
            {
                if (atakan[0, col] == symbol && atakan[1, col] == symbol && atakan[2, col] == symbol)
                {
                    tempDikey = true;
                    break;
                }
            }

            // Çapraz
            if ((atakan[0, 0] == symbol && atakan[1, 1] == symbol && atakan[2, 2] == symbol) ||
                (atakan[0, 2] == symbol && atakan[1, 1] == symbol && atakan[2, 0] == symbol))
            {
                tempCapraz = true;
            }

            return tempYatay || tempDikey || tempCapraz;
        }
        
        
        private void RandomHareket(int symbol)
        {

            List<(int r, int c)> boslist = new List<(int r, int c)>();

            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    if (atakan[r, c] == 5)
                    {
                        boslist.Add((r, c));
                        
                    }
                }
            }

            

            if (boslist.Count > 0)
            {
                Random rnd = new Random();

                var (rr, cc) = boslist[rnd.Next(boslist.Count)];

                atakan[rr, cc] = symbol;

                SembolYerlestirme(rr, cc, symbol);

            }

        }



        // Bilgisayarın sembolünü yerleştirme yeri
        private void SembolYerlestirme(int row, int col, int symbol)
        {

            PictureBox picture = (symbol == 0) ? o_ : x_;

            Panel hedefPanel = GetPanelByRowCol(row, col);

            clooner(picture, new Point(hedefPanel.Location.X, hedefPanel.Location.Y));

        }


        private Panel GetPanelByRowCol(int r, int c)
        {
            if (r == 0 && c == 0) return panel1;
            if (r == 0 && c == 1) return panel2;
            if (r == 0 && c == 2) return panel3;
            if (r == 1 && c == 0) return panel6;
            if (r == 1 && c == 1) return panel5;
            if (r == 1 && c == 2) return panel4;
            if (r == 2 && c == 0) return panel7;
            if (r == 2 && c == 1) return panel8;
            if (r == 2 && c == 2) return panel9;
            return null; // güvenlik amaçlı
        }


        private void sonrakiHamleGec()
        {
            siraKimde = 1;
            oynanmaSayisi = 0;

        }

        private bool Berabere()
        {
            int doluHücreSayisi = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (atakan[i, j] != 5) // Dolu hücre
                    {
                        doluHücreSayisi++;
                    }
                }
            }
            return doluHücreSayisi == 9; // 9 hücre dolduysa berabere
        }
        int score = 0;

        private bool KazananKontrolEt()
        {
            yatayTespit = false;
            dikeyTespit = false;
            caprazTespit = false;

            // TARAMA
            dikeyTarama();
            yatayTarama();
            caprazTarama();

            if (yatayTespit || dikeyTespit || caprazTespit)
            {
                timer1.Stop();
                int kazanan = yatayTespit ? yatayTespitSonucu : dikeyTespit ? dikeyTespitSonucu : caprazTespitSonucu;
                string kazananSatir;
                if (kazanan == secilenKarakter)
                {
                    kazananSatir = "Oyuncu Kazandı!";
                    score++;
                    lbl_Score.Text = score.ToString();
                }
                else
                {
                    kazananSatir = "Bilgisayar Kazandı!";
                    if(score > 0)
                    {
                        score--;
                    }
                    else
                    {
                        score = 0;
                    }
                    lbl_Score.Text = score.ToString();
                }

                MessageBox.Show(kazananSatir);

                DialogResult dr = MessageBox.Show("Oyunu yeniden başlatmak ister misiniz?", "Yeniden Başlat", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    oyunuSifirla();
                }
                else
                {
                    this.Close();
                }
                return true; // Oyun bitti
            }
            if (Berabere())
            {
                timer1.Stop(); // Timer'ı durdur
                DialogResult dr1 = MessageBox.Show("Oyun berabere bitti tekrar oynamak ister misiniz?", "Yeniden başlat", MessageBoxButtons.YesNo);
                if (dr1 == DialogResult.Yes)
                {
                    oyunuSifirla();
                }
                else
                {
                    this.Close();
                }
                return true;
            }


            return false; // Oyun devam ediyor
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            if (secimSirasiMi == true)
                return;

            if (siraKimde == 0 && oynanmaSayisi == 0)
            {
                AImove();
            }
            else if (siraKimde == 1 && oynanmaSayisi == 0)
            {
                oynanmaSayisi = 1;
            }

            KazananKontrolEt();
        }


        private void start_btn_Click(object sender, EventArgs e)
        {

            if (secimSirasiMi == false)
            {
                start_btn.Enabled = false;
            }
            
            baslayaniSec();
            timer1.Start();

        }


        private void oyunuSifirla()
        {
            tahtayiTemizle();

            // Panel üzerindeki PictureBox'ları temizle
            List<Control> silinecekler = new List<Control>();
            List<Point> noktalarListesi = new List<Point>();
            foreach (Control ctrl in this.Controls)
            {
               
                if (ctrl is Panel)
                {
                    noktalarListesi.Add(ctrl.Location);
                }

            }

            foreach (Control ctrl in this.Controls)
            {
                if(ctrl is PictureBox)
                {
                    foreach (Point lokasyon in noktalarListesi)
                    {
                        if (ctrl.Location == lokasyon)
                        {
                            silinecekler.Add(ctrl);
                        }
                    }
                }
                
            }
            
            foreach (var item in silinecekler)
            {
                this.Controls.Remove(item);
                item.Dispose();
            }
           

            x_pct.Visible = true;
            o_pct.Visible = true;
            
            baslayaniSec();
            oynanmaSayisi = 0;


            timer1.Start();
        }




        private void x_pct_Click(object sender, EventArgs e)
        {
            if (secimSirasiMi == true)
            {
                secilenKarakter = 1; // X harfi
                x_pct.Visible = true;
                o_pct.Visible = false;
                secimSirasiMi = false;
            }
            
            
        }

        private void o_pct_Click(object sender, EventArgs e)
        {
            if (secimSirasiMi == true)
            {
                secilenKarakter = 0; // O harfi
                o_pct.Visible = true;
                x_pct.Visible = false;
                secimSirasiMi = false;
            }
            
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            if (siraKimde == 1)
            {

                if (atakan[0, 0] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


                if (secilenKarakter == 1)
                {

                    clooner(x_, new Point(panel1.Location.X, panel1.Location.Y));
                    atakan[0, 0] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel1.Location.X, panel1.Location.Y));
                    atakan[0, 0] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
                KazananKontrolEt();
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


                if (atakan[0, 1] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }

                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel2.Location.X, panel2.Location.Y));
                    atakan[0, 1] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel2.Location.X, panel2.Location.Y));
                    atakan[0, 1] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
                KazananKontrolEt();
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

                if (atakan[0, 2] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


                if (secilenKarakter == 1)
                {

                    clooner(x_, new Point(panel3.Location.X, panel3.Location.Y));
                    atakan[0, 2] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel3.Location.X, panel3.Location.Y));
                    atakan[0, 2] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
                KazananKontrolEt();
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


                if (atakan[1, 0] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }

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
                KazananKontrolEt();
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

                if (atakan[1, 1] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


                if (secilenKarakter == 1)
                {
                    clooner(x_, new Point(panel5.Location.X, panel5.Location.Y));
                    atakan[1, 1] = 1;
                }
                else if (secilenKarakter == 0)
                {
                    clooner(o_, new Point(panel5.Location.X, panel5.Location.Y));
                    atakan[1, 1] = 0;
                }



                siraKimde = 0;
                oynanmaSayisi = 0;
                timer1.Start();
                KazananKontrolEt();
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

                if (atakan[1, 2] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


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
                KazananKontrolEt();
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

                    if (atakan[2, 0] != 5)
                    {
                        MessageBox.Show("Bu hücre zaten dolu!");
                        return;
                    }


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
                KazananKontrolEt();
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


                if (atakan[2, 1] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


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
                KazananKontrolEt();
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

                if (atakan[2, 2] != 5)
                {
                    MessageBox.Show("Bu hücre zaten dolu!");
                    return;
                }


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
                KazananKontrolEt();
            }
            else
            {
                MessageBox.Show("Sıra bilgisayarda");
            }
        }

       
    }
}



























































































// 1000 satırlık kod olsun istedim. \\