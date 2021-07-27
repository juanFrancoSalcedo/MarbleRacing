using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.SceneManagement;
using UnityEditor;

public class DataController : Singleton<DataController>
{
    public MarbleDataList allMarbles;
    private Cups _cups = null;
    public Cups allCups
    {
        get {
            if (_cups == null)
            {
                _cups = Resources.Load<Cups>("cups");
            }
            return _cups;
        }
        set {
            _cups = value;
        }
    }
    public int GetSpecificKeyInt(string keyStorage)
    {
        return PlayerPrefs.GetInt(keyStorage,0);
    }

    public string GetSpecificKeyString(string keyStorage)
    {
        return PlayerPrefs.GetString(keyStorage);
    }
    public void SetSpecificKeyInt(string keyStorage,int _value)
    {
        PlayerPrefs.SetInt(keyStorage, _value);
    }

    public int GetCupsWon()
    {
        int wonsCups = PlayerPrefs.GetInt(KeyStorage.CUPSWON_I,-1);
        return wonsCups;
    }

    public void WinCup()
    {
        int cupsWin = PlayerPrefs.GetInt(KeyStorage.CUPSWON_I,-1);
        if (cupsWin == PlayerPrefs.GetInt(KeyStorage.CUPSWON_I, -1))
        {
            cupsWin++;
            PlayerPrefs.SetInt(KeyStorage.CUPSWON_I, cupsWin);
        }
    }

    public void UnlockCup()
    {
        int cupsUn = PlayerPrefs.GetInt(KeyStorage.CUPSUNLOCKED_I, 0);
        if (cupsUn == PlayerPrefs.GetInt(KeyStorage.CURRENTCUP_I, 0))
        {
            cupsUn++;
            PlayerPrefs.SetInt(KeyStorage.CUPSUNLOCKED_I, cupsUn);
        }
    }

    public void WinTrophy(int trophy)
    {
        PlayerPrefs.SetInt(KeyStorage.TROPHYS_I, PlayerPrefs.GetInt(KeyStorage.TROPHYS_I,0) + trophy);
    }

    public int GetTrophys()
    {
        return PlayerPrefs.GetInt(KeyStorage.TROPHYS_I,0);
    }

    public void DepositMoney(int moneyPlus)
    {
        PlayerPrefs.SetInt(KeyStorage.MONEY_I, PlayerPrefs.GetInt(KeyStorage.MONEY_I, 0) + moneyPlus);
    }

    public int GetMoney()
    {
        return PlayerPrefs.GetInt(KeyStorage.MONEY_I,0);
    }

    public void IncreaseMarblePercentage(int addPercent)
    {
        PlayerPrefs.SetInt(KeyStorage.MARBLEPERCENTAGE_I,PlayerPrefs.GetInt(KeyStorage.MARBLEPERCENTAGE_I,0) + addPercent);
    }

    public int GetMarblePercentage()
    {
        return PlayerPrefs.GetInt(KeyStorage.MARBLEPERCENTAGE_I,0);
    }

    public void IncreaseItemUnlocked()
    {
        int per = PlayerPrefs.GetInt(KeyStorage.ITEMS_UNLOCKED_I,0);
        if (per <allMarbles.GetLengthList()-1)
            per++;
        PlayerPrefs.SetInt(KeyStorage.ITEMS_UNLOCKED_I,per);
    }

    public int GetItemUnlockedCount()
    {
        return PlayerPrefs.GetInt(KeyStorage.ITEMS_UNLOCKED_I,0);
    }
    public int GetNextUnlockedItem()
    {
        FillSeed();
        if (Wrapper<UniqueList.UniqueListWrapper>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.SEED_ITEMS_S))
            .listaInt.Count+1 == allMarbles.GetLengthList())
        {
            return Wrapper<UniqueList.UniqueListWrapper>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.SEED_ITEMS_S))
                .listaInt[1];
        }
        else
        {
            return Wrapper<UniqueList.UniqueListWrapper>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.SEED_ITEMS_S))
                .listaInt[PlayerPrefs.GetInt(KeyStorage.ITEMS_UNLOCKED_I)+1];
        }
    }

    public MarbleData GetItemByIndex(int indexItem) 
    {
        FillSeed();
        MarbleData data = ScriptableObject.CreateInstance<MarbleData>();
        indexItem = Mathf.Clamp(indexItem,0,allMarbles.GetLengthList());
        int realIndex = Wrapper<UniqueList.UniqueListWrapper>.FromJsonsimple(PlayerPrefs.GetString(KeyStorage.SEED_ITEMS_S)).listaInt[indexItem];
        data = allMarbles.GetSpecificMarble(realIndex);
        return data;
    }
    private void FillSeed() 
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.SEED_ITEMS_S)))
        {
            UniqueList.UniqueListWrapper listNumbers = new UniqueList.UniqueListWrapper();
            //listNumbers.listaInt.Add(0);
            listNumbers.listaInt = UniqueList.CreateRandomListWithoutRepeating(0, allMarbles.GetLengthList(), allMarbles.GetLengthList());
            int indexZero = listNumbers.listaInt.IndexOf(0);
            int valueSwitch = listNumbers.listaInt[0];

            listNumbers.listaInt[0] = 0;
            listNumbers.listaInt[indexZero] = valueSwitch;
            PlayerPrefs.SetString(KeyStorage.SEED_ITEMS_S, Wrapper<UniqueList.UniqueListWrapper>.ToJsonSimple(listNumbers));
        }
    }

    public MarbleData GetCustom() 
    {
        MarbleData data = ScriptableObject.CreateInstance<MarbleData>();
        data = allMarbles.GetSpecificMarble(PlayerPrefs.GetInt(KeyStorage.CUSTOM_MAT_I,0));
        data.objectInside = allMarbles.GetSpecificMarble(PlayerPrefs.GetInt(KeyStorage.CUSTOM_OBJ_INSIDE_I,0)).objectInside;
        data.ObjectSecond = allMarbles.GetSpecificMarble(PlayerPrefs.GetInt(KeyStorage.CUSTOM_TRAIL_I,0)).objectInside;
        return data;
    }

    [ButtonMethod]
    public void EraseAll()
    {
        PlayerPrefs.DeleteAll();
    }

#if UNITY_EDITOR
    [MenuItem("Tools/DataManager/Delete All")]
#endif
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
        System.IO.File.Delete(Application.persistentDataPath + "/Pilotos.json");
    }

#if UNITY_EDITOR
    [MenuItem("Tools/DataManager/700 Money")]
#endif
    static void Money()
    {
        PlayerPrefs.SetInt(KeyStorage.MONEY_I,700);
        MoneyManager.UpdateMoney();
    }

#if UNITY_EDITOR
    [MenuItem("Tools/DataManager/Get 85 Items")]
#endif
    static void UnlockAllItems()
    {
        for (int i = 0; i < 85; i++)
            FindObjectOfType<DataController>().IncreaseItemUnlocked();
    }

    public void EraseAllAndLoad()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [ButtonMethod]
    public void EraseLeague() 
    {
        LeagueManager.LeagueRunning = null;
        LeagueManager.LeagueManufacturers = null;
        PlayerPrefs.DeleteKey(KeyStorage.LEAGUE_S);
        PlayerPrefs.DeleteKey(KeyStorage.LEAGUE_MANUFACTURERS_S);
    }

    [ButtonMethod]
    public void EraseMarblePercentage()
    {
        PlayerPrefs.DeleteKey(KeyStorage.MARBLEPERCENTAGE_I);
    }
}

public static class Wrapper<T>
{
    public static string ToJsonSimple(T arg1)
    {
        return JsonUtility.ToJson(arg1);
    }

    public static T FromJsonsimple(string arg1)
    {
        return JsonUtility.FromJson<T>(arg1);
    }
}

public static class KeyStorage
{
 //   public static string FIRSTENTER_I = "FIRSTENTER";
    public static readonly string LEAGUE_S = "LEAGUEDATA";
    public static readonly string LEAGUE_MANUFACTURERS_S = "LEAGUE_MANUFACTURERS";
    public static readonly string MONEY_I = "MONEY";
    public static readonly string MARBLEPERCENTAGE_I = "MARBLEPERCENTAGE";
    public static readonly string SEED_ITEMS_S = "SEED_ITEMS";
    public static readonly string ITEMS_UNLOCKED_I = "ITEMS_UNLOCKED";
    public static readonly string TROPHYS_I = "TROPHYS";
    public static readonly string CURRENTMARBLESELECTED_I = "CURRENTMARBLESELECTED";
    public static readonly string CUPSUNLOCKED_I = "CUPSUNLOCKED";
    public static readonly string CUPSWON_I = "CUPSWON";
    public static readonly string CURRENTCUP_I = "CURRENTCUP";
    public static readonly string NAME_PLAYER = "NAME_PLAYER";
    public static readonly string ABBREVIATION_PLAYER = "NAME_PLAYER";
    public static readonly string GRAPHICS_SETTING_S = "GRAPHICS_SETTING";
    public static readonly string SOUND_SETTING_I = "SOUND_SETTING";
    public static readonly string GIFT_CLAIMED_I = "GIFT_CLAIMED";
    public static readonly string SECOND_PILOT_UNLOCKED_I = "SECOND_PILOT_UNLOCKED";
    //-------customizing
    public static readonly string CUSTOM_OBJ_INSIDE_I = "CUSTOM_OBJ_INSIDE";
    public static readonly string CUSTOM_MAT_I = "CUSTOM_MAT";
    public static readonly string CUSTOM_TRAIL_I = "CUSTOM_TRAIL";
    //------- Tutos
    public static readonly string TUTO_COVERING_I = "TUTO_COVERING";
}
