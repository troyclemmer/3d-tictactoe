using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CPI.Plot3D;
using System.Collections.Generic;

namespace SeniorProject
{
    public partial class Game : Form
    {

        //board is an array that goes: plane, row, column
        private Board board;

        //display contains all the draw events
        private Display d;

        //prints out everything that happens in this current game
        private LogWindow lw;

        //the list of players playing in the current game
        private List<Player> playerList;

        //winner of the game, null if there is none yet
        private Player winner;

        //total number of turns
        private int numberOfTurns;

        //computer thinking timer
        Timer timer1;

        //display board timer
        Timer timer2;

        //display winning line timer
        Timer timer3;


        public Game(Display display)
        {
            InitializeComponent();
            lw = new LogWindow(this, listBox_log);
            lw.Show();
            lw.setVisiblity(false);
            d = display;
            playerList = new List<Player>();
            winner = null;
            timerInitialization();
            numberOfTurns = 0;
        }

        private void timerInitialization()
        {
            timer1 = new Timer();
            timer1.Interval = 2500;
            timer1.Tick += new EventHandler(computerThinking);

            timer2 = new Timer();
            timer2.Interval = 100;
            timer2.Tick += new EventHandler(updateDisplayOnTimer);

            timer3 = new Timer();
            timer3.Interval = 100;
            timer3.Tick += new EventHandler(displayWinningLine);
        }

        private void setupHumanPlayer()
        {
            ColorSelectMenu csm = new ColorSelectMenu(this, d.getColorList());
            csm.ShowDialog(this);
        }

        public Player setupComputerPlayer(int difficulty)
        {
            Random random = new Random();

            //pick a random  color from the remaining colors for the computer to be
            int randomNumber = random.Next(0, d.getColorList().Count);

            ComputerPlayer cpu = new ComputerPlayer(d.getColorList()[randomNumber],difficulty);

            return cpu;
        }

        public void addPlayer(Player player)
        {
            //need to remove this player's color choice from available colors
            d.removeColor(player.getColor());

            playerList.Add(player);

            lw.write(player.getType() + " '" + player.getName() + "' was added to the game.");
            lw.write(player.getName() + " chose the color " + player.getColor() + ".");
        }

        //mode is 1 for one player, mode is 2 for two player
        private void playGame(int mode, int difficulty)
        {
            viewToolStripMenuItem.Enabled = true;
            //writes to log if a one player or two player game was started
            lw.write("Game against " + (mode == 1 ? "the computer" : "a friend") + " started.");
            panel1.Visible = false;
            label7.Visible = true;

            createBoard();
            updateDisplay();
            
            //setup and add the human player to the game
            setupHumanPlayer();
            

            //if its a one player game
            if (mode == 1)
            {
                //setup and add the computer player to the game
                addPlayer(setupComputerPlayer(difficulty));
            }
            else
            {
                //its a 2 player game, so set up another human player
                setupHumanPlayer();
            }

            inputLabelVisibility(true);
            textBox3.Focus();
            //write to the log which player's turn it is
            lw.write(playerList[0].getName() + "'s turn.");
            updateTurnLabel(playerList[0]);
            updateDisplay();
            viewToolStripMenuItem1.Enabled = true;
            
        }


        private void createBoard()
        {
            board = new Board();
            lw.write("New " + d.getBoardX() + "x" + d.getBoardY() + "x" + d.getBoardZ() + " board created.");
            lw.write("Drawing the game board...");
            lw.write("Finished drawing the game board.");
        }

        private void updateDisplay()
        {
            d.drawBoard(this);
        }

        private void updateDisplayOnTimer(object sender, EventArgs e)
        {
            timer2.Stop();
            d.drawBoard(this);
        }

        private void displayWinningLine(object sender, EventArgs e)
        {
            timer3.Stop();
            d.displayWinningLine(this);
        }

        private void gameOver(bool draw)
        {
            string output;

            if (!draw)
            {
                output = winner.getName() + " wins!";
                label7.ForeColor = Color.FromName(winner.getColor());
                label7.Text = output;
            }
            else
            {
                output = "The board is filled, it is a draw!";
                label7.ForeColor = Color.White;
                label7.Text = "It is a Draw!";
            }

            lw.write("Game lasted "+numberOfTurns+" turns.");
            lw.write(output);
            inputLabelVisibility(false);
            label8.Visible = false;
            label9.Visible = false;
            buttonPlayAgain.Visible = true;
            timer3.Start();
        }


        //play a turn, human is used to see if input should be hidden or not
        private void playTurn()
        {
            int p = 0;
            int r = 0;
            int c = 0;

            if (playerList[0].getType().Equals("HumanPlayer"))
            {
                p = int.Parse(textBox3.Text);
                r = int.Parse(textBox1.Text);
                c = int.Parse(textBox2.Text);
                numberOfTurns++;
            }

            //player submits their move, exit on bad input
            Board nextState = playerList[0].takeTurn(this, p, r, c);
            

            if (nextState != null)
            {
                board = nextState;
            }
            else
            {
                sendError("Sorry " + playerList[0].getName() + ", that spot is taken.");
                clearInputFields();
                return;
            }

            updateDisplay();

            //check if there is a winner,
            Player winningPlayer = board.checkWin(playerList);
            if (winningPlayer != null)
            {
                winner = winningPlayer;
                clearInputFields();
                gameOver(false);
                return;
            }
            //draw check
            else if (board.isFull())
            {
                clearInputFields();
                gameOver(true);
                return;
            }

            clearInputFields();


            Player tmp = playerList[0];
            playerList.Remove(playerList[0]);
            playerList.Add(tmp);

            //write to the log which player's turn it is (next)
            lw.write(playerList[0].getName() + "'s turn.");
            updateTurnLabel(playerList[0]);


            //if next player is a computer player, let's run the next turn
            if (playerList[0].getType().Equals("ComputerPlayer(" + playerList[0].getDifficultyType() + ")"))
            {
                inputLabelVisibility(false);

                timer1.Start();
                //playTurn();
                //inputLabelVisibility(true); 
            }
      
        }

        public void computerThinking(object sender, EventArgs e)
        {
            timer1.Stop();
            playTurn();
            numberOfTurns++;
            inputLabelVisibility(true); 
        } 

        public LogWindow getLogWindow()
        {
            return lw;
        }

        public Board getBoard()
        {
            return board;
        }

        public Display getDisplay()
        {
            return d;
        }

        public Player getWinner()
        {
            return winner;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool baseResult = base.ProcessCmdKey(ref msg, keyData);

            if (keyData == Keys.Tab)
            {
                timer3.Start();
                return true;
            }
            else if (keyData == Keys.Left)
            {
                d.increaseBoardAngle(this);
            }
            else if (keyData == Keys.Right)
            {
                d.decreaseBoardAngle(this);
            }
            else if (keyData == Keys.Down)
            {
                d.resetBoardAngle(this);
            }

            return baseResult;
        }

        //show the error in a popup, and write it to the log window
        public void sendError(string errorMsg)
        {
            lw.write("ERROR: " + "'" + errorMsg + "'");
            MessageBox.Show(errorMsg);
        }
        


        private void button_onePlayer_Click(object sender, EventArgs e)
        {
            difficultyVisibilty();
        }

        private void button_twoPlayer_Click(object sender, EventArgs e)
        {
            playGame(2,-1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void quitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void logWindowToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            bool vis = false;

            if (logWindowToolStripMenuItem.Checked)
            {
                vis = true;
            }

            lw.setVisiblity(vis);
        }

        public void logWindowToolStripMenuItem_ChangeChecked()
        {
            logWindowToolStripMenuItem.Checked = !logWindowToolStripMenuItem.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {       
            textBox3.Focus();

            //input error checking
            if (textBox1.Text.Length == 1 && textBox2.Text.Length == 1 && textBox3.Text.Length == 1)
            {
                playTurn();
            }
            else
            {
                sendError("Please insert a number 1 to 4 in each of the three boxes.");
            }
        }

        private void inputLabelVisibility(bool value)
        {
            label4.Visible = value;
            label5.Visible = value;
            label6.Visible = value;
            label8.Visible = value;
            label9.Visible = value;
            textBox1.Visible = value;
            textBox2.Visible = value;
            textBox3.Visible = value;
            button2.Visible = value;
        }

        private void updateTurnLabel(Player player)
        {
            label7.Text = player.getName() + "'s Turn";
            label7.ForeColor = Color.FromName(player.getColor());
        }

        private void clearInputFields()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
        }

        //plane input box
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            limitInput(textBox3);
            button2.ForeColor = Color.White;

            //code to go to the next open box, if none open, selects the submit button
            if (!limitInput(textBox3))
            {
                textBox3.Focus();
            }
            else if (!limitInput(textBox1))
            {
                textBox1.Focus();
            }
            else if (!limitInput(textBox2))
            {
                textBox2.Focus();
            }
            else
            {
                button2.Focus();
                button2.ForeColor = Color.Red;
            }

            updateDisplay();
        }

        //row input box
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            limitInput(textBox1);
            button2.ForeColor = Color.White;

            //code to go to the next open box, if none open, selects the submit button
            if (!limitInput(textBox1))
            {
                textBox1.Focus();
            }
            else if (!limitInput(textBox2))
            {
                textBox2.Focus();
            }
            else if (!limitInput(textBox3))
            {
                textBox3.Focus();
            }
            else
            {
                button2.Focus();
                button2.ForeColor = Color.Red;
            }

            updateDisplay();
        }

        //column input box
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            limitInput(textBox2);
            button2.ForeColor = Color.White;

            //code to go to the next open box, if none open, selects the submit button
            if (!limitInput(textBox2))
            {
                textBox2.Focus();
            }
            else if (!limitInput(textBox3))
            {
                textBox3.Focus();
            }
            else if (!limitInput(textBox1))
            {
                textBox1.Focus();
            }
            else
            {
                button2.Focus();
                button2.ForeColor = Color.Red;
            }

            updateDisplay();
        }

        //
        public int[] getInput()
        {
            string z = textBox3.Text;
            if (z.Length == 0)
            { z = "-1"; }
            string y = textBox1.Text;
            if (y.Length == 0)
            { y = "-1"; }
            string x = textBox2.Text;
            if (x.Length == 0)
            { x = "-1"; }

            int[] inputArray = new int[3];
            inputArray[0] = int.Parse(z)-1;
            inputArray[1] = int.Parse(y)-1;
            inputArray[2] = int.Parse(x)-1;
            return inputArray;
        }

        //don't let user put incorrect input in the text boxes
        //returns false if it is empty, else it returns true
        private bool limitInput(TextBox textBox)
        {
            String tmp = textBox.Text.Trim();

            foreach (char c in textBox.Text.ToCharArray())
            {
                // don't let user input a space or a non-number
                if (!Char.IsNumber(c))
                {
                    tmp = tmp.Replace(c.ToString(), "");
                }
                else
                {
                    // don't let user input a number greater than 4, or a 0 (this helps eliminate confusion on
                    // whether to insert a 0 or a 1 to pick the 1st, because it isnt possible to insert a 0
                    if (int.Parse(c.ToString()) > 4 || int.Parse(c.ToString()) == 0)
                    {
                        tmp = tmp.Replace(c.ToString(), "");
                    }
                }
            }

            textBox.Text = tmp;
            if (tmp.Length < 1)
            {
                return false;
            }
            return true;
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void easyButton_Click(object sender, EventArgs e)
        {
            playGame(1, 0);
        }

        private void mediumButton_Click(object sender, EventArgs e)
        {
            playGame(1, 1);
        }

        private void hardButton_Click(object sender, EventArgs e)
        {
            playGame(1, 2);
        }

        private void difficultyVisibilty()
        {
            button_onePlayer.Visible = false;
            button_twoPlayer.Visible = false;
            difficultyLabel.Visible = true;
            easyButton.Visible = true;
            mediumButton.Visible = true;
            hardButton.Visible = true;

        }

        private void listBox_log_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string s = listBox_log.SelectedItem.ToString();
                Clipboard.SetText(s);
                //write("Copied LogWindow line " + (logBox.SelectedIndex + 1) + " to the clipboard.");
                lw.clearSelected();
            }
            catch (Exception) { }
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void decreaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d.increaseBoardAngle(this);
        }

        private void shiftAngleRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d.decreaseBoardAngle(this);
        }

        private void resetAngleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            d.resetBoardAngle(this);
        }

        private void printBoardStateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lw.printBoard(board);
        }

        private void buttonPlayAgain_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("3D Tic Tac Toe made by Troy Clemmer in 2012");
        }

        private void printTurnCountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int tn = numberOfTurns + 1;
            lw.write("Game is on turn " + tn+".");
        }

        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("userManual.pdf");
            }
            catch (Exception) {sendError("Help file 'userManual.pdf' not found."); }
        }

    }
}
