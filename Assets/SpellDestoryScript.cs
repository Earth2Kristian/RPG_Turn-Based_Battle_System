using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDestoryScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
       
    }
}
