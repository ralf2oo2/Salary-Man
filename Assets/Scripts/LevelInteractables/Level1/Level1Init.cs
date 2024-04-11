using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Init : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ObjectiveManager.SetObjective("Try to find a way into the building");
        GameObject.FindGameObjectWithTag("Code").transform.position = CodeSpawnPoint.codeSpawnPoints[Random.Range(0, CodeSpawnPoint.codeSpawnPoints.Count - 1)].transform.position;
    }
}
