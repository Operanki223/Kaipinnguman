using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Parallax : MonoBehaviour
{
    private float speed = 1;

    void Update()
    {
        transform.position -= new Vector3(0, Time.deltaTime * speed);

        if (transform.position.y <= -11)
        {
            transform.position = new Vector3(0, 21.1f);
        }
    }
}