using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UIElements.Image;

public class SoloBigDialogueTrigger : MonoBehaviour
{
    public SoloBigDialogue bigDialogue;
    public BoxCollider2D boxCollider;
    public Player player;
    public bool triggered;
    public string[] lines;
    public int react;
    public int[] emotionChanges;

    public BigDialogueSprite char1;
    public CameraController mainCamera;
    public bool sceneTransition;
    public string sceneName;
    private float dialogueTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Player.FindAnyObjectByType<Player>();
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        triggered = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (triggered)
        {
            return;
        }

        if (collision.tag == "Player" && player.IsGrounded())
        {
            dialogueTimer += Time.deltaTime;

            if (dialogueTimer > 0.1)
            {
                mainCamera.state = CameraController.State.StayStill;

                player.StopMoving(react);
                player.state = Player.State.NoMove;
                SoloBigDialogue newBigDialogue = Instantiate(bigDialogue);
                newBigDialogue.lines = lines;
                newBigDialogue.emotionChanges = emotionChanges;

                newBigDialogue.character1.anim = newBigDialogue.character1.GetComponent<Animator>();
                newBigDialogue.character1.anim.runtimeAnimatorController = char1.anim.runtimeAnimatorController;
                newBigDialogue.character1.image = char1.image;

                newBigDialogue.sceneTransition = sceneTransition;
                newBigDialogue.sceneName = sceneName;
                triggered = true;
            }
        }
    }
}
