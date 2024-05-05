using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    PlayerInput playerInput; 
    public static Action shootInput;
    public static Action reloadInput;
    public static Action triggerUpInput;

    private InputAction shootAction;
    private InputAction reloadAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["fire"];
        reloadAction = playerInput.actions["reload"];
    }

    // Update is called once per frame
    private void Update()
    {
        if (shootAction.ReadValue<float>() > 0)
        {
            shootInput?.Invoke();
        }
        else
        {
            triggerUpInput?.Invoke();
        }

        if (reloadAction.ReadValue<float>() > 0) 
        {
            reloadInput?.Invoke();
        }
    }
}
