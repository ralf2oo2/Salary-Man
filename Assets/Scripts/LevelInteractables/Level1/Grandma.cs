using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Health))]
public class Grandma : MonoBehaviour
{
    Health health;
    [SerializeField] HeartrateMonitor monitor;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if(health.health <= 0 && !isDead)
        {
            isDead = true;
            monitor.Flatline();
        }
    }
}
