using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using static CardGame.AlphanumericCheck;

namespace CardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if the correct number of arguments is provided
            if (args.Length != 2)
            {
                Console.WriteLine("Please provide the input and output file names as command line arguments.");
                return;
            }

            string inputFile = args[0];
            string outputFile = args[1];

            try
            {

                bool needTieBreak = false;
                List<Player> tiedTeams = null;
                int tieScore = 0;

                // Read the data from the input file
                string[] data = File.ReadAllLines(inputFile);

                if (data.Length == 0)
                {
                    Console.WriteLine("Error: The text file is empty.");
                    return;
                }

                var nonBlankLines = new List<string>();

                // Iterate through each line and check if it is blank
                foreach (string line in data)
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        // Add non-blank lines to the list
                        nonBlankLines.Add(line);
                    }
                }

                // Write the non-blank lines to the temporary file
                File.WriteAllLines(inputFile, nonBlankLines);

                // Replace the original file with the temporary file
                //File.Replace(inputFile, inputFile, null);

                Dictionary<string, string[]> userCards = new Dictionary<string, string[]>();

                string[] lines = File.ReadAllLines(inputFile);

                // Check if each line contains a valid card
                foreach (string line in lines)
                {
                    // Split the line into player name and card values
                    string[] parts = line.Split(':');
                    string playerName = parts[0];
                    string[] cardValues = parts[1].Trim().Split(',');
                    if(cardValues.Length != 5)
                    {
                        Console.WriteLine($"Error: 5 cards is required to play");
                        return;
                    }
                    if (!line.Contains(":"))
                    {
                        Console.WriteLine($"Error: Please enter player name seperated by a colon");
                        return;
                    }
                    else
                    {

                        userCards.Add(playerName, cardValues);
                    }
                }

                if (userCards.Count != 7)
                {
                    Console.WriteLine("Error: 7 players required to play.");
                    return;
                }
                else if (userCards.Count == 7)
                {
                    var x = ValidateCards.ValidateInputCards(lines);
                    foreach (var item in x)
                    {
                        Console.WriteLine(String.Join("\n", $"{item} has in-correct player cards"));
                    }
                }



                // Create a list to store the players and their scores
                List<Player> players = new List<Player>();

                // Create a list to store the players and their scores
                List<Player> playersTied = new List<Player>();

                // Iterate over each line in the input file
                foreach (string line in lines)
                {
                    // Split the line into player name and card values
                    string[] parts = line.Split(':');
                    string playerName = parts[0];
                    string[] cardValues = parts[1].Trim().Split(',');

                    // Calculate the score for each player
                    int score = 0;
                    foreach (string cardValue in cardValues)
                    {
                        score += GetCardValue(cardValue);
                    }


                    // Create a new player object and add it to the list
                    Player player = new Player(playerName, score);
                    players.Add(player);


                }

                // Find the highest score among the players
                int highestScore = 0;
                foreach (Player player in players)
                {
                    if (player.Score > highestScore)
                    {
                        highestScore = player.Score;
                    }
                }
                int baseValue = 0;

                // Create a list to store the winners
                List<Player> winners = new List<Player>();
                Card card = new Card();
                List<string> playerNames = new List<string>();

                List<KeyValuePair<string, string>> suitScores = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> u = new List<KeyValuePair<string, string>>();

                var duplicate = players.GroupBy(x => new { x.Score })
                   .Where(x => x.Skip(1).Any()).Select(x => x.Key).ToList();


                tieScore = duplicate.ToList().Max(x => x.Score);

                var orderedTeams = players.OrderBy(t => t.Score).ToList();
                tiedTeams = orderedTeams.Where(t => t.Score == tieScore).ToList();
                needTieBreak = tiedTeams.Count() > 0;

                // Find the players with the highest score
                foreach (Player player in players)
                {
                    if (player.Score == highestScore)
                    {
                        winners.Add(player);
                    }
                }

                bool numberOfTiedScores = tiedTeams.Count() > 1;
                bool winnersCount = winners.Count() == 1;


                if (needTieBreak == true && numberOfTiedScores && winnersCount == false)
                {
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(':');
                        string playerName = parts[0];
                        playerNames.Add(playerName);
                        string[] cardValues = parts[1].Trim().Split(',');

                        foreach (var tied in tiedTeams)
                        {
                            foreach (string x in lines.Where(x => x.Contains(tied.Name)))
                            {
                                if (x.Contains(tied.Name))
                                {
                                    string[] parts2 = x.Split(':');
                                    string playerName2 = parts2[0];
                                    playerNames.Add(playerName);
                                    string[] cardValues2 = parts2[1].Split(',');

                                    u = CardHelper.FindHighestCard(tied.Name, cardValues2);

                                    suitScores.Add(new KeyValuePair<string, string>(u.Max(x => x.Key), u.Max(x => x.Value)));

                                }
                            }
                        }
                    }
                    foreach (var item in u)
                    {
                        suitScores.Add(new KeyValuePair<string, string>(item.Key, item.Value));
                    }

                    var v = CardHelper.FindMax(suitScores);
                    card = (from i in v.Cards
                            let maxId = v.Cards.Max(m => m.Value)
                            where i.Value == maxId
                            select i).FirstOrDefault();

                    baseValue = CardHelper.GetBaseCardValue(card.Suit);

                    using (StreamWriter writer = new StreamWriter(outputFile))
                    {
                        foreach (Player winner in winners)
                        {
                            if (winner.Name == card.Name)
                            {
                                writer.WriteLine($"{winner.Name}: {winner.Score + baseValue}");
                            }
                        }
                    }
                }
                else
                {
                    List<PlayerScores> p = new List<PlayerScores>();
                    using (StreamWriter writer = new StreamWriter(outputFile))
                    {
                        if (winners.Count() > 1)
                        {
                            foreach (Player winner in winners)
                            {
                                var player = new Player(winner.Name, winner.Score);
                                p.Add(new PlayerScores()
                                {
                                    Name = player.Name,
                                    Score = player.Score
                                });
                            }
                            PlayerScores[] names = p.ToArray();
                            string[] name = names.Select(c => c.Name.ToString()).ToArray();
                            string commaSeparatedString = string.Join(",", name);

                            string score = p.Max(x => x.Score).ToString();
                            writer.WriteLine($"{commaSeparatedString}:{score}");
                        }
                        else
                        {
                            foreach (Player winner in winners)
                            {
                                writer.WriteLine($"{winner.Name}: {winner.Score}");
                            }
                        }
                    }
                }



                Console.WriteLine("The winners have been written to the output file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }



            static int GetCardValue(string cardValue)
            {
                switch (cardValue.ToUpper())
                {
                    case "2C":
                        return 2;
                    case "2D":
                        return 2;
                    case "2S":
                        return 2;
                    case "2H":
                        return 2;
                    case "3C":
                        return 3;
                    case "3H":
                        return 3;
                    case "3S":
                        return 3;
                    case "3D":
                        return 3;
                    case "4H":
                        return 4;
                    case "4D":
                        return 4;
                    case "4S":
                        return 4;
                    case "4C":
                        return 4;
                    case "5D":
                        return 5;
                    case "5S":
                        return 5;
                    case "5C":
                        return 5;
                    case "5H":
                        return 5;
                    case "6C":
                        return 6;
                    case "6D":
                        return 6;
                    case "6S":
                        return 6;
                    case "6H":
                        return 6;
                    case "7H":
                        return 7;
                    case "7D":
                        return 7;
                    case "7C":
                        return 7;
                    case "7S":
                        return 7;
                    case "8D":
                        return 8;
                    case "8S":
                        return 8;
                    case "8C":
                        return 8;
                    case "8H":
                        return 8;
                    case "9D":
                        return 9;
                    case "9C":
                        return 9;
                    case "9S":
                        return 9;
                    case "9H":
                        return 9;
                    case "10D":
                        return 10;
                    case "10S":
                        return 10;
                    case "10C":
                        return 10;
                    case "10H":
                        return 10;
                    case "AH":
                        return 11;
                    case "AD":
                        return 11;
                    case "AC":
                        return 11;
                    case "AS":
                        return 11;
                    case "KH":
                        return 13;
                    case "KC":
                        return 13;
                    case "KS":
                        return 13;
                    case "KD":
                        return 13;
                    case "QD":
                        return 12;
                    case "QC":
                        return 12;
                    case "QH":
                        return 12;
                    case "QS":
                        return 12;
                    case "JH":
                        return 11;
                    case "JC":
                        return 11;
                    case "JD":
                        return 11;
                    case "JS":
                        return 11;
                    default:
                        return 0;
                }
            }
        }
    }
        class Player
        {
            public string Name { get; set; }
            public int Score { get; set; }

            public Player(string name, int score)
            {
                Name = name;
                Score = score;
            }

        }

        class PlayerScores
        {
            public string Name { get; set; }
            public int Score { get; set; }
        }

        public static class AlphanumericCheck
        {
            public static bool HasLettersAndNumbersOnly(string value)
            {
                var userScores = value.Split(',').Reverse().ToList<string>();
                bool isMatch = false;
                Regex r = new Regex(@"\d");
                foreach (var s in userScores)
                {
                    isMatch = r.IsMatch(s);
                }

                return isMatch;
            }

            static int GetTiedCardValue(string cardValue)
            {
                switch (cardValue.ToUpper())
                {
                    case "2":
                        return 2;
                    case "3":
                        return 3;
                    case "4":
                        return 4;
                    case "5":
                        return 5;
                    case "6":
                        return 6;
                    case "7":
                        return 7;
                    case "8":
                        return 8;
                    case "9":
                        return 9;
                    case "10":
                        return 10;
                    case "J":
                        return 11;
                    case "Q":
                        return 12;
                    case "K":
                        return 13;
                    case "A":
                        return 11;
                    default:
                        return int.Parse(cardValue);
                }
            }



            public class Card
            {
                public string Name;
                public int Value;
                public string Suit;
            }

            public class CardViewModel
            {
                public List<Card> Cards { get; set; }
            }
        }
    }