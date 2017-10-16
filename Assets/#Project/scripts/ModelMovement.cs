using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ModelMovement : MonoBehaviour {

    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public Transform model;

    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 leftPosPrev;
    private Vector3 rightPosPrev;

	void Update () {
        leftPos = leftController.transform.position;
        rightPos = rightController.transform.position;

        if (leftController.gripPressed ^ rightController.gripPressed) {
            OneHandDrag();
        }
         
        if(leftController.gripPressed && rightController.gripPressed){
            TwoHandDrag();
            Rotate();
            Scale();
        }

        leftPosPrev = leftController.transform.position;
        rightPosPrev = rightController.transform.position;
    }

    private void OneHandDrag() {
        if(leftController.gripPressed) {
            Translate(leftPos, leftPosPrev);
        } else {
            Translate(rightPos, rightPosPrev);
        }
    }

    private void TwoHandDrag() {
        Vector3 pos = (leftController.transform.position + rightController.transform.position) / 2f;
        Vector3 prevPos = (leftPosPrev + rightPosPrev) / 2f;
        Translate(pos, prevPos);
    }

    private void Translate(Vector3 pos, Vector3 prevPos) {
        Vector3 distance = pos - prevPos;
        model.Translate(distance);
    }

    private void Rotate() {
        //project on XZ plane to restric rotation to flat table
        Vector3 pos = Vector3.ProjectOnPlane(rightPos - leftPos, Vector3.up);
        Vector3 prevPos = Vector3.ProjectOnPlane(rightPosPrev - leftPosPrev, Vector3.up);

        //center of first hand pos
        Vector3 center = (leftPos + rightPos) / 2f;

        //rotate around center 
        float angle = Vector3.Angle(pos, prevPos);
        Vector3 cross = Vector3.Cross(pos, prevPos);
        float dot = Vector3.Dot(cross, Vector3.down);
        float sign = Mathf.Sign(dot);

        //perform rotation
        model.RotateAround(center, Vector3.up, sign * angle);
    }

    private void Scale() {
        float dist = Vector3.Distance(leftPos, rightPos);
        float prevDist = Vector3.Distance(leftPosPrev, rightPosPrev);
        float scaleFactor = dist / prevDist;

        Vector3 midPosPreScale = (rightPos + leftPos) / 2f;
        Vector3 midPosLocal = model.InverseTransformPoint(midPosPreScale);

        model.localScale *= scaleFactor;

        Vector3 midPosPostScale = model.TransformPoint(midPosLocal);
        Translate(midPosPreScale, midPosPostScale);
    }

}
