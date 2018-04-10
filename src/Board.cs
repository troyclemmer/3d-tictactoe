using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeniorProject
{
    public class Board
    { 
        private const int PLANES = 4;
        private const int ROWS = 4;
        private const int COLUMNS = 4;
        private Player[, ,] board;

        public Board()
        {
            board = new Player[PLANES, ROWS, COLUMNS];
        }

        public int getPlanes()
        {
            return PLANES;
        }

        public int getRows()
        {
            return ROWS;
        }

        public int getColumns()
        {
            return COLUMNS;
        }


        public Board addMove(Player player, int plane, int row, int column)
        {
            board[plane-1, row-1, column-1] = player;
            return this;
        }

        public Board removeMove(int plane, int row, int column)
        {
            board[plane - 1, row - 1, column - 1] = null;
            return this;
        }

        public Player getPlayerInPosition(int p, int r, int c)
        {
            return board[p - 1, r - 1, c - 1];
        }

        public Player[, ,] getBoard()
        {
            return board;
        }


        //returns null if no winner, else returns winning player
        //splits the board into different 2D planes and checks if there is a win in each plane
        public Player checkWin(List<Player> playerList)
        {
            Player winner = null;

            Player[,] board2DX = new Player[PLANES, COLUMNS];
            Player[,] board2DY = new Player[PLANES, ROWS];
            Player[,] board2DZ = new Player[ROWS, COLUMNS];

            //for every plane
            for (int k = 0; k < PLANES; k++)
            {              
                //for every row
                for (int i = 0; i < ROWS; i++)
                {
                    //for every column
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        board2DX[i, j] = board[k, i, j];
                        board2DY[i, j] = board[j, k, i];
                        board2DZ[i, j] = board[i, j, k];
                    }
                }

                winner = checkWin2D(playerList, board2DX,'x',k);
                if (winner != null)
                {
                    return winner;
                }
                winner = checkWin2D(playerList, board2DY,'y',k);
                if (winner != null)
                {
                    return winner;
                }
                winner = checkWin2D(playerList, board2DZ,'z',k);
                if (winner != null)
                {
                    return winner;
                }      
            }

            //check the 4 main diagonals

            //for both players
            foreach (Player player in playerList)
            {
                if ((board[0, 0, 0] == player) && (board[1, 1, 1] == player) && (board[2, 2, 2] == player) && (board[3, 3, 3] == player))
                {
                    player.setWin(new int[4, 3] { { 0, 0, 0 }, { 1, 1, 1 }, { 2, 2, 2 }, { 3, 3, 3 } });
                    winner = player;
                }
                else if ((board[0, 3, 0] == player) && (board[1, 2, 1] == player) && (board[2, 1, 2] == player) && (board[3, 0, 3] == player))
                {
                    player.setWin(new int[4, 3] { { 0, 3, 0 }, { 1, 2, 1 }, { 2, 1, 2 }, { 3, 0, 3 } });
                    winner = player;
                }
                else if ((board[3, 3, 0] == player) && (board[2, 2, 1] == player) && (board[1, 1, 2] == player) && (board[0, 0, 3] == player))
                {
                    player.setWin(new int[4, 3] { { 3, 3, 0 }, { 2, 2, 1 }, { 1, 1, 2 }, { 0, 0, 3 } });
                    winner = player;
                }
                else if ((board[3, 0, 0] == player) && (board[2, 1, 1] == player) && (board[1, 2, 2] == player) && (board[0, 3, 3] == player))
                {
                    player.setWin(new int[4, 3] { { 3, 0, 0 }, { 2, 1, 1 }, { 1, 2, 2 }, { 0, 3, 3 } });
                    winner = player;
                }

            }



            return winner;

        }

        //returns null if no winner, else returns winning player
        //this is an internal method, used by checkWin
        private Player checkWin2D(List<Player> playerList, Player[,] board2D, char dimension, int dimValue)
        {
            //count is counting how many in a row so far for each piece
            int count = 0;          

            //for both players
            foreach (Player player in playerList)
            {
                if (dimension == 'z')
                {
                    //checking horizontal n in a rows
                    for (int i = 0; i < ROWS; i++)
                    {
                        count = 0;

                        for (int j = 0; j < COLUMNS; j++)
                        {
                            if (board2D[i, j] == player)
                            {
                                count++;
                                if (count == 4)
                                {
                                    if (dimension == 'x')
                                    {
                                        player.setWin(new int[4, 3] { { dimValue, i, j }, { dimValue, i, j - 1 }, { dimValue, i, j - 2 }, { dimValue, i, j - 3 } });
                                    }
                                    else if (dimension == 'y')
                                    {
                                        player.setWin(new int[4, 3] { { j - 1, dimValue, i }, { j - 2, dimValue, i }, { j - 3, dimValue, i }, { j, dimValue, i } });
                                    }
                                    else
                                    {
                                        player.setWin(new int[4, 3] { { i, j, dimValue }, { i, j - 1, dimValue }, { i, j - 2, dimValue }, { i, j - 3, dimValue } });
                                    }
                                    return player;
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                    }
                }

                if (dimension == 'z' || dimension == 'y')
                {
                    //checking vertical 4 in a rows
                    for (int j = 0; j < COLUMNS; j++)
                    {
                        count = 0;

                        for (int i = 0; i < ROWS; i++)
                        {
                            if (board2D[i, j] == player)
                            {
                                count++;
                                if (count == 4)
                                {
                                    if (dimension == 'x')
                                    {
                                        player.setWin(new int[4, 3] { { dimValue, i, j }, { dimValue, i - 1, j }, { dimValue, i - 2, j }, { dimValue, i - 3, j } });
                                    }
                                    else if (dimension == 'y')
                                    {
                                        player.setWin(new int[4, 3] { { j, dimValue, i }, { j, dimValue, i - 1 }, { j, dimValue, i - 2 }, { j, dimValue, i - 3 } });
                                    }
                                    else
                                    {
                                        player.setWin(new int[4, 3] { { i, j, dimValue }, { i - 1, j, dimValue }, { i - 2, j, dimValue }, { i - 3, j, dimValue } });
                                    }
                                    return player;
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                    }
                }

                
                //checking diagonal 4 in a rows
                int winLength = 4;
                //checking for positive slope winner
                for (int row = 0; row < ROWS - winLength + 1; row++)
                {
                    for (int column = 0; column < COLUMNS - winLength + 1; column++)
                    {
                        if (board2D[row, column] != null &&
                            board2D[row + 1, column + 1] == board2D[row, column] &&
                            board2D[row + 2, column + 2] == board2D[row, column] &&
                            board2D[row + 3, column + 3] == board2D[row, column])
                        {
                            if (dimension == 'x')
                            {
                                player.setWin(new int[4, 3] { { dimValue, row, column }, { dimValue, row + 1, column + 1 }, { dimValue, row + 2, column + 2 }, { dimValue, row + 3, column + 3 } });
                            }
                            else if (dimension == 'y')
                            {
                                player.setWin(new int[4, 3] { { column, dimValue, row }, { column + 1, dimValue, row + 1 }, { column + 2, dimValue, row + 2 }, { column + 3, dimValue, row + 3 } });
                            }
                            else
                            {
                                player.setWin(new int[4, 3] { { row, column, dimValue }, { row + 1, column + 1, dimValue }, { row + 2, column + 2, dimValue }, { row + 3, column + 3, dimValue } });
                            }
                            return board2D[row, column];
                        }
                    }
                }
                //checking for negative slope winner
                for (int row = winLength - 1; row < ROWS; row++)
                {
                    for (int column = 0; column < COLUMNS - winLength + 1; column++)
                    {
                        if (board2D[row, column] != null &&
                            board2D[row - 1, column + 1] == board2D[row, column] &&
                            board2D[row - 2, column + 2] == board2D[row, column] &&
                            board2D[row - 3, column + 3] == board2D[row, column])
                        {
                            if (dimension == 'x')
                            {
                                player.setWin(new int[4, 3] { { dimValue, row, column }, { dimValue, row - 1, column + 1 }, { dimValue, row - 2, column + 2 }, { dimValue, row - 3, column + 3 } });
                            }
                            else if (dimension == 'y')
                            {
                                player.setWin(new int[4, 3] { { column, dimValue, row }, { column + 1, dimValue, row - 1 }, { column + 2, dimValue, row - 2 }, { column + 3, dimValue, row - 3 } });
                            }
                            else
                            {
                                player.setWin(new int[4, 3] { { row, column, dimValue }, { row - 1, column + 1, dimValue }, { row - 2, column + 2, dimValue }, { row - 3, column + 3, dimValue } });
                            }
                            return board2D[row, column];
                        }
                    }
                }

            }

            return null;
        }


        //return true if board is full
        public bool isFull()
        {
            //for every row
            for (int i = 0; i < ROWS; i++)
            {

                //for every column
                for (int j = 0; j < COLUMNS; j++)
                {
                    //for every plane
                    for (int k = 0; k < PLANES; k++)
                    {
                        if (board[k,i,j] == null)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
