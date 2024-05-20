using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float speedMult;
    
    private Vector3 velocity;
    private Rigidbody rb;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate() {
        rb.velocity = velocity * speedMult;
    }

    public void setVelocity(Vector3 velocity) {
        this.velocity = velocity;
    }
}
