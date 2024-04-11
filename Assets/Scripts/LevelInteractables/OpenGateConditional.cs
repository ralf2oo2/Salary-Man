using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateConditional : MonoBehaviour
{
    [SerializeField] string state;
    [SerializeField] GameObject door;
    Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Interract);
    }

    private void Interract()
    {
        if (door != null)
        {
            if (StateManager.states.ContainsKey(state))
            {
                if (StateManager.states[state])
                {
                    GameObject.Destroy(door);
                }
            }
        }
    }
}
