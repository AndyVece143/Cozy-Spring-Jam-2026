using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Player player;
    public CameraController mainCamera;
    public SoloBigDialogue cutscene;
    private bool triggered = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && triggered == false)
        {
            player.StopMoving(2);
            player.state = Player.State.NoMove;
            mainCamera.anim.enabled = true;
            mainCamera.anim.SetTrigger("cutscene1");
            triggered = true;
            StartCoroutine(CutsceneWaiter());
        }
    }

    private IEnumerator CutsceneWaiter()
    {
        yield return new WaitForSeconds(2);
        SoloBigDialogue newDialogue = Instantiate(cutscene);
    }
}
