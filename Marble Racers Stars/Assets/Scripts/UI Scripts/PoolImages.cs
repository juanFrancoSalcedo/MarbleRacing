using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PoolImages : Singleton<PoolImages>
{
    public GameObject imagePrefab = null;
    List<GameObject> listImages = new List<GameObject>();
    public void PushImage(Sprite spriteNew)
    {
        bool inPool = false;

        if (listImages.Count == 0)
            CreateObject(spriteNew);
        else
        {
            foreach (GameObject image in listImages)
            {
                if (!image.gameObject.activeInHierarchy)
                {
                    ActiveImage(image, spriteNew);
                    inPool = true;
                    break;
                }
            }
        }
        if (!inPool)
            CreateObject(spriteNew);
    }

    public void PushImage(Sprite spriteNew,Vector2 scale, Vector2 position)
    {
        bool inPool = false;

        if (listImages.Count == 0)
            CreateObject(spriteNew,scale,position);
        else
        {
            foreach (GameObject image in listImages)
            {
                if (!image.gameObject.activeInHierarchy)
                {
                    ActiveImage(image, spriteNew, scale, position);
                    inPool = true;
                    break;
                }
            }
        }

        if (!inPool)
            CreateObject(spriteNew, scale, position);
    }

    private void CreateObject(Sprite spriteNew) 
    {
        GameObject imageLanding = Instantiate(imagePrefab, transform);
        listImages.Add(imageLanding);
        ActiveImage(imageLanding, spriteNew);
    }
    private void CreateObject(Sprite spriteNew, Vector2 scale, Vector2 position)
    {
        GameObject imageLanding = Instantiate(imagePrefab, transform);
        listImages.Add(imageLanding);
        ActiveImage(imageLanding, spriteNew,scale,position);
    }
    private void ActiveImage(GameObject imageObj,Sprite spriteImage)
    {
        imageObj.gameObject.SetActive(true);
        imageObj.GetComponent<Image>().sprite = spriteImage;
    }
    private void ActiveImage(GameObject imageObj, Sprite spriteImage, Vector2 scale, Vector2 position)
    {
        imageObj.transform.localScale = scale;
        imageObj.GetComponent<RectTransform>().localPosition = position;
        imageObj.gameObject.SetActive(true);
        imageObj.GetComponent<Image>().sprite = spriteImage;
    }
}
