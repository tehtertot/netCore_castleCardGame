using System;

namespace castle
{
    class Program
    {
        static void Main(string[] args)
        {
            Splash.Title();
            Console.WriteLine("Are you ready to play Castle?");
            Console.WriteLine("[i] Instructions\n[p] Play");
            string response = Console.ReadLine();
            if (response == "i") {
                //display instructions
            }
            else if (response == "p") {
                Game game = new Game();
            }
        }
    }
}
