using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineRender : Graphic
{
    public List<Vector2> linePoints;
    public Vector2Int gridSize = new Vector2Int(12,12);
    public bool useAngle;
    float width;
    float height;
    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        if (linePoints.Count < 2)
            return;

        float angle =0;
        for (int i = 0; i < linePoints.Count; i++)
        {
            Vector2 point = linePoints[i];
            if (i < linePoints.Count - 1)
            {
                angle = GetAngle(linePoints[i], linePoints[i + 1]) + 90;
            }

            if(!useAngle)
                DrawVerticesForPoints(point, vh);
            else
                DrawVerticesForPoints(point, vh,angle);
        }

        for (int i = 0; i < linePoints.Count-1; i++) 
        {
            int index = i * 2;
            vh.AddTriangle(index +0,index +1,index +3);
            vh.AddTriangle(index +3,index +2,index +0);
        }
    }

    private float GetAngle(Vector2 origin, Vector2 target) 
    {
        return (float)(Mathf.Atan2(target.y-origin.y,target.x-origin.x)*(180/Mathf.PI));
    }

    public void UpdateMesh()=> SetVerticesDirty();

    private void DrawVerticesForPoints(Vector2 point, VertexHelper vh, float angle)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = Quaternion.Euler(0,0,angle)* new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(point.x ,point.y,0);
        vh.AddVert(vertex);
        vertex.position = Quaternion.Euler(0, 0, angle) * new Vector3(thickness / 2,0);
        vertex.position += new Vector3(point.x ,point.y,0);
        vh.AddVert(vertex);
    }

    private void DrawVerticesForPoints(Vector2 point, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;
        vertex.position = new Vector3(-thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y, 0);
        vh.AddVert(vertex);
        vertex.position = new Vector3(thickness / 2, 0);
        vertex.position += new Vector3(point.x, point.y, 0);
        vh.AddVert(vertex);
    }
}
