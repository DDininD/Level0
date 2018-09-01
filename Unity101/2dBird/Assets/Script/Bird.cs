using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {


    Rigidbody2D birdRigi2d;
    public bool isOnDrag;
    public bool isOnCatapult;
    public bool isFlying;
    public bool isOnCatapultTrigger;

    public AudioSource flying;
    public AudioSource collision;


    private Animator birdAnim;
    public float speed;

    public Transform clip;

    private void Awake()
    {
        isFlying = false;
        birdRigi2d = GetComponent<Rigidbody2D>();
        birdRigi2d.freezeRotation = true;
        birdAnim = GetComponent<Animator>();

        clip.gameObject.SetActive(false);
    }


    public void Fly(Vector2 force2d)
    {
        flying.Play();
        birdAnim.SetBool("isDraged", false);
        birdAnim.SetBool("isFlying", true);

        isFlying = true;

        clip.gameObject.SetActive(false);
        FindObjectOfType<GameMode>().SetTrailVisible(false);

        FindObjectOfType<CameraFollow>().enabled = true;
        FindObjectOfType<FixCamera>().enabled = false;
        isOnCatapult = false;
        isOnDrag = false;
        GetComponent<Rigidbody2D>().isKinematic = false;
        birdRigi2d.freezeRotation = false;
        birdRigi2d.velocity = force2d * speed;

        Invoke("Die", 3.0f);

        FindObjectOfType<CameraFollow>().ResetCamera();
        //GameObject.FindGameObjectWithTag("catapultCenter").SetActive(true);
    }

    public void OnDrag()
    {
        isOnDrag = true;
        GetComponent<Rigidbody2D>().isKinematic = true;
        birdAnim.SetBool("isDraged", true);
    }


    public void OnCatapult()
    {
        clip.gameObject.SetActive(true);
        isOnCatapult = true;
        isOnCatapultTrigger = true;
        FindObjectOfType<GameMode>().SetTrailVisible(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        birdAnim.SetBool("isFlying", false);
        this.collision.Play();
    }

    public void OutDrag()
    {
        isOnDrag = false;
        birdAnim.SetBool("isDraged", false);
        GetComponent<Rigidbody2D>().isKinematic = false;

    }

    void Die()
    {

        Destroy(gameObject);

        FindObjectOfType<GameMode>().birdCount--;
    }

}
