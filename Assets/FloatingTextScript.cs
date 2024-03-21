using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextScript : MonoBehaviour
{
    public float destoryTime;
    void Start()
    {
        destoryTime = 3f;
    }

    void Update()
    {
        destoryTime -= Time.deltaTime;

        if (destoryTime <= 0 )
        {
            Destroy( this.gameObject);
        }
    }
}
