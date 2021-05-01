using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ReferenceKeeping reference;
    public bool myturn = false;
    
    // Start is called before the first frame update
    void Awake()
    {
        reference = GameObject.FindWithTag("Reference").GetComponent<ReferenceKeeping>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            SendRay();
        }
    }

    public void SendRay()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(LayerMask.NameToLayer("Slot"));
        if(Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Slot")))
        {
            Debug.Log(2);
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
