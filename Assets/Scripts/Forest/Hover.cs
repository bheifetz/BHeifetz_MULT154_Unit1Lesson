using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    //private float moveLength;
    private float speed = 1.0f;
    private const int TIMESTEPS = 30;
    private const float DELTATIME = 0.05f;

    void Start()
    {
        float moveLength = Random.Range(0.75f, 1.5f);
        StartCoroutine(HoverMovement(moveLength));
    }

    private IEnumerator HoverMovement(float length)
    {

        while (true)
        {
            float translationYSum = 0;
            while(translationYSum < length)
            {
                float translationY = (length / TIMESTEPS) * speed;
                transform.Translate(0, translationY, 0);
                translationYSum += translationY;
                yield return new WaitForSeconds(DELTATIME);
            }
            while (translationYSum > 0)
            {
                float translationY = (length / TIMESTEPS) * speed;
                transform.Translate(0, -translationY, 0);
                translationYSum -= translationY;
                yield return new WaitForSeconds(DELTATIME);
            }

        }
    }
}
