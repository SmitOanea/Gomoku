using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Five_in_Line
{

    public partial class Form1 : Form
    {
        int turn = 0, h;
        int nr_patratele_ocupate = 0;
        bool game_ended = false;
        Button[][] buttonArray = new Button[20][];
        Button buff_red = new Button(), buff_blue = new Button();

        public Form1()
        {
            InitializeComponent();
        }

        private void GenereazaTabla()
        {
            for (int i = 0; i <= h - 1; i++)
            {
                buttonArray[i] = new Button[h];
                for (int j=0;j<h;++j)
                {
                    buttonArray[i][j] = new Button();
                    buttonArray[i][j].Size = new Size(25, 25 );
                    buttonArray[i][j].Name = "" + i*h+j + "";
                    buttonArray[i][j].Click += button_Click;//function
                    buttonArray[i][j].Location = new Point(20*j, 20 + (i * 20));
                    panel1.Controls.Add(buttonArray[i][j]);
                }
            }
            panel1.Location = new Point(
            this.ClientSize.Width / 2 - panel1.Size.Width / 2,
            this.ClientSize.Height *3/10 - panel1.Size.Height / 2);
            panel1.Anchor = AnchorStyles.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buff_blue.BackgroundImage = Properties.Resources.albastru;
            buff_blue.BackgroundImageLayout = ImageLayout.Stretch;
            buff_red.BackgroundImage = Properties.Resources.rosu;
        }

        bool Victorie()
        {
            ///Verific daca a castigat cineva
            int i, j, secv;
            secv = 1;
            //orizontala
            for (i = 0; i < h; ++i)
            {
                for (j = 0; j < h; ++j)
                {
                    if (j > 0 && buttonArray[i][j].BackgroundImage != null && buttonArray[i][j].BackgroundImage == buttonArray[i][j - 1].BackgroundImage)
                        secv++;
                    else
                        secv = 1;
                    if (secv == 5)
                    {
                        for (int jj = j; jj >= j - 5 + 1; --jj)
                        {
                            buttonArray[i][jj].BackColor = Color.Green;
                        }
                        return true;
                    }
                }
            }
            //verticala
            for(j = 0; j < h; ++j)
            {
                for (i = 0; i < h; ++i)
                {
                    if (i > 0 && buttonArray[i][j].BackgroundImage != null && buttonArray[i][j].BackgroundImage == buttonArray[i-1][j].BackgroundImage)
                        secv++;
                    else
                        secv = 1;
                    if (secv == 5)
                    {
                        for (int ii = i; ii >= i - 5 + 1; --ii)
                        {
                            buttonArray[ii][j].BackColor = Color.Green;
                        }
                        return true;
                    }
                }
            }


            int x, y;
            //diagonale paralele cu diagonala principala
            for(i=0;i<h;i++)
                for(j=0;j<h;++j)
                {
                    x = i;
                    y = j;
                    secv = 1;
                    while(x<h && y<h)
                    {
                        if (x > 0 && y > 0 && buttonArray[x][y].BackgroundImage != null && buttonArray[x][y].BackgroundImage == buttonArray[x - 1][y - 1].BackgroundImage)
                            secv++;
                        else
                            secv = 1;
                        if(secv == 5)
                        {
                            for(int val = 0; val < 5; val++)
                                buttonArray[x - val][y - val].BackColor = Color.Green;
                            return true;
                        }
                        x++;
                        y++;

                    }
                }

            //diagonale paralele cu diagonala secundara
            for (i = 0; i < h; i++)
                for (j = 0; j < h; ++j)
                {
                    x = i;
                    y = j;
                    secv = 1;
                    while (x >= 0 && y < h)
                    {
                        if (x < h-1 && y > 0 && buttonArray[x][y].BackgroundImage != null && buttonArray[x][y].BackgroundImage == buttonArray[x + 1][y - 1].BackgroundImage)
                            secv++;
                        else
                            secv = 1;
                        if (secv == 5)
                        {
                            for (int val = 0; val < 5; val++)
                                buttonArray[x + val][y - val].BackColor = Color.Green;
                            return true;
                        }
                        x--;
                        y++;

                    }
                }


            return false;


        }

        private void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            
            if (b.BackgroundImage != null)
                return;//sa nu pot desena peste un patratel deja folosit
            if (game_ended)
                return;
            
            if (turn==0)//rosu
            {
                b.BackgroundImage = buff_red.BackgroundImage;
                b.BackgroundImageLayout = ImageLayout.Stretch;           
            }
            else
                if(turn==1)//albastru
                {
                    b.BackgroundImage = buff_blue.BackgroundImage;
                    b.BackgroundImageLayout = ImageLayout.Stretch;
                }

            nr_patratele_ocupate++;

            //MessageBox.Show("nr_patratele_ocupate = " + nr_patratele_ocupate + "\n" + "h*h = " + h * h); 

            if (Victorie())
            {
                game_ended = true;
                MessageBox.Show("Castigatorul este: " + turn);
            }

            if (nr_patratele_ocupate == h * h)//s-a facut ultima mutare posibila
            {                                 //nu mai sunt patratele libere
                if(!game_ended)
                    MessageBox.Show("Remiza!");
            }

            if (turn == 0)
            {
                turn = 1;
                pictureBox1.BackgroundImage = Properties.Resources.albastru;
               
            }
            else
            {
                if (turn == 1)
                {
                    turn = 0;
                    pictureBox1.BackgroundImage = Properties.Resources.rosu;
                }
                else
                    MessageBox.Show("Eroare in button_Click");
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            h = (int)numericUpDown1.Value;
            label1.Visible = true;
            pictureBox1.Visible = true;
            label2.Visible = false;
            label3.Visible = false;
            numericUpDown1.Visible = false;
            buttonStart.Visible = false;
            GenereazaTabla();
        }
    }
}
