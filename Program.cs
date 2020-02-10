using System;
using UglyTrivia;

namespace Trivia.NetCore
{
    public class GameRunner
    {
        private static bool notAWinner;
        public static void Main(String[] args)
        {
            Game aGame = new Game();
            aGame.Setup();

            Random rand = new Random(2);
            do
            { 
                aGame.roll(rand);
                if (rand.Next(9) == 7)
                {
                    notAWinner = aGame.wrongAnswer();
                }
                else
                {
                    notAWinner = aGame.wasCorrectlyAnswered();
                }
            } 
            while (notAWinner);

            if (Console.ReadKey().Key == ConsoleKey.Enter) 
                System.Environment.Exit(0);
        }
        
    }
}