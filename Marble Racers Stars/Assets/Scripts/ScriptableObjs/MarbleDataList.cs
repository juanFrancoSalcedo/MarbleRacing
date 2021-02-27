using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewListMarbles",menuName = "Inventory/ListMarbles")]
[System.Serializable]
public class MarbleDataList : ScriptableObject
{
    [SerializeField] List<string> marblesDataList = new List<string>();
    string tagInit = "(M)";


    //TODO cambiar esto
    public MarbleData GetEspecificMarble(string nameMarbleData)
    {
        if (!nameMarbleData[0].Equals('(')) nameMarbleData = tagInit + nameMarbleData;

        MarbleData newMar = Resources.Load<MarbleData>("MarblesInfo/" + nameMarbleData);
        return newMar;
        //MarbleData marbleFinding = null;
        //foreach (MarbleData marble in marblesDataList)
        //{
        //    if (nameMarbleData.Equals(marble.nameMarble))
        //    {
        //        marbleFinding =  marble;
        //    }
        //}
        //return marbleFinding;
    }

    public MarbleData GetSpecificMarble(int indexMarbleInList)
    {
        MarbleData asset = Resources.Load<MarbleData>("MarblesInfo/" + marblesDataList[indexMarbleInList]);
        return asset;
    }
    public int GetLengthList()
    {
        return marblesDataList.Count;
    }

    public int GetIndexOfSpecificName(string nameMarble) 
    {
        if (!nameMarble[0].Equals('(')) nameMarble = tagInit + nameMarble;
        return marblesDataList.IndexOf(nameMarble);
    }

    public string GetNameSpecificIndex(int indexMarble)
    {
        return marblesDataList[indexMarble];
    }

    public void PrintInIndex(int indexcool) 
    {
        //Debug.Log(marblesDataList[indexcool]);
    }
}