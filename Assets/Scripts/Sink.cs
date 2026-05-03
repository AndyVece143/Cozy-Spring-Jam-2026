using Unity.VisualScripting;
using UnityEngine;

public class Sink : MonoBehaviour
{
    private Animator anim;
    public enum State
    {
        Off,
        On,
    }
    public State state;
    public AudioSource water;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Off:
                Off();
                break;
            case State.On:
                On();
                break;
        }
    }

    private void Off()
    {
        water.volume = 0;
        anim.SetBool("refill", false);

    }

    private void On()
    {
        water.volume = 1;
        anim.SetBool("refill", true);

    }
}
