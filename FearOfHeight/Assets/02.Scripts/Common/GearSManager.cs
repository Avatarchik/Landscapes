using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GearSManager : MonoBehaviour
{
	private static GearSManager instance = null;

	private bool isConnected = false;
	private int currentHeartbeatRate = -1;

	private int baselineHeartbeatRate = -1;
	private int phase1HeartbeatRate = -1;
	private int phase2HeartbeatRate = -1;

	private List<int> baselineHeartbeatList = null;
	private List<int> phase1HeartbeatList = null;
	private List<int> phase2HeartbeatList = null;


	private enum CHECK_MODE
	{
		MODE_NONE = -1,
		MODE_BASELINE = 0,
		MODE_PHASE1,
		MODE_PHASE2,
		MODE_MAX
	}
	private CHECK_MODE checkMode = CHECK_MODE.MODE_NONE;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}

	public static GearSManager Instance()
	{
		return instance;
	}

	void Start ()
	{
		StartCoroutine(GearSConnect());
	}

	public bool IsConnected()
	{
		return isConnected;
	}

	public void CheckBaseline(bool isStart)
	{
		if(isStart == true)
		{
			checkMode = CHECK_MODE.MODE_BASELINE;
			baselineHeartbeatRate = -1;
			baselineHeartbeatList = new List<int>();
			AndroidPluginManager.Instance().DemandCheckHBR(true);
			StartCoroutine("GetHBR");
		}
		else
		{
			checkMode = CHECK_MODE.MODE_NONE;
			baselineHeartbeatRate = (int)baselineHeartbeatList.Average();
			baselineHeartbeatList = null;
			StopCoroutine("GetHBR");
			AndroidPluginManager.Instance().DemandCheckHBR(false);
		}
	}

	public int GetBaselineHeartbeatCount()
	{
		if(baselineHeartbeatList != null)
		{
			return baselineHeartbeatList.Count;
		}
		else
		{
			return 0;
		}
	}

	public int GetBaselineHeartbeatRate()
	{
		return baselineHeartbeatRate;
	}

	public void CheckPhase1(bool isStart)
	{
		if(isStart == true)
		{
			checkMode = CHECK_MODE.MODE_PHASE1;
			phase1HeartbeatRate = -1;
			phase1HeartbeatList = new List<int>();
			AndroidPluginManager.Instance().DemandCheckHBR(true);
			StartCoroutine("GetHBR");
		}
		else
		{
			checkMode = CHECK_MODE.MODE_NONE;
			phase1HeartbeatRate = (int)phase1HeartbeatList.Average();
			phase1HeartbeatList = null;
			StopCoroutine("GetHBR");
			AndroidPluginManager.Instance().DemandCheckHBR(false);
		}
	}

	public int GetPhase1HeartbeatRate()
	{
		return phase1HeartbeatRate;
	}

	public void CheckPhase2(bool isStart)
	{
		if(isStart == true)
		{
			checkMode = CHECK_MODE.MODE_PHASE2;
			phase2HeartbeatRate = -1;
			phase2HeartbeatList = new List<int>();
			AndroidPluginManager.Instance().DemandCheckHBR(true);
			StartCoroutine("GetHBR");
		}
		else
		{
			checkMode = CHECK_MODE.MODE_NONE;
			phase2HeartbeatRate = (int)phase2HeartbeatList.Average();
			phase2HeartbeatList = null;
			StopCoroutine("GetHBR");
			AndroidPluginManager.Instance().DemandCheckHBR(false);
		}
	}

	public int GetPhasesHeartbeatRate()
	{
		return phase2HeartbeatRate;
	}

	IEnumerator GearSConnect()
	{
		AndroidPluginManager.Instance().DemandIsGearSConnected();
		isConnected = AndroidPluginManager.Instance().IsGearSConnected();

		yield return new WaitForSeconds(1.0f);

		if(isConnected == false)
		{
			StartCoroutine(GearSConnect());
		}
	}

	IEnumerator GetHBR()
	{
		AndroidPluginManager.Instance().DemandGetHBR();
		currentHeartbeatRate = AndroidPluginManager.Instance().GetHBR();

		if(currentHeartbeatRate > 0)
		{
			switch(checkMode)
			{
			case CHECK_MODE.MODE_BASELINE:
				baselineHeartbeatList.Add(currentHeartbeatRate);
				break;
			case CHECK_MODE.MODE_PHASE1:
				phase1HeartbeatList.Add(currentHeartbeatRate);
				break;
			case CHECK_MODE.MODE_PHASE2:
				phase2HeartbeatList.Add(currentHeartbeatRate);
				break;
			}
		}

		yield return new WaitForSeconds(1.0f);

		StartCoroutine("GetHBR");
	}
}
