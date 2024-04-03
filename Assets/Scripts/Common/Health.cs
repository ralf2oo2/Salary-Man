using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDeath;

    public float health = 100;

    private void Update()
    {
        if(health < 0)
        {
            OnDeath?.Invoke();
        }
    }
}
