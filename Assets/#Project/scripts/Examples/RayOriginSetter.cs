using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class RayOriginSetter : MonoBehaviour {
    public PrefabSpawner spawner;
    public Transform viveOrigin;
    public Transform oculusOrigin;

    private bool _setup = false;

    private void Update() {
        if (!string.IsNullOrEmpty(XRDevice.model) && _setup == false) {
            SetupDot();
            _setup = true;
        }
    }

    private void SetupDot() {
        if (XRDevice.model.Contains("Oculus")) {
            spawner.rayOrigin = oculusOrigin;
        } else {
            spawner.rayOrigin = viveOrigin;
        }
    }
}
