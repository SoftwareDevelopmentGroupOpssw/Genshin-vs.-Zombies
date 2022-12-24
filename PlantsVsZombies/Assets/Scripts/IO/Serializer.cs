using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serializer<T>
{
    public string Serialize(T obj)
    {
        return JsonUtility.ToJson(obj);//ʹ��json���洢
    }
    public T Deserialize(string jsonString)
    {
        return JsonUtility.FromJson<T>(jsonString);
    }
}
