using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public int moveSpeed;

    float xSpeed;
    Transform tran;

    private void Awake()
    {
        tran = GetComponent<Transform>();
        xSpeed = moveSpeed * Time.deltaTime;
    }

    private void Update()
    {
        //Move
        tran.position += new Vector3(1, 0) * xSpeed;
    }
}
