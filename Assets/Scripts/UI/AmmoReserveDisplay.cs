using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmmoReserveDisplay : MonoBehaviour
{
    TextMeshProUGUI tmp;
    Gun gun;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        gun = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        tmp.text = gun.AmmoReserve.ToString();
    }
}
