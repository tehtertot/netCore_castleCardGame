using System;
using System.Collections.Generic;

namespace castle
{
    public class Game {
        public List<Player> players { get; set; }
        public Deck deck { get; set; }
        public List<Card> thePot { get; set; }

        public Game() {
            deck = new Deck();
            deck.shuffle();
            players = initiatePlayers();
            initializeHands();
            thePot = new List<Card>();
            startPlay();
        }

        private List<Player> initiatePlayers() {
            Console.Write("How many players? (2-4) >>");
            string response = Console.ReadLine();
            int numPlayers;
            List<Player> players = new List<Player>();
            if(!int.TryParse(response, out numPlayers) || numPlayers > 4 || numPlayers < 2) {
                System.Console.WriteLine("Invalid entry.");
                return initiatePlayers();
            }
            else {
                for (int i = 0; i < numPlayers; i++) {
                    Console.Write($"Enter player {i+1}'s name: >>");
                    Player p = new Player(Console.ReadLine());
                    players.Add(p);
                }
            }
            Console.Clear();
            return players;
        } //end initiatePlayers()

        private void initializeHands() {
            //deal hidden castle
            for (int i = 0; i < 3; i++) {
                foreach (Player p in players) {
                    p.dealHiddenCastle(deck);
                }
            }
            //deal 6 cards each
            for (int i = 0; i < 6; i++) {
                foreach (Player p in players) {
                    p.draw(deck);
                }
            }
            //each player chooses 3 to add to visible castle
            int t = 0;
            foreach (Player p in players) {
                Console.Clear();
                Console.WriteLine($"{p.name}, here are your cards:");
                for (int i = 0; i < 3; i++) {
                    int counter = 0;
                    foreach(Card c in p.hand) {
                        Console.WriteLine(counter + " - " + c.ToString() + " ");
                        counter++;
                    }
                    Console.WriteLine("Which card would you like to add to your castle? One at a time please!");
                    int response = Turn.playSelection(p.hand.Count);
                    p.castle.Add(p.discard(response));
                }
                Console.Clear();
                Console.WriteLine($"You've finished building your castle. Press enter and pass the computer to {players[(t + 1) % players.Count].name}.");
                Console.ReadLine();
                Console.WriteLine($"{players[(t + 1) % players.Count].name}, press enter when ready.");
                Console.ReadLine();
                t++;
            }
        }//end initializeHands()

        public void startPlay() {
            bool gameInPlay = true;
            bool skipNext = false;
            int playCount = 0;
            while (gameInPlay) {
                Player p = players[playCount % players.Count];
                skipNext = Turn.playTurn(p, thePot, deck, skipNext);
                if (p.hand.Count == 0 && p.castle.Count == 0) {
                    Splash.Finish();
                    gameInPlay = false;
                }
                else {
                    Console.Clear();
                    Console.WriteLine($"Pass the computer to {players[(playCount + 1) % players.Count].name}.");
                    Console.ReadLine();
                }
                playCount++;
            }
        }
        
    }
}