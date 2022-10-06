using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(FieldOfView))]
public class FieldofViewEditor : Editor
{       
    void OnSceneGUI() // edit in Editor
    {
        FieldOfView fow = (FieldOfView)target; // adding target from FieldOfView
        Handles.color = Color.white; // initializing handles.color by color white 
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius); //  DrawArc 
        Vector3 viewAngleA = fow.DirFormAngle(-fow.viewAngle / 2,false); // viewAngleA initialize
        Vector3 viewAngleB = fow.DirFormAngle(fow.viewAngle / 2,false); //  viewAngleB initialize
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius); // Drawline 
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius); // DrawLine

        Handles.color = Color.red;// initializing handles.color by color red
        foreach(Transform visibleTarget in fow.visibleTargets)// adding visibletarget from FieldOfView
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);// DrawLine
        }
}
}