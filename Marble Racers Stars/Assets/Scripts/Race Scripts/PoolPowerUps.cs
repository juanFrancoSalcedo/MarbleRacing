using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using MyBox;

public class PoolPowerUps : Singleton<PoolPowerUps>
{
    [SerializeField] private PowerUpPrefabs[] powObjs = null;
    private List<GameObject> insideObjs = new List<GameObject>();
    public Material materialZombie = null;
    public GameObject brokenMarble = null;
    public GameObject materialDirty = null;
    public void CreatePow(Vector3 posObj, Quaternion rotObj, PowerUpType powType)
    {
        bool inPool = false; 

        foreach (var item in insideObjs)
        {
            if (item.name == powType.ToString() && !item.activeInHierarchy) 
            {
                item.transform.position = posObj;
                item.transform.rotation = rotObj;
                item.SetActive(true);
                inPool = true;
                break;
            }
        }

        if (inPool) { return; }

        foreach (var item in powObjs)
        {
            if (item.typePowPref == powType)
            {
                GameObject pass = Instantiate(item.prefab,posObj,rotObj, transform);
                pass.name = powType.ToString();
                insideObjs.Add(pass);
            }
        }
    }

    public GameObject CreatePow(PowerUpType powType)
    {
        bool inPool = false;
        GameObject pass = null;

        foreach (var item in insideObjs)
        {
            if (item.name == powType.ToString() && !item.activeInHierarchy)
            {
                pass = item;
                inPool = true;
                break;
            }
        }

        if (!inPool)
        {
            foreach (var item in powObjs)
            {
                if (item.typePowPref == powType)
                {
                    pass = Instantiate(item.prefab, transform);
                    pass.name = powType.ToString();
                    insideObjs.Add(pass);
                }
            }
        }
        return pass;
    }
}

[System.Serializable]
public struct PowerUpPrefabs 
{
    public GameObject prefab;
    public PowerUpType typePowPref;
}


