using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public BoardParticipant[] participantScores;
    [HideInInspector]
    public BoardParticipant[] sortedParti;
    public bool lowToHigh;


    private void Awake()=> sortedParti = participantScores;

    public BoardParticipant GetPlayerAtPosition(int boardPosition)
    {
        return sortedParti[boardPosition];
    }

    public void SortScores()
    {
        for (int i = 0; i < sortedParti.Length - 1; i++)
        {
            for (int j = 0; j < sortedParti.Length - 1; j++)
            {
                if (lowToHigh)
                {
                    if (sortedParti[j].score > sortedParti[j + 1].score)
                    {
                        BoardParticipant buffer = sortedParti[j + 1];
                        sortedParti[j + 1] = sortedParti[j];
                        sortedParti[j] = buffer;
                    }
                }
                else
                {
                    if (sortedParti[j].score < sortedParti[j + 1].score)
                    {
                        BoardParticipant buffer = sortedParti[j + 1];
                        sortedParti[j + 1] = sortedParti[j];
                        sortedParti[j] = buffer;
                    }
                }
            }
        }
        
        for (int h = 0; h < sortedParti.Length; h++)
        {
            sortedParti[h].transform.SetSiblingIndex(h);
            //queTal +=  sortedTexts[h].name+" "+h;
        }
    }
}
