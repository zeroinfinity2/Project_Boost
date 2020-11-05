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

    [SerializeField]
    private ParticleSystem _mainEngineParticles;
    [SerializeField]
    private ParticleSystem _explosionParticles;
    [SerializeField]
    private ParticleSystem _levelLoadParticles;

    private float _levelTimer = 2f;

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
            _mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        _rigidBody.AddRelativeForce(Vector3.up * _thrustThisFrame);
        if (_rocketAudio.isPlaying == false)
        {
            _rocketAudio.PlayOneShot(_mainEngine);
        }
        _mainEngineParticles.Play();
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
                //do nothing
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        _rocketAudio.Stop();
        _rocketAudio.PlayOneShot(_explosion);
        _explosionParticles.Play();
        Invoke("DecreaseLevel", _levelTimer);
        Debug.Log("ded");
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        _rocketAudio.Stop();
        _rocketAudio.PlayOneShot(_levelLoad);
        _levelLoadParticles.Play();
        Invoke("IncreaseLevel", _levelTimer);
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
