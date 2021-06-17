using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotation = Vector3.one;

    private void Update()
    {
        transform.Rotate(rotation);
    }
}
