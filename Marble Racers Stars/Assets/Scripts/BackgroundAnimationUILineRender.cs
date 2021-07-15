using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BackgroundAnimationUILineRender : MonoBehaviour
{
    [SerializeField] private UILineRender line = null;
    [SerializeField] private Transform[] targets = null;
    [SerializeField] private CanvasScaler canvasScaler = null;
    int current =0;

    private void Awake()
    {
        line.linePoints = GetRandoListBorderToBorder();
        line.UpdateMesh();
        for (int i = 0; i < targets.Length; i++)
            StartCoroutine(Road(targets[i], i * Random.Range(1f, 3f)));
    }

    private IEnumerator Road(Transform ball, float _time)
    {
        yield return new WaitForSeconds(_time);
        ball.position = line.linePoints[current];
        while (current < line.linePoints.Count - 1)
        {
            current++;
            float t = (line.linePoints[current] - (Vector2)ball.localPosition).magnitude;
            ball.GetComponent<RectTransform>().DOAnchorPos(line.linePoints[current], t / 360).SetEase(Ease.Linear).SetUpdate(true);
            yield return new WaitForSeconds((t / 360) + 0.01f);
            if (current >= line.linePoints.Count - 1)
                current = 0;
        }
    }

    private List<Vector2> GetRandoList()
    {
        List<Vector2> p = new List<Vector2>();
        Vector2 previous = new Vector2(left, RandomHeight());
        Vector2 end = new Vector2(right, RandomHeight());
        p.Add(previous);
        for (int i = 1; i < 9; i++)
        {
            //Vector2 interpolate = Vector2.Lerp(previous,end,i);
            Vector2 rando = new Vector2(0, Random.Range(-1f, 1f));
            p.Add(new Vector2(
                (i - 5) * 200,
                Vector2.Lerp(previous, end, ((float)i * rando.y / 10)).y
                ));
        }
        p.Add(end);
        return p;
    }

    private List<Vector2> GetRandoListBorderToBorder()
    {
        List<Vector2> p = new List<Vector2>();
        p.Add(new Vector2(left,RandomHeight()));
        p.Add(new Vector2(RandomWidth(),lower));
        p.Add(new Vector2(right,RandomHeight()));
        p.Add(new Vector2(RandomWidth(),upper));
        p.Add(new Vector2(left,RandomHeight()));
        p.Add(new Vector2(RandomWidth(), lower));
        p.Add(new Vector2(right, RandomHeight()));
        p.Add(new Vector2(RandomWidth(), upper));
        p.Add(new Vector2(left,p[0].y));
        return p;
    }

    float left => (-canvasScaler.referenceResolution.x / 2) - 100;
    float lower => (-canvasScaler.referenceResolution.y / 2) - 100;
    float right => (canvasScaler.referenceResolution.x / 2) + 100;
    float upper => (canvasScaler.referenceResolution.y / 2) + 100;
    private float RandomHeight()=> Random.Range((-canvasScaler.referenceResolution.y / 2), canvasScaler.referenceResolution.y / 2);

    private float RandomWidth()=> Random.Range((-canvasScaler.referenceResolution.y / 2) + 100, canvasScaler.referenceResolution.y / 2);
}
