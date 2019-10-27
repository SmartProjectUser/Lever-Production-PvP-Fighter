using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteWhenPartStop : MonoBehaviour
{

    ParticleSystem part;
    public bool DeleteParent = false;

    // Use this for initialization
    void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (part.isStopped)
        {
            if (DeleteParent) { Destroy(gameObject.transform.parent.gameObject); }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
