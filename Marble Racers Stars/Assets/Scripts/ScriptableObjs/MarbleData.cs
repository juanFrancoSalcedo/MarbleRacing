using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMarbleInfo", menuName = "Inventory/MarbleInfo")]
[System.Serializable]
public class MarbleData : ScriptableObject
{
    public string nameMarble = null;
    public Material mat = null;
    public Color color1 = Color.white;
    public Color color2 = Color.white;
    public GameObject objectInside = null;
    private GameObject objectSecond = null;
    public GameObject ObjectSecond { get { return objectSecond; } set { objectInside = value; } }
    public Sprite spriteMarbl;
    public string abbreviation;

    private void OnValidate()
    {
        if (abbreviation.Length > 0)
            abbreviation = abbreviation.ToUpper();
        if (abbreviation.Length > 3)
            abbreviation = abbreviation.Substring(0,3);
    }
}