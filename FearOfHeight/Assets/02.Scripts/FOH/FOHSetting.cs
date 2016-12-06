using UnityEngine;
using System.Collections;
using I2.Loc;

public class FOHSetting : MonoBehaviour
{
    public enum LanguageType
    {
        English,
        German,
        Korean,
        Russian,
        HongKong,
        /*
        Portuguese,
        Spanish,
        */
        Auto
    }

    public LanguageType languageType;

    void Awake()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        // S6
	    if (SystemInfo.deviceModel.Contains("G920") || SystemInfo.deviceModel.Contains("G925") ||
	        SystemInfo.deviceModel.Contains("G928") || SystemInfo.deviceModel.Contains("N920"))
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
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Korean:
                    LocalizationManager.CurrentLanguage = "Korean";
                    break;
                case SystemLanguage.English:
                    LocalizationManager.CurrentLanguage = "English";
                    break;
                case SystemLanguage.German:
                    LocalizationManager.CurrentLanguage = "German";
                    break;
                case SystemLanguage.Russian:
                    LocalizationManager.CurrentLanguage = "Russian";
                    break;
                case SystemLanguage.ChineseTraditional:
                    LocalizationManager.CurrentLanguage = "HongKong";
                    break;
                    /*
                case SystemLanguage.Portuguese:
                    LocalizationManager.CurrentLanguage = "Portuguese";
                    break;
                case SystemLanguage.Spanish:
                    LocalizationManager.CurrentLanguage = "Spanish";
                    break;
                    */
            }
        }

        switch (languageType)
        {
            case LanguageType.Korean:
                LocalizationManager.CurrentLanguage = "Korean";
                break;
            case LanguageType.English:
                LocalizationManager.CurrentLanguage = "English";
                break;
            case LanguageType.German:
                LocalizationManager.CurrentLanguage = "German";
                break;
            case LanguageType.Russian:
                LocalizationManager.CurrentLanguage = "Russian";
                break;
            case LanguageType.HongKong:
                LocalizationManager.CurrentLanguage = "HongKong";
                break;
                /*
            case LanguageType.Portuguese:
                LocalizationManager.CurrentLanguage = "Portuguese";
                break;
            case LanguageType.Spanish:
                LocalizationManager.CurrentLanguage = "Spanish";
                break;            
                */
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
