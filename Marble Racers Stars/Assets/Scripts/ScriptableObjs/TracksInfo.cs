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
}
