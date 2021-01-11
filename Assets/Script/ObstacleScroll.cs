using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScroll : MonoBehaviour
{
    public float checkNum;
    public int startIndex;
    public int endIndex;
    public GameObject[] sprites;
    public bool isWideCheck;

    int maxIndex;
    int minIndex;

    public Vector3[] spritelist;

    private void Awake()
    {
        maxIndex = startIndex;
        minIndex = endIndex;
    }

    private void Start()
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            spritelist[i] = sprites[i].transform.position;
        }
    }

    private void Update()
    {
        if (sprites[endIndex].transform.position.x < checkNum * (-1))
        {
            if (sprites[endIndex].activeInHierarchy == false)
                sprites[endIndex].SetActive(true);


            Debug.Log(sprites[endIndex].transform.position);

            sprites[endIndex].transform.localPosition = spritelist[endIndex] + Vector3.right;

            Debug.Log(spritelist[endIndex]);

            //Vector3 backSpritePos = sprites[startIndex].transform.localPosition;
            //Vector3 frontSpritePos = sprites[endIndex].transform.localPosition;
            //if (isWideCheck)
            //    sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right * (checkNum / 2.0f);
            //else
            //    sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right;

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
