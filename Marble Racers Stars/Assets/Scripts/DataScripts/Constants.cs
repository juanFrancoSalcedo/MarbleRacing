using UnityEngine;
public static class Constants 
{
    public static int[] pointsPerRacePosition= {21,16,12,9,6,4,2,1,0,0,0,0};
    public static string NORMI = "Normi";
    //public static int LimitDates=6;
    public static int moneyPerTrophy = 100;
    public static int debtBase = 10;
    public static string exploUpWord = "U";
    public static Color exploUpColor = Color.red;
    public static string freezeWord = "F";
    public static Color freezeColor = Color.cyan;
    public static string shrinkWord = "S";
    public static Color shrinkColor = Color.green;
    public static string enlargeWord = "E";
    public static Color enlargeColor = Color.yellow;
    public static string wallWord = "W";
    public static Color wallColor = new Color(164f/255f, 0,219f/255f);//Morado
    public static string bumpWord = "B";
    public static Color bumpColor = new Color(252f / 255f, 166f/255f, 82f / 255f);//Naranja
    public static string sceneAward = "Award";
    public static string sceneNewMarble = "NewMarble";
    public static string sceneCups = "UnlockCup";
    public static string sceneUnlockEndurance = "EnduranceRace";
    public const float timeBigSize = 7f;
    public const float timeAceleration = 6f;
    public const float timeDriving = 2f;
}

public interface IMainExpected
{
   void SubscribeToTheMainMenu();
    void ReadyToPlay();
}
