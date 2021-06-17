using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pillow : MonoBehaviour
{
    [SerializeField] PillowPart[] bonesOfPillow = null;
    public string enemyTag { get; set; } = "";
    public Action<bool> OnPillowAttacking;
    public Action OnPillowDroped;
    public Action OnPillowThrown;
    public bool isTrowable = false;

    public void SetPillowSettings(string tagEnemy)
    {
        enemyTag = tagEnemy;
        if (enemyTag.Equals(""))
        {
            Debug.LogError("ATENCION EL TAG NO QUEDO");
        }
        for (int i = 0; i < bonesOfPillow.Length; i++)
        {
            bonesOfPillow[i].SetPillowManager(this, enemyTag,isTrowable);
        }
    }

    public void DropPillow()
    {
        PillowAttacking(false);
        OnPillowDroped?.Invoke();
        transform.SetParent(null);
        Destroy(gameObject,5f);
    }

    public void ThrowPillow()
    {
        PillowAttacking(true);
        OnPillowThrown?.Invoke();
        transform.SetParent(null);
    }

    public void PillowAttacking(bool canAttack)
    {
        OnPillowAttacking?.Invoke(canAttack);
    }
}
