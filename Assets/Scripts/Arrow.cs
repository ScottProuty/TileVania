using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed = 10f;
    [SerializeField] int secsBeforeDestroy = 8;
    Rigidbody2D myRigidbody;
    GameObject player;
    float xSpeed;
    bool collidedWithWall = false;



    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        xSpeed = player.transform.localScale.x * arrowSpeed;
        transform.localScale = new Vector2 (Mathf.Sign(xSpeed),1f);
    }

    void Update()
    {
        myRigidbody.velocity = !collidedWithWall? new Vector2(xSpeed, 0f) : new Vector2(0f,0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trigger enter with " + collision.tag);
        
    }
    private IEnumerator OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision with " + collision.collider.name + " type: " + collision.collider.tag);
        
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        collidedWithWall = true;
        yield return new WaitForSeconds(secsBeforeDestroy);
        Destroy(gameObject);
        
    }
}
