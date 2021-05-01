using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class DeleteList : ScriptableObject
{
    public List<GameObject> objects;

    public void Setup()
    {
        objects = new List<GameObject>();
    }

    public void DestroyList()
    {
        if (objects != null)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Destroy(objects[i]);
            }
        }
    }
}
