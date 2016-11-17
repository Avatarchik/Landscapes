using UnityEngine;
using System.Collections;

public static class Util
{
    public static int Int32ParseFast(string value)
    {
        int result = 0;
        int length = value.Length;
        bool minus = false;
        for (int i = 0; i < length; i++)
        {
            if (value[i].CompareTo('-') == 0)
            {
                minus = true;
                continue;
            }
            result = 10 * result + (value[i] - 48);
        }
        return minus == false ? result : -result;
    }

    public static bool IsPlatformAndroid()
    {
        return Application.platform == RuntimePlatform.Android;
    }

    public static bool IsPlatformIOS()
    {
        return Application.platform == RuntimePlatform.IPhonePlayer;
    }

    public static bool IsMobilePlatform()
    {
        if (Application.platform == RuntimePlatform.Android
            || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return true;
        }
        return false;
    }

    public static bool IsEditorPlatform()
    {
#if UNITY_EDITOR
        return true;
#else
            return false;
#endif
    }

    public static int LoadLocalDataInt(string myKey, int defaultValue = 0)
    {
        return UnityEngine.PlayerPrefs.GetInt(myKey, defaultValue);
    }

    public static float LoadLocalDataFloat(string myKey, float defaultValue = 0)
    {
        return UnityEngine.PlayerPrefs.GetFloat(myKey, defaultValue);
    }

    public static bool HasLocalData(string key)
    {
        return UnityEngine.PlayerPrefs.HasKey(key);
    }

    public static string LoadLocalDataString(string myKey)
    {
        return UnityEngine.PlayerPrefs.GetString(myKey);
    }

    public static bool LoadLocalDataBool(string myKey)
    {
        int boolValue = UnityEngine.PlayerPrefs.GetInt(myKey);

        return (1 == boolValue);
    }


    public static void SaveLocalData(string myKey, int myData)
    {
        UnityEngine.PlayerPrefs.SetInt(myKey, myData);
    }

    public static void SaveLocalData(string myKey, bool myData)
    {
        int boolValue = (true == myData) ? 1 : 0;

        UnityEngine.PlayerPrefs.SetInt(myKey, boolValue);
    }

    public static void SaveLocalData(string myKey, string myData)
    {
        UnityEngine.PlayerPrefs.SetString(myKey, myData);
    }

    public static void SaveLocalData(string myKey, float myData)
    {
        UnityEngine.PlayerPrefs.SetFloat(myKey, myData);
    }

    public static bool HasKey(string myKey)
    {
        return UnityEngine.PlayerPrefs.HasKey(myKey);
    }
}
