using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class CodePaper : MonoBehaviour
{
    Interactable interactable;
    void Start()
    {
        interactable = GetComponent<Interactable>();
        interactable.SetAction(Interract);
    }

    private void Interract()
    {
        StateManager.states.Add("code", true);
        ObjectiveManager.SetObjective("Use the codepad");
        GameObject.Destroy(interactable.gameObject);
    }

}
