using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
 
[ExecuteAlways]
public class DrawColorGizmosCollider : MonoBehaviour
{
    public enum DrawStates
    {
        DrawCube,
        DrawWireCube
    }


    public Transform boxColliderToDrawOn;
    [Range(0f, 1f)]
    public float colorTransparency;
    public bool draw = false;
    public DrawStates drawStates;

    private BoxCollider boxCollider = null;

    // Start is called before the first frame update
    void Start()
    {
        if (boxColliderToDrawOn != null)
        {
            boxCollider = boxColliderToDrawOn.GetComponent<BoxCollider>();
        }

        drawStates = DrawStates.DrawCube;
    }

    private void DrawGizmosOnRunTime(Color color)
    {
        if (boxCollider != null && draw)
        {
            Gizmos.color = color;
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(boxCollider.transform.position, boxCollider.transform.rotation, boxCollider.transform.lossyScale);
            Gizmos.matrix = rotationMatrix;

            switch (drawStates)
            {
                case DrawStates.DrawCube:
                    Gizmos.DrawCube(boxCollider.center, boxCollider.size);
                    break;

                case DrawStates.DrawWireCube:
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Color c = Color.red;
        c.a = colorTransparency;
        DrawGizmosOnRunTime(c);
    }
}