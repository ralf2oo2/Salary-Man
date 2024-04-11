using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class ObjectiveChangeArea : MonoBehaviour
{
    [SerializeField] string objective;
    private void OnTriggerEnter(Collider other)
    {
        ObjectiveManager.SetObjective(objective);
        Destroy(gameObject);
    }
}
