using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.LWRP;
using UnityEngine.Rendering;
using UnityEngine.Audio;
using MyBox;
public class Configuration :Singleton<Configuration>
{
    [SerializeField] AudioMixer mixer = null;
    [SerializeField] private UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset Low = null;
    [SerializeField] private UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset Medium = null;
    [SerializeField] private UnityEngine.Rendering.Universal.UniversalRenderPipelineAsset High = null;
    private int m_frameCounter = 0;
    private float m_timeCounter = 0.0f;
    private float m_lastFramerate = 0.0f;
    public float m_refreshTime = 0.5f;
    public System.Action<ButtonConfiguration> OnConfigurationSelected;
    public bool firstSet;

    private void OnEnable()
    {
        if (firstSet)
        {
            if (string.IsNullOrEmpty(PlayerPrefs.GetString(KeyStorage.GRAPHICS_SETTING_S)))
            {
                PlayerPrefs.SetString(KeyStorage.GRAPHICS_SETTING_S, "Medium");
                SetQuality(PlayerPrefs.GetString(KeyStorage.GRAPHICS_SETTING_S));
            }

            if (!PlayerPrefs.HasKey(KeyStorage.SOUND_SETTING_I))
                SetSoundSettings(true);
            else
                SetSoundSettings((PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I,0) == 1) ? true : false);
        }
    }

    void Update()
    {
        if (m_timeCounter < m_refreshTime)
        {
            m_timeCounter += Time.deltaTime;
            m_frameCounter++;
        }
        else
        {
            //This code will break if you set your m_refreshTime to 0, which makes no sense.
            m_lastFramerate = (float)m_frameCounter / m_timeCounter;
            m_frameCounter = 0;
            m_timeCounter = 0.0f;
        }
    }

    public void ShiftSound()
    {
        int _sound = PlayerPrefs.GetInt(KeyStorage.SOUND_SETTING_I, 0);
        _sound = (_sound == 0) ? 1 : 0;
        if (_sound == 0)
        {
            SetSoundSettings(false);
        }
        else
        {
            SetSoundSettings(true);
        }
    }
    
    public void SetSoundSettings(bool soundEnable)
    {
        if (soundEnable)
        {
            mixer.SetFloat("Volume", 0);
            PlayerPrefs.SetInt(KeyStorage.SOUND_SETTING_I,1);
        }
        else
        {
            PlayerPrefs.SetInt(KeyStorage.SOUND_SETTING_I, 0);
            mixer.SetFloat("Volume", -80);
        }
    }

    public void SetQuality(ButtonConfiguration buttonSetting)
    {
        switch (buttonSetting.settingName)
        {
            case "Low":
                QualitySettings.SetQualityLevel(1);
                GraphicsSettings.renderPipelineAsset = Low;
                PlayerPrefs.SetString(KeyStorage.GRAPHICS_SETTING_S,"Low");
                break;
            case "Medium":
                QualitySettings.SetQualityLevel(2);
                GraphicsSettings.renderPipelineAsset = Medium;
                PlayerPrefs.SetString(KeyStorage.GRAPHICS_SETTING_S, "Medium");
                break;
            case "High":
                QualitySettings.SetQualityLevel(3);
                GraphicsSettings.renderPipelineAsset = High;
                PlayerPrefs.SetString(KeyStorage.GRAPHICS_SETTING_S, "High");
                break;
        }
        OnConfigurationSelected?.Invoke(buttonSetting);
    }

    public void SetQuality(string quality)
    {
        switch (quality)
        {
            case "Low":
                QualitySettings.SetQualityLevel(1);
                GraphicsSettings.renderPipelineAsset = Low;
                break;
            case "Medium":
                QualitySettings.SetQualityLevel(2);
                GraphicsSettings.renderPipelineAsset = Medium;
                break;
            case "High":
                QualitySettings.SetQualityLevel(3);
                GraphicsSettings.renderPipelineAsset = High;
                break;
        }
    }
}
