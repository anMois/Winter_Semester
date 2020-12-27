﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundScroll : MonoBehaviour
{
    public float speed;
    public float checkNum;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

    float viewHeight;

    private void Update()
    {
        //move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.right * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.x > checkNum)
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.left * checkNum;

            int startIndexNum = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexNum - 1 == -1) ? sprites.Length - 1 : startIndexNum - 1;
        }
    }
}
