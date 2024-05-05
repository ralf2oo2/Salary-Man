using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGateConditional : MonoBehaviour
{
    [SerializeField] PressKeyOpenDoor door;
    [SerializeField] Light EnabledLight;
    [SerializeField] Light DisabledLight;
    Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Interract);
    }

    private void Interract()
    {
        door.ForceOpen();
        DisabledLight.enabled = false;
        EnabledLight.enabled = true;
    }
}
