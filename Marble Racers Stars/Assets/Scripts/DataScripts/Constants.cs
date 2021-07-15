using UnityEngine;
using System.Threading;
public static class Constants 
{
    public static int[] pointsPerRacePosition= {20,14,10,8,6,5,4,3,2,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
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

    // Stats---------------------------
    public const float timeBigSize = 7f;
    public const float timeAceleration = 7f;
    public const float timeDriving = 3.2f;
    public static readonly float frictionBase = 0.00012f;
    public static readonly int baseHp = 100;
    public static readonly float timeReduceAcelerationBase = 0.5f;
    public static readonly float timeReduceDrivingBase = 0.28f;
    public static readonly float forceBaseForward = 0.21f;
    public static readonly float forceBaseDriving= 0.19f;
    public static readonly int maxForceTurbo = 6;
    public static readonly int maxForceDirection = 4;
    public static readonly int fractionStats = 9;
    public static readonly int baseMoney = 90;

    public static string ReplaceNameNormi(DataController dataManager) 
    {
        string key = KeyStorage.NAME_PLAYER;
        return (PlayerPrefs.HasKey(key)) ? dataManager.GetSpecificKeyString(key): NORMI;
    }

    public static string AbbrevetionNameNormi(DataController dataManager)
    {
        string abbr = ReplaceNameNormi(dataManager).ToUpper();
        abbr = (abbr.Length > 2)? abbr.Substring(0,3):"NOR";
        return abbr;
    }
}

