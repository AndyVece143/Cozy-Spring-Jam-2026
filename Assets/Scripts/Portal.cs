using UnityEngine;

public class Portal : MonoBehaviour
{
    public string sceneName;
    public Player player;
    public BoxCollider2D boxCollider;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && triggered == false)
        {
            player.state = Player.State.Float;
            StartCoroutine(player.Floating(transform.position, sceneName));
            triggered = true;
        }
    }
}
