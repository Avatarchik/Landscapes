using UnityEngine;
using System.Collections.Generic;
using System;

public class Container<T> where T : UnityEngine.Object
{
    private readonly Dictionary<string, UnityEngine.Object> resources = new Dictionary<string, UnityEngine.Object>();

    public Container(string subFolder)
    {
        UnityEngine.Object[] newResources = Resources.LoadAll(subFolder);
        for (int i = 0; i < newResources.Length; i++)
        {
            UnityEngine.Object newPrefab = newResources[i];
            resources[newPrefab.name] = newPrefab;
        }

    }
    
    public T Get(Enum type)
    {
        UnityEngine.Object temp;
        if (resources.TryGetValue(type.ToString(), out temp))
            return temp as T;

        Debug.LogError("not found data : " + type);
        return null;
    }
    /*
    public T Get(string path)
    {
        UnityEngine.Object temp;
        if (resources.TryGetValue(path, out temp))
            return temp as T;

        Debug.LogError("not found data : " + path);
        return null;
    }
    */
}
