using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using MyBox;

public class MarbleSelector : Singleton<MarbleSelector>
{
    public MarbleDataList allMarbles = null;
    [SerializeField] DataController dataManager = null;
    [SerializeField] List<Button> buttonsSelction = new List<Button>();
    [SerializeField] private Animator animatorTraffic = null;
    RectTransform rectTrans = null;
    public System.Action<int>  OnSelectedButton;

    void Start()
    {
        rectTrans = GetComponent<RectTransform>();
        buttonsSelction[0].gameObject.SetActive(true);
    }

    public async Task<bool> InstanciateAllItems() 
    {
        List<Task<bool>> tasks = new List<Task<bool>>();
        for (int i = 1; i < dataManager.GetItemUnlockedCount()+1; i++)
            tasks.Add(CreateItem());
        await Task.WhenAll(CreateItem());
        return true;
    }

    private async Task<bool> CreateItem() 
    {
        Button but = Instantiate(buttonsSelction[0], buttonsSelction[0].transform.parent);
        buttonsSelction.Add(but);
        but.gameObject.SetActive(true);
        await Task.Yield();
        return true;
    }

    public void OpenClose()
    {
        if (rectTrans.localScale.x == 0)
        {
            rectTrans.DOScaleX(1, 0.4f).OnComplete(EnableVisualComponents).SetEase(Ease.OutQuart);
            animatorTraffic.SetBool("Customizing",true);
        }
        else
        {
            rectTrans.DOScaleX(0, 0.4f).OnComplete(DisableVisualComponents).SetEase(Ease.OutQuart);
            animatorTraffic.SetBool("Customizing", false);
        }
    }

    void EnableVisualComponents()
    {
        GetComponent<Image>().enabled = true;
        GetComponent<ScrollRect>().enabled = true;
    }

    void DisableVisualComponents()
    {
        GetComponent<Image>().enabled = false;
        GetComponent<ScrollRect>().enabled = false;
    }

    public void SelectMarble(int indexInAll, int siblingIndex)
    {
        OnSelectedButton?.Invoke(siblingIndex);
        RaceController.Instance.marblePlayerInScene.SetMarbleSettings(indexInAll);
    }
}
