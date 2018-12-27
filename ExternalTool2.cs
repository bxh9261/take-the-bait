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
/*
 * Creator: Brad Hanel
 * Name: ExternalTool2
 * External tool which replaces the settings menu and allows us to use text boxes and the mouse a lot easier than monogame
 */
namespace Take_The_Bait
{
    public partial class ExternalTool2 : Form
    {
        string color;
        string name1;
        string name2;
        const char DEL = '\xE001';

        public ExternalTool2()
        {
            InitializeComponent();

            

            StreamReader st = new StreamReader("highscore.txt");
            List<string> playerScores = new List<string>();
            for(int i = 0; i < 5; i++)
            {
                playerScores.Add(st.ReadLine());
            }
            try
            {
                label7.Text = playerScores[0].Split(DEL)[0];
                label8.Text = playerScores[0].Split(DEL)[1];
                label9.Text = playerScores[1].Split(DEL)[0];
                label10.Text = playerScores[1].Split(DEL)[1];
                label11.Text = playerScores[2].Split(DEL)[0];
                label12.Text = playerScores[2].Split(DEL)[1];
                label13.Text = playerScores[3].Split(DEL)[0];
                label14.Text = playerScores[3].Split(DEL)[1];
                label15.Text = playerScores[4].Split(DEL)[0];
                label16.Text = playerScores[4].Split(DEL)[1];
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            

            st.Close();

            StreamReader st2 = new StreamReader("save.txt");
            try
            {
                ExternalToolLoader loader = new ExternalToolLoader();
                string[] loaded = loader.Load();
                if(loaded[0] == "Red")
                {
                    radioButton1.Checked = true;
                }
                if (loaded[0] == "Black")
                {
                    radioButton2.Checked = true;
                }
                if (loaded[0] == "Blue")
                {
                    radioButton3.Checked = true;
                }
                textBox1.Text = loaded[1];
                textBox2.Text = loaded[2];

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            st2.Close();


        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            color = "Red";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            color = "Black";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            color = "Blue";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name1 = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            name2 = textBox2.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Create the streamwriter object
            StreamWriter saveState = new StreamWriter("save.txt");
            if(color == null || name1 == null || name2 == null)
            {
                label5.Visible = true;
                saveState.Close();
            }
            else
            {
                saveState.WriteLine(color + DEL + name1 + DEL + name2);
                saveState.WriteLine();
                saveState.Close();
                label5.Visible = false;
                this.Close();
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
