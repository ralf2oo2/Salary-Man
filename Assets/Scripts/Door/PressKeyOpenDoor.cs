using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class PressKeyOpenDoor : MonoBehaviour
{
    //public GameObject Instruction;
    public GameObject AnimeObject;
    //public GameObject ThisTrigger;
    public AudioSource DoorOpenSound;

    private Interactable interactable;

    void Start()
    {
        //Instruction.SetActive(false);
        interactable = GetComponent<Interactable>();
        interactable.SetAction(OpenDoor);
    }

    private void OpenDoor()
    {
        interactable.AllowInteraction = false;
        Debug.Log("open door2");
        //Instruction.SetActive(false);
        AnimeObject.GetComponent<Animator>().Play("DoorOpen");
        //ThisTrigger.SetActive(false);
        DoorOpenSound.Play();
        Debug.Log("open door");
    }

    public void ForceOpen()
    {
        OpenDoor();
    }

    /*void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Player")
        {
            Instruction.SetActive(true);
            Action = true;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        Instruction.SetActive(false);
        Action = false;
    }*/


/*    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
        }

    }*/
}


