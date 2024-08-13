using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Controller_Updated : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;
    private bool isEngineStarted = false;

    // Settings
    [SerializeField] private float motorForce = 4000f; // Increase this value significantly
    [SerializeField] private float breakForce = 3000f;
    [SerializeField] private float maxSteerAngle = 30f;

    // Wheel Colliders
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Wheels
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    // Audio
    [SerializeField] private AudioSource engineStartAudioSource;
    [SerializeField] private AudioSource idleEngineAudioSource;
    [SerializeField] private AudioSource revvingEngineAudioSource;
    [SerializeField] private AudioSource crashAudioSource;
    [SerializeField] private AudioClip engineStartClip;
    [SerializeField] private AudioClip idleEngineClip;
    [SerializeField] private AudioClip revvingEngineClip;
    [SerializeField] private AudioClip crashClip;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.mass = 1000f; // Ensure the mass is reasonable

        // Ensure AudioSource components are attached
        if (engineStartAudioSource == null)
        {
            engineStartAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (idleEngineAudioSource == null)
        {
            idleEngineAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (revvingEngineAudioSource == null)
        {
            revvingEngineAudioSource = gameObject.AddComponent<AudioSource>();
        }

        if (crashAudioSource == null)
        {
            crashAudioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign audio clips to respective AudioSources
        engineStartAudioSource.clip = engineStartClip;
        idleEngineAudioSource.clip = idleEngineClip;
        revvingEngineAudioSource.clip = revvingEngineClip;
        crashAudioSource.clip = crashClip;

        idleEngineAudioSource.loop = true;
        revvingEngineAudioSource.loop = true;

        // Adjust drag for better performance
        rb.drag = 0.1f; // Reduce drag for better acceleration
        rb.angularDrag = 0.1f;
    }

    private void FixedUpdate()
    {
        if (isEngineStarted)
        {
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
            HandleEngineSounds();
        }

        // Check for engine start input (e.g., the "E" key)
        if (Input.GetKeyDown(KeyCode.E) && !isEngineStarted)
        {
            StartCoroutine(StartEngine());
        }
    }

    private void GetInput()
    {
        // Steering Input
        horizontalInput = Input.GetAxis("Horizontal");

        // Acceleration Input
        verticalInput = Input.GetAxis("Vertical");

        // Breaking Input
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot; 
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private IEnumerator StartEngine()
    {
        if (engineStartClip != null)
        {
            engineStartAudioSource.Play();
            yield return new WaitForSeconds(engineStartClip.length);
        }

        isEngineStarted = true;

        if (idleEngineClip != null)
        {
            idleEngineAudioSource.Play();
        }
    }

    private void HandleEngineSounds()
    {
        if (isEngineStarted)
        {
            if (Mathf.Abs(verticalInput) > 0.1f)
            {
                if (idleEngineAudioSource.isPlaying)
                {
                    idleEngineAudioSource.Stop();
                }
                if (!revvingEngineAudioSource.isPlaying)
                {
                    revvingEngineAudioSource.Play();
                }
                revvingEngineAudioSource.volume = Mathf.Abs(verticalInput);
            }
            else
            {
                if (!idleEngineAudioSource.isPlaying)
                {
                    idleEngineAudioSource.Play();
                }
                if (revvingEngineAudioSource.isPlaying)
                {
                    revvingEngineAudioSource.Stop();
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        PlayCrashSound();
    }

    private void PlayCrashSound()
    {
        if (crashClip != null)
        {
            crashAudioSource.Play();
        }
    }
}
