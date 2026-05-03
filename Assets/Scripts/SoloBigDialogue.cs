using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SoloBigDialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index;
    public int[] emotionChanges;

    public Player player;
    public Canvas canvas;

    public BigDialogueSprite character1;

    public float duration;
    public float moveDuration;

    private Vector3 character1Position;
    public GameObject textBox;
    private Vector3 textBoxPosition;

    private Vector3 character1EndPosition;
    private Vector3 textBoxEndPosition;

    public AudioClip audioClip;

    public CameraController mainCamera;

    private const string HTML_ALPHA = "<color=#00000000>";
    public bool ready = false;

    public bool sceneTransition;
    public LevelLoader loader;
    public string sceneName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        player = Player.FindAnyObjectByType<Player>();
        mainCamera = CameraController.FindAnyObjectByType<CameraController>();
        textComponent.text = string.Empty;
        if (sceneTransition)
        {
            loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
        }

        BeginningSprite();
        SetPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            if (ready == true)
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        ready = false;
        if (index < lines.Length - 1)
        {
            index++;

            //if (talkChanges[index] == true)
            //{
            //    ChangeBothSprites();
            //}
            ChangeEmotion();
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            StartCoroutine(MoveSpritesEnd());
            //player.state = player.initialState;
            //Destroy(gameObject);
        }
    }

    //If a character is not set to talk in the beginning, they get resized
    void BeginningSprite()
    {
        //if (!character1.isActiveSpeaker)
        //{
        //    StartCoroutine(ChangeSprite(false, character1));

        //}

        //if (!character2.isActiveSpeaker)
        //{
        //    StartCoroutine(ChangeSprite(false, character2));
        //}
        ChangeEmotion();
    }

    void ChangeEmotion()
    {
        character1.ChangeEmotion(emotionChanges[index]);
    }

    void SetPositions()
    {
        character1Position = character1.transform.position;
        textBoxPosition = textBox.transform.position;

        character1.transform.position = new Vector3(character1Position.x - 14f, character1Position.y, character1Position.z);
        textBox.transform.position = new Vector3(textBoxPosition.x, textBoxPosition.y - 4.5f, textBoxPosition.z);

        character1EndPosition = character1.transform.position;
        textBoxEndPosition = textBox.transform.position;

        StartCoroutine(MoveSpritesBeginning());
    }

    IEnumerator TypeLine()
    {
        int i = 4;
        string originalText = lines[index];
        string displayedText = "";
        int alphaIndex = 0;

        foreach (char c in lines[index].ToCharArray())
        {
            alphaIndex++;
            textComponent.text = originalText;
            displayedText = textComponent.text.Insert(alphaIndex, HTML_ALPHA);
            textComponent.text = displayedText;

            i++;
            if (i == 5)
            {
                SoundManager.instance.PlaySound(audioClip);
                i = 0;
            }

            yield return new WaitForSeconds(textSpeed);
        }
        ready = true;
    }

    IEnumerator MoveSpritesBeginning()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1Position, time / moveDuration);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxPosition, time / moveDuration);
            yield return null;
        }
        StartDialogue();
    }

    IEnumerator MoveSpritesEnd()
    {
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.deltaTime;
            character1.gameObject.transform.position = Vector3.Lerp(character1.gameObject.transform.position, character1EndPosition, time / moveDuration);
            textBox.transform.position = Vector3.Lerp(textBox.transform.position, textBoxEndPosition, time / moveDuration);
            yield return null;
        }
        if (sceneTransition)
        {
            loader.LoadNextLevel(sceneName);
        }



        //gameUI.SetActive(true);

        Destroy(gameObject);
        player.state = Player.State.Standard;
        mainCamera.state = CameraController.State.FollowPlayer;
        mainCamera.anim.enabled = false;
    }
}
