using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class BodyPickup : MonoBehaviour
{
    Interactable interactable;
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Pickup);
    }

    public void Pickup()
    {
        if (PlayerPickupManager.HasPickupItem()) return;
        Debug.Log("interract");
        GameObject.Destroy(gameObject.transform.parent.gameObject.GetComponent<Rigidbody>());
        PlayerPickupManager.SetPickupItem(gameObject.transform.parent.gameObject);
        gameObject.transform.parent.gameObject.transform.parent = GameObject.FindGameObjectWithTag("ItemHolder").transform;
        gameObject.transform.parent.gameObject.transform.rotation = GameObject.FindGameObjectWithTag("ItemHolder").transform.rotation;
        gameObject.transform.parent.gameObject.transform.position = GameObject.FindGameObjectWithTag("ItemHolder").transform.position;
    }
}
