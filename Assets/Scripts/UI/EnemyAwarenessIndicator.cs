using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAwarenessIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float awareness = (EnemyAwareness.GetGlobalPlayerAwareness() / EnemyAwareness.awarenessThreshold * 100);
        if (awareness > 100) awareness = 100;
        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = awareness.ToString("0") +"%";
    }
}
