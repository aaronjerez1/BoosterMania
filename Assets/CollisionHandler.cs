using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip Landing;
    [SerializeField] AudioClip crashing;

    [SerializeField] ParticleSystem LandingParticles;
    [SerializeField] ParticleSystem crashingParticles;

    AudioSource audioSource;
    Movement movement;
    BoxCollider boxCollider;
    
    bool isTransitioning = false;
    bool collisionDisable = false;
    
    void Start() 
    {
        audioSource = GetComponent<AudioSource>();
        movement = GetComponent<Movement>();
        boxCollider = GetComponent<BoxCollider>();
        
    }

    void Update()
    {
        RespondToDebugKey();
    }


    void RespondToDebugKey()
    {
        if(Input.GetKey(KeyCode.L))
        {
            ReloadNextLevel();
        }
        if(Input.GetKey(KeyCode.C))
        {
          collisionDisable = !collisionDisable; // toggle
        }
    }
    void OnCollisionEnter(Collision other) 
    {
        if(isTransitioning || collisionDisable) {return;}

        switch (other.gameObject.tag) 
        {
            case "Landing":
                StartNextLevelSequence();
                break;
            case "Obstacle":
                StartCrashSequence();
                break;
            default:
                Debug.Log("idk what you hit");
                break;
        }
    }
    void StartCrashSequence()
    {   
        // todo add SFX upan crash
        // todo add particle effect upan crash
        isTransitioning = true;
        crashingParticles.Play();
        audioSource.PlayOneShot(crashing);
        movement.enabled = false;
        Invoke("ReloadLevel", levelLoadDelay);
    }

    void StartNextLevelSequence()
    {
        isTransitioning = true;
        LandingParticles.Play();
        audioSource.PlayOneShot(Landing);
        movement.enabled = false;
        Invoke("ReloadNextLevel", levelLoadDelay);
    }

    void ReloadLevel() 
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void ReloadNextLevel() 
    {   
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings) {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);

    }
}
