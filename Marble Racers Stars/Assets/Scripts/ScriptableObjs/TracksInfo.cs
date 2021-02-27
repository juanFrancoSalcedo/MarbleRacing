using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewTrackInfo", menuName = "Inventory/TrackInfo")]
[System.Serializable]
public class TracksInfo : ScriptableObject
{
    [SerializeField] string nameTrack;
    [SerializeField] Sprite spriteTrack;
    public Sprite SpriteTrack => spriteTrack;
    public string NameTrack => nameTrack;
}
