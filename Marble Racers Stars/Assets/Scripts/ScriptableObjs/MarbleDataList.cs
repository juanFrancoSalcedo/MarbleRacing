using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewListMarbles", menuName = "Inventory/ListMarbles")]
[System.Serializable]
public class MarbleDataList : ScriptableObject
{
    [SerializeField] ListWrapper listDataMarbles;
    string tagInit = "(M)";
    public string tagInitItem { get; set; } = "(Item)";

    public MarbleData GetSpecificMarble(string nameMarbleData)
    {
        if (!nameMarbleData[0].Equals('(')) nameMarbleData = tagInit + nameMarbleData;
        
        if (!listDataMarbles.marblesDataList.Contains(nameMarbleData)) nameMarbleData += tagInitItem;
        MarbleData newMar = Resources.Load<MarbleData>("MarblesInfo/" + nameMarbleData);
        //Debug.Log(nameMarbleData+" "+newMar.name);
        return newMar;
    }

    public bool CheckIndexMarbleIsItem(int indexMarble) => listDataMarbles.marblesDataList[indexMarble].Contains(tagInitItem);
    public MarbleData GetSpecificMarble(int indexMarbleInList)
    {
        indexMarbleInList = Mathf.Clamp(indexMarbleInList,0,listDataMarbles.marblesDataList.Count);
        MarbleData asset = Resources.Load<MarbleData>("MarblesInfo/" + listDataMarbles.marblesDataList[indexMarbleInList]);
        return asset;
    }
    public int GetLengthList()=> listDataMarbles.marblesDataList.Count;

    public int GetIndexOfSpecificName(string nameMarble) 
    {
        if (!nameMarble[0].Equals('(')) nameMarble = tagInit + nameMarble;
        if (!listDataMarbles.marblesDataList.Contains(nameMarble)) nameMarble += tagInitItem;
        return listDataMarbles.marblesDataList.IndexOf(nameMarble);
    }

    public string GetNameSpecificIndex(int indexMarble)
    {
        return listDataMarbles.marblesDataList[indexMarble];
    }

    public void PrintInIndex(int indexcool) 
    {
        //Debug.Log(marblesDataList[indexcool]);
    }

    [System.Serializable]
    public class ListWrapper 
    {
        public List<string> marblesDataList = new List<string>();
    }
}