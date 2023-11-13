using System;
namespace TennisScore
{
    public class Player
    {
        //Klassen tillhandahåller attribut för en tennisspelare.
        //Det är tennisspelaren som har funktionaliteten för att ge sig själv poäng.
        //Det är dock klassen TennisMatch som styr logiken kring hur poängen fördelas.
        //Logiken för att hantera poängsemantiken lagras även här.

        public string playerName;
        public int playerPoint = 0; //Poäng i ett gem
        public int playerGame = 0; //Gempoäng, heter game på engelska.
        public int playerSet = 0; //Setpoäng
        public string ConvertedPoints;

        public Player(string playername)
        {
            //Constructorn används i mainmetoden och tar in användarinput för att sätta namnet.
            playerName = playername;
            ConvertedPoints = ConvertScore(playerPoint);

        }

        public void addPoint()
        {
            playerPoint++;
            ConvertedPoints = ConvertScore(playerPoint);
        }
        public void addGame()
        {
            playerGame++;

        }
        public void addSet()
        {
            playerSet++;
        }
        public void ResetPoints()
        {
            playerPoint = 0;
            ConvertedPoints = ConvertScore(playerPoint);
        }
        public void ResetGames()
        {
            playerGame = 0;
        }
        //Logiken för att hantera tennisens poängsemantik.
        public string ConvertScore(int points)
        {
            switch (points)
            {
                case 0:
                    return "Love (0)";
                case 1:
                    return "15";
                case 2:
                    return "30";
                case 3:
                    return "40";
                default:
                    return "0";
            }


        }


    }
}

