using System.Collections;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public float timer;
    public int appleState = 0;
    public Animator anim;
    private Rigidbody2D body;
    private BoxCollider2D boxCollider;
    public bool dead = false;

    [SerializeField] private AudioClip landSound;
    private bool noiseBool = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //timer = Random.Range(1, 3);
        timer = 1f;
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && appleState < 3)
        {
            //timer = Random.Range(1, 3);
            timer = 1f;
            if (appleState == 0)
            {
                anim.SetTrigger("growing1");
            }
            if (appleState == 1)
            {
                anim.SetTrigger("growing2");
            }
            if (appleState == 2)
            {
                anim.SetTrigger("growing3");
            }
            appleState++;
        }

        if (timer <= 0 && appleState == 3)
        {
            body.bodyType = RigidbodyType2D.Dynamic;
            transform.Rotate(Vector3.forward * (180 * Time.deltaTime));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        body.bodyType = RigidbodyType2D.Static;
        PlayNoise();
        StartCoroutine(waiterDrop(1f));
    }

    private void PlayNoise()
    {
        if (noiseBool == true)
        {
            //SoundManager.instance.PlaySound(landSound);
            noiseBool = false;
        }
    }

    IEnumerator waiterDrop(float duration)
    {
        yield return new WaitForSeconds(0.05f);
        dead = true;
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            renderer.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }
        Destroy(gameObject);
    }
}
