using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI tmp;
    Health health;
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = health.health.ToString();
    }
}
