using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactable))]

public class OpenGateButton : MonoBehaviour
{
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
        if(door != null)
        {
            GameObject.Destroy(door);
        }
    }
}
