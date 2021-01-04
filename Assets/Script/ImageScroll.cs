using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageScroll : MonoBehaviour
{
    public float speed;
    public float checkNum;
    public int startIndex;
    public int endIndex;
    public Transform[] sprites;

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
            sprites[endIndex].transform.localPosition = backSpritePos + Vector3.right * checkNum;

            int startIndexNum = startIndex;
            startIndex = endIndex;
            endIndex = (startIndexNum - 1 == -1) ? sprites.Length - 1 : startIndexNum - 1;
        }
    }
}
