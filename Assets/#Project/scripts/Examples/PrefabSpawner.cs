using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PrefabSpawner : MonoBehaviour {

    public Transform parent;
    public GameObject prefab;
    public VRTK_ControllerEvents controller;
    public Transform rayOrigin;
    public Material previewMat;

    private RaycastHit _hit;
    private GameObject _preview;

    void Start () {
        controller.TriggerPressed += StartSpawning;
        _preview = SpawnPreviewModel();
    }

    void Update() {
        DoRayCast();
        PositionPreview();
    }

    private void DoRayCast() {
        RaycastHit hit;
        Physics.Raycast(rayOrigin.position, rayOrigin.transform.forward, out _hit);
    }

    private void PositionPreview() {
        _preview.transform.position = _hit.point;
        _preview.transform.parent = parent;
        _preview.transform.localScale = Vector3.one;
    }

    private GameObject SpawnPreviewModel() {
        GameObject go = Instantiate(prefab);
        foreach(Renderer renderer in go.GetComponentsInChildren<Renderer>()) {
            renderer.material = previewMat;
        }
        return go;
    }

    private void StartSpawning(object sender, ControllerInteractionEventArgs e) {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        _preview.SetActive(false);

        GameObject go = Instantiate(prefab);
        go.transform.parent = parent;
        go.transform.localScale = Vector3.one;

        while (controller.triggerPressed) {
            go.transform.position = _hit.point;

            yield return null;
        }

        _preview.SetActive(true);
    }
}
