﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdaterMarbleImage : MonoBehaviour
{
    Image spriteImageComp;

    IEnumerator Start()
    {
        spriteImageComp = GetComponent<Image>();

        while (spriteImageComp.sprite == null) 
        {
            spriteImageComp.sprite = RaceController.Instance.marblePlayerInScene.marbleInfo.spriteMarbl;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
