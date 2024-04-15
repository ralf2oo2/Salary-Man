using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerRaycast : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;

    public void OnUse()
    {
        Debug.Log("intera2");
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
                interactable.Interract();
                Debug.Log("intera");
            }
        }
    }

    private void Update()
    {

    }
}