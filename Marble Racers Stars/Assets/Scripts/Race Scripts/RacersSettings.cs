using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LeagueSYS;

public class RacersSettings : MonoBehaviour
{
    private static RacersSettings Instance;
    public MarbleDataList allMarbles;
    public System.Func<MarbleDataList> OnListLoaded;
    public AudioClip roadClip;
    public AudioClip bigSizeRoadClip;
    // LAMENTO DECIRLE PERO ESTO NO SE PUEDE CAMBIAR POR QUE TENDRIAMOS PROBLEMAS CON GUARDAR LOS NOIOMBRES DE NORMI
    // RECUERDE PLAYER Y NORMI EN LA POSICION 4 [3]

    public static RacersSettings GetInstance()
    {
        if (Instance == null)
        {
            Instance = GameObject.FindObjectOfType<RacersSettings>();
        }
        else
        {
            RacersSettings[] raceSetti = GameObject.FindObjectsOfType<RacersSettings>();
            if (raceSetti.Length > 1)
            {
                Debug.LogError("THERE ARE TWO RACE SETTINGS");
            }
        }
        return Instance;
    }

    public MarbleDataList GetMarbleDataList()
    {
        return allMarbles;
    }

    //public MarbleData GetMarbleDataByName(string nameParticipant)
    //{
    //    MarbleData dtaMarble = null;

    //    foreach (var item in allMarbles.marblesDataList)
    //    {
    //        if (nameParticipant.Equals(item.nameMarble))
    //        {
    //            dtaMarble = item;
    //        }
    //    }
    //    return dtaMarble;
    //}
}
