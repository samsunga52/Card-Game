using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static CardGame.AlphanumericCheck;

namespace CardGame
{
    public static class CardHelper
    {
        public static List<KeyValuePair<string, string>> FindHighestCard(string playername, string[] card)
        {
            List<string> list = card.Cast<string>().ToList();
            var userShapes1 = new List<KeyValuePair<string, string>>();
            var userNumbers = new List<KeyValuePair<string, string>>();
            var scores = new List<KeyValuePair<string, string>>();

            Dictionary<string, int> usertiedCard = new Dictionary<string, int>();

            foreach (var item in list)
            {
                userShapes1.Add(new KeyValuePair<string, string>(playername, item));
            }

            foreach (var x in userShapes1)
            {
                string cardValue = GetCardValue(x.Value);
                scores.Add(new KeyValuePair<string, string>(playername,cardValue));
            }

            return scores;
        }

        public static string GetCardValue(string card)
        {
            switch (card.ToUpper())
            {
                case "KD":
                    return 17.ToString()+","+card;
                case "KC":
                    return 16.ToString() + "," + card;
                case "KS":
                    return 15.ToString() + "," + card;
                case "KH":
                    return 14.ToString() + "," + card;
                case "QD":
                    return 16.ToString() + "," + card;
                case "QC":
                    return 15.ToString() + "," + card;
                case "QS":
                    return 14.ToString() + "," + card;
                case "QH":
                    return 13.ToString() + "," + card;
                case "JD":
                    return 15.ToString() + "," + card;
                case "JC":
                    return 14.ToString() + "," + card;
                case "JS":
                    return 13.ToString() + "," + card;
                case "JH":
                    return 12.ToString() + "," + card;
                case "AD":
                    return 15.ToString() + "," + card;
                case "AC":
                    return 14.ToString() + "," + card;
                case "AS":
                    return 13.ToString() + "," + card;
                case "AH":
                    return 12.ToString() + "," + card;
                case "2D":
                    return 8.ToString() + "," + card;
                case "2C":
                    return 5.ToString() + "," + card;
                case "2S":
                    return 4.ToString() + "," + card;
                case "2H":
                    return 3.ToString() + "," + card;
                case "3D":
                    return 7.ToString() + "," + card;
                case "3C":
                    return 6.ToString() + "," + card;
                case "3S":
                    return 5.ToString() + "," + card;
                case "3H":
                    return 4.ToString() + "," + card;
                case "4D":
                    return 8.ToString() + "," + card;
                case "4C":
                    return 7.ToString() + "," + card;
                case "4S":
                    return 6.ToString() + "," + card;
                case "4H":
                    return 5.ToString() + "," + card;
                case "5D":
                    return 9.ToString() + "," + card;
                case "5C":
                    return 8.ToString() + "," + card;
                case "5S":
                    return 7.ToString() + "," + card;
                case "5H":
                    return 6.ToString() + "," + card;
                case "6D":
                    return 10.ToString() + "," + card;
                case "6C":
                    return 9.ToString() + "," + card;
                case "6S":
                    return 8.ToString() + "," + card;
                case "6H":
                    return 7.ToString() + "," + card;
                case "7D":
                    return 11.ToString() + "," + card;
                case "7C":
                    return 10.ToString() + "," + card;
                case "7S":
                    return 9.ToString() + "," + card;
                case "7H":
                    return 8.ToString() + "," + card;
                case "8D":
                    return 12.ToString() + "," + card;
                case "8C":
                    return 11.ToString() + "," + card;
                case "8S":
                    return 10.ToString() + "," + card;
                case "8H":
                    return 9.ToString() + "," + card;
                case "9D":
                    return 13.ToString() + "," + card;
                case "9C":
                    return 12.ToString() + "," + card;
                case "9S":
                    return 11.ToString() + "," + card;
                case "9H":
                    return 10.ToString() + "," + card;
                case "10D":
                    return 14.ToString() + "," + card;
                case "10C":
                    return 13.ToString() + "," + card;
                case "10S":
                    return 12.ToString() + "," + card;
                case "10H":
                    return 11.ToString() + "," + card;
                default:
                    return card;
            }
        }

        public static CardViewModel FindMax(IEnumerable<KeyValuePair<string, string>> lsd)
        {
            var cardviewmodel = new CardViewModel();
            cardviewmodel.Cards = new List<Card>();
            foreach (KeyValuePair<string, string> pair in lsd)
            {
                string originalValue = pair.Value;
                string shape = originalValue.Substring(pair.Value.IndexOf(",") +1);
                string number = originalValue.Substring(0,pair.Value.IndexOf(","));
                cardviewmodel.Cards.Add(new Card()
                {
                    Name = pair.Key,
                    Suit = shape,
                    Value = Convert.ToInt32(number)
                });
            }

            return cardviewmodel;
        }

        public static int GetBaseCardValue(string suit)
        {
            switch (suit.ToUpper())
            {
                case "KD":
                    return 4;
                case "KC":
                    return 3;
                case "KS":
                    return 2;
                case "KH":
                    return 1;
                case "QD":
                    return 4;
                case "QC":
                    return 3;
                case "QS":
                    return 2;
                case "QH":
                    return 1;
                case "JD":
                    return 4;
                case "JC":
                    return 3;
                case "JS":
                    return 2;
                case "JH":
                    return 1;
                case "AD":
                    return 4;
                case "AC":
                    return 3;
                case "AS":
                    return 2;
                case "AH":
                    return 1;
                case "2D":
                    return 4;
                case "2C":
                    return 3;
                case "2S":
                    return 2;
                case "2H":
                    return 1;
                case "3D":
                    return 4;
                case "3C":
                    return 3;
                case "3S":
                    return 2;
                case "3H":
                    return 1;
                case "4D":
                    return 4;
                case "4C":
                    return 3;
                case "4S":
                    return 2;
                case "4H":
                    return 1;
                case "5D":
                    return 4;
                case "5C":
                    return 3;
                case "5S":
                    return 2;
                case "5H":
                    return 1;
                case "6D":
                    return 4;
                case "6C":
                    return 3;
                case "6S":
                    return 2;
                case "6H":
                    return 1;
                case "7D":
                    return 4;
                case "7C":
                    return 3;
                case "7S":
                    return 2;
                case "7H":
                    return 1;
                case "8D":
                    return 4;
                case "8C":
                    return 3;
                case "8S":
                    return 2;
                case "8H":
                    return 1;
                case "9D":
                    return 4;
                case "9C":
                    return 3;
                case "9S":
                    return 2;
                case "9H":
                    return 1;
                case "10D":
                    return 4;
                case "10C":
                    return 3;
                case "10S":
                    return 2;
                case "10H":
                    return 1;
                default:
                    return 0;
            }
        }
    }
}
