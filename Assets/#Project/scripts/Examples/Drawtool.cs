using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Drawtool : MonoBehaviour {

    public Transform parent;
    public Transform drawTip;
    public VRTK_ControllerEvents rightController;
    public float lineWidth = .01f;
    public Material lineMaterial;

    void Start() {
        rightController.TriggerPressed += StartDrawing;
        rightController.TriggerReleased += StopDrawing;
    }

    private void StartDrawing(object sender, ControllerInteractionEventArgs e) {
        LineRenderer line = CreateNewLine();
        StartCoroutine(AddPoints(line));
    }

    private void StopDrawing(object sender, ControllerInteractionEventArgs e) {
        StopAllCoroutines();
    }

    private LineRenderer CreateNewLine() {
        GameObject obj = new GameObject();
        obj.transform.parent = parent;

        LineRenderer lr = obj.AddComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        lr.material = lineMaterial;
        lr.useWorldSpace = false;

        return lr;
    }

    IEnumerator AddPoints(LineRenderer line) {
        line.positionCount = 0;

        while (true) {
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, drawTip.position);

            yield return null;
        }
    }
}
