using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(FieldOfView))]

public class EnemyAwareness : MonoBehaviour
{
    public event Action OnAlerted;

    FieldOfView fieldOfView;

    private Dictionary<int, float> awareness;
    private float alertnessThreshold = 40;
    private bool alerted = false;

    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
        awareness = new Dictionary<int, float>();
    }

    void Update()
    {
        UpdateAlertness();
    }

    private void UpdateAlertness()
    {

        int[] visibleTargets = fieldOfView.getVisibleTargets();
        int[] awareTargets = awareness.Keys.Cast<int>().ToArray();

        int[] nonAwareTargets = visibleTargets.Where(x => !awareTargets.Contains(x)).ToArray();

        foreach (int target in nonAwareTargets)
        {
            awareness.Add(target, 0);
        }

        foreach (int key in awareTargets)
        {
            if (fieldOfView.CanSeeTarget(key))
            {
                awareness[key] += 10 * Time.deltaTime;
            }
            else
            {
                awareness[key] -= 10 * Time.deltaTime;
            }
            if (awareness[key] < 0)
            {
                awareness.Remove(key);
            }
            if (awareness[key] > alertnessThreshold && !alerted)
            {
                OnAlerted();
                alerted = true;
            }
            Debug.Log(awareness[key]);
        }
    }
}
