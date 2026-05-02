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
        anim.SetBool("refill", false);
    }

    private void On()
    {
        anim.SetBool("refill", true);
    }
}
