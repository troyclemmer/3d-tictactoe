using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using CPI.Plot3D;
using System.Collections.Generic;

namespace SeniorProject
{
    public class Display
    {
        private const int boardX = 4;
        private const int boardY = 4;
        private const int boardZ = 4;
        private double defaultBoardAngle = 115;
        private double boardAngle = 115;
        private float defaultXCord = 240;
        private float defaultYCord = 85;
        private float sideLength = 25;
        private int planeDistance = 115;

        //array that is 3 dimensions, [plane, row, column] that contains a coordinate of where that spot is drawn on the screen
        private Coordinate[, ,] spotMap = new Coordinate[boardX, boardY, boardZ];

        //the colors that a player can choose to represent 
        List<string> colorList = new List<string>();

        public Display()
        {
            setupSpotMap();
            setupColorList();
        }

        public List<string> getColorList()
        {
            return colorList;
        }


        public int getBoardX()
        {
            return boardX;
        }

        public float getSideLength()
        {
            return sideLength;
        }

        public int getBoardY()
        {
            return boardY;
        }

        public int getBoardZ()
        {
            return boardZ;
        }

        public double getBoardAngle()
        {
            return boardAngle;
        }


        //set up the list of colors a player can choose from to represent their moves
        private void setupColorList()
        {
            colorList.Add("Chartreuse");
            colorList.Add("Cyan");
            colorList.Add("DeepPink");
            colorList.Add("Green");
            colorList.Add("LightSalmon");
	        colorList.Add("MediumOrchid");
            colorList.Add("Orange");
	        colorList.Add("Red");
            colorList.Add("Silver");
            colorList.Add("Yellow");
        }

        public void removeColor(string color)
        {
            colorList.Remove(color);
        }

        //set up the coordinates [plane,row,column] of where to draw each spot on the screen based on which spot it is
        private void setupSpotMap()
        {
            for (int p = 0; p < 4; p++)
            {
                for (int r = 0; r < 4; r++)
                {
                    for (int c = 0; c < 4; c++)
                    {
                        spotMap[p, r, c] = new Coordinate((int)defaultXCord - (r * 11) + (int)sideLength * c * 3, (int)defaultYCord - (r * 2) + (int)sideLength * r + planeDistance * p);
                    }
                }
            }
        }

        private void drawSpotMap(Form form)
        {
            using (Graphics g = form.CreateGraphics())
            using (CPI.Plot3D.Plotter3D p = new CPI.Plot3D.Plotter3D(g))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(form.BackColor);
                p.PenWidth = (float)2;
                p.PenColor = Color.FromName("White");

                foreach (Coordinate c in spotMap)
                {
                    if (c != null)
                    {
                        p.Location = new CPI.Plot3D.Point3D(c.getX(), c.getY(), 0);
                        //DrawPlayerMove(p);
                    }
                }
            }
        }


        private void DrawRhombus(Plotter3D p, int x, int y, int z, Board board)
        {

            Player player = null;
            p.PenWidth = (float)2;
            p.PenColor = Color.White;

            try
            {
                player = board.getPlayerInPosition(z + 1, y + 1, x + 1);
            }
            catch (Exception) { }

            if (player != null)
            {
                //set pen color to the player's color
                p.PenColor = Color.FromName(player.getColor());
                p.PenWidth = (float)6;
                float initX = p.Location.X;
                float initY = p.Location.Y;

                p.PenUp();

                //colored in squares
                p.Forward(sideLength * 3);
                p.TurnRight(boardAngle);
                p.Forward(sideLength / 2);
                p.TurnRight(180 - boardAngle);

                p.PenWidth = (float)sideLength / (float)1.25;
                p.PenDown();
                p.Forward(sideLength * 3);  // Draw a line sideLength long
                p.TurnRight(boardAngle);
                p.TurnRight(180 - boardAngle);
                p.Location = new CPI.Plot3D.Point3D(initX, initY, 0);
                p.PenWidth = (float)6;

                // draws the thick color outline
                p.Forward(sideLength * 3);  // Draw a line sideLength long
                p.TurnRight(boardAngle);        // Turn right 90 degrees

                p.Forward(sideLength);  // Draw a line sideLength long
                p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                p.Forward(sideLength * 3);  // Draw a line sideLength long
                p.TurnRight(boardAngle);        // Turn right 90 degrees

                p.Forward(sideLength);  // Draw a line sideLength long
                p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

            }

            //white thin outline
            p.PenWidth = (float)2;
            p.PenColor = Color.White;

            p.Forward(sideLength * 3);  // Draw a line sideLength long
            p.TurnRight(boardAngle);        // Turn right 90 degrees

            p.Forward(sideLength);  // Draw a line sideLength long
            p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

            p.Forward(sideLength * 3);  // Draw a line sideLength long
            p.TurnRight(boardAngle);        // Turn right 90 degrees

            p.Forward(sideLength);  // Draw a line sideLength long
            p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

            //this commented out secion is for displaying the win that works with all angles
            /*  
             
            int[,] win = null;

            try
            {
                win = player.getWin();
            }
            catch (Exception) { }

                if (win != null)
                {
                    

                    if ((win[0, 0] == z && win[0, 1] == y && win[0, 2] == x) ||
                        (win[1, 0] == z && win[1, 1] == y && win[1, 2] == x) ||
                        (win[2, 0] == z && win[2, 1] == y && win[2, 2] == x) ||
                        (win[3, 0] == z && win[3, 1] == y && win[3, 2] == x))
                    {
                        p.PenWidth = (float)7.5;
                        p.PenColor = Color.White;
                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    }
                }  
             */

        }

        private void DrawRhombusRow(Plotter3D p, int y, int z, Board board)
        {
            float initX = p.Location.X;
            float initY = p.Location.Y;

            for (int i = 0; i < boardX; i++)
            {
                DrawRhombus(p, i, y, z, board);
                p.Location = new CPI.Plot3D.Point3D(p.Location.X + sideLength * (float)3, p.Location.Y, 0);
            }
            p.Location = new CPI.Plot3D.Point3D(initX, initY, 0);
        }

        private void DrawRhombusPanel(Plotter3D p, int z, Board board)
        {
            float initX = p.Location.X;
            float initY = p.Location.Y;

            for (int i = 0; i < boardY; i++)
            {
                DrawRhombusRow(p, i, z, board);
                p.PenUp();
                p.Forward(sideLength * 3);  // Draw a line sideLength long
                p.TurnRight(boardAngle);        // Turn right 90 degrees
                p.Forward(sideLength);  // Draw a line sideLength long
                p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                p.Forward(sideLength * 3);  // Draw a line sideLength long
                p.TurnRight(180);        // Turn right 90 degrees
                p.PenDown();
            }
            p.Location = new CPI.Plot3D.Point3D(initX, initY, 0);
        }

        public void DrawLayersBoard(Form form, Board board)
        {
            using (Graphics g = form.CreateGraphics())
            using (CPI.Plot3D.Plotter3D p = new CPI.Plot3D.Plotter3D(g))
            {
                //p.PenWidth = (float)1.5;
                p.PenWidth = (float)2;
                p.PenColor = Color.FromName("White");

                //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(form.BackColor);

                
                for (int q = 0; q < (planeDistance * 4); q += planeDistance)
                {
                    p.Location = new CPI.Plot3D.Point3D(defaultXCord, defaultYCord + q, 0);
                    DrawRhombusPanel(p, q, board);
                }
            }
        }

        public void drawBoard(Form form)
        {
            Game game = (Game)form;
            int[] input = game.getInput();

            displayBoard(form, input[2], input[1], input[0]);
        }

        private void drawMovePreview(Plotter3D p, int x, int y, int z, Board board)
        {
            p.PenWidth = (float)7.5;
            p.PenColor = Color.White;

            if (boardAngle == defaultBoardAngle || y <= 0)
            {

                if (x >= 0 && y >= 0 && z >= 0)
                {
                    p.Location = new CPI.Plot3D.Point3D((int)defaultXCord - (y * 11) + (int)sideLength * x * 3, (int)defaultYCord - (y * 2) + (int)sideLength * y + planeDistance * z, 0);

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                }
                else if (x < 0 && y < 0 && z >= 0)
                {
                    p.Location = new CPI.Plot3D.Point3D(defaultXCord, defaultYCord + planeDistance * z, 0);

                    p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 4);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 4);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                }
                else if (x < 0 && y >= 0 && z < 0)
                {
                    for (int zc = 0; zc < 4; zc++)
                    {
                        p.Location = new CPI.Plot3D.Point3D((int)defaultXCord - (y * 11), (int)defaultYCord - (y * 2) + (int)sideLength * y + planeDistance * zc, 0);

                        p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                    }
                }
                else if (x >= 0 && y < 0 && z < 0)
                {
                    for (int zc = 0; zc < 4; zc++)
                    {
                        p.Location = new CPI.Plot3D.Point3D((int)defaultXCord + (int)sideLength * x * 3, (int)defaultYCord + planeDistance * zc, 0);

                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 4);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 4);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                    }
                }
                else if (x < 0 && y >= 0 && z >= 0)
                {
                    p.Location = new CPI.Plot3D.Point3D((int)defaultXCord - (y * 11), (int)defaultYCord - (y * 2) + (int)sideLength * y + planeDistance * z, 0);

                    p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 3 * 4);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                }
                else if (x >= 0 && y < 0 && z >= 0)
                {
                    p.Location = new CPI.Plot3D.Point3D((int)defaultXCord + (int)sideLength * x * 3, (int)defaultYCord + planeDistance * z, 0);

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 4);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 4);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                }
                else if (x >= 0 && y >= 0 && z < 0)
                {
                    for (int zc = 0; zc < 4; zc++)
                    {
                        p.Location = new CPI.Plot3D.Point3D((int)defaultXCord - (y * 11) + (int)sideLength * x * 3, (int)defaultYCord - (y * 2) + (int)sideLength * y + planeDistance * zc, 0);

                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength * 3);  // Draw a line sideLength long
                        p.TurnRight(boardAngle);        // Turn right 90 degrees

                        p.Forward(sideLength);  // Draw a line sideLength long
                        p.TurnRight(180 - boardAngle);        // Turn right 90 degrees
                    }
                }
            }
        }

        public void displayBoard(Form form, int x, int y, int z)
        {
            Game game = (Game)form;
            Board board = game.getBoard();

            using (Graphics g = form.CreateGraphics())
            using (CPI.Plot3D.Plotter3D p = new CPI.Plot3D.Plotter3D(g))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(form.BackColor);


                int i = 0;

                for (int q = 0; q < (planeDistance * 4); q += planeDistance)
                {
                    p.Location = new CPI.Plot3D.Point3D(defaultXCord, defaultYCord + q, 0);
                    DrawRhombusPanel(p, i, board);
                    i++;
                }
                drawMovePreview(p, x, y, z, board); 
            }
        }

        public void displayWinningLine(Form form)
        {
            Game game = (Game)form;
            Board board = game.getBoard();
            Player winner = game.getWinner();

            using (Graphics g = form.CreateGraphics())
            using (CPI.Plot3D.Plotter3D p = new CPI.Plot3D.Plotter3D(g))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(form.BackColor);

                boardAngle = defaultBoardAngle;

                int i = 0;

                for (int q = 0; q < (planeDistance * 4); q += planeDistance)
                {
                    p.Location = new CPI.Plot3D.Point3D(defaultXCord, defaultYCord + q, 0);
                    DrawRhombusPanel(p, i, board);
                    i++;
                }
                if (winner != null)
                {
                    drawWinningLine(p, board, winner);
                }
            }
        }
        private void drawWinningLine(Plotter3D p, Board board, Player winner)
        {
            p.PenWidth = (float)7.5;
            p.PenColor = Color.White;
            int[,] winningLine = winner.getWin();

            if ((boardAngle == defaultBoardAngle) && winningLine != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    int z = winningLine[i,0];
                    int y = winningLine[i,1];
                    int x = winningLine[i, 2];

                    p.Location = new CPI.Plot3D.Point3D((int)defaultXCord - (y * 11) + (int)sideLength * x * 3, (int)defaultYCord - (y * 2) + (int)sideLength * y + planeDistance * z, 0);

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength * 3);  // Draw a line sideLength long
                    p.TurnRight(boardAngle);        // Turn right 90 degrees

                    p.Forward(sideLength);  // Draw a line sideLength long
                    p.TurnRight(180 - boardAngle);        // Turn right 90 degrees  
                }
            }
        }
         

        public void resetBoardAngle(Form form)
        {
            boardAngle = defaultBoardAngle;
            Game game = (Game)form;

            if (game.getWinner() == null)
            {
                drawBoard(form);
            }
            else
            {
                displayWinningLine(form);
            }
            
        }

        public void decreaseBoardAngle(Form form)
        {
            if (boardAngle > 65)
            {
                boardAngle -= 5;
                Game game = (Game)form;

                if (boardAngle != defaultBoardAngle || game.getWinner() == null)
                {
                    drawBoard(form);
                }
                else
                {
                    displayWinningLine(form);
                }
            }
        }

        public void increaseBoardAngle(Form form)
        {
            if (boardAngle < 195)
            {
                boardAngle += 5;
                Game game = (Game)form;

                if (boardAngle != defaultBoardAngle || game.getWinner() == null)
                {
                    drawBoard(form);
                }
                else
                {
                    displayWinningLine(form);
                }
            }
        }

    }


}
