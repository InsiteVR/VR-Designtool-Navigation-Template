using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DrawdotPositioner : MonoBehaviour
{
    public Transform dot;
    public Vector3 vivePos;
    public Vector3 oculusPos;

    private bool _setup = false;
  
    private void Update() {
        if (!string.IsNullOrEmpty(XRDevice.model) && _setup == false) {
            SetupDot();
            _setup = true;
        }
    }
    
    private void SetupDot() {
        if (XRDevice.model.Contains("Oculus")) {
            dot.localPosition = oculusPos;
        } else {
            dot.localPosition = vivePos;
        }
    }
}
