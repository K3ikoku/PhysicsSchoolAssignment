using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private AnimationCurve radius;
    [SerializeField] private AnimationCurve force;
    [SerializeField] private AnimationCurve falloff;

    [SerializeField] private float explosionDelay;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool detonate;

    private float explosionEndTime;
    [SerializeField] private float timer = 0;
    float currentRadius = 0;
    float currentForce = 0;

    // Use this for initialization
    void Start()
    {
        explosionDelay += Time.time;

        if (force.length <= 0 || radius.length <= 0)
            Destroy(gameObject);
        foreach (Keyframe key in radius.keys)
            explosionEndTime = Mathf.Max(explosionEndTime,explosionDelay + key.time);

        foreach (Keyframe key in force.keys)
            explosionEndTime = Mathf.Max(explosionEndTime, explosionDelay + key.time);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time >= explosionDelay)
        {
            currentRadius = radius.Evaluate(timer);
            currentForce = force.Evaluate(timer);


            Collider[] Colliders = Physics.OverlapSphere(transform.position, currentRadius);

            foreach (Collider hit in Colliders)
            {
                if (hit == gameObject)
                    continue;

                Rigidbody body = hit.GetComponent<Rigidbody>();
                if (body != null)
                {
                    float distance = Vector3.Distance(body.transform.position, transform.position);

                    Vector3 explosionPosition = transform.position + transform.rotation * offset;

                    body.AddExplosionForce(currentForce * falloff.Evaluate(distance / currentRadius), explosionPosition, 0.0f, 0.0f, ForceMode.Force);
                }
            }

            if (timer >= explosionEndTime)
                Destroy(gameObject);
            else
                timer += Time.deltaTime;
        }
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(Time.time >= explosionDelay)
        {
            //Gizmos.color = new Color(0.5f + currentForce / 500, 0.5f - currentForce, 0, Mathf.Clamp(Mathf.Abs(currentForce) / 1000, 0.1f, 0.75f));
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, currentRadius);
        }
    }
#endif
}
