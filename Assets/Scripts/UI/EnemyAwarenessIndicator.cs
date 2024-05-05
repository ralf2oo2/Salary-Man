using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class EnemyAwarenessIndicator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] RectTransform DetectionBar;
    TextMeshProUGUI tmp;
    float originalHeight;
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        originalHeight = DetectionBar.sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        float detection = EnemyAwareness.GetGlobalPlayerDetection();
        if (detection > 100) detection = 100;
        tmp.text = detection.ToString("0");
        DetectionBar.sizeDelta = new Vector2(DetectionBar.sizeDelta.x, originalHeight * (detection / 100));
    }
}
