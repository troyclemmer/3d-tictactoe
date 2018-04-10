using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeniorProject
{
    class AI
    {
        int difficulty;
        ComputerPlayer player;
        Random random;

        public AI(int difficulty_, ComputerPlayer player_)
        {
            difficulty = difficulty_;
            player = player_;
            random = new Random();
        }

        //returns best move in p,r,c format
        public int[] computeMove(Board board)
        {
            int hscore = 0;

            int[] bestMove = new int[3];

            for (int p = 1; p <= 4; p++)
            {
                for (int r = 1; r <= 4; r++)
                {
                    for (int c = 1; c <= 4; c++)
                    {
                        if (board.getPlayerInPosition(p, r, c) == null)
                        {
                            int currentValue = heuristic(board.addMove(player, p, r, c));
                            board.removeMove(p, r, c);
                            //System.Threading.Thread.Sleep(1);

                            if (currentValue > hscore)
                            {
                                //update best move to this one
                                bestMove[0] = p;
                                bestMove[1] = r;
                                bestMove[2] = c;

                                //update heighest heuristic value to this one
                                hscore = currentValue;
                            }

                        }
                    }
                }
            }

            return bestMove;
        }

        //ranks a given board state based on heuristics for each difficulty
        private int heuristic(Board board)
        {
            int value = 1;

            //easy
            if (difficulty == 0)
            {
                value = random.Next(1, 100);
            }
            //medium
            else if (difficulty == 1)
            {
                value +=
                    //offensive piece
                1000 * checkNumberInARow(board, 4) +
                9 * checkNumberInARow(board, 3) +
                2 * checkNumberInARow(board, 2) +

                //defensive piece
                120 * checkNumberInARowBlocks(board, 3) +
                4 * checkNumberInARowBlocks(board, 2) +
                checkNumberInARowBlocks(board, 1);
            }
            //hard
            else if (difficulty == 2)
            {
                value +=
                    //offensive piece
                1000 * checkNumberInARow(board, 4) +
                4 * checkNumberInARow(board, 3) +
                 checkNumberInARow(board, 2) +

                //defensive piece
                120 * checkNumberInARowBlocks(board, 3) +
                9 * checkNumberInARowBlocks(board, 2) +
                2 * checkNumberInARowBlocks(board, 1);
            }

            return value;
        }

        // checks the number of n in a rows for 
        // a specified player on a specified board.  
        //n is an integer such as 3 (to check 3 in a rows)
        private int checkNumberInARow(Board board, int n)
        {
            Player[, ,] board_ = board.getBoard();

            int result = 0;

            Player[,] board2DX = new Player[board.getPlanes(), board.getColumns()];
            Player[,] board2DY = new Player[board.getPlanes(), board.getRows()];
            Player[,] board2DZ = new Player[board.getRows(), board.getColumns()];

            //for every plane
            for (int k = 0; k < board.getPlanes(); k++)
            {
                //for every row
                for (int i = 0; i < board.getRows(); i++)
                {
                    //for every column
                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        board2DX[i, j] = board_[k, i, j];
                        board2DY[i, j] = board_[j, k, i];
                        board2DZ[i, j] = board_[i, j, k];
                    }
                }

                result += checkNumberInARow2D(board, board2DX, n, 'x') + checkNumberInARow2D(board, board2DY, n, 'y') + checkNumberInARow2D(board, board2DZ, n, 'z');
            }

                //4 in a row main diagonals
                if (n == 4 && 
                    (((board_[0, 0, 0] == player) && (board_[1, 1, 1] == player) && (board_[2, 2, 2] == player) && (board_[3, 3, 3] == player)) || 
                    ((board_[0, 3, 0] == player) && (board_[1, 2, 1] == player) && (board_[2, 1, 2] == player) && (board_[3, 0, 3] == player)) || 
                    ((board_[3, 3, 0] == player) && (board_[2, 2, 1] == player) && (board_[1, 1, 2] == player) && (board_[0, 0, 3] == player)) || 
                    ((board_[3, 0, 0] == player) && (board_[2, 1, 1] == player) && (board_[1, 2, 2] == player) && (board_[0, 3, 3] == player))))
                {
                    result++;
                }
                //3 in a row main diagonals
                else if (n == 3 &&
                    (((board_[0, 0, 0] == player) && (board_[1, 1, 1] == player) && (board_[2, 2, 2] == player)) ||
                    ((board_[1, 1, 1] == player) && (board_[2, 2, 2] == player) && (board_[3, 3, 3] == player)) ||
                    ((board_[0, 3, 0] == player) && (board_[1, 2, 1] == player) && (board_[2, 1, 2] == player)) ||
                    ((board_[1, 2, 1] == player) && (board_[2, 1, 2] == player) && (board_[3, 0, 3] == player)) ||
                    ((board_[3, 3, 0] == player) && (board_[2, 2, 1] == player) && (board_[1, 1, 2] == player)) ||
                    ((board_[2, 2, 1] == player) && (board_[1, 1, 2] == player) && (board_[0, 0, 3] == player)) ||
                    ((board_[3, 0, 0] == player) && (board_[2, 1, 1] == player) && (board_[1, 2, 2] == player)) ||
                    ((board_[2, 1, 1] == player) && (board_[1, 2, 2] == player) && (board_[0, 3, 3] == player))))
                {
                    result++;
                }
                //2 in a row main diagonals
                else if (n == 2 &&
                    (((board_[0, 0, 0] == player) && (board_[1, 1, 1] == player)) ||
                    ((board_[1, 1, 1] == player) && (board_[2, 2, 2] == player)) ||
                    ((board_[2, 2, 2] == player) && (board_[3, 3, 3] == player)) ||

                    ((board_[0, 3, 0] == player) && (board_[1, 2, 1] == player)) ||
                    ((board_[1, 2, 1] == player) && (board_[2, 1, 2] == player)) ||
                    ((board_[2, 1, 2] == player) && (board_[3, 0, 3] == player)) ||

                    ((board_[3, 3, 0] == player) && (board_[2, 2, 1] == player)) ||
                    ((board_[2, 2, 1] == player) && (board_[1, 1, 2] == player)) ||
                    ((board_[1, 1, 2] == player) && (board_[0, 0, 3] == player)) ||

                    ((board_[3, 0, 0] == player) && (board_[2, 1, 1] == player)) ||
                    ((board_[2, 1, 1] == player) && (board_[1, 2, 2] == player)) ||
                    ((board_[1, 2, 2] == player) && (board_[0, 3, 3] == player))))
                {
                    result++;
                }

            return result;
        }

        // checks the number of n in a rows for 
        // a specified player on a specified board.  
        //n is an integer such as 3 (to check 3 in a rows)
        private int checkNumberInARow2D(Board board, Player[,] board2D, int n, char dimension)
        {
            //count is counting how many in a row so far for each piece
            int count = 0;
            //result is the total number of n in a rows found so far
            int result = 0;


            if (dimension == 'z')
            {
                //checking horizontal n in a rows
                for (int i = 0; i < board.getRows(); i++)
                {
                    count = 0;

                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        if (board2D[i, j] == player)
                        {
                            count++;
                            if (count == n)
                            {
                                result++;
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
                    //checking vertical n in a rows
                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        count = 0;

                        for (int i = 0; i < board.getRows(); i++)
                        {
                            if (board2D[i, j] == player)
                            {
                                count++;
                                if (count == n)
                                {
                                    result++;
                                }
                            }
                            else
                            {
                                count = 0;
                            }
                        }
                    }
                }

                //checking diagonal n in a rows

                if (n == 4)
                {
                    if (((board2D[0, 0] == player) &&
                         (board2D[1, 1] == player) &&
                         (board2D[2, 2] == player) &&
                         (board2D[3, 3] == player)) ||

                        ((board2D[0, 3] == player) &&
                         (board2D[1, 2] == player) &&
                         (board2D[2, 1] == player) &&
                         (board2D[3, 0] == player)))
                    {
                        result++;
                    }
                }
                else if (n == 3)
                {
                    if (((board2D[0, 0] == player) && (board2D[1, 1] == player) && (board2D[2, 2] == player)) ||
                        ((board2D[1, 1] == player) && (board2D[2, 2] == player) && (board2D[3, 3] == player)) ||
                        ((board2D[0, 3] == player) && (board2D[1, 2] == player) && (board2D[2, 1] == player)) ||
                        ((board2D[1, 2] == player) && (board2D[2, 1] == player) && (board2D[3, 0] == player)))
                    {
                        result++;
                    }
                }
                else if (n == 2)
                {
                    if (
                        ((board2D[0, 0] == player) && (board2D[1, 1] == player)) ||
                        ((board2D[1, 1] == player) && (board2D[2, 2] == player)) ||
                        ((board2D[2, 2] == player) && (board2D[3, 3] == player)) ||

                        ((board2D[0, 3] == player) && (board2D[1, 2] == player)) ||
                        ((board2D[1, 2] == player) && (board2D[2, 1] == player)) ||
                        ((board2D[2, 1] == player) && (board2D[3, 0] == player))
                        )

                    {
                        result++;
                    }
                }       

            return result;
        }


        // checks the number of n in a row blocks
        // for the computer player on a specified board.  
        // n is an integer such as 3 (to check 
        // how many 3 in a rows of the other player are blocked by the computer) 
        public int checkNumberInARowBlocks(Board board, int n)
        {
            Player[, ,] board_ = board.getBoard();

            int result = 0;

            Player[,] board2DX = new Player[board.getPlanes(), board.getColumns()];
            Player[,] board2DY = new Player[board.getPlanes(), board.getRows()];
            Player[,] board2DZ = new Player[board.getRows(), board.getColumns()];

            //for every plane
            for (int k = 0; k < board.getPlanes(); k++)
            {
                //for every row
                for (int i = 0; i < board.getRows(); i++)
                {
                    //for every column
                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        board2DX[i, j] = board_[k, i, j];
                        board2DY[i, j] = board_[j, k, i];
                        board2DZ[i, j] = board_[i, j, k];
                    }
                }

                result += checkNumberInARowBlocks2D(board, board2DX, n, 'x') + checkNumberInARowBlocks2D(board, board2DY, n, 'y') + checkNumberInARowBlocks2D(board, board2DZ, n, 'z');
            }

            //3 in a row main diagonals
            if (n == 3 &&
                (
                ((board_[0, 0, 0] != null && board_[0,0,0] != player) && 
                (board_[1, 1, 1] != null && board_[1,1,1] != player) && 
                (board_[2, 2, 2] != null && board_[2,2,2] != player) && 
                (board_[3, 3, 3] == player)) ||
                ((board_[3, 3, 3] != null && board_[3,3,3] != player) && 
                (board_[1, 1, 1] != null && board_[1,1,1] != player) && 
                (board_[2, 2, 2] != null && board_[2,2,2] != player) && 
                (board_[0, 0, 0] == player)) ||

                ((board_[3, 0, 3] != null && board_[3,0,3] != player) && 
                (board_[1, 2, 1] != null && board_[1,2,1] != player) && 
                (board_[2, 1, 2] != null && board_[2,1,2] != player) && 
                (board_[0, 3, 0] == player)) ||
                ((board_[0, 3, 0] != null && board_[0,3,0] != player) && 
                (board_[1, 2, 1] != null && board_[1,2,1] != player) && 
                (board_[2, 1, 2] != null && board_[2,1,2] != player) && 
                (board_[3, 0, 3] == player)) ||

                ((board_[3, 3, 0] != null && board_[3,3,0] != player) && 
                (board_[2, 2, 1] != null && board_[2,2,1] != player) && 
                (board_[1, 1, 2] != null && board_[1,1,2] != player) && 
                (board_[0, 0, 3] == player)) ||
                ((board_[0, 0, 3] != null && board_[0,0,3] != player) && 
                (board_[2, 2, 1] != null && board_[2,2,1] != player) && 
                (board_[1, 1, 2] != null && board_[1,1,2] != player) && 
                (board_[3, 3, 0] == player)) ||

                ((board_[3, 0, 0] != null && board_[3,0,0] != player) && 
                (board_[2, 1, 1] != null && board_[2,1,1] != player) && 
                (board_[1, 2, 2] != null && board_[1,2,2] != player) && 
                (board_[0, 3, 3] == player)) ||
                ((board_[0, 3, 3] != null && board_[0,3,3] != player) && 
                (board_[2, 1, 1] != null && board_[2,1,1] != player) && 
                (board_[1, 2, 2] != null && board_[1,2,2] != player) && 
                (board_[3, 0, 0] == player))
                ))
            {
                result++;
            }
            //2 in a row main diagonals
            if (n == 2 &&
                (
                ((board_[0, 0, 0] != null && board_[0,0,0] != player) && 
                (board_[1, 1, 1] != null && board_[1,1,1] != player) && 
                (board_[2, 2, 2] == player)) ||
                ((board_[3, 3, 3] != null && board_[3,3,3] != player) && 
                (board_[2, 2, 2] != null && board_[2,2,2] != player) && 
                (board_[1, 1, 1] == player)) ||
                ((board_[2, 2, 2] != null && board_[2,2,2] != player) && 
                (board_[1, 1, 1] != null && board_[1,1,1] != player) && 
                (board_[0, 0, 0] == player || board_[3,3,3] == player)) ||

                ((board_[3, 0, 3] != null && board_[3,0,3] != player) && 
                (board_[1, 2, 1] != null && board_[1,2,1] != player) && 
                (board_[2, 1, 2] == player)) ||
                ((board_[0, 3, 0] != null && board_[0,3,0] != player) && 
                (board_[2, 1, 2] != null && board_[2,1,2] != player) && 
                (board_[1, 2, 1] == player)) ||
                ((board_[2, 1, 2] != null && board_[2,1,2] != player) && 
                (board_[1, 2, 1] != null && board_[1,2,1] != player) && 
                (board_[3, 0, 3] == player || board_[0,3,0] == player)) ||

                ((board_[3, 3, 0] != null && board_[3,3,0] != player) && 
                (board_[2, 2, 1] != null && board_[2,2,1] != player) && 
                (board_[1, 1, 2] == player)) ||
                ((board_[0, 0, 3] != null && board_[0,0,3] != player) && 
                (board_[1, 1, 2] != null && board_[1,1,2] != player) && 
                (board_[2, 2, 1] == player)) ||
                ((board_[1, 1, 2] != null && board_[1,1,2] != player) && 
                (board_[2, 2, 1] != null && board_[2,2,1] != player) && 
                (board_[3, 3, 0] == player || board_[0,0,3] == player)) ||

                ((board_[3, 0, 0] != null && board_[3,0,0] != player) && 
                (board_[2, 1, 1] != null && board_[2,1,1] != player) && 
                (board_[1, 2, 2] == player)) ||
                ((board_[0, 3, 3] != null && board_[0,3,3] != player) && 
                (board_[1, 2, 2] != null && board_[1,2,2] != player) && 
                (board_[2, 1, 1] == player)) ||
                ((board_[1, 2, 2] != null && board_[1,2,2] != player) && 
                (board_[2, 1, 1] != null && board_[2,1,1] != player) && 
                (board_[3, 0, 0] == player || board_[0,3,3] == player))
                ))
            {
                result++;
            }

            return result;
        }

        // checks the number of n in a row blocks
        // for the computer player on a specified board.  
        // n is an integer such as 3 (to check 
        // how many 3 in a rows of the other player are blocked by the computer) 
        private int checkNumberInARowBlocks2D(Board board, Player[,] board2D, int n, char dimension)
        {
            //count is counting how many in a row so far for each piece
            int count = 0;
            //result is the total number of n in a rows found so far
            int result = 0;

            if (dimension == 'z')
            {
                //checking horizontal n in a rows
                for (int i = 0; i < board.getRows(); i++)
                {
                    count = 0;

                    for (int j = 0; j < board.getColumns(); j++)
                    {
                        if (board2D[i, j] != null && board2D[i, j] != player)
                        {
                            count++;
                            if (count == n)
                            {
                                //see if blocked after the n in a row
                                try
                                {
                                    if (board2D[i,j + 1] == player)
                                    {
                                        result++;
                                    }
                                }
                                catch (Exception) { }

                                //see if blocked before the n in a row
                                try
                                {
                                    if (board2D[i,j - n] == player)
                                    {
                                        result++;
                                    }
                                }
                                catch (Exception) { }
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
                //checking vertical n in a rows
                for (int j = 0; j < board.getColumns(); j++)
                {
                    count = 0;

                    for (int i = 0; i < board.getRows(); i++)
                    {
                        if (board2D[i, j] != null && board2D[i, j] != player)
                        {
                            count++;
                            if (count == n)
                            {
                                //see if blocked after the n in a row
                                try
                                {
                                    if (board2D[i + 1, j] == player)
                                    {
                                        result++;
                                    }
                                }
                                catch (Exception) { }

                                //see if blocked before the n in a row
                                try
                                {
                                    if (board2D[i - n, j] == player)
                                    {
                                        result++;
                                    }
                                }
                                catch (Exception) { }
                            }
                        }
                        else
                        {
                            count = 0;
                        }
                    }
                }
            }

            //checking diagonal n in a rows

            if (n == 3)
            {
                if (((board2D[0, 0] == player) &&
                     (board2D[1, 1] != null && board2D[1, 1] != player) &&
                     (board2D[2, 2] != null && board2D[2, 2] != player) &&
                     (board2D[3, 3] != null && board2D[3, 3] != player)) ||

                     ((board2D[0, 0] != null && board2D[0, 0] != player) &&
                     (board2D[1, 1] != null && board2D[1, 1] != player) &&
                     (board2D[2, 2] != null && board2D[2, 2] != player) &&
                     (board2D[3, 3] == player)) ||



                    ((board2D[0, 3] == player) &&
                     (board2D[1, 2] != null && board2D[1, 2] != player) &&
                     (board2D[2, 1] != null && board2D[2, 1] != player) &&
                     (board2D[3, 0] != null && board2D[3, 0] != player)) ||

                    ((board2D[0, 3] != null && board2D[0, 3] != player) &&
                     (board2D[1, 2] != null && board2D[1, 2] != player) &&
                     (board2D[2, 1] != null && board2D[2, 1] != player) &&
                     (board2D[3, 0] == player)))
                {
                    result++;
                }
            }
            else if (n == 2)
            {
                if (((board2D[2, 2] == player) &&
                     (board2D[1, 1] != null && board2D[1, 1] != player) &&
                     (board2D[0, 0] != null && board2D[0, 0] != player)) ||

                     ((board2D[3, 3] != null && board2D[3, 3] != player) &&
                     (board2D[2, 2] != null && board2D[2, 2] != player) &&
                     (board2D[1, 1] == player)) ||

                     ((board2D[1, 1] != null && board2D[1, 1] != player) &&
                     (board2D[2, 2] != null && board2D[2, 2] != player) &&
                     (board2D[0, 0] == player || board2D[3, 3] == player)) ||




                    ((board2D[2, 1] == player) &&
                     (board2D[1, 2] != null && board2D[1, 2] != player) &&
                     (board2D[0, 3] != null && board2D[0, 3] != player)) ||

                     ((board2D[3, 0] != null && board2D[3, 0] != player) &&
                     (board2D[2, 1] != null && board2D[2, 1] != player) &&
                     (board2D[1, 2] == player)) ||

                     ((board2D[1, 2] != null && board2D[1, 2] != player) &&
                     (board2D[2, 1] != null && board2D[2, 1] != player) &&
                     (board2D[0, 3] == player || board2D[3, 0] == player)))
                {
                    result++;
                }
            }
            
            return result;
        }
    }
}