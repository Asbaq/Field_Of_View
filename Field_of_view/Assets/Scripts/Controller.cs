using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float mouseSensitivity = 10f;
    public float movespeed = 6;
    new Rigidbody rigidbody;
    Camera ViewCamera;
    Vector3 velocity;  
    void Start()
    {
        rigidbody = GetComponent<Rigidbody> ();
        ViewCamera = Camera.main;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);
        // Looking at by mouse input
        velocity = new Vector3 (Input.GetAxisRaw ("Horizontal"),0,Input.GetAxisRaw("Vertical")).normalized * movespeed;      
        // Movement in x and z axis 
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.fixedDeltaTime);
        // to move the rigidbody in fixed frames
    }
}
