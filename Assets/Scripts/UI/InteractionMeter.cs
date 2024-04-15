using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionMeter : MonoBehaviour
{
    Image radialImage;
    TextMeshProUGUI tmp;
    PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        radialImage = GetComponentInChildren<Image>();
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        string text = "";
        if (Interactable.IsInteracting)
        {
            text = Interactable.InteractionText.Replace("{InteractionButton}", playerInput.actions["use"].GetBindingDisplayString());
            tmp.text = text;
        }
        radialImage.enabled = Interactable.IsInteracting;
        tmp.enabled = Interactable.IsInteracting;
        Debug.Log(Mathf.Clamp(Interactable.InteractionPercentage, 0f, 1f));
        radialImage.fillAmount = Interactable.InteractionPercentage / 100;
    }
}
