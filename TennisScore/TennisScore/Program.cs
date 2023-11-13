using System;

namespace TennisScore
{
    class Program
    {

        static void Main(string[] args)
        {
            //Programmet startar med att ta in två text-strängar från användaren som ska representera tennisspelarnas namn.
            Console.Write("Enter Player 1 name: ");
            Player player1 = new Player(Console.ReadLine());

            Console.Write("Enter Player 2 name: ");
            Player player2 = new Player(Console.ReadLine());

            //Instansierar ett matchobjekt genom att ange spelarna som ska delta.
            TennisGame TennisMatch = new TennisGame(player1, player2);

            //Resterande är funktionalitet från Tennismatchobjektet som dikterar logik och feedback tillbaka till användaren.
            TennisMatch.StartGameInfo();

            while (TennisMatch.LiveGame)
            {
                TennisMatch.DisplayScore();
                TennisMatch.GivePlayerScore();
                TennisMatch.InvokeGameLogic();
                TennisMatch.CheckMatchStatus();
            }

            TennisMatch.RunGameSummary();

        }



    }
}
