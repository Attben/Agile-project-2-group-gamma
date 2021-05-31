using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LudoLike.Classes
{
    enum Hand
    {
        Rock, Scissor, Paper
    }
    class StenMiniGame : Minigame
    {
        Player player1;
        Player player2;

        int points;

        Hand player1hand;
        Hand player2hand;

        Player Winner;

        public StenMiniGame(Player one, Player two,int points)
        {
            player1 = one;
            player2 = two; 
            this.points = points;
        }

        void chooseHand()
        {
            bool one = false;
            bool two = false;

            do
            {
                if (one)
                {
                    ConsoleKeyInfo ok = Console.ReadKey();
                    if (ok.Key == ConsoleKey.A) player1hand = Hand.Rock;

                    if (ok.Key == ConsoleKey.S) player1hand = Hand.Scissor;

                    if (ok.Key == ConsoleKey.D) player1hand = Hand.Paper;
                }

                if (two)
                {
                    ConsoleKeyInfo ok = Console.ReadKey();
                    if (ok.Key == ConsoleKey.A) player2hand = Hand.Rock;

                    if (ok.Key == ConsoleKey.S) player2hand = Hand.Scissor;

                    if (ok.Key == ConsoleKey.D) player2hand = Hand.Paper;
                }
            } while (one || two);
        }

        void CalculateResults()
        {


            if (player1hand == Hand.Rock && player2hand == Hand.Scissor)
            {
                Winner = player1;
            }
            else if (player1hand == Hand.Scissor && player2hand == Hand.Paper)
            {
                Winner = player1;
            }
            else if (player1hand == Hand.Paper && player2hand == Hand.Rock)
            {
                Winner = player1;
            }
            else if (player2hand == Hand.Rock && player1hand == Hand.Scissor)
            {
                Winner = player1;
            }
            else if (player2hand == Hand.Scissor && player1hand == Hand.Paper)
            {
                Winner = player1;
            }
            else if (player2hand == Hand.Paper && player1hand == Hand.Rock)
            {
                Winner = player1;
            }
            else
            {

            }

            pointsDistribution(Winner);

        }

         void pointsDistribution(Player winner)
        {
            if (player1 == winner)
            {
                player1.ChangeScore(points);
                player2.ChangeScore(-points);
            }
            else
            {
                player2.ChangeScore(points);
                player1.ChangeScore(-points);
            }
        }


    }
}
