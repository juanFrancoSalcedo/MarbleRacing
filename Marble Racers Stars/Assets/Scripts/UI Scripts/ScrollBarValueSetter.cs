using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class ScrollBarValueSetter : MonoBehaviour
{
    [SerializeField] Scrollbar bar;
    [SerializeField] RectTransform content;
    [SerializeField] ScrollRect rectScroll;

    void OnEnable()
    {
        StartCoroutine(UpdatescrollValue());
    }

    IEnumerator UpdatescrollValue()
    {
        yield return new WaitForEndOfFrame();
        bar.value = 01f;
    }
    IEnumerator FocusPlayer() 
    {
        List<BoardUIController> listContent = content.GetComponentsInChildren<BoardUIController>().OfType<BoardUIController>().ToList();
        while (listContent.Count == 0) 
        {
            listContent = content.GetComponentsInChildren<BoardUIController>().OfType<BoardUIController>().ToList();
            yield return new WaitForEndOfFrame();
        }

        FocusOn(listContent.Find(x => x.bufferMarble.isPlayer).GetComponent<RectTransform>());
    }

    public void FocusOn(RectTransform target) 
    {
        Canvas.ForceUpdateCanvases();

       content.anchoredPosition = (Vector2)rectScroll.transform.InverseTransformPoint(content.transform.position) 
            - (Vector2)rectScroll.transform.InverseTransformPoint(target.transform.position);
    }
}
