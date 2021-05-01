using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetKinmatic : MonoBehaviour
{
    public float timer;
    public DeleteList list;
    void Start()
    {
        list.objects.Add(this.gameObject);
        StartCoroutine(DisableBod());
    }

    IEnumerator DisableBod()
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }
}
