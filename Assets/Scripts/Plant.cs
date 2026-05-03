using UnityEngine;

public class Plant : MonoBehaviour
{
    public GameObject plant;
    public enum State
    {
        NotGrowing,
        Growing,
    }
    public State state;
    public LevelLoader loader;
    public AudioSource water;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loader = LevelLoader.FindAnyObjectByType<LevelLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.NotGrowing:
                water.volume = 0;
                break;
            case State.Growing:
                Growing();
                break;
        }

        if (plant.transform.localPosition.y >= 5.36f)
        {
            loader.LoadNextLevel("AfterThePlant");
        }
    }

    private void Growing()
    {
        plant.transform.position = new Vector2(plant.transform.position.x, plant.transform.position.y + 0.015f);
        water.volume = 1;
    }
}
