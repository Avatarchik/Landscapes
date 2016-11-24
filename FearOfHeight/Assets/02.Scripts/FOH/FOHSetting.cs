using UnityEngine;
using System.Collections;
using I2.Loc;

public class FOHSetting : MonoBehaviour
{
    public enum LanguageType
    {
        English,
        Korean,
        Auto
    }

    public LanguageType languageType;

    private I2.Loc.SetLanguage m_SetLanguage;

    void Awake()
    {
        m_SetLanguage = FindObjectOfType<SetLanguage>();

        if (languageType == LanguageType.Auto)
        {
            if (Application.systemLanguage == SystemLanguage.Korean)
            {
                m_SetLanguage._Language = "Kor";
                m_SetLanguage.ApplyLanguage();
            }
            else
            {
                m_SetLanguage._Language = "Eng";
                m_SetLanguage.ApplyLanguage();
            }
        }

        else if (languageType == LanguageType.English)
        {
            m_SetLanguage._Language = "Eng";
            m_SetLanguage.ApplyLanguage();
        }

        else if (languageType == LanguageType.Korean)
        {
            m_SetLanguage._Language = "Kor";
            m_SetLanguage.ApplyLanguage();
        }
    }

    void FixedUpdate()
    {
        if (OVRManager.isPowerSavingActive)
        {
            OVRPlugin.vsyncCount = 2;
            return;
        }

        if (OVRPlugin.vsyncCount == 1)
            return;

        OVRPlugin.vsyncCount = 1;
    }
}