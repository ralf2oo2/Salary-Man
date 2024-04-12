using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class EnemyAwarenessIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rTransform;
    float originalHeight;
    void Start()
    {
        rTransform = GetComponent<RectTransform>();
        originalHeight = rTransform.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        float detection = EnemyAwareness.GetGlobalPlayerDetection();
        if (detection > 100) detection = 100;
        rTransform.sizeDelta = new Vector2(rTransform.sizeDelta.x, originalHeight * (detection / 100));
    }
}
