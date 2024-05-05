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

    private Hashtable visibleTargets = new Hashtable();

    void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    public bool CanSeeTarget(int targetInstanceId)
    {
        return visibleTargets.Keys.Cast<int>().Contains(targetInstanceId);
    }

    public Collider GetColliderFromInstanceID(int instanceId)
    {
        return visibleTargets[instanceId] as Collider;
    }

    public int GetPlayerInstanceId()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().GetInstanceID();
    }

    public bool CanSeePlayer()
    {
        return visibleTargets.ContainsKey(GetPlayerInstanceId());
    }

    public int VisibleTargetCount()
    {
        return visibleTargets.Count;
    }

    public int[] getVisibleTargets()
    {
        return visibleTargets.Keys.Cast<int>().ToArray();
    }

    public object GetVisibleTarget(int targetInstanceId)
    {
        return visibleTargets[targetInstanceId];
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
        if (visibleTargets.ContainsKey(collider.GetInstanceID())) return;
        visibleTargets.Add(collider.GetInstanceID(), collider);
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        List<int> visibleTargetInstanceIds = visibleTargets.Keys.Cast<int>().ToList();

        foreach (int instanceId in visibleTargetInstanceIds.Where(item => rangeChecks.Where(x => x.GetInstanceID() == item).FirstOrDefault() == null))
        {
            if (visibleTargets.Contains(instanceId)){
                visibleTargets.Remove(instanceId);
            }
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
                        if (!visibleTargets.Contains(collider.GetInstanceID()))
                        {
                            AddVisibleObject(collider);
                        }
                    }
                    else if (visibleTargets.Contains(collider.GetInstanceID()))
                    {
                        visibleTargets.Remove(collider.GetInstanceID());
                    }
                }
                else if (visibleTargets.Contains(collider.GetInstanceID()))
                {
                    visibleTargets.Remove(collider.GetInstanceID());
                }
            }
        }
    }
}
