using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow.Locks;

[CustomEditor(typeof(FieldOfView))]

public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewingAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewingAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewingAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewingAngle02 * fov.radius);

        if (fov.CanSeeObject())
        {
            Handles.color = Color.green;
            foreach(int objectId in fov.getVisibleObjects())
            {
                Collider gameObject = (Collider)EditorUtility.InstanceIDToObject(objectId);
                if (gameObject != null)
                {
                    Handles.DrawLine(fov.transform.position, gameObject.transform.position);
                }
            }
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
