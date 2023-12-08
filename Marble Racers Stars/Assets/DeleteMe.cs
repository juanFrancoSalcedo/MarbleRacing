using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    [SerializeField] private TracksInfo infoTrack = null;

    private void Start()
    {
        print(JsonUtility.ToJson(infoTrack));
    }
}
