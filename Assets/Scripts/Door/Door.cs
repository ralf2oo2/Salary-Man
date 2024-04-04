using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class door : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField]
    private bool IsRotatingDoor = true;
    [SerializeField]
    private float speed = 1f;
    [Header("Rotation Conifgs")]
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;

    private Vector3 StartRotation;
    private Vector3 Forward;

    private Coroutine AnimatedCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        Forward = transform.right;
    }

    public void Open(Vector3 UserPosition)
    {
        if(!IsOpen)
        {
            if(AnimatedCoroutine != null)
            {
                StopCoroutine(AnimatedCoroutine);
            }
            if(IsRotatingDoor) 
            {
                float dot = Vector3.Dot(Forward,(UserPosition - transform.position).normalized);
                Debug.Log($"Dot:{dot.ToString("N3")}");
                AnimatedCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
        }
    }
    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if(ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, startRotation.y + RotationAmount, 0));
        }
        IsOpen = true;

        float time = 0;
        while(time <1) 
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime;
        }
    }
    public void Close()
    {
        if(IsOpen)
        {
            if(AnimatedCoroutine!=null)
            {
                StopCoroutine(AnimatedCoroutine);
            }
            if(IsRotatingDoor) 
            {
                AnimatedCoroutine = StartCoroutine(DoRotationClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while(time <1) 
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * speed;
        }
    }
}

