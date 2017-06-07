using System;
using System.Collections.Generic;

namespace castle
{
    public class Deck {
        public List<Card> cards = new List<Card>();
        public Deck() {
            setDeck();
        }
        private void setDeck() {
            string[] suits = new string[4]{"Clubs", "Spades", "Hearts", "Diamonds"};
            // string[] suits = new string[4]{"\u2663", "\u2660", "\u2665", "\u2666"};
            // string[] suits = new string[4]{"♣", "♠", "♥", "♦"};
            for (int i = 0; i < suits.Length; i++) {
                //aces low
                // for (int j = 1; j <= 13; j++) {
                //     Card c = new Card(j, suits[i]);
                //     cards.Add(c);
                // }
                //aces high
                for (int j = 2; j <= 14; j++) {
                    Card c = new Card(j, suits[i]);
                    cards.Add(c);
                }
            }
        }
        public Card deal() {
            Card topCard = cards[cards.Count-1];
            cards.RemoveAt(cards.Count-1);
            return topCard;
        }
        public void shuffle() {
            Random rand = new Random();
            for (int i = 0; i < cards.Count; i++) {
                int r = rand.Next(cards.Count);
                Card tmp = cards[i];
                cards[i] = cards[r];
                cards[r] = tmp;
            }
        }
        public void reset() {
            setDeck();
        }
    }
}