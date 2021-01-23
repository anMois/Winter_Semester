using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Statu_Fountain : MonoBehaviour
{
    public float Find_x;
    public GameObject Fountain;
    public GameObject Statue;

    bool isCheck;

    private void Awake()
    {
        Fountain.SetActive(true);
        Statue.SetActive(false);
        isCheck = false;
    }

    private void Update()
    {
        //서로 번갈아 가면서 나왔다 들어갔다
        if (transform.position.x < Find_x * (-1))
        {
            if (isCheck)
                return;

            isCheck = true;
            Change();
        }

        if (transform.position.x > Find_x)
            isCheck = false;
    }

    void Change()
    {
        if (Fountain.activeInHierarchy == true)
        {
            Fountain.SetActive(false);
            Statue.SetActive(true);
        }
        else
        {
            Fountain.SetActive(true);
            Statue.SetActive(false);
        }
    }
}
