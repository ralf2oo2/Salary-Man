using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FieldOfView))]

public class EnemyAwareness : MonoBehaviour
{
    public static List<EnemyAwareness> globalAwareness = new List<EnemyAwareness>();
    public static float awarenessThreshold = 20;

    public event Action<System.Object> OnAlerted;

    FieldOfView fieldOfView;

    private Dictionary<int, float> awareness = new Dictionary<int, float>();
    private bool alerted = false;

    void Start()
    {
        globalAwareness.Add(this);
        fieldOfView = GetComponent<FieldOfView>();
    }

    void Update()
    {
        UpdateAlertness();
    }

    public static float GetGlobalPlayerAwareness()
    {
        return globalAwareness.Max(x => x.GetPlayerAwareness());
    }

    public float GetPlayerAwareness()
    {
        int playerInstanceId = GameObject.Find("Player").GetComponent<Collider>().GetInstanceID();
        if (awareness.ContainsKey(playerInstanceId))
        {
            float playerAwareness = awareness[playerInstanceId];
            if(playerAwareness > 100) playerAwareness = 100;
            return playerAwareness;
        }
        return 0;
    }

    public int AwareTargetCount()
    {
        return awareness.Count;
    }

    public bool IsAlerted()
    {
        return alerted;
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
            if (!alerted)
            {
                if (fieldOfView.CanSeeTarget(key))
                {
                    awareness[key] += 10 * Time.deltaTime;
                    if (awareness[key] > 100) awareness[key] = 100;
                }
                else
                {
                    awareness[key] -= 10 * Time.deltaTime;
                }
            }
            if (awareness[key] > awarenessThreshold && !alerted)
            {
                alerted = true;
                Debug.Log("alerted!");

                if (OnAlerted != null)
                {
                    OnAlerted(fieldOfView.GetVisibleTarget(key));
                }
            }
            if (!alerted)
            {
                //Debug.Log(awareness[key]);
            }
            if (awareness[key] < 0)
            {
                awareness.Remove(key);
            }
        }
    }
}
