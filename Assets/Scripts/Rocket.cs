using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Rocket : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private AudioSource _rocketAudio;

    [SerializeField]
    private LevelManager _levelManager;

    private float _rotationThisFrame;
    private float _thrustThisFrame;

    [SerializeField]
    private float _rcsThrust = 80f;

    [SerializeField]
    private float _thrustForce = 1000f;

    [SerializeField]
    private AudioClip _mainEngine;

    [SerializeField]
    private AudioClip _explosion;

    [SerializeField]
    private AudioClip _levelLoad;

    enum State
    {
        Alive,
        Dying,
        Transcending
    }

    State state = State.Alive;



    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = this.transform.GetComponent<Rigidbody>();
        _rocketAudio = this.transform.GetComponent<AudioSource>();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (state != State.Dying)
        {
            RotationInput();
            ThrustInput();
        }
    }

    private void RotationInput()
    {
        _rotationThisFrame = _rcsThrust * Time.deltaTime;

        _rigidBody.freezeRotation = true; // take manual control of rotation
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * _rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * _rotationThisFrame);
        }
        _rigidBody.freezeRotation = false; //release control
    }

    private void ThrustInput()
    {
        _thrustThisFrame = _thrustForce * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();

        }
        else
        {
            _rocketAudio.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Vector3.up * _thrustThisFrame);
        if (_rocketAudio.isPlaying == false)
        {
            _rocketAudio.PlayOneShot(_mainEngine);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (state != State.Alive)
        {
            return;
        }
        
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("Ok");
                break;
            case "Finish":
                state = State.Transcending;
                _rocketAudio.Stop();
                _rocketAudio.PlayOneShot(_levelLoad);
                Invoke("IncreaseLevel", 1f);
                break;
            default:
                state = State.Dying;
                _rocketAudio.Stop();
                _rocketAudio.PlayOneShot(_explosion);
                Invoke("DecreaseLevel", 1f);
                Debug.Log("ded");
                break;
        }
    }

    private void IncreaseLevel()
    {
        _levelManager.IncreaseLevel();

    }

    private void DecreaseLevel() 
    {
        _levelManager.DecreaseLevel();
    }
}
