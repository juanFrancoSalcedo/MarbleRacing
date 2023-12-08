using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;
using UnityEngine.UI;
using DG.Tweening;

public class TracksShow : Singleton<TracksShow>
{
    [SerializeField] private Image imageBase = null;
    List<Image> imagesTracks = new List<Image>();
    [SerializeField] private Transform contentTransform = null;

    private void Awake() => imagesTracks.Add(imageBase);
    public void ShowTracks(LeagueSYS.League league)
    {
        if (league.listPrix.Count > imagesTracks.Count) 
        {
            for (int i = imagesTracks.Count; i<league.listPrix.Count; i++) 
                imagesTracks.Add((Image)Instantiate(imageBase,contentTransform));
        }

        for (int i = 0; i < imagesTracks.Count; i++)
        {
            if (i < league.listPrix.Count)
            { 
                var trackSprite = TracksInfo.GetTrackByName(league.listPrix[i].trackInfo).SpriteTrack;
                imagesTracks[i].sprite = trackSprite;
                imagesTracks[i].gameObject.SetActive(true);
                imagesTracks[i].transform.GetChild(0).gameObject.SetActive(league.listPrix[i].usePowers);
            }
            else
                imagesTracks[i].gameObject.SetActive(false);
        }
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, -400), 0.6f);
    }

    public void HidePanel() 
    {
        GetComponent<RectTransform>().DOAnchorPos(new Vector2(0,200),0.6f);
    }
}
