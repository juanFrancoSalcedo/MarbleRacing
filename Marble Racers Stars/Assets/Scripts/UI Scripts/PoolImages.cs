using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class PoolImages : Singleton<PoolImages>
{
    [SerializeField] private GameObject imagePrefab;
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
                    PlaySoundOfShot(image, spriteNew);
                    inPool = true;
                    break;
                }
            }
        }

        if (!inPool)
            CreateObject(spriteNew);
    }

    private void CreateObject(Sprite spriteNew) 
    {
        GameObject imageLanding = Instantiate(imagePrefab, transform);
        listImages.Add(imageLanding);
        PlaySoundOfShot(imageLanding, spriteNew);
    }

    private void PlaySoundOfShot(GameObject imageObj,Sprite spriteImage)
    {
        imageObj.gameObject.SetActive(true);
        imageObj.GetComponent<Image>().sprite = spriteImage;
    }
}
