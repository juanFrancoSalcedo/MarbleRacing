using System.Collections;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    public BoardParticipant[] participantScores;
    public BoardParticipant[] sortedParti { get; set;}
    public bool lowToHigh;

    private void Awake() => ResetParticipantSorted();
    public void DeleteAllParticipants() 
    {
        foreach (var participant in participantScores) 
        {
            Destroy(participant.gameObject);
        }
        participantScores = new BoardParticipant[0];
    }
    public void ResetParticipantSorted()
    {
        sortedParti = participantScores;
    }
    public BoardParticipant GetPlayerAtPosition(int boardPosition)
    {
        return sortedParti[boardPosition];
    }
    public void SortScores()
    {
        for (int i = 0; i < sortedParti.Length - 1; i++)
        {
            for (int j = i+1; j < sortedParti.Length; j++)
            {
                if (lowToHigh)
                {
                    if (sortedParti[i].score > sortedParti[j].score)
                    {
                        BoardParticipant buffer = sortedParti[j];
                        sortedParti[j] = sortedParti[i];
                        sortedParti[i] = buffer;
                    }
                    else if (sortedParti[i].score == sortedParti[j].score)
                    {
                        if (sortedParti[i].secondScore > sortedParti[j].secondScore)
                        {
                            BoardParticipant buffer = sortedParti[j];
                            sortedParti[j] = sortedParti[i];
                            sortedParti[i] = buffer;
                            //print("sorted igual"+sortedParti[j].secondScore);
                        }
                    }
                }
                else
                {
                    if (sortedParti[i].score < sortedParti[j].score)
                    {
                        BoardParticipant buffer = sortedParti[j];
                        sortedParti[j] = sortedParti[i];
                        sortedParti[i] = buffer;
                    }
                    else if (sortedParti[i].score == sortedParti[j].score)
                    {
                        if (sortedParti[j].secondScore < sortedParti[j].secondScore)
                        {
                            BoardParticipant buffer = sortedParti[j];
                            sortedParti[j] = sortedParti[i];
                            sortedParti[i] = buffer;
                        }
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
