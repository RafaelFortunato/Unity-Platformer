using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.position += Vector3.up * Mathf.Cos(Time.time) * 0.003f;

        transform.forward = mainCamera.transform.forward;
    }
}
