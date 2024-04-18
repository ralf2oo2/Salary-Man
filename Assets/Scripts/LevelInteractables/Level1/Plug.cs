using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class Plug : MonoBehaviour
{
    [SerializeField] HeartrateMonitor monitor;
    [SerializeField] GameObject plug;
    Interactable interactable;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Interact);
    }

    void Interact()
    {
        monitor.Flatline();
        plug.SetActive(false);
    }
}
