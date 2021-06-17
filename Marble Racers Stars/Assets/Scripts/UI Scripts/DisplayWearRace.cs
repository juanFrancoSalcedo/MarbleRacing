using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LeagueSYS;

public class DisplayWearRace : MonoBehaviour
{
    [SerializeField] private Text textPercentage = null;
    [SerializeField] protected Image imageStat = null;
    Color[] colors = {Color.red,new Color(255/255,128/255,0/255,1),Color.yellow, Color.green, Color.cyan};

    public void ShowWear(MarbleStats initStats, MarbleStats currentStats) 
    {
        textPercentage.text =""+currentStats.hp *100 / initStats.hp+"%";
        imageStat.color = InterpolatedColorByPercentage((float)currentStats.hp/ initStats.hp);
    }

    private Color InterpolatedColorByPercentage(float amount) 
    {
        int maxCount = colors.Length - 1;
        float amountInArray = amount * maxCount;
        Color result = Color.Lerp(colors[Mathf.FloorToInt(amountInArray)],colors[Mathf.CeilToInt(amountInArray)],1-((maxCount- amountInArray)/4));
        Color.RGBToHSV(result, out float H, out float S, out float V);
        V = 1;
        Color realColor = Color.HSVToRGB(H, S, V);
        return realColor;
    }

    public void ShowDirt(float dynamicFriction)
    {
        imageStat.fillAmount = (float)(dynamicFriction/0.3f);
        textPercentage.text = dynamicFriction.ToString("f3") ;
    }
}
