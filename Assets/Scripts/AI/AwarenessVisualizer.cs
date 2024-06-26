using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[RequireComponent(typeof(EnemyAwareness))]
public class AwarenessVisualizer : MonoBehaviour
{
    [SerializeField] public GameObject anchor;
    [SerializeField] public float verticalOffset = 2;

    internal bool enabled = true;

    private EnemyAwareness enemyAwareness;
    private GameObject billboard;
    private Texture suspiciousTexture;
    private Texture alertTexture;
    // Start is called before the first frame update
    void Start()
    {
        enemyAwareness = GetComponent<EnemyAwareness>();

        billboard = PrimitiveHelper.CreatePrimitive(PrimitiveType.Quad, false);
        billboard.GetComponent<Renderer>().material = Resources.Load("Materials/Billboard") as Material;
        billboard.transform.parent = anchor.transform;
        billboard.transform.localPosition = new Vector3(0, verticalOffset, 0);
        billboard.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        billboard.gameObject.layer = LayerMask.NameToLayer("Indicator");


        suspiciousTexture = Resources.Load<Texture2D>("Textures/Interface/suspicious");
        alertTexture = Resources.Load<Texture2D>("Textures/Interface/alert");
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyAwareness == null)
        {
            Destroy(this);
        }
        if (enemyAwareness.AwareTargetCount() > 0 && !enemyAwareness.IsAlerted() && !enemyAwareness.IsSuspicious() && enabled)
        {
            billboard.GetComponent<Renderer>().material.SetTexture("_MainTex", suspiciousTexture);
            billboard.GetComponent<Renderer>().enabled = true;
        }
        else if (enemyAwareness.IsAlerted() && enabled || enemyAwareness.IsSuspicious() && enabled)
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
