using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrackInfo", menuName = "Inventory/TrackInfo")]
[System.Serializable]
public class TracksInfo : ScriptableObject
{
    [SerializeField] string nameTrack = null;
    [SerializeField] Sprite spriteTrack = null;
    [SerializeField] Sprite logoTrack = null;
    public Sprite SpriteTrack => spriteTrack;
    public Sprite LogoTrack => logoTrack;
    public string NameTrack => nameTrack;

    public static TracksInfo GetTrackByName(string nameTrack) 
    {
        var item = Resources.Load<TracksInfo>("TracksInfo/" + nameTrack);
        return item;
    }
}
