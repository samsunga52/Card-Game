using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CardGame.AlphanumericCheck;

namespace CardGame
{
    public class ValidateCards
    {
        public static List<string> ValidateInputCards(string[] lines)
        {
            // Create a dictionary to store the player's hand
            Dictionary<string, List<string>> playerHands = new Dictionary<string, List<string>>();
            List<string> playerWithIncorrect = new List<string>();

            // Process each line in the input file
            foreach (string line in lines)
            {
                // Split the line into player name and cards
                string[] parts = line.Split(':');
                string playerName = parts[0].Trim();
                string[] cards = parts[1].Split(',');

                // Create a list to store the player's cards
                List<string> playerCards = new List<string>();

                // Process each card in the line
                foreach (string c in cards)
                {
                    // Remove any spaces and convert to uppercase
                    string trimmedCard = c.Trim().ToUpper();

                    // Add the card to the player's hand
                    playerCards.Add(trimmedCard);
                }

                // Add the player's hand to the dictionary
                playerHands.Add(playerName, playerCards);
            }
            foreach (KeyValuePair<string, List<string>> playerHand in playerHands)
            {
                Console.WriteLine(playerHand.Key + ":" + string.Join(",", playerHand.Value));
                foreach(var item in playerHand.Value)
                {
                    var x = CardHelper.GetBaseCardValue(item);

                    if(x == 0)
                    {
                        playerWithIncorrect.Add(playerHand.Key);
                    }
                }
            }

            return playerWithIncorrect;
        }
    }
}
