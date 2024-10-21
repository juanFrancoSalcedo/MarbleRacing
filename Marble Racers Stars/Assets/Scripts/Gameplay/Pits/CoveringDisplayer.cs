using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoveringDisplayer : MonoBehaviour
{
    [SerializeField] private Image _image = null;
    [SerializeField] private Text _text = null;
    [SerializeField] private List<SpritesCovering> coverings = new List<SpritesCovering>();

    private void OnEnable()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {

    }

    private void OnDestroy()
    {
        
    }


    private string GetNameCovering(TypeCovering coveringType) 
    {
        switch (coveringType) 
        {
            case TypeCovering.SoftRough:
                return "Soft Rough";
            case TypeCovering.HardElastic:
                return "Hard Elastic";
            case TypeCovering.Medium:
                return TypeCovering.Medium.ToString();
            default:
                return TypeCovering.Medium.ToString();
        }
    }

    private Color SearchColor(TypeCovering coveringType) => coverings.Find(c => c.typeCovering.Equals(coveringType)).sptCovering; 

    [System.Serializable]
    struct SpritesCovering
    {
        public Color sptCovering;
        public TypeCovering typeCovering;
    }

}
