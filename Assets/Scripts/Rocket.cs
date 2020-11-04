using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource rocketThrust;

    private float _rotationThisFrame;
    private float _thrustThisFrame;

    [SerializeField]
    private float _rcsThrust = 80f;

    [SerializeField]
    private float _thrustForce = 1000f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = this.transform.GetComponent<Rigidbody>();
        rocketThrust = this.transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotation();
        Thrust();
    }

    private void Rotation()
    {
        _rotationThisFrame = _rcsThrust * Time.deltaTime;

        rigidBody.freezeRotation = true; // take manual control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * _rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * _rotationThisFrame);
        }
        rigidBody.freezeRotation = false; //release control
    }

    private void Thrust()
    {
        _thrustThisFrame = _thrustForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * _thrustThisFrame);
            if (rocketThrust.isPlaying == false)
            {
                rocketThrust.Play();
            }

        }
        else
        {
            rocketThrust.Stop();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Ok");
                break;
            case "Fuel":
                Debug.Log("Fuel");
                break;
            default:
                Debug.Log("ded");
                break;
        }
    }
}
