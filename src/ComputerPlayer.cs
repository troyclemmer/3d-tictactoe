using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeniorProject
{
    public class ComputerPlayer : Player
    {
        private int difficulty;
        private AI ai;

        private List<string> nameList;

        public ComputerPlayer(string color_, int difficulty_)
        {
            color = color_;
            difficulty = difficulty_;
            ai = new AI(difficulty, this);

            initializeNameList();
            randomName();
        }



        private void initializeNameList()
        {
            nameList = new List<string>();
            nameList.Add("Dr. Crichlow");
            nameList.Add("Prometheus");
            nameList.Add("Butch");
            nameList.Add("Julio");
            nameList.Add("Justin");
            nameList.Add("Troy Clemmer");
            nameList.Add("Cierra");
            nameList.Add("Justin");
            nameList.Add("Kevin");
            nameList.Add("Kurt");
            nameList.Add("Sally");
            nameList.Add("Selena");
            nameList.Add("Boogie Man");
            nameList.Add("Bob");
            nameList.Add("Poncho");
            nameList.Add("Skeeter");
            nameList.Add("Laquisha");
            nameList.Add("Aquaniqua");
            nameList.Add("Dexter");
            nameList.Add("Chuckie");
            nameList.Add("Catdog");
            nameList.Add("Rocco");
            nameList.Add("Spongebob");
            nameList.Add("Taylor Swift");
            nameList.Add("Happy Gilmore");
            nameList.Add("Rebecca Black");
            nameList.Add("Marshall Mathers");
            nameList.Add("Tim Tebow");
            nameList.Add("Ricky Martin");
            nameList.Add("Enrique Iglesias");
            nameList.Add("Rihanna");
            nameList.Add("Phineas");

        }

        private void randomName()
        {
            Random random = new Random();

            //pick a random name from the list of colors
            int randomNumber = random.Next(0, nameList.Count);

            name = nameList[randomNumber];
        }

        public override Board takeTurn(Game game, int p, int r, int c)
        {
            Board board = game.getBoard();
            int[] move = ai.computeMove(board);
            p = move[0];
            r = move[1];
            c = move[2];

            board = board.addMove(this, p, r, c);
            game.getLogWindow().write(getName() + " claimed " + p.ToString() + "p, " + r.ToString() + "r, " + c.ToString() + "c" + ".");


            /* DIAGNOSTIC PRINTS
            game.getLogWindow().write("There are this many computer 2 in a rows: " + ai.checkNumberInARow(board, 2).ToString());
            game.getLogWindow().write("There are this many computer 3 in a rows: "+ai.checkNumberInARow(board, 3).ToString());
            game.getLogWindow().write("There are this many computer 4 in a rows: " + ai.checkNumberInARow(board, 4).ToString());
            
            game.getLogWindow().write("There are this many 2 in a row blocks: " + ai.checkNumberInARowBlocks(board, 2).ToString());
            game.getLogWindow().write("There are this many 3 in a row blocks: " + ai.checkNumberInARowBlocks(board, 3).ToString());
            */

            return board;
        }

        public override string getType()
        {
            return "ComputerPlayer(" + getDifficultyType() + ")";
        }

        public override int getDifficulty()
        {
            return difficulty;
        }

        // 0 for easy
        // 1 for medium
        // 2 for hard
        public override string getDifficultyType()
        {
            string s = "easy";

            if (difficulty == 1)
            {
                s = "medium";
            }
            else if (difficulty == 2)
            {
                s = "hard";
            }

            return s;
        }
    }
}
