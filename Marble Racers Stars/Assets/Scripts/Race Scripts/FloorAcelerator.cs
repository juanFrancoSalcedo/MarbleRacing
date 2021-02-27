using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAcelerator : MonoBehaviour
{
    [SerializeField] private TriggerDetector detectorTrigger;
    private Renderer childVisual;

    void Start()
    {
        detectorTrigger.OnTriggerEntered += AcelerateMarble;
        detectorTrigger.OnTriggerExited += AcelerateMarble;
        childVisual = transform.GetChild(0).GetComponent<Renderer>();
    }

    private void AcelerateMarble(Transform other)
    {
        other.SendMessage("ApplyForce",SendMessageOptions.DontRequireReceiver);
        PoolAmbientSounds.GetInstance().PushShoot(SoundType.Accelerator,other.position,childVisual.isVisible);
    }
}
