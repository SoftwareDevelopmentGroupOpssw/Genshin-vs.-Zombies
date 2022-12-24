using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer<T>
{
    public string Serialize(T obj)
    {
        return JsonUtility.ToJson(obj);// π”√json¿¥¥Ê¥¢
    }
    public T Deserialize(string jsonString)
    {
        return JsonUtility.FromJson<T>(jsonString);
    }
}
