using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMarbleInfo", menuName = "Inventory/MarbleInfo")]
[System.Serializable]
public class MarbleData : ScriptableObject
{
    public string nameMarble;
    public Material mat;
    public Color color1;
    public Color color2;
    public GameObject objectInside;
    public Sprite spriteMarbl;
}