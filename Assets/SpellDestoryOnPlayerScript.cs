using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDestoryOnPlayerScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }

    }
}
