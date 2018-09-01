using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    Bird bird;
    Vector2 m_posW;
    Vector2 catapultCenter;
    Vector2 force;

    public float maxDistance = 1.0f;

    void Awake ()
    {
        bird = GetComponent<Bird>();
        force = Vector2.zero;
    }

	void Update () {
        Vector2 m_posW = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var o = Physics2D.OverlapPoint(m_posW);



        if (Input.GetMouseButton(0))
        {
            if (o != null)
            {
                if (o.tag == "bird")
                {
                    bird.OnDrag();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (bird.isOnCatapult)
            {
                bird.Fly(force);
            }
            else
            {
                bird.OutDrag();
            }
        }

        if (bird.isOnDrag)
        {
            //Debug.Log(m_posW);

            if (bird.isOnCatapult)
            {

                Vector2 dir = m_posW  - catapultCenter;
                if (dir.sqrMagnitude > maxDistance * maxDistance)
                {
                    m_posW = catapultCenter + dir.normalized * maxDistance;
                    force = catapultCenter - (Vector2)transform.position;
                }

            }

            bird.transform.position = m_posW;
            //Debug.LogError("Bird: " + bird.transform.position);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "catapultCenter")
        {
            bird.OnCatapult();
            catapultCenter = collision.transform.position;
            Destroy(collision.gameObject);
        }
    }
}
