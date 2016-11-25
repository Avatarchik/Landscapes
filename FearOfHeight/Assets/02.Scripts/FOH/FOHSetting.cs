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