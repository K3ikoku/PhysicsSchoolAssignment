using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float maxVelocityChange = 10.0f;
    [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpHeight = 2.0f;

    [SerializeField] private bool grounded = false;

    private Rigidbody body;
    private Quaternion rotation;

    // Use this for initialization
    void Awake()
    {
        body = gameObject.GetComponent<Rigidbody>();
        body.freezeRotation = true;
        body.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (grounded)
        {
            //Calculate how fast character is should move
            Vector3 targetMoveSpeed = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            targetMoveSpeed = transform.TransformDirection(targetMoveSpeed);
            targetMoveSpeed *= moveSpeed;

            // Apply force
            Vector3 currentSpeed = body.velocity;
            Vector3 speedChange = (targetMoveSpeed - currentSpeed);
            speedChange.x = Mathf.Clamp(speedChange.x, -maxVelocityChange, maxVelocityChange);
            speedChange.z = Mathf.Clamp(speedChange.z, -maxVelocityChange, maxVelocityChange);
            speedChange.y = 0;

            body.AddForce(speedChange, ForceMode.VelocityChange);

            if (canJump == true && Input.GetButton("Jump"))
                body.velocity = new Vector3(currentSpeed.x, CalculateJumpSpeed(), currentSpeed.z);
        }

        body.AddForce(new Vector3(0, -gravity * body.mass, 0));

        grounded = false;

    }

    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }

    float CalculateJumpSpeed()
    {
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

}
