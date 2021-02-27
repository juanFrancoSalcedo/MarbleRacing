using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    [SerializeField] Transform startAgain;
    [SerializeField] private TriggerDetector triggerDetect;

    public event System.Action<Transform> OnFirstEnter;
    public event System.Action<Transform> OnExitPortal;
    private void Start()
    {
        triggerDetect.OnTriggerEntered += TeleportMarble;
    }

    private int count;

    private void TeleportMarble(Transform other)
    {
        if (other.CompareTag("Finish"))
        {
            return;
        }

        Vector3 v1 = other.transform.position;
        v1.y = startAgain.position.y;
        other.transform.position = v1;


        //if (other.GetComponent<Marble>().isPlayer)
        //{
        //    print("Copacavana");
        //}


        if (count == 0)
        {
            OnFirstEnter?.Invoke(other);
        }

        if (count >0 && count < 12)
        {
            OnExitPortal?.Invoke(other);
        }

        count++;

        if (count >= 12)
        {
            count = 0;
        }

    }

}
