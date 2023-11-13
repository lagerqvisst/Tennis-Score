using System;
namespace TennisScore
{
    public class TennisGame
    {
        //Denna klass är hjärtat i applikationen och hanterar all spellogik.
        //Utifrån de utfall som spellogiken kan ge, samspelar vi med Player-klassen för att reglera poäng.

        //En match består av två tennisspelare som måste deklareras genom constructorn.
        private Player Player1;
        private Player Player2;

        //Dessa boolska värden används som triggers, ex. avslutas matchen när LiveGame blir false.
        public bool LiveGame;
        public bool Tiebreak;

        //Estetiskt tillägg som tar in textsträngar med information när ett set avslutas.
        //När matchen är över skrivs en sammanfattning ut av resultatet i varje gem samt vem som vann matchen.
        public List<string> GameSummary = new List<string>();


        public TennisGame(Player p1, Player p2)
        {
            Player1 = p1;
            Player2 = p2;
            LiveGame = true;
            Tiebreak = false;
        }

        public void StartGameInfo()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Game starting...");
            Console.WriteLine("Best of 3 sets, whomever reaches two sets first wins.");
            Console.WriteLine("Tiebreak is activated at the score of 6-6 in games in a set.\n");
            Console.ResetColor();
        }

        public void GivePlayerScore()
        {
            int choice;

            //Som användare har man inte mycket till val, man ger antingen poäng till spelare 1 eller 2.
            //Felhantering: upprepa tills användaren lyckas mata in 1 eller 2.
            do
            {
                Console.WriteLine($"Press (1) to give {Player1.playerName} a point.\nPress (2) to give {Player2.playerName} a point.");

                if (!int.TryParse(Console.ReadLine(), out choice) || (choice != 1 && choice != 2))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid input. Enter either 1 or 2 to assign a players score");
                    Console.ResetColor();
                }

            } while (choice != 1 && choice != 2);

            if (choice == 1)
            {
                Player1.addPoint();
            }
            else if (choice == 2)
            {
                Player2.addPoint();
            }
        }


        public void InvokeGameLogic()
        {
            //Hantera att ge gem-poäng, måste vinna gemet med minst 2 poäng.
            //Ett gem kan bli långdraget när man hamnar i 40-40 läget och de underliggande värdena i poäng kan bli höga...
            //...rent logisk bryr vi oss endast om att en spelare ska vinna med två poäng.
            //Se metoden DisplayScore hur vi hanterar detta för användaren.
            if (Player1.playerPoint >= 4 && Player1.playerPoint - Player2.playerPoint >= 2)
            {
                Player1.addGame();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Player1.playerName} won the game");
                Console.ResetColor();

                ResetBothPlayerPoints();
            }
            else if (Player2.playerPoint >= 4 && Player2.playerPoint - Player1.playerPoint >= 2)
            {
                Player2.addGame();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Player2.playerName} won the game");
                Console.ResetColor();

                ResetBothPlayerPoints();
            }

            //Hantera tiebreak, skapade enskild metod. Anropas när det står 6-6 i gem i ett set.
            //Frångår 15,30,40 semantiken i poängställningen.
            //Se dokumentation vid anrop för vidare info.
            if (Player1.playerGame == 6 && Player2.playerGame == 6)
            {
                TieBreakLogic();
            }

            //Hantera att ge set-poäng, följer samma logik som att vinna ett gem, måste vinna med 2.
            //Först till 6 poäng med marginal på 2, kan även bli 7-5. Annars triggas tiebreak ovan.
            if (Player1.playerGame >= 6 && Player1.playerGame - Player2.playerGame >= 2)
            {
                Player1.addSet();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Player1.playerName} won the set");
                Console.ResetColor();

                GameSummary.Add($"{Player1.playerName} won with a score of {Player1.playerGame}-{Player2.playerGame}");
                ResetBothPlayerGames();
            }
            else if (Player2.playerGame >= 6 && Player2.playerGame - Player1.playerGame >= 2)
            {
                Player2.addSet();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"{Player2.playerName} won the set");
                Console.ResetColor();

                GameSummary.Add($"{Player2.playerName} won with a score of {Player2.playerGame}-{Player1.playerGame}");
                ResetBothPlayerGames();
            }
        }


        public void DisplayScore()
        {
            //Visa ställning för gem & set
            Console.WriteLine("-------------------------------------");
            Console.WriteLine($"SET     | {Player1.playerName} {Player1.playerSet}  -  {Player2.playerSet}  {Player2.playerName}");
            Console.WriteLine($"GAME(s) | {Player1.playerName} {Player1.playerGame}  -  {Player2.playerGame}  {Player2.playerName}");
            Console.WriteLine("-------------------------------------\n");



            // Hantera semantiken när det blir 40-40 i ett gem.
            //Som nämnt kan ett gem bli långdraget och bryr vi oss inte att de underliggande värdena egentligen kan vara 10-10...
            //...När vi når 40-40 behöver vi bara ta hänsyn till vad differensen mellan de underliggande värdena är och detta dikterar vad vi visar användaren.
            if (Player1.playerPoint >= 3 && Player2.playerPoint >= 3)
            {
                if (Player1.playerPoint == Player2.playerPoint)
                {
                    Console.WriteLine($"DEUCE");
                }
                else if (Player1.playerPoint - Player2.playerPoint == 1)
                {
                    Console.WriteLine($"ADVANTAGE: {Player1.playerName}");
                }
                else if (Player2.playerPoint - Player1.playerPoint == 1)
                {
                    Console.WriteLine($"ADVANTAGE: {Player2.playerName}");
                }
            }
            else
            {
                //När vi inte är i ett 40-40 läge visar vi bara den omvandlade poängställningen, ex. 1-0 är 15-LOVE. 
                Console.WriteLine($"Current game:\n{Player1.playerName}: {Player1.ConvertedPoints} | {Player2.ConvertedPoints}: {Player2.playerName}\n");
            }

        }

        public void TieBreakLogic()
        {
            //Frångår semantiken 15,30,40, deuce etc. Försten till 7 poäng med skilje vinner setet.
            Tiebreak = true;
            ResetBothPlayerPoints();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"TIEBREAK STARTING...");
            Console.ResetColor();

            //När en spelare vinner tiebreaket, blir tiebreak false och därmed bryter vi ur loopen.
            while (Tiebreak)
            {
                //Notera att vi inte visar ConvertedPoints, utan det faktiska poängen, så ska det vara enligt tiebreak.
                Console.WriteLine($"Tiebreak Score:\n{Player1.playerName} ({Player1.playerPoint}) | ({Player2.playerPoint}) {Player2.playerName}");
                GivePlayerScore();

                //Precis som annan poänglogik i tennis tillämpar vi samma här med att man vinner med 2 i marginal.
                //Annars fortsätter vi, dvs. Tiebreak = true.
                if (Player1.playerPoint >= 7 && Player1.playerPoint - Player2.playerPoint >= 2)
                {
                    Player1.addGame();
                    Player1.addSet();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{Player1.playerName} won the tiebreak and set");
                    Console.ResetColor();

                    GameSummary.Add($"{Player1.playerName} won with a score of {Player1.playerGame}-{Player2.playerGame}");
                    ResetBothPlayerGames();
                    Tiebreak = false;
                }
                else if (Player2.playerPoint >= 7 && Player2.playerPoint - Player1.playerPoint >= 2)
                {
                    Player2.addGame();
                    Player2.addSet();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{Player2.playerName} won the tiebreak and set");
                    Console.ResetColor();

                    GameSummary.Add($"{Player2.playerName} won with a score of {Player2.playerGame}-{Player1.playerGame}");
                    ResetBothPlayerGames();
                    Tiebreak = false;
                }
            }
        }

        public void ResetBothPlayerPoints() // När ett gem avslutas nollställer vi poängen till 0.
        {
            Player1.ResetPoints();
            Player2.ResetPoints();
        }
        public void ResetBothPlayerGames() // När ett set avslutas nollställer vi både gem och gempoäng.
        {
            Player1.ResetGames();
            Player2.ResetGames();
            ResetBothPlayerPoints();
        }

        public void CheckMatchStatus()
        {
            //Matchen är konfigruerad med ett BO3-system, dvs. bäst av 3 set, så den spelare som först får 2 set vinner.
            //När ett matchobjekt instansieras är LiveGame alltid true.
            if (Player1.playerSet == 2 || Player2.playerSet == 2)
            {
                LiveGame = false;
                Console.WriteLine("Game concluded...\n");
            }


        }
        public void RunGameSummary()
        {
            Console.WriteLine("Match summary:");

            for (int i = 0; i < GameSummary.Count; i++)
            {
                Console.WriteLine($"Set {i + 1}: {GameSummary[i]}");
            }

            if (Player1.playerSet == 2)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Congratulations {Player1.playerName}");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Congratulations {Player2.playerName}");
                Console.ResetColor();
            }

            Console.ReadLine();

        }



    }
}

