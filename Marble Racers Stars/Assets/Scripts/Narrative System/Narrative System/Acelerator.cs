using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerator : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Time.timeScale = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Time.timeScale = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Time.timeScale = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Time.timeScale = 20;
        }
    }
}
