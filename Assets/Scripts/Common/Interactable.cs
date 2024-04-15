using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public static float InteractionPercentage = 0f;
    public static bool IsInteracting = false;
    public static string InteractionText = "";

    private Action interactionAction;
    private Action endAction;

    private Coroutine interactCoroutine;

    [SerializeField] bool timeInteraction = false;
    [SerializeField] float interactionTime = 1f;
    [SerializeField] string interactionText = "";

    [SerializeField] public bool AllowInteraction = true;

    private float currentTime = 0f;
    public void Interract()
    {
        if (!AllowInteraction) return;
        InteractionPercentage = 0;
        if (timeInteraction)
        {
            interactCoroutine = StartCoroutine(timeInteract());
            InteractionText = interactionText;
            IsInteracting = true;
        }
        else
        {
            interactionAction?.Invoke();
        }
    }

    public void EndInterraction()
    {
        endAction?.Invoke();
        currentTime = 0f;
        IsInteracting = false;
        if (timeInteraction)
        {
            StopCoroutine(interactCoroutine);
        }
    }

    public void SetAction(Action action)
    {
        interactionAction = action;
    }

    private IEnumerator timeInteract()
    {
        while (currentTime < interactionTime)
        {
            yield return null;
            currentTime += Time.deltaTime;
            InteractionPercentage = currentTime / interactionTime * 100;
            if(InteractionPercentage > 100) InteractionPercentage = 100;
        }
        IsInteracting = false;
        interactionAction?.Invoke();
    }

    public void SetEndAction(Action action)
    {
        endAction = action;
    }
}
