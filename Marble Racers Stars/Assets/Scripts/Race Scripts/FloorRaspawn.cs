using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorRaspawn : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Marble>())
        {
            collision.collider.GetComponent<Marble>().RespawnMarble();
        }
    }
}
