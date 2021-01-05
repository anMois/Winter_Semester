using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImagesScroll : MonoBehaviour
{
    public float speed;
    public float checkNum;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;
    public bool isWideCheck;

    int maxIndex;
    int minIndex;

    private void Awake()
    {
        maxIndex = startIndex;
        minIndex = endIndex;
    }

    private void Update()
    {
        //move
        Vector3 curPos = transform.position;
        Vector3 nextPos = Vector3.left * speed * Time.deltaTime;
        transform.position = curPos + nextPos;

        if (sprites[endIndex].position.x < checkNum*(-1))
        {
            Vector3 backSpritePos = sprites[startIndex].localPosition;
            Vector3 frontSpritePos = sprites[endIndex].localPosition;
            if(isWideCheck)
                sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right * (checkNum / 2.0f);
            else
                sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right * checkNum;

            checkIndex();
        }
    }

    void checkIndex()
    {
        //10 0, 0 1, 1 2, 2 3, 3 4, 4 5, 5 6, 6 7, 7 8, 8 9, 9 10, 10 0
        int startIndexNum = startIndex;
        startIndex = endIndex;

        if (startIndexNum == maxIndex)
            endIndex = minIndex + 1;
        else if (startIndexNum == minIndex)
            endIndex = minIndex + 2;
        else if (startIndex == maxIndex)
            endIndex = minIndex;
        else
            endIndex = startIndex + 1;
    }
}
