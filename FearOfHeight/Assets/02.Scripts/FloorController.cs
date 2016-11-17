using UnityEngine;
using System.Collections;

public class FloorController : MonoBehaviour {

    Animation m_Animation;

	void Awake ()
    {
        m_Animation = GetComponent<Animation>();
	    m_Animation["Take 001"].wrapMode = WrapMode.Clamp;
    }

    public void OpenFloor()
    {
        m_Animation["Take 001"].speed = 1;
        m_Animation.Play();
    }
}
