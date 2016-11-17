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

#if UNITY_ANDROID && !UNITY_EDITOR
        // S6
	    if (SystemInfo.deviceModel.Contains("G920") || SystemInfo.deviceModel.Contains("G925") ||
	        SystemInfo.deviceModel.Contains("G928"))
	    {
	        QualitySettings.SetQualityLevel(2);
	    }

        // Others
	    else
	    {
            QualitySettings.SetQualityLevel(1);
        }
#endif
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
}