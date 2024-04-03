using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHoleManager : MonoBehaviour
{
    [SerializeField] GameObject bulletHolePrefab;
    [SerializeField] GameObject bulletHoleContainer;
    Ray currentRay;
    RaycastHit currentHit;

    // Update is called once per frame
    void Update()
    {
        
    }
}
