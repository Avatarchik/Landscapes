using UnityEngine;
using System.Collections;

public class AndroidPluginManager : MonoBehaviour
{
	private static AndroidPluginManager instance = null;

	private AndroidJavaClass pluginClass = null;
	private AndroidJavaObject pluginObject = null;

	void Awake ()
	{
		if(instance == null)
		{
			instance = this;
		}

		PluginInit();
	}

	public static AndroidPluginManager Instance()
	{
		return instance;
	}

	void PluginInit()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		pluginObject = pluginClass.GetStatic<AndroidJavaObject>("currentActivity");
#endif
	}

	public void PluginCall(string functionName)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginObject.Call(functionName);
#endif
	}

	public void PluginCall(string functionName, params object[] objects)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginObject.Call(functionName, objects);
#endif
	}

	public void DemandIsGearSConnected()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginObject.Call("DemandIsConnected");
#endif
	}

	public void DemandCheckHBR(bool isCheck)
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginObject.Call("DemandCheckHBR", isCheck);
#endif
    }

	public void DemandGetHBR()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		pluginObject.Call("DemandGetHBR");
#endif
	}

	public bool IsGearSConnected()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return pluginObject.CallStatic<bool>("IsConnected");
#else
		return false;
#endif
	}

	public int GetHBR()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		return pluginObject.CallStatic<int>("GetHeartbeatRate");
#else
		return -1;
#endif
	}
}