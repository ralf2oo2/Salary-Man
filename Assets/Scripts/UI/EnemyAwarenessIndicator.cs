using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyAwarenessIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = (EnemyAwareness.GetGlobalPlayerAwareness() / EnemyAwareness.awarenessThreshold * 100).ToString("0");
    }
}
