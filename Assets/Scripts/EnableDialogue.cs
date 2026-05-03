using Unity.VisualScripting;
using UnityEngine;

public class EnableDialogue : MonoBehaviour
{
    public Player player;
    public SoloBigDialogueTrigger dialogue;
    public BigDialogueTrigger dialogue2;
    private bool triggered = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && triggered == false)
        {
            dialogue.gameObject.SetActive(true);
            dialogue2.gameObject.SetActive(true);
            triggered = true;
        }
    }
}
