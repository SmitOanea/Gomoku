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

    public partial class FormStatus : Form
    {
        short turn = 0, h;
        short my_color, his_color;
        bool computer_is_thinking = false;
        const short DEPTH_MAX = 3;
        const short INF = 32000;
        Button buff_blue = new Button(), buff_red = new Button();
        Button[][] buttonArray;

        public FormStatus()
        {
            InitializeComponent();
        }

        void GenereazaTabla()
        {
            buttonArray = new Button[h + 2][];
            for (int i = 0; i <= h + 1; i++)
            {
                buttonArray[i] = new Button[h + 2];

                for (int j = 0; j < h + 2; ++j)
                {
                    buttonArray[i][j] = new Button();
                    buttonArray[i][j].Size = new Size(25, 25);
                    buttonArray[i][j].Name = "" + (i * h + j) + "";
                    buttonArray[i][j].Click += button_Click;//function
                    buttonArray[i][j].Location = new Point(20 * j, 20 + (i * 20));
                    buttonArray[i][j].Tag = "nimic";

                    if (j == 0 || j == h + 1 || i == 0 || i == h + 1)
                        buttonArray[i][j].Visible = false;
                    panel1.Controls.Add(buttonArray[i][j]);
                }
            }
            panel1.Location = new Point(
            this.ClientSize.Width / 2 - panel1.Size.Width / 2,
            this.ClientSize.Height * 3 / 10 - panel1.Size.Height / 2);
            panel1.Anchor = AnchorStyles.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buff_blue.BackgroundImage = Properties.Resources.albastru;
            buff_blue.BackgroundImageLayout = ImageLayout.Stretch;
            buff_red.BackgroundImage = Properties.Resources.rosu;
        }

        short Victory_or_Draw(ref Position pos, bool current)
        {
            ///Verific daca a castigat cineva
            int i, j, secv;
            secv = 1;
            //orizontala
            for (i = 1; i <= h; ++i)
            {
                secv = 1;
                for (j = 1; j <= h; ++j)
                {
                    if (j > 1 && pos.gameTable[i][j]!=-1 && pos.gameTable[i][j] == pos.gameTable[i][j - 1])
                        secv++;
                    else
                        secv = 1;
                    if (secv == 5)
                    {
                        if(current)
                            for (int jj = j; jj >= j - 5 + 1; --jj)
                            {
                                buttonArray[i][jj].BackColor = Color.Green;
                            }
                        return 1;
                    }
                }
            }

            //verticala
            for (j = 1; j <= h; ++j)
            {
                for (i = 1; i <= h; ++i)
                {
                    if (i > 1 && pos.gameTable[i][j] != -1 && pos.gameTable[i][j] == pos.gameTable[i - 1][j])
                        secv++;
                    else
                        secv = 1;
                    if (secv == 5)
                    {
                        if(current)
                            for (int ii = i; ii >= i - 5 + 1; --ii)
                            {
                                buttonArray[ii][j].BackColor = Color.Green;
                            }
                        return 1;
                    }
                }
            }


            int x, y;
            //diagonale paralele cu diagonala principala
            for (i = 1; i <= h; i++)
                for (j = 1; j <= h; ++j)
                {
                    x = i;
                    y = j;
                    secv = 1;
                    while (x <= h && y <= h)
                    {
                        if (x > 1 && y > 1 && pos.gameTable[x][y] != -1 && pos.gameTable[x][y] == pos.gameTable[x - 1][y - 1])
                            secv++;
                        else
                            secv = 1;
                        if (secv == 5)
                        {
                            if(current)
                                for (int val = 0; val < 5; val++)
                                    buttonArray[x - val][y - val].BackColor = Color.Green;
                            return 1;
                        }
                        x++;
                        y++;

                    }
                }

            //diagonale paralele cu diagonala secundara
            for (i = 1; i <= h; i++)
                for (j = 1; j <= h; ++j)
                {
                    x = i;
                    y = j;
                    secv = 1;
                    while (x >= 1 && y <= h)
                    {
                        if (x <= h - 1 && y > 1 && pos.gameTable[x][y] != -1 && pos.gameTable[x][y] == pos.gameTable[x + 1][y - 1])
                            secv++;
                        else
                            secv = 1;
                        if (secv == 5)
                        {
                            if(current)
                                for (int val = 0; val < 5; val++)
                                    buttonArray[x + val][y - val].BackColor = Color.Green;
                            return 1;
                        }
                        x--;
                        y++;

                    }
                }
            if (pos.nr_patratele_ocupate == h * h)//s-a facut ultima mutare posibila
            {                                 //nu mai sunt patratele libere
                return 2;
            }

            return 0;


        }

        private void radioButtonHumanComputer_CheckedChanged(object sender, EventArgs e)
        {
            panelColor.Visible = true;
        }

        private void radioButtonHumanHuman_CheckedChanged(object sender, EventArgs e)
        {
            panelColor.Visible = false;
        }

        private void Move_turn(ref Button b)
        {

            if (b.BackgroundImage != null)
                return;//sa nu pot desena peste un patratel deja folosit

            if (turn == 0)//rosu
            {
                b.BackgroundImage = buff_red.BackgroundImage;
                b.Tag = "rosiatic";
                b.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
                if (turn == 1)//albastru
            {
                b.BackgroundImage = buff_blue.BackgroundImage;
                b.Tag = "albastriu";
                b.BackgroundImageLayout = ImageLayout.Stretch;
            }

            //pos.nr_patratele_ocupate++;

            


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


            Position pst = new Position(h, ref buttonArray);
            short v_or_d = Victory_or_Draw(ref pst, true);
            if (v_or_d>0)
            {
                labelStatus.Text = "Game over";
                labelStatus.Visible = false;
                labelStatus.Visible = true;
                if (v_or_d==2)
                {
                    MessageBox.Show("Draw!");
                }
                else
                {
                    MessageBox.Show("The winner is: " + turn);
                }
            }
            //else
                //MessageBox.Show("Nu e nicio victorie.\n nr_patratele_ocupate= " + nr_patratele_ocupate);
        }

        short evaluation_of_position(Position position, bool MaximizingPlayer)
        {
            short my_almost_done_lines = 0, his_almost_done_lines = 0;
            short[] my_nr_ap = new short[6], op_nr_ap = new short[6];
            my_nr_ap[1] = op_nr_ap[1] = 0;
            my_nr_ap[2] = op_nr_ap[2] = 0;
            my_nr_ap[3] = op_nr_ap[3] = 0;
            my_nr_ap[4] = op_nr_ap[4] = 0;
            my_nr_ap[5] = op_nr_ap[5] = 0;
            //rows
            for (short i = 1; i <= h; ++i)
            {
                short my_len = 0, op_len = 0;
                for (short j = 1; j <= h+1; ++j)
                {
                    //substring of my pieces
                    if (position.gameTable[i][j] == my_color)
                        my_len++;
                    else
                    {
                        if (my_len == 5)//I win
                        {
                            //MessageBox.Show("Aia de mai jos nu e eroare, am gasit solutie de castig.");
                            return +INF;
                        }
                        if (my_len > 5 || my_len < 0)
                            MessageBox.Show("error in heuristic function(my_len). my_len==" + my_len);
                        my_nr_ap[my_len]++;
                        if (my_len == 4)
                        {
                            if (j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] == -1 && position.gameTable[i][j] != -1)
                                my_almost_done_lines++;//open ending only to left
                            if (j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] != -1 && position.gameTable[i][j] == -1)
                                my_almost_done_lines++;//open ending only to right
                            if (!MaximizingPlayer)//my turn
                            {
                                if (my_almost_done_lines >= 1)
                                {
                                    MessageBox.Show("here?\n" + "i=" + i + ", j=" + j);
                                    return +INF;//4 pieces with at least one open ending
                                }
                            }
                            else//his turn
                            {
                                if (my_almost_done_lines >= 2)
                                    return +INF;//I win
                                if ((j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] == -1) && (position.gameTable[i][j] == -1))
                                {
                                    MessageBox.Show("I should never reach this point. If I do, it's an error.");
                                    return +INF;//4 pieces with two open ending
                                }
                            }
                        }

                        my_len = 0;
                    }

                    //substring of opponent's pieces
                    if (position.gameTable[i][j]==his_color)
                        op_len++;
                    else
                    {
                        if (op_len == 5)//he wins
                        {
                            //MessageBox.Show("Aia de mai jos nu e eroare, oponentul a gasit solutie de castig.");
                            return -INF;
                        }
                        if (op_len > 5 || op_len < 0)
                            MessageBox.Show("error in heuristic function(op_len). op_len==" + op_len);
                        op_nr_ap[op_len]++;
                        if (op_len == 4)
                        {
                            if (j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] == -1 && position.gameTable[i][j] != -1)
                                his_almost_done_lines++;//open ending only to left
                            if (j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] != -1 && position.gameTable[i][j] == -1)
                                his_almost_done_lines++;//open ending only to right
                            if (MaximizingPlayer)//his turn
                            {
                                if (his_almost_done_lines >= 1)
                                    return -INF;//4 pieces with at least one open ending
                            }
                            else//my turn
                            {
                                if (his_almost_done_lines >= 2)
                                    return -INF;//he wins
                                if ((j - 4 - 1 >= 1 && position.gameTable[i][j - 4 - 1] == -1) && (position.gameTable[i][j] == -1))
                                {
                                    MessageBox.Show("I should never reach this point. If I do, it's an error.");
                                    return -INF;//4 pieces with two open ending
                                }
                            }
                        }

                        op_len = 0;
                    }
                }
                
            }
            if (my_nr_ap[4] != op_nr_ap[4])
                return (short)(my_nr_ap[4] - op_nr_ap[4]);

            if (my_nr_ap[3] != op_nr_ap[3])
                return (short)(my_nr_ap[3] - op_nr_ap[3]);

            if (my_nr_ap[2] != op_nr_ap[2])
                return (short)(my_nr_ap[2] - op_nr_ap[2]);

            return (short)(my_nr_ap[1] - op_nr_ap[1]);
        }

        void AfisareGameTable(Position pstn)
        {

            String tabla = new String('a', 2);
            tabla = "";
            for (short i = 0; i <= h + 1; ++i)
            {
                for (short j = 0; j <= h + 1; j++)
                {
                    tabla = tabla + pstn.gameTable[i][j] + "  ";
                }
                tabla += "\n";
            }
            short evl;
            evl = evaluation_of_position(pstn, false);

            MessageBox.Show("Afisare game_table:\n\n\n" + tabla + "\n\n\n The evaluation is + " + evl);
        }

        AlphaBetaStruct Alpha_Beta(Position pos, short depth, short alpha, short beta, bool MaximizingPlayer)
        {
            if(depth==-1)
            {
                MessageBox.Show("depth a ajuns sa fie -1");
            }
            AlphaBetaStruct res_pair = new AlphaBetaStruct();
            AlphaBetaStruct child_pair = new AlphaBetaStruct();
            res_pair.Evaluation = 0;
            res_pair.NextMoveToDo.x = 0;
            res_pair.NextMoveToDo.y = 0;


            short v_o_d = Victory_or_Draw(ref pos, false);
            if (v_o_d>0)
            {
                res_pair.NextMoveToDo.x = -1;
                res_pair.NextMoveToDo.y = -1;//I don't think about the next move now
                if (!(v_o_d==2))//someone won
                {
                    if (MaximizingPlayer)//that's me :)
                        res_pair.Evaluation = INF;
                    else
                        res_pair.Evaluation = -INF;//that's the opponent :'(
                }
                else
                    res_pair.Evaluation = 0;
                return res_pair;
            }
            if (depth == 0)
            {
                res_pair.NextMoveToDo.x = -1;
                res_pair.NextMoveToDo.y = -1;//I don't think about the next move now
                res_pair.Evaluation = evaluation_of_position(pos, !MaximizingPlayer);

                return res_pair;
            }

            if (MaximizingPlayer)
            {
                res_pair.Evaluation = -INF-1;
                res_pair.NextMoveToDo.x = -2;
                res_pair.NextMoveToDo.y = -2;
                //I will consider each child of the current position
                Position child_pos = new Position((short)h, ref pos);
                bool stop = false, little_flag = false, big_flag=false ;
                short i=1, j=1;
                for (i = 1; i <= h && !stop; ++i)
                    for (j = 1; j <= h && !stop; ++j)
                    {
                        big_flag = true;
                        if (pos.gameTable[i][j] == -1)//empty cell
                        {
                            little_flag = true;
                            //MessageBox.Show("(" + i + "," + j + ") is an empty cell");
                            ///child_pos.gameTable[i][j] = my_color;
                            //MessageBox.Show("Acum patratelele sunt " + child_pos.nr_patratele_ocupate);
                            child_pos.Atribuie(h,i, j, my_color);
                            //MessageBox.Show("Acum, dupa atribuire, patratelele sunt " + child_pos.nr_patratele_ocupate);
                            if (child_pos.nr_patratele_ocupate > h*h)
                                MessageBox.Show("nrpatrateleocupate = " + child_pos.nr_patratele_ocupate + "i=" + i + "j=" + j);

                            //call the function:
                            child_pair = Alpha_Beta(child_pos, (short)(depth - 1), alpha, beta, false);


                            if (child_pair.Evaluation > res_pair.Evaluation)
                            {
                                res_pair.Evaluation = child_pair.Evaluation;
                                res_pair.NextMoveToDo.x = i;
                                res_pair.NextMoveToDo.y = j;
                            }
                            /*else
                                if(child_pair.Evaluation == res_pair.Evaluation)
                                {
                                    Random rnd = new Random();
                                    short nmb = (short)rnd.Next(0, 8);
                                    if(nmb==1)
                                    {
                                        res_pair.Evaluation = child_pair.Evaluation;
                                        res_pair.NextMoveToDo.x = i;
                                        res_pair.NextMoveToDo.y = j;
                                    }
                                }
                            */
                            if (child_pair.Evaluation > alpha)
                                alpha = child_pair.Evaluation;
                            if (beta <= alpha)
                                stop = true;

                            //reset the position for the next child
                            child_pos.Atribuie(h,i,j,-1);
                        }
                    }

                if (res_pair.NextMoveToDo.x < 0 || res_pair.NextMoveToDo.y < 0)
                {
                    MessageBox.Show("Error in AlphaBeta algorithm, part1!\n x=" + res_pair.NextMoveToDo.x + ", y=" + res_pair.NextMoveToDo.y
                             + "   i=" + i + " ,j=" + j);
                    MessageBox.Show("res_pair.Evaluation=" + res_pair.Evaluation + ", alpha=" + alpha + ", beta=" + beta
                         + " ,child_pair.Evaluation=" + child_pair.Evaluation + " ,little_flag=" + little_flag
                          + " ,big_flag=" + big_flag + " ,depth=" + depth + " ,nr_patratele_ocupate=" + pos.nr_patratele_ocupate );
                    AfisareGameTable(pos);


                }
                return res_pair;
            }
            //else --> minimizing player

            res_pair.Evaluation = +INF+1;
            res_pair.NextMoveToDo.x = -1;
            res_pair.NextMoveToDo.y = -1;
            //I will consider each child of the current position
            Position child = new Position((short)(h), ref pos);
            bool stopp = false;
            for (short i = 1; i <= h && !stopp; i++)
                for (short j = 1; j <= h && !stopp; ++j)
                    if (pos.gameTable[i][j] == -1)//empty cell
                    {
                        if (my_color == 1)
                            child.Atribuie(h,i,j,2);
                        else
                            child.Atribuie(h,i,j,1);

                        //call the function:
                        child_pair = Alpha_Beta(child, (short)(depth - 1), alpha, beta, true);

                        if (child_pair.Evaluation <= res_pair.Evaluation)
                        {
                            res_pair.Evaluation = child_pair.Evaluation;
                            res_pair.NextMoveToDo.x = i;
                            res_pair.NextMoveToDo.y = j;
                        }
                        /*else
                            if (child_pair.Evaluation == res_pair.Evaluation)
                            {
                                Random rnd = new Random();
                                short nmb = (short)rnd.Next(0, 8);
                                if (nmb == 1)
                                {
                                    res_pair.Evaluation = child_pair.Evaluation;
                                    res_pair.NextMoveToDo.x = i;
                                    res_pair.NextMoveToDo.y = j;
                                }
                            }
                        */
                        if (child_pair.Evaluation <= beta)
                            {
                            res_pair.Evaluation = child_pair.Evaluation;
                            res_pair.NextMoveToDo.x = i;
                            res_pair.NextMoveToDo.y = j;
                            beta = child_pair.Evaluation;
                        }
                        if (beta <= alpha)
                            stopp = true;

                        //reset the position for the next child
                        child.Atribuie(h,i,j,-1);
                    }
            if (res_pair.NextMoveToDo.x < 0 || res_pair.NextMoveToDo.y < 0)
                MessageBox.Show("Error in AlphaBeta algorithm, part2!\n x=" + res_pair.NextMoveToDo.x + ", y=" + res_pair.NextMoveToDo.y);
            return res_pair;
        }

        public void button_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            
            if (!computer_is_thinking && b.BackgroundImage == null)
            {
                Move_turn(ref b);
                if (radioButtonHumanComputer.Checked == true)
                {
                    computer_is_thinking = true;
                    labelStatus.Text = "Computer is thinking...";
                    //MessageBox.Show("calculatorul reflecta asupra situatiei...");
                    Position pos = new Position((short)(h), ref buttonArray);
                    AlphaBetaStruct strct = new AlphaBetaStruct();
                    strct = Alpha_Beta(pos, DEPTH_MAX, -INF, +INF, true);
                    MessageBox.Show("Calculatorul va muta la pozitia (" + strct.NextMoveToDo.x + "," + strct.NextMoveToDo.y + "), pentru ca acoro are valoarea " + strct.Evaluation);





                    if (strct.NextMoveToDo.x < 0 || strct.NextMoveToDo.y < 0)
                        return;
                    Move_turn(ref buttonArray[strct.NextMoveToDo.x][strct.NextMoveToDo.y]);
                    labelStatus.Text = "Yourn turn";
                    computer_is_thinking = false;

                    Position pstn = new Position(h, ref buttonArray);

                    String tabla = new String('a', 2);
                    tabla = "";
                    for (short i = 0; i <= h + 1; ++i)
                    {
                        for (short j = 0; j <= h + 1; j++)
                        {
                            tabla = tabla + pstn.gameTable[i][j] + "  ";
                        }
                        tabla += "\n";
                    }
                    short evl;
                    evl = evaluation_of_position(pstn, false);

                    MessageBox.Show("Tabla este:\n\n\n" + tabla + "\n\n\n The evaluation is + " + evl);

                }
            }
            
        }
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (radioButtonHumanHuman.Checked == true)
            {
                h = (short)numericUpDown1.Value;
                label1.Visible = true;
                pictureBox1.Visible = true;
                label2.Visible = false;
                label3.Visible = false;
                numericUpDown1.Visible = false;
                buttonStart.Visible = false;
                panelGameType.Visible = false;
                GenereazaTabla();
                labelStatus.Visible = true;
            }
            else
                if (radioButtonHumanComputer.Checked == true)
                {
                    h = (short)numericUpDown1.Value;
                    label1.Visible = true;
                    pictureBox1.Visible = true;
                    label2.Visible = false;
                    label3.Visible = false;
                    numericUpDown1.Visible = false;
                    buttonStart.Visible = false;
                    panelGameType.Visible = false;
                    panelColor.Visible = false;
                if (radioButtonRed.Checked)
                {
                    my_color = 2;
                    his_color = 1;
                }
                else
                {
                    my_color = 1;
                    his_color = 2;
                }
                GenereazaTabla();
                    Position pstn = new Position(h, ref buttonArray);

                    String tabla = new String('a',2);
                    tabla = "";
                    for(short i=0;i<=h+1;++i)
                    {
                        for(short j=0; j<=h+1;j++)
                        {
                            tabla = tabla + pstn.gameTable[i][j] + "  ";
                        }
                        tabla += "\n";
                    }
                    MessageBox.Show("Tabla este:\n\n\n" + tabla );
                    labelStatus.Visible = true;
                }
        }
    }

}
