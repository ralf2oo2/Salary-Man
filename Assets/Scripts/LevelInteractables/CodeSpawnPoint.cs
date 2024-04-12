using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeSpawnPoint : MonoBehaviour
{
    public static List<CodeSpawnPoint> codeSpawnPoints = new List<CodeSpawnPoint>();
    public Vector3 spawnPoint;
    void Start()
    {
        codeSpawnPoints.Add(this);
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
