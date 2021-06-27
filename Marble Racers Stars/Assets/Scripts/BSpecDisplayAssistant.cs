using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BSpecDisplayAssistant : MonoBehaviour
{
    [SerializeField] Image imgbrokenMarble;
    [SerializeField] Image imgSearching;
    [SerializeField] TextMeshProUGUI textSearch;
    [SerializeField] Image imgAlways;

    private void OnEnable()
    {
        BSpecManager.Instance.onMarbleBrokenAssigned += delegate(bool c){ imgbrokenMarble.gameObject.SetActive(c); };
        BSpecManager.Instance.onFocused += delegate(bool c){ imgAlways.gameObject.SetActive(c); };
        BSpecManager.Instance.onSearched += delegate(bool c,string str){
            imgSearching.gameObject.SetActive(c);
            textSearch.text = str; };
    }

    private void OnDisable()
    {
        BSpecManager.Instance.onMarbleBrokenAssigned -= delegate(bool c){ imgbrokenMarble.gameObject.SetActive(c); };
        BSpecManager.Instance.onFocused -= delegate(bool c){ imgAlways.gameObject.SetActive(c); };
        BSpecManager.Instance.onSearched -= delegate(bool c,string str){ 
            imgSearching.gameObject.SetActive(c);
            textSearch.text = str; };
    }
}
