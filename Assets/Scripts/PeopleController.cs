using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleController : MonoBehaviour {

    public float speed;
    public GameObject particleSystem;
    public GameObject soundObject;

    Vector3 velocity;
    bool isRunning = true;

    Rigidbody rb;
    float timer = 0.0f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        velocity = new Vector3(-speed, 0, 0);
    }

    void Update ()
    {
        if (isRunning) rb.transform.position += velocity;
        else
        {
            timer += Time.deltaTime;
            if (timer > 10)
            {
                Destroy(this.gameObject);
            }
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            print(collision.relativeVelocity.magnitude);
            if (collision.relativeVelocity.magnitude > 3.5f)
            {
                GameObject.Instantiate(particleSystem, gameObject.transform.position, Quaternion.identity).GetComponent<ParticleSystem>().Play();
                AudioSource s =  GameObject.Instantiate(soundObject, gameObject.transform.position, Quaternion.identity).GetComponent<AudioSource>();
                s.clip = SoundManager.Instance.GetSound("Killed3");
                s.volume = 0.07f;
                s.Play();
                Destroy(this.gameObject);
            }
            else
            {
                AudioSource audioS = this.GetComponent<AudioSource>();
                audioS.clip = SoundManager.Instance.GetSound("Killed1");
                audioS.volume = 0.07f; ;
                audioS.Play();
                isRunning = false;
                rb.AddRelativeForce(new Vector3(10, 5, 0), ForceMode.Impulse);
            }
            
        }
    }
}
