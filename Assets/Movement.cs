using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
  
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationSpeed = 100f;
    [SerializeField] AudioClip mainEngine;


    [SerializeField] ParticleSystem mainBoosterParticles;
    [SerializeField] ParticleSystem leftBoosterParticles;
    [SerializeField] ParticleSystem rightBoosterParticles;

    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Thrusting();
        } else  {
            StopThrusting();
        }
    }

    void ProcessRotation() {
        if (Input.GetKey(KeyCode.A))
            {
                AppliedRotation(rotationSpeed);
            } else if(Input.GetKey(KeyCode.D))
            {
                AppliedRotation(-rotationSpeed);
            }
    }

    void AppliedRotation(float r) 
    {
        if(r > 1) {
            leftBoosterParticles.Play();
        } else {
            rightBoosterParticles.Play();
        }
        leftBoosterParticles.Play();
        rb.freezeRotation = true; // freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * Time.deltaTime * r);
        rb.freezeRotation = false; // unfreezing rotation so the physics system can take over
    }

    void Thrusting()
    {
            if(!audioSource.isPlaying) {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!mainBoosterParticles.isPlaying)
            {
               mainBoosterParticles.Play();
            }
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);

    }
    void StopThrusting()
    {
        if(audioSource.isPlaying) {
            audioSource.Stop();
        }
        if(!mainBoosterParticles.isPlaying)
        {
            mainBoosterParticles.Stop();
        }
    }
}
