using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MyBox;

public class ButtonConfiguration : BaseButtonComponent
{
    public bool isSound = false;
    [ConditionalField( nameof(isSound), true)] public string settingName;
    [SerializeField] private Sprite spriteEnable = null;
    [SerializeField] private Sprite spriteDisable = null;
    [SerializeField] private Image imageComp = null;
    [SerializeField] AnimationUIController anim;
    [ConditionalField(nameof(anim))] [SerializeField] private int indexAnimationShow=0;
    [ConditionalField(nameof(anim))] [SerializeField] private int indexAnimationHide=1;
    [ConditionalField(nameof(anim))] [SerializeField] private Color colorShowed = Color.white;
    [ConditionalField(nameof(anim))] [SerializeField] private Color colorHiden = Color.white;

    void OnEnable()
    {
        if (isSound)
        {
            imageComp.sprite = (PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I, 0) == 0) ? spriteDisable : spriteEnable;
            buttonComponent.onClick.AddListener(SetSpriteSelected);
        }
        else
        {
            if (PlayerPrefs.GetString(KeyStorage.GRAPHICS_SETTING_S).Equals(settingName))
            {
                buttonComponent.image.color = colorShowed;
                imageComp.sprite = spriteEnable;
                StartCoroutine(CallAnimation(true, 0.3f));
            }
            else
            {
                buttonComponent.image.color = colorHiden;
                imageComp.sprite = spriteDisable;
                StartCoroutine(CallAnimation(false, 0));
            }

            buttonComponent.onClick.AddListener(delegate { Configuration.Instance.SetQuality(this);});
            Configuration.Instance.OnConfigurationSelected += SetSpriteSelectedQuality;
        }
    }

    private void OnDisable()
    {
        Configuration.Instance.OnConfigurationSelected -= SetSpriteSelectedQuality;
    }

    public void SetSpriteSelected()
    {
        Configuration.Instance.ShiftSound();
        int _sound = PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I,0);
        if (_sound == 0)
            imageComp.sprite = spriteDisable;
        else
            imageComp.sprite = spriteEnable;
    }

    private void SetSpriteSelectedQuality(ButtonConfiguration btton)
    {
        if (ReferenceEquals(btton, this))
        {
            buttonComponent.image.color = colorShowed;
            StartCoroutine(CallAnimation(true,0));
            imageComp.sprite = spriteEnable;
        }
        else
        {
            buttonComponent.image.color = colorHiden;
            StartCoroutine(CallAnimation(false,0));
            imageComp.sprite = spriteDisable;
        }
    }

    IEnumerator CallAnimation(bool show, float _time) 
    {
        yield return new WaitForSeconds(_time);
        if (anim != null) anim.ActiveAnimation((show)?indexAnimationShow:indexAnimationHide);
    }
}
