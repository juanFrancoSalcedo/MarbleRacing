using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTexture : MonoBehaviour
{

    Renderer rend;
    [SerializeField] float yVelocity=1;
    [SerializeField] string nameTexture;
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    float offsetY;
    // Update is called once per frame
    void Update()
    {
        offsetY -= Time.deltaTime;
        Vector2 solid = new Vector2(0, offsetY * yVelocity);
        rend.material.SetTextureOffset(nameTexture, solid);
        
    }
}
