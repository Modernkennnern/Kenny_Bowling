﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {
    public AimerController aimer;
    public Camera cam;
    public float aimDistance;
    public float maxPower;
    public float jumpForce;

    public bool IsMoving { get; private set; }

    // Use this for initialization
    void Start() {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update() {

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            // Debug.Log("Raycast collided with: " + hit.collider.name);
            Transform objectHit = hit.transform;

            // aimer position = ball position + normalized vector between ball and click
            var distance = hit.point - transform.position;
            distance.Normalize(); // make sure lenght is 1
            distance *= aimDistance;
            var targetPos = transform.position + distance;
            aimer.transform.position = new Vector3(targetPos.x, aimer.transform.position.y, targetPos.z);
        }

        if (transform.position.y <= -10) {
            DestroyObject(this.gameObject);
        }
    }

    public void Throw(float power) {
        aimer.gameObject.SetActive(false);
        var direction = aimer.transform.position - transform.position;
        direction.Normalize();
        direction *= power * maxPower;
        GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        IsMoving = true;
    }

    public void Jump() {
        if (IsMoving) {
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
