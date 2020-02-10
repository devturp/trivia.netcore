using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyTrivia
{
    public class Game
    {
        #region Init

        int numberOfPlayers = 0;
        DiceOptions diceOptions = null;
        List<string> players = new List<string>();
        List<int> places = new List<int>();
        List<int> purses = new List<int>();
        List<bool> inPenaltyBox = new List<bool>();

        LinkedList<string> popQuestions = new LinkedList<string>();
        LinkedList<string> scienceQuestions = new LinkedList<string>();
        LinkedList<string> sportsQuestions = new LinkedList<string>();
        LinkedList<string> rockQuestions = new LinkedList<string>();

        int currentPlayer = 0;
        bool isGettingOutOfPenaltyBox;

        public Game()
        {
            for (int i = 0; i < 50; i++)
            {
                popQuestions.AddLast("Pop Question " + i);
                scienceQuestions.AddLast(("Science Question " + i));
                sportsQuestions.AddLast(("Sports Question " + i));
                rockQuestions.AddLast("Rock Question " + i);
            }
        }
        #endregion

        #region Rolling and Moving
        public void roll(Random rand)
        {
            // Get Roll Number
            var roll = rand.Next(diceOptions.MinNumber, diceOptions.MaxNumber);
            Console.WriteLine(players[currentPlayer] + " is the current player");
            Console.WriteLine("They have rolled a " + roll);

            if (inPenaltyBox[currentPlayer])
                MoveFromPenaltyBox(roll);
            else
                Move(roll);
        }
        private void Move(int roll)
        {
            places[currentPlayer] = places[currentPlayer] + roll;
            if (places[currentPlayer] > 11) places[currentPlayer] = places[currentPlayer] - 12;

            Console.WriteLine($"{players[currentPlayer]}'s new location is {places[currentPlayer]}");
            Console.WriteLine("The category is " + currentCategory());
            askQuestion();
        }
        private void MoveFromPenaltyBox(int roll)
        {
            if (roll % 2 != 0)
            {
                isGettingOutOfPenaltyBox = true;
                inPenaltyBox[currentPlayer] = false;
                Console.WriteLine(players[currentPlayer] + " is getting out of the penalty box");
                Move(roll);
            }
            else
            {
                isGettingOutOfPenaltyBox = false;
                Console.WriteLine(players[currentPlayer] + " is not getting out of the penalty box");
            }
        }
        #endregion

        private void askQuestion()
        {
            if (currentCategory() == "Pop")
            {
                Console.WriteLine(popQuestions.First());
                popQuestions.RemoveFirst();
            }
            if (currentCategory() == "Science")
            {
                Console.WriteLine(scienceQuestions.First());
                scienceQuestions.RemoveFirst();
            }
            if (currentCategory() == "Sports")
            {
                Console.WriteLine(sportsQuestions.First());
                sportsQuestions.RemoveFirst();
            }
            if (currentCategory() == "Rock")
            {
                Console.WriteLine(rockQuestions.First());
                rockQuestions.RemoveFirst();
            }
        }
        private String currentCategory()
        {
            if (places[currentPlayer] == 0) return "Pop";
            if (places[currentPlayer] == 4) return "Pop";
            if (places[currentPlayer] == 8) return "Pop";
            if (places[currentPlayer] == 1) return "Science";
            if (places[currentPlayer] == 5) return "Science";
            if (places[currentPlayer] == 9) return "Science";
            if (places[currentPlayer] == 2) return "Sports";
            if (places[currentPlayer] == 6) return "Sports";
            if (places[currentPlayer] == 10) return "Sports";
            return "Rock";
        }
        private bool didPlayerWin()
        {
            return !(purses[currentPlayer] == 6);
        }

        #region Answers
        public bool wasCorrectlyAnswered()
        {
            if (inPenaltyBox[currentPlayer] && !isGettingOutOfPenaltyBox)
            {
                currentPlayer++;
                if (currentPlayer == players.Count) currentPlayer = 0;
                return true;
            }
            else
            {
                return CorrectAnswer();
            }
        }
        private bool CorrectAnswer()
        {
            Console.WriteLine("Answer was correct!");
            purses[currentPlayer]++;
            Console.WriteLine($"{players[currentPlayer]} now has {purses[currentPlayer]} Gold Coins.");

            bool winner = didPlayerWin();
            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return winner;
        }
        public bool wrongAnswer()
        {
            Console.WriteLine("Question was incorrectly answered");
            Console.WriteLine(players[currentPlayer] + " was sent to the penalty box");
            inPenaltyBox[currentPlayer] = true;

            currentPlayer++;
            if (currentPlayer == players.Count) currentPlayer = 0;
            return true;
        }
        #endregion

        #region Game Setup
        public void Setup()
        {
            PlayerCount();

            Console.WriteLine("The game has two types of dice. Please select one.");
            Console.WriteLine("   1. Six sided dice, with sides numbered 1-6. (Traditional)");
            Console.WriteLine("   2. Ten sided dice, with sides numbered 0-9");
            Console.WriteLine("");
            DiceSelection();
        }
        public bool addPlayer(String playerName)
        {
            players.Add(playerName);
            places.Add(0);
            purses.Add(0);
            inPenaltyBox.Add(false);

            Console.WriteLine(playerName + " was added");
            return true;
        }
        private void DiceSelection()
        {
            Console.Write("Your dice choice: ");
            var diceSelection = Console.ReadKey();
            Console.WriteLine("");
            if (diceSelection.Key == ConsoleKey.D1 || diceSelection.Key == ConsoleKey.D2)
            {
                diceOptions = new DiceOptions();
                switch(diceSelection.Key)
                {
                    case ConsoleKey.D1:
                        diceOptions.MinNumber = 1;
                        diceOptions.MaxNumber = 6;
                        break;
                    case ConsoleKey.D2:
                        diceOptions.MinNumber = 0;
                        diceOptions.MaxNumber = 9;
                        break;
                }
            }
            else 
            {
                Console.WriteLine("Error: Please select #1 or #2.");
                DiceSelection();
            }
        }
        private void PlayerCount()
        {
            Console.Write("How many players: ");
            string v = Console.ReadLine();
            numberOfPlayers = int.Parse(v);

            if (numberOfPlayers > 8 || numberOfPlayers < 1) 
            {
                 Console.WriteLine("Error: Please enter 2-8 players.");
                 PlayerCount();
            }
            else 
            {
                for (int i = 1; i < numberOfPlayers + 1; i++)
                {
                    Console.Write($"Player {i} Name: ");
                    string playerName = Console.ReadLine();
                    addPlayer(playerName);
                }
            }
        }
        #endregion
    }
    
    public class DiceOptions
    {
        public int MaxNumber { get; set;}
        public int MinNumber { get; set;}
    }
}