using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class ModelMovement : MonoBehaviour {

    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public Transform model;

    private Vector3 L;
    private Vector3 R;
    private Vector3 prevL;
    private Vector3 prevR;

	void Update () {
        L = leftController.transform.position;
        R = rightController.transform.position;

        if (leftController.gripPressed ^ rightController.gripPressed) {
            OneHandDrag();
        }

        if(leftController.gripPressed && rightController.gripPressed){
            TwoHandDrag();
            Rotate();
            Scale();
        }

        prevL = leftController.transform.position;
        prevR = rightController.transform.position;
    }

    private void OneHandDrag() {
        if(leftController.gripPressed) {
            Translate(L, prevL);
        } else {
            Translate(R, prevR);
        }
    }

    private void TwoHandDrag() {
        Vector3 pos = (leftController.transform.position + rightController.transform.position) / 2f;
        Vector3 prevPos = (prevL + prevR) / 2f;
        Translate(pos, prevPos);
    }

    private void Translate(Vector3 pos, Vector3 prevPos) {
        Vector3 distance = pos - prevPos;
        model.Translate(distance, Space.World);
    }

    private void Rotate() {
        //project on XZ plane to restric rotation to horizontal rotation
        Vector3 pos = Vector3.ProjectOnPlane(R - L, Vector3.up);
        Vector3 prevPos = Vector3.ProjectOnPlane(prevR - prevL, Vector3.up);

        //center of first hand pos
        Vector3 center = (L + R) / 2f;

        //rotate around center 
        float angle = Vector3.Angle(pos, prevPos);
        Vector3 cross = Vector3.Cross(pos, prevPos);
        float dot = Vector3.Dot(cross, Vector3.down);
        float sign = Mathf.Sign(dot);

        //perform rotation
        model.RotateAround(center, Vector3.up, sign * angle);
    }

    private void Scale() {
        float dist = Vector3.Distance(L, R);
        float prevDist = Vector3.Distance(prevL, prevR);
        float scaleFactor = dist / prevDist;

        Vector3 midPosPreScale = (R + L) / 2f;
        Vector3 midPosLocal = model.InverseTransformPoint(midPosPreScale);

        model.localScale *= scaleFactor;

        Vector3 midPosPostScale = model.TransformPoint(midPosLocal);
        Translate(midPosPreScale, midPosPostScale);
    }

}
