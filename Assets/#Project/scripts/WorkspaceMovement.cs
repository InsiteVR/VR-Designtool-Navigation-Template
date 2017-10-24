using UnityEngine;
using VRTK;

public class WorkspaceMovement : MonoBehaviour {

    public VRTK_ControllerEvents leftController;
    public VRTK_ControllerEvents rightController;
    public Transform workspace;

    private Vector3 leftPos;
    private Vector3 rightPos;
    private Vector3 leftPosPrev;
    private Vector3 rightPosPrev;

	void Update () {
        //current position of controllers
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

        //previous position of controllers, to be used in the next frame
        leftPosPrev = leftController.transform.position;
        rightPosPrev = rightController.transform.position;
    }

    private void OneHandDrag() {
        if(leftController.gripPressed) {
            Translate(leftPosPrev, leftPos);
        } else {
            Translate(rightPosPrev, rightPos);
        }
    }

    private void Translate(Vector3 startPos, Vector3 endPos) {
        Vector3 distance = endPos - startPos;
        workspace.Translate(distance, Space.World);
    }

    private void TwoHandDrag() {
        //get middle position of hands
        Vector3 midPos = (leftPos + rightPos) / 2f;
        Vector3 prevMidPos = (leftPosPrev + rightPosPrev) / 2f;
        Translate(prevMidPos, midPos);
    }

    private void Rotate() {
        //project on XZ plane to restric rotation to flat table
        Vector3 dir = Vector3.ProjectOnPlane(rightPos - leftPos, Vector3.up);
        Vector3 prevDir = Vector3.ProjectOnPlane(rightPosPrev - leftPosPrev, Vector3.up);

        //center of hand pos
        Vector3 center = (leftPos + rightPos) / 2f;

        float angle = Vector3.Angle(dir, prevDir);

        //calculate sign of rotation
        Vector3 cross = Vector3.Cross(dir, prevDir);
        float dot = Vector3.Dot(cross, Vector3.down);
        float sign = Mathf.Sign(dot);

        //perform rotation
        workspace.RotateAround(center, Vector3.up, sign * angle);
    }

    private void Scale() {
        //distance between hands 
        float dist = Vector3.Distance(leftPos, rightPos);
        float prevDist = Vector3.Distance(leftPosPrev, rightPosPrev);

        //scale factor based on difference in hand distance
        float scaleFactor = dist / prevDist;

        //convert middle position of hands from global to local space
        Vector3 midPosPreScale = (rightPos + leftPos) / 2f;
        Vector3 midPosLocal = workspace.InverseTransformPoint(midPosPreScale);

        //apply scale to model
        workspace.localScale *= scaleFactor;

        //convert local position back to global and perform corrective translation
        Vector3 midPosPostScale = workspace.TransformPoint(midPosLocal);
        Translate(midPosPostScale, midPosPreScale);
    }

}
