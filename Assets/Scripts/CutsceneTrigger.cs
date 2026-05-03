using System.Collections;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public Player player;
    public CameraController mainCamera;
    public SoloBigDialogue cutscene;
    private bool triggered = false;
    public int cutsceneNumber;
    private float dialogueTimer;

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && triggered == false && cutsceneNumber == 1 && player.IsGrounded())
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer > 0.1f)
            {
                player.StopMoving(2);
                player.state = Player.State.NoMove;
                mainCamera.anim.enabled = true;
                mainCamera.anim.SetTrigger("cutscene1");
                triggered = true;
                StartCoroutine(CutsceneWaiter(3.5f));
            }

        }

        if (collision.tag == "Player" && triggered == false && cutsceneNumber == 2 && player.IsGrounded())
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer > 0.1f)
            {
                player.StopMoving(2);
                player.state = Player.State.NoMove;
                mainCamera.anim.enabled = true;
                mainCamera.anim.SetTrigger("cutscene2");
                triggered = true;
                StartCoroutine(CutsceneWaiter(3.5f));
            }

        }
    }

    private IEnumerator CutsceneWaiter(float timing)
    {
        yield return new WaitForSeconds(timing);
        SoloBigDialogue newDialogue = Instantiate(cutscene);
    }
}
