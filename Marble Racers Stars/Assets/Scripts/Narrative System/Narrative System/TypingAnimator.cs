using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyBox;
using UnityEngine.EventSystems;

public class TypingAnimator : MonoBehaviour
{
    public TextMeshProUGUI textCompo { get; set; }
    [SerializeField] private float speedText= 0.4f;
    private string fullString;
    private string currentString;
    [SerializeField] private bool invokeActionAtEnd;
    public event System.Action OnComplitedText;

    private void OnEnable()
    {
        textCompo = GetComponent<TextMeshProUGUI>();
    }

    public void EraseAndSaveText()
    {
        textCompo = GetComponent<TextMeshProUGUI>();
        fullString = textCompo.text;
        textCompo.text = "";
    }

    public void SaveText()
    {
        fullString = textCompo.text;
    }

    public void SetTextFull(string newString)
    {
        fullString = newString;
    }

    public void SetColor(Color newColor)
    {
        textCompo.color = newColor;
    }

    public void SetFont(TMP_FontAsset _fontNew)
    {
        textCompo = GetComponent<TextMeshProUGUI>();
        textCompo.font = _fontNew;
    }

    public void ShowFullText()
    {
         textCompo.text = fullString;
    }

    public void StartAnimation()
    {
        if(gameObject.activeInHierarchy)
        StartCoroutine(ShowPartial( ));
    }

    private IEnumerator ShowPartial()
    {
        textCompo = GetComponent<TextMeshProUGUI>();

        textCompo.text = "";

        for (int i = 0; i < fullString.Length; i++)
        {
            currentString = fullString.Substring(0,i);
            textCompo.text = currentString;
            yield return new WaitForSeconds(speedText);
        }
        ShowFullText();
        if(invokeActionAtEnd) OnComplitedText?.Invoke();
    }

    public void EndAnimation()
    {
        if (textCompo.text.Length == fullString.Length)
        {
            OnComplitedText?.Invoke();
        }
    }
}
