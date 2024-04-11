using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    private Action interactionAction;
    public void Interract()
    {
        interactionAction?.Invoke();
    }

    public void SetAction(Action action)
    {
        interactionAction = action;
    }
}
