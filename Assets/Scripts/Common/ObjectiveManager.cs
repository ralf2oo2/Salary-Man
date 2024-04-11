using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    private static string objective = "";
    public static string Objective => objective;
    public static void SetObjective(string objectiveString) 
    { 
        objective = objectiveString;
    }
}
