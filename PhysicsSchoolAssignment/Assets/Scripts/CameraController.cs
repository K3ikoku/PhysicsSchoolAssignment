﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector2 mouseLook;
    Vector2 smoothingVector;

    [SerializeField] private float sensitivity = 5.0f;
    [SerializeField] private float smoothing = 2.0f;
    [SerializeField] private GameObject[] grenadeTypes;
    [SerializeField] private float throwCooldown = 5.0f;
    [SerializeField] private float throwForce = 10.0f;
    [SerializeField] private int currentGrenade;

    public GameObject CurrentGrenade
    {
        get { return grenadeTypes[currentGrenade]; }
    }


    GameObject character;
    private float attackTimer = 0;
    // Use this for initialization
    void Start()
    {
        character = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        smoothingVector.x = Mathf.Lerp(smoothingVector.x, mouseDelta.x, smoothing * Time.deltaTime);
        smoothingVector.y = Mathf.Lerp(smoothingVector.y, mouseDelta.y, smoothing * Time.deltaTime);
        mouseLook += smoothingVector;

        mouseLook.y = Mathf.Clamp(mouseLook.y, -90.0f, 90.0f);

        transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        character.transform.localRotation = Quaternion.AngleAxis(mouseLook.x, character.transform.up);

        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        if (Input.GetKeyDown("1"))
            currentGrenade = 0;

        if (Input.GetKeyDown("2"))
            currentGrenade = 1;

        if (Input.GetKeyDown("3"))
            currentGrenade = 2;

        if (Input.GetKeyDown("4"))
            currentGrenade = 3;

    }

    private void FixedUpdate()
    {

        if (Input.GetButton("Fire1") && attackTimer <= 0)
            ThrowGrenade();

        if (attackTimer > 0)
            attackTimer -= Time.deltaTime;
    }


    void ThrowGrenade()
    {
        attackTimer = throwCooldown;
        GameObject grenade = Instantiate(CurrentGrenade) as GameObject;
        grenade.transform.position = transform.position + transform.forward;
        Rigidbody grenadeBody = grenade.GetComponent<Rigidbody>();
        grenadeBody.AddForce(transform.forward * throwForce);
    }
}
