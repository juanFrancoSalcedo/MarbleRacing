using MyBox;
using UnityEngine;
using DG.Tweening;


public class SwipeMovement : MonoBehaviour
{
    [MinMaxRangeAttribute(0f,600f)]
    public RangedFloat rangeY = new RangedFloat(1f,6f);

    Vector3 deltaPos = Vector3.zero;
    Vector3 oldPosition;
    Vector3 futurePosi;
    bool touchReleased;
    int countFrames;
    int bufferCountFrames;

    bool canTouch = true;
    
    void Update()
    {
        if (!canTouch) return;

        if (Input.GetMouseButtonDown(0))
        {
            oldPosition = Input.mousePosition;
            RestoreTouch();
        }

        if (Input.GetMouseButtonUp(0))
        {
            touchReleased = true;
            countFrames = (int)deltaPos.magnitude;
            bufferCountFrames = countFrames;
            futurePosi = transform.position + deltaPos.normalized * 6;
        }

        if (Input.GetMouseButton(0))
        {
            deltaPos = Input.mousePosition - oldPosition;
            Vector3 pos = Camera.main.ScreenToViewportPoint(oldPosition - Input.mousePosition);
            Vector3 move = new Vector3(0, pos.y * 40, 0);
            transform.Translate(move, Space.World);
            oldPosition = Input.mousePosition;
        }
        
        if (touchReleased && countFrames >=0)
        {
            countFrames--;
            transform.position = new Vector3(0,
                Mathf.Lerp(transform.position.y, transform.position.y-deltaPos.normalized.y,((float)countFrames/bufferCountFrames)),
                transform.position.z);
        }

        LimitPosition();
    }

    public void FollowTrophyPosition(Vector3 posFuture)
    {
        posFuture.y += 3;
        posFuture.z = transform.position.z;
        posFuture.x = transform.position.x;
        transform.DOMove(posFuture,0.9f);
    }

    void LimitPosition()
    {
        if (transform.position.y < rangeY.Min-10)
        {
            PreventContinuity();
            canTouch = false;
            Vector3 posElastic = transform.position;
            posElastic.y = rangeY.Min;
            transform.DOMove(posElastic, 0.6f);
            Invoke("RestoreTouch",0.6f);
        }

        if (transform.position.y > rangeY.Max+10)
        {
            PreventContinuity();
            canTouch = false;
            Vector3 posElastic = transform.position;
            posElastic.y = rangeY.Max;
            transform.DOMove(posElastic, 0.6f);
            Invoke("RestoreTouch", 0.6f);
        }
    }

    void PreventContinuity()
    {
        countFrames = 0;
        bufferCountFrames = 0;
        touchReleased = false;
    }

    void RestoreTouch()
    {
        canTouch = true;
    }
}
