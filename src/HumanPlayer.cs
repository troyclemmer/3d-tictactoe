using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SeniorProject
{
    public class HumanPlayer : Player
    {
        public HumanPlayer(string name_, string color_)
        {
            name = name_;
            color = color_;
        }

        public override Board takeTurn(Game game, int p, int r, int c)
        {
            Board board = game.getBoard();

            if (board.getPlayerInPosition(p, r, c) == null)
            {
                board = board.addMove(this, p, r, c);
                game.getLogWindow().write(getName() + " claimed " + p.ToString() + "p, " + r.ToString() + "r, " + c.ToString() + "c" + ".");
            }
            else
            {
                return null;
            } 

            return board;
        }

        public override string getType()
        {
            return "HumanPlayer";
        }

        public override int getDifficulty()
        {
            return -1;
        }

        public override string getDifficultyType()
        {
            return "";
        }
    }
}
