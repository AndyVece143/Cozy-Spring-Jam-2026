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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.NotGrowing:
                break;
            case State.Growing:
                Growing();
                break;
        }
    }

    private void Growing()
    {
        plant.transform.position = new Vector2(plant.transform.position.x, plant.transform.position.y + 0.025f);
    }
}
