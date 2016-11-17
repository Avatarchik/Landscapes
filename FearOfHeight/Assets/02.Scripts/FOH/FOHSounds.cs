using UnityEngine;
using System.Collections.Generic;

public class FOHSounds
{
    private Game game { get { return Game.Instance; } }
    private Dictionary<string, AudioClip> clip { set; get; }

    public void Load()
    {
        clip = new Dictionary<string, AudioClip>();

        Object[] resource = Resources.LoadAll("Sounds");
        for (int i = 0; i < resource.Length; i++)
        {
            AudioClip newClip = resource[i] as AudioClip;
            clip[newClip.name] = newClip;
        }
    }
    public AudioClip GetClip(string strSoundName)
    {
        AudioClip tmp;
        if (clip.TryGetValue(strSoundName, out tmp))
            return tmp;

        Debug.LogError("not found : " + strSoundName);
        return null;
    }

    public void Play(string type, AudioSource source)
    {
        source.clip = clip[type];
        source.PlayOneShot(source.clip);
    }

    public void UnPause()
    {
        if(game.NAVI.clip)
            game.NAVI.UnPause();
    }

    public void Pause()
    {
        game.NAVI.Pause();
    }

    public void Stop()
    {
        game.NAVI.Stop();
    }
}
