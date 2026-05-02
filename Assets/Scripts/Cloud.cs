using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(transform.position.x - speed, transform.position.y);

        if (transform.position.x <= -23)
        {
            transform.position = new Vector2(23, transform.position.y);
        }
    }
}
