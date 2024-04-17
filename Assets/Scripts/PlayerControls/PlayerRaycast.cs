using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerRaycast : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;
    PlayerInput playerInput;
    InputAction useAction;

    Interactable currentInteractable;

    private bool pressedUse = false;

    private void Start()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        useAction = playerInput.actions["use"];
    }

    private void Update()
    {
        if(useAction.ReadValue<float>() > 0f)
        {
            if(!pressedUse)
            {
                if (PlayerPickupManager.HasPickupItem())
                {
                    GameObject pickupItem = PlayerPickupManager.GetPickupItem();
                    pickupItem.transform.parent = null;
                    Rigidbody rb = pickupItem.AddComponent<Rigidbody>();
                    rb.isKinematic = false;
                    rb.constraints = RigidbodyConstraints.FreezeRotation;
                    rb.AddForce(Camera.forward * 200);
                    PlayerPickupManager.SetPickupItem(null);
                    Debug.Log("throw");
                }
                if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
                {
                    if (hit.collider.TryGetComponent<door>(out door door))
                    {
                        if (door.IsOpen)
                        {
                            door.Close();
                        }
                        else
                        {
                            door.Open(transform.position);
                        }
                    }
                    else if (hit.collider.TryGetComponent<Interactable>(out Interactable interactable))
                    {
                        if(interactable.AllowInteraction)
                        {
                            if(!interactable.checkState || StateManager.states.ContainsKey(interactable.State) && StateManager.states[interactable.State])
                            {
                                currentInteractable = interactable;
                                interactable.Interract();
                                Debug.Log("intera");
                            }
                        }
                    }
                }
                pressedUse = true;
            }
        }
        else
        {
            if(pressedUse && currentInteractable != null)
            {
                currentInteractable.EndInterraction();
                currentInteractable = null;
            }
            pressedUse = false;
        }
    }
}