using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeniorProject
{
    public partial class LogWindow : Form
    {
        Game game;
        ListBox lb;

        public LogWindow(Game game_, ListBox lb_)
        {
            InitializeComponent();
            game = game_;
            lb = lb_;
            setVisiblity(false);
            write("LogWindow created.");
        }

        public void write(string s)
        {
            logBox.Items.Add(s);
            lb.Items.Add(s);

           //autoscroll down to show more recently added items all the time
           logBox.TopIndex = logBox.Items.Count - 1;
           lb.TopIndex = lb.Items.Count - 1;
           clearSelected();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            game.logWindowToolStripMenuItem_ChangeChecked();
            setVisiblity(false);
            clearSelected();
        }

        public void clearSelected()
        {
            logBox.ClearSelected();
            lb.ClearSelected();
        }

        public Game returnGame()
        {
            return game;
        }

        public void setVisiblity(bool visible)
        {
            clearSelected();
            Visible = visible;
        }

        public void printBoard(Board b)
        {
            Player[, ,] board = b.getBoard();

            Player player = null;

            for (int p = 0; p < 4; p++)
            {
                for (int r = 0; r < 4; r++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        try
                        {
                            player = b.getPlayerInPosition(p + 1, r + 1, c + 1);
                        }
                        catch (Exception) 
                        { 
                            return; 
                        }

                        if (player != null)
                        {
                            write((p + 1).ToString() + "p, " + (r + 1).ToString() + "r, " + (c + 1).ToString() + "c is claimed by " + player.getName() + ".");
                        }
                    }
                }
            }
        }

        public void clearLogBox()
        {
            logBox.Items.Clear();
            lb.Items.Clear();
        }


        public void logBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string s = logBox.SelectedItem.ToString();
                Clipboard.SetText(s);
                //write("Copied LogWindow line " + (logBox.SelectedIndex + 1) + " to the clipboard.");
                clearSelected();
            }
            catch (Exception) { }
        }


    }
}
