using System;
using System.Collections.Generic;

namespace castle
{

    public static class Turn {
        public static bool playTurn(Player p, List<Card> pot, Deck deck, bool skip) {
            bool skipNext = false;
            Console.WriteLine($"{p.name}, your turn! Ready? Press any key(s) to continue! And enter!");
            Console.ReadLine();
            showBoard(pot, deck); //display the cards in the pot
            if (skip) {
                takePot(pot, p);
                p.sortHand();
                Console.WriteLine("You got skipped! Taking the pot...");
                Console.ReadLine();
                return false;
            }
            int topVal = getTopValue(pot); //get top value of the Pot
            bool handPlayable = isPlayable(p.hand, topVal, false);
            if(handPlayable) {
                skipNext = playFromHand(p, topVal, pot);
                drawCard(p, deck);
            }
            else if (p.hand.Count > 0) {
                System.Console.WriteLine("You can't play. Taking the pot...");
                takePot(pot, p);
                Console.ReadLine();
            }
            else if (p.hand.Count < 1 && deck.cards.Count == 0 && p.castle.Count > 3) {
                skipNext = playVisibleCastle(p, topVal, pot);
                Console.ReadLine();
            }
            //time to play from the invisible hand!
            else if (p.hand.Count < 1 && deck.cards.Count == 0 && p.castle.Count < 4) {
                skipNext = playInvisibleCastle(p, topVal, pot);                
                Console.ReadLine();
            }
            return skipNext;
        }//end playTurn()
        public static void showBoard(List<Card> pot, Deck deck) {
            //show current inplay deck (top 3 cards and total card count)
            Console.WriteLine("*********************************************************************************************");
            Console.Write("Top Cards: ");
            for (int i = 0; i < pot.Count; i++) {
                if (i == 3) { break; }
                Console.Write(pot[pot.Count-i-1].ToString() + " ");
            }
            Console.WriteLine("...");
            Console.Write($"{pot.Count} total cards in the pot");
            if (deck.cards.Count > 0) {
                Console.Write($"            Cards Left in Draw Pile: {deck.cards.Count}");
            }
            else { Console.Write("                                                   No Cards Left in Draw Pile");}
            Console.WriteLine();
            Console.WriteLine("*********************************************************************************************");
        }//end showBoard()

        public static int getTopValue(List<Card> pot) {
            int topVal;
            if (pot.Count == 0) {return 0;}
            else {topVal = pot[pot.Count-1].val;}
            //double check for 7's on top
            if (topVal != 7) {
                return topVal;
            }
            else {
                for (int i = pot.Count-2; i >= 0; i--) {
                    if (pot[i].val != 7) {
                        topVal = pot[i].val;
                        return topVal;
                    }
                }
                return 0;
            }
        }
        public static int playSelection(int numCards, int min = 0) {
            string response = Console.ReadLine();
            int choice;
            //if invalid choice
            if(!int.TryParse(response, out choice) || choice >= numCards || choice < min) {
                System.Console.WriteLine("I'm sorry but that wasn't a recognizable choice. Please try again.");
                return playSelection(numCards);
            }
            return choice;
        }//end playSelection(2)
        public static int playSelection(int numCards, int topVal, List<Card> playerCards) {
            int choice = playSelection(numCards);
            if (playerCards[choice].val == 10 || playerCards[choice].val == 3 || playerCards[choice].val == 7 ){
                return choice;
            }
            else if (playerCards[choice].val < topVal) {
                System.Console.WriteLine("Uh, no. Try again.");
                return playSelection(numCards, topVal, playerCards);
            }
            else {
                return choice;
            }
        }//end playSelection(3)
        public static bool isPlayable(List<Card> cards, int topVal, bool is_castle)
        {
            bool playable = false;
            int cardIndex = 0;
            if (is_castle && cards.Count > 3) 
            {
                cards = cards.GetRange(3,cards.Count-3);
                cardIndex = 3;
            }
            foreach (Card c in cards) {
                    Console.WriteLine(cardIndex + " - " + c.ToString() + " ");
                    cardIndex++;
                    if(c.val >= topVal || c.val == 3 || c.val == 7 || c.val == 10 )
                    {
                        playable = true;
                    }
                }
            return playable;
        }//end isPlayable()
        public static bool playFromHand(Player p, int topVal, List<Card> pot) {
            Console.WriteLine("Which card would you like to play?");
            bool skip = false;
            int r = playSelection(p.hand.Count, topVal, p.hand);
            //put this in an else
            Card playedCard = p.hand[r];
            p.discard(r);
            skip = playCard(playedCard, pot);
            return skip;
        }//end playFromHand()
        public static bool playCard(Card playedCard, List<Card> pot) {
            bool skip = false;
            if (playedCard.val == 3) { //next player takes the pot and their turn is skipped
                return true;
            }
            else if(playedCard.val == 10) { //whole pot is removed from play
                pot.Add(playedCard);
                pot.Clear();
                Console.WriteLine("10's clear the pot!");
            }
            else { //includes 7's
                pot.Add(playedCard);
            }
            return skip;
        }//end playCard()
        public static void drawCard(Player p, Deck deck) {
            //redraw up to 3
            while (p.hand.Count < 3) {
                if (deck.cards.Count < 1) {
                    break;
                }
                p.draw(deck);
            }
        }
        public static void takePot(List<Card> pot, Player p) {
            foreach(Card c in pot) {
                p.hand.Add(c);
            }
            p.sortHand();
            pot.Clear();
        }//end takePot()
        public static bool playVisibleCastle(Player p, int topVal, List<Card> pot) {
            bool skip = false;
            bool vCastlePlayable = isPlayable(p.castle, topVal, true);
            if (vCastlePlayable) {
                Console.WriteLine("Which castle card would you like to play?");
                int r = playSelection(p.castle.Count,3);
                Card playedCard = p.castle[r];
                skip = playCard(playedCard, pot);
                p.castle.Remove(playedCard);
            }
            else { //castle is not playable
                System.Console.WriteLine("You can't play from your castle. Have all the cards!");
                takePot(pot, p);
            }
            return skip;
        }//end playVisibleCastle()
        public static bool playInvisibleCastle(Player p, int topVal, List<Card> pot) {
            //fix playing special cards from hand, discarding
            bool skip = false;
            Console.WriteLine($"Pick a card from your hidden castle....ooOooOooOOh. Pick from 0 to {p.castle.Count-1}");
            int r = playSelection(p.castle.Count);
            List<Card> invisibleSelection = new List<Card>(){p.castle[r]};
            if (isPlayable(invisibleSelection,topVal,true)) {
                skip = playCard(p.castle[r], pot);
                Console.WriteLine($"You played a {p.castle[r].ToString()} with your invisible hand.");
                p.castle.RemoveAt(r);
            }
            else {
                Console.WriteLine($"You tried to play a {p.castle[r].ToString()} with your invisible hand.");
                System.Console.WriteLine("Oops! That card didn't work.");
                p.hand.Add(p.castle[r]);
                p.castle.RemoveAt(r);
                takePot(pot, p);
            }
            return skip;
        }//end playInvisibleCastle()
    }//end Turn

}