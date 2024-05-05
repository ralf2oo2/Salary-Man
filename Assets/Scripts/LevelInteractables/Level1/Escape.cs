using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Escape : MonoBehaviour
{
    Interactable interactable;
    [SerializeField] RestartMenu restartMenu;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Interact);
    }

    void Interact()
    {
        restartMenu.ShowScreen();
    }
}
