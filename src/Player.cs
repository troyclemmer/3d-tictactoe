using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeniorProject
{
    public abstract class Player
    {
        protected string name;
        protected string color;
        private int[,] win = null;

        public Player()
        {
        }

        public String getName()
        {
            return name;
        }

        public String getColor()
        {
            return color;
        }

        public int[,] getWin()
        {
            return win;
        }

        public void setWin(int[,] win_)
        {
            win = win_;
        }

        public abstract Board takeTurn(Game game, int p, int r, int c);

        public abstract string getType();

        public abstract int getDifficulty();

        public abstract string getDifficultyType();

    }
}
