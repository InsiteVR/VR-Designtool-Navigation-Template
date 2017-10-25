using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PrefabSpawner : MonoBehaviour {

    public Transform parent;
    public GameObject prefab;
    public VRTK_ControllerEvents controller;

    void Start () {
        controller.TriggerPressed += StartSpawning;
    }

    private void StartSpawning(object sender, ControllerInteractionEventArgs e) {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop() {
        GameObject go = Instantiate(prefab);
        go.transform.parent = parent;
        go.transform.localScale = Vector3.one;

        while (controller.triggerPressed) {
            RaycastHit hit;
            if (Physics.Raycast(controller.transform.position, controller.transform.forward, out hit)) {
                go.transform.position = hit.point;
            }

            yield return null;
        }
    }
}
