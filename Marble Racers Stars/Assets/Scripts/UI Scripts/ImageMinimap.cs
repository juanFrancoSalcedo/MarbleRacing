using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageMinimap : MonoBehaviour
{
    private Marble marbleTrans;
    public Marble MarbleTrans { 
        get { return marbleTrans; } 
        set 
        {   marbleTrans = value;
            //(marbleTrans.isPlayer) ? marbleTrans.bufferPlayer.spriteMarbl :
            //render.sprite =  marbleTrans.marbleInfo.spriteMarbl;
            //(marbleTrans.isPlayer) ? marbleTrans.bufferPlayer.color1 :
            //renderOutline.color =  marbleTrans.marbleInfo.color1;
        }
    }
    float defaultSize = 30;
    SpriteRenderer render => GetComponent<SpriteRenderer>();
    SpriteRenderer renderOutline => transform.GetChild(0).GetComponent<SpriteRenderer>();
    void OnEnable()
    {
        if (CameraMiniMap.Instance != null)
            CameraMiniMap.Instance.onChangedMiniMap += UpdateSize;
        else
            gameObject.SetActive(false);
        transform.SetParent(null);
    }

    private void OnDisable()
    {
        if (CameraMiniMap.Instance != null)
            CameraMiniMap.Instance.onChangedMiniMap -= UpdateSize;
    }

    private void UpdateSize() 
    {
        float sizeCamera = (float)CameraMiniMap.Instance?.cameraComponent.orthographicSize / defaultSize;
        transform.localScale = new Vector3(sizeCamera, sizeCamera, 1);
    }

    private void Update()
    {
        transform.position = marbleTrans.transform.position + new Vector3(0, 10, 0);
    }
}
