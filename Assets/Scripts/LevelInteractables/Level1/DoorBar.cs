using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class DoorBar : MonoBehaviour
{
    [SerializeField]
    PressKeyOpenDoor[] doors;

    Interactable interactable;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        boxCollider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        interactable.SetAction(Interact);
    }

    private void Interact()
    {
        boxCollider.enabled = false;
        meshRenderer.enabled = false;

        if(doors != null && doors.Length > 0) 
        {
            for(int i = 0; i < doors.Length; i++)
            {
                doors[i].ForceOpen();
            }
        }
    }
}
