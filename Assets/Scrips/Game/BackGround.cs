using Unity.VisualScripting;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private float scrollSpeed_x = 0.0f;
    [SerializeField] private float scrollSpeed_y = 2f;

    void Start()
    {

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position -= new Vector3(Time.deltaTime * scrollSpeed_x, Time.deltaTime * scrollSpeed_y);

        if (transform.position.y <= -11)
        {
            transform.position = new Vector3(0, 21.1f);
        }

        if (transform.position.x <= -18.4f)
        {
            transform.position = new Vector3(18.4f, 0);
        }
    }
}