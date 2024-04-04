using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAwareness))]
public class AwarenessVisualizer : MonoBehaviour
{
    [SerializeField] public GameObject anchor;
    [SerializeField] public float verticalOffset = 2;

    private EnemyAwareness enemyAwareness;
    private GameObject billboard;
    private Texture suspiciousTexture;
    private Texture alertTexture;
    // Start is called before the first frame update
    void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();

        billboard = GameObject.CreatePrimitive(PrimitiveType.Quad);
        billboard.GetComponent<Renderer>().material = Resources.Load("Materials/Billboard") as Material;
        billboard.transform.parent = anchor.transform;
        billboard.transform.localPosition = new Vector3(0, verticalOffset, 0);
        billboard.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);


        suspiciousTexture = Resources.Load<Texture2D>("Textures/Interface/suspicious");
        alertTexture = Resources.Load<Texture2D>("Textures/Interface/alert");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyAwareness.AwareTargetCount() > 0 && !enemyAwareness.IsAlerted() && !enemyAwareness.IsSuspicious())
        {
            billboard.GetComponent<Renderer>().material.SetTexture("_MainTex", suspiciousTexture);
            billboard.GetComponent<Renderer>().enabled = true;
        }
        else if (enemyAwareness.IsAlerted() || enemyAwareness.IsSuspicious())
        {
            billboard.GetComponent<Renderer>().material.SetTexture("_MainTex", alertTexture);
            billboard.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            billboard.GetComponent<Renderer>().enabled = false;
        }
    }
}
