using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _movementVector = new Vector3(10f, 10f, 10f);

    [SerializeField]
    [Range(0, 1)]
    private float _movementFactor;

    [SerializeField]
    private float _period = 2f;

    private Vector3 _startingPos;

    private Vector3 _offset;

    // Start is called before the first frame update
    void Start()
    {
        _startingPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_period > Mathf.Epsilon)
        { 
            float cycles = Time.time / _period;
            const float tau = Mathf.PI * 2;
            float rawSinWave = Mathf.Sin(tau * cycles);

            _movementFactor = (rawSinWave / 2) + 0.5f;

            _offset = _movementFactor * _movementVector;
            transform.position = _startingPos + _offset;
        }

    }
}
