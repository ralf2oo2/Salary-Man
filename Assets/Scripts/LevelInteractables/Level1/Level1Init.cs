using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Init : MonoBehaviour
{
    private bool placedCode = false;
    // Start is called before the first frame update
    void Start()
    {
        ObjectiveManager.SetObjective("Try to find a way into the building");
        StateManager.states.Clear();
    }

    private IEnumerator SetCodeLocation()
    {
        yield return new WaitForSeconds(5);
        if(this != null)
        {
            GameObject.FindGameObjectWithTag("Code").transform.position = CodeSpawnPoint.codeSpawnPoints[Random.Range(0, CodeSpawnPoint.codeSpawnPoints.Count - 1)].transform.position;
        }
    }

    private void Update()
    {
        if(CodeSpawnPoint.codeSpawnPoints.Count > 0 && !placedCode)
        {
            placedCode = true;
            StartCoroutine(SetCodeLocation());
        }
    }
}
