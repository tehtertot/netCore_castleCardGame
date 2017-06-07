using System.Collections.Generic;
using System;

namespace castle
{
    public class Player {
        public string name;
        public List<Card> hand = new List<Card>();
        public List<Card> castle = new List<Card>(); //first 3 hidden, last 3 selected by player
        public Player(string n) {
            name = n;
        }
        public void dealHiddenCastle(Deck d) {
            Card newCard = d.deal();
            castle.Add(newCard);
        }
        public Card draw(Deck d) {
            Card newCard = d.deal();
            hand.Add(newCard);
            sortHand();
            return newCard;
        }
        public Card discard(int i) {
            if (i >= hand.Count || i < 0) {
                return null;
            }
            else {
                Card toDiscard = hand[i];
                hand.RemoveAt(i);
                return toDiscard;
            }
        }
        public void sortHand() {
            hand.Sort(new Comparison<Card>((x,y) => x.val.CompareTo(y.val)));
        }
    }
}