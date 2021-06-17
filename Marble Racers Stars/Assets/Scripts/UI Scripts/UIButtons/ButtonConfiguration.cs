using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonConfiguration : BaseButtonComponent
{
    public string settingName;
    [SerializeField] private Sprite spriteEnable = null;
    [SerializeField] private Sprite spriteDisable = null;
    private Image imageComp = null;
    public bool isSound = false;

    void OnEnable()
    {
        imageComp = GetComponent<Image>();

        if (isSound)
        {
            imageComp.sprite = (PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I, 0) == 0) ? spriteDisable : spriteEnable;
            buttonComponent.onClick.AddListener(SetSpriteSelected);
        }
        else
        {
            imageComp.sprite = (PlayerPrefs.GetString(KeyStorage.GRAPHICS_SETTING_S).Equals(settingName)) ? spriteEnable : spriteDisable;
            buttonComponent.onClick.AddListener(delegate { Configuration.Instance.SetQuality(this);});
            Configuration.Instance.OnConfigurationSelected += SetSpriteSelectedQuality;
        }
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
            imageComp.sprite = spriteEnable;
        else
            imageComp.sprite = spriteDisable;
    }
    
}
