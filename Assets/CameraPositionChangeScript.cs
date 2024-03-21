using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraPositionChangeScript : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    void Start()
    {
        cam1.SetActive(true);
        cam2.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            cam1.SetActive(false);
            cam2.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            cam1.SetActive(true);   
            cam2.SetActive(false);  
        }
    }
}
