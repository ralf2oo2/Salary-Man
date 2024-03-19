using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] public float radius;
    [Range(0, 360)]
    [SerializeField] public float angle;

    [SerializeField] public LayerMask targetMask;
    [SerializeField] public LayerMask obstructionMask;

    public event Action OnAlerted;

    private float alertnessThreshold = 40;
    private bool alerted = false;

    private Hashtable visibleObjects = new Hashtable();
    private Dictionary<int, float> alertness;



    void Start()
    {
        alertness = new Dictionary<int, float>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.2f;
        WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

        while (true)
        {
            yield return waitForSeconds;
            FieldOfViewCheck();

        }
    }

    private void AddVisibleObject(Collider collider)
    {
        if (visibleObjects.ContainsKey(collider.GetInstanceID())) return;
        visibleObjects.Add(collider.GetInstanceID(), collider);
        if (alertness.ContainsKey(collider.GetInstanceID())) return;
        alertness.Add(collider.GetInstanceID(), 0);
    }

    public bool CanSeeObject()
    {
        return visibleObjects.Count > 0;
    }

    public int[] getVisibleObjects()
    {
        return visibleObjects.Keys.Cast<int>().ToArray();
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        foreach (int instanceId in visibleObjects.Keys.Cast<int>().Where(item => rangeChecks.Where(x => x.GetInstanceID() == item).FirstOrDefault() == null))
        {
            visibleObjects.Remove(instanceId);
        }


        if(rangeChecks.Length > 0)
        {
            foreach(Collider collider in rangeChecks)
            {
                Transform target = collider.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2) 
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        if (!visibleObjects.Contains(collider.GetInstanceID()))
                        {
                            AddVisibleObject(collider);
                        }
                    }
                    else if (visibleObjects.Contains(collider.GetInstanceID()))
                    {
                        visibleObjects.Remove(collider.GetInstanceID());
                    }
                }
                else if (visibleObjects.Contains(collider.GetInstanceID()))
                {
                    visibleObjects.Remove(collider.GetInstanceID());
                }
            }
        }
    }

    private void UpdateAlertness()
    {
        List<int> toBeRemoved = new List<int>();
        int[] keys = alertness.Keys.ToArray();
        foreach (int key in keys)
        {
            if (visibleObjects.ContainsKey(key))
            {
                alertness[key] += 10 * Time.deltaTime;
            }
            else
            {
                alertness[key] -= 10 * Time.deltaTime;
            }
            if (alertness[key] < 0)
            {
                alertness.Remove(key);
            }
            if (alertness[key] > alertnessThreshold && !alerted)
            {
                OnAlerted();
                alerted = true;
            }
            Debug.Log(alertness[key] + "");
        }
        if(toBeRemoved.Count > 0)
        {
            foreach(int key in toBeRemoved)
            {
                alertness.Remove(key);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAlertness();
        //Debug.Log(visibleObjects.Count);
    }
}
