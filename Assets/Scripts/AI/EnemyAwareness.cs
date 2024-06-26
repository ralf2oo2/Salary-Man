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
    private Dictionary<int, bool> detectionCooldown = new Dictionary<int, bool>();
    private Dictionary<int, float> prevFrameTime = new Dictionary<int, float>();
    private Dictionary<int, float> detectionPercentage = new Dictionary<int, float>();
    private bool alerted = false;
    private bool suspicious = false;

    void Start()
    {
        globalAwareness.Add(this);
        fieldOfView = GetComponent<FieldOfView>();
    }

    void Update()
    {
        UpdateAlertness();
    }

    public void RemoveFromGlobalAwareness()
    {
        globalAwareness.Remove(this);
    }

    public static float GetGlobalPlayerDetection()
    {
        return globalAwareness.Max(x => x.GetPlayerDetection());
    }

    public void MakeSuspicious()
    {
        suspicious = true;
    }

    public int GetPlayerInstanceId()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>().GetInstanceID();
    }

    public float GetPlayerDetection()
    {
        int playerInstanceId = GetPlayerInstanceId();
        if (detectionPercentage.ContainsKey(playerInstanceId))
        {
            float playerDetection = detectionPercentage[playerInstanceId];
            if(playerDetection > 100) playerDetection = 100;
            return playerDetection;
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

    public bool IsSuspicious()
    {
        return suspicious;
    }

    private IEnumerator StartCooldown(int id, float time)
    {
        detectionCooldown[id] = true;
        yield return new WaitForSeconds(time);
        detectionCooldown[id] = false;
    }

    private void UpdateAlertness()
    {

        int[] visibleTargets = fieldOfView.getVisibleTargets();
        int[] awareTargets = awareness.Keys.Cast<int>().ToArray();

        int[] nonAwareTargets = visibleTargets.Where(x => !awareTargets.Contains(x)).ToArray();

        foreach (int target in nonAwareTargets)
        {
            awareness.Add(target, 0);
            detectionCooldown.Add(target, false);
            prevFrameTime.Add(target, 0f);
            detectionPercentage.Add(target, 0f);
        }

        foreach (int key in awareTargets)
        {
            if (detectionCooldown[key] == true) 
            {
                continue;
            }
            float delta = 0;
            if (prevFrameTime[key] != 0f) 
            { 
                delta = Time.time - prevFrameTime[key];
            }
            float visionMultiplier = 0;
            float detectionTime = 0;
            prevFrameTime[key] = Time.time;
            if (!alerted)
            {
                if (fieldOfView.CanSeeTarget(key))
                {
                    if(fieldOfView.GetColliderFromInstanceID(key).gameObject.tag == "Player")
                    {
                        float distance = Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
                        //float distanceModifier = fieldOfView.radius - distance;
                        //distanceModifier += distanceModifier * 0.5f;
                        //distanceModifier *= 0.2f;



                        //if (distanceModifier < 1) distanceModifier = 1;



                        //visionMultiplier *= distanceModifier;

                        Vector3 directionToTarget = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
                        float detectionMultiplier = (float)(0.75 * distance / fieldOfView.radius + 0.25 * Vector3.Angle(transform.forward, directionToTarget) / (fieldOfView.angle / 2));

                        float minimumDetectionDelay = 0.2f;
                        float maximumDetectionDelay = 2.0f;

                        detectionTime = minimumDetectionDelay + (maximumDetectionDelay - minimumDetectionDelay) * detectionMultiplier;
                        StartCoroutine(StartCooldown(key, 0.1f));

                        Debug.Log(detectionTime);

                        visionMultiplier = 0;




                        PlayerCrouching crouching = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCrouching>();
                        if (crouching.IsCrouching)
                        {
                            //visionMultiplier *= 0.2f;
                        }
                        /*if (distance < 2f || suspicious)
                        {
                            awareness[key] = 100;
                            Debug.Log("seen");
                        }*/
                    }
                    else
                    {
                        float distance = Vector3.Distance(transform.position, (fieldOfView.GetVisibleTarget(key) as Collider).transform.position);
                        float distanceModifier = fieldOfView.radius - distance;
                        distanceModifier += distanceModifier * 0.5f;
                        distanceModifier *= 0.2f;

                        if (distanceModifier < 1) distanceModifier = 1;

                        visionMultiplier *= distanceModifier;

                    }
                    awareness[key] += delta;
                    detectionPercentage[key] = awareness[key] / detectionTime * 100;
                    if (detectionPercentage[key] > 100) detectionPercentage[key] = 100;
                }
                else
                {
                    awareness[key] -= 10 * delta;
                }
            }
            if (awareness[key] > detectionTime && !alerted)
            {
                if(key == GetPlayerInstanceId())
                {
                    alerted = true;
                    if (OnAlerted != null)
                    {
                        OnAlerted(fieldOfView.GetVisibleTarget(key));
                    }
                }
                else
                {
                    suspicious = true;
                }
            }
            if (!alerted)
            {
                //Debug.Log(awareness[key]);
            }
            if (awareness[key] < 0)
            {
                awareness.Remove(key);
                detectionCooldown.Remove(key);
                prevFrameTime.Remove(key);
                detectionPercentage.Remove(key);
            }
        }
    }
}
