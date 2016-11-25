using UnityEngine;
using System.Collections;
using I2.Loc;

public class FOHSetting : MonoBehaviour
{
    public enum LanguageType
    {
        English,
        Korean,
        German,
        Russian,
        Portuguese,
        Spanish,
        Chinese,
        Auto
    }

    public LanguageType languageType;

    void Awake()
    {
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
                case SystemLanguage.Portuguese:
                    LocalizationManager.CurrentLanguage = "Portuguese";
                    break;
                case SystemLanguage.Spanish:
                    LocalizationManager.CurrentLanguage = "Spanish";
                    break;
                case SystemLanguage.Chinese:
                    LocalizationManager.CurrentLanguage = "Spanish";
                    break;
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
            case LanguageType.Portuguese:
                LocalizationManager.CurrentLanguage = "Portuguese";
                break;
            case LanguageType.Spanish:
                LocalizationManager.CurrentLanguage = "Spanish";
                break;
            case LanguageType.Chinese:
                LocalizationManager.CurrentLanguage = "Spanish";
                break;
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