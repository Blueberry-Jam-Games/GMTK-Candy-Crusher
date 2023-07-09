using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public GameObject explosion;
    private Rigidbody rb;
    public delegate void OnRocketLand();

    public AudioSource launch;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 start)
    {
        transform.position = start + new Vector3(0, 10, 0);
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        rb.velocity = new Vector3(0, -5, 0);
        launch.Play();
    }

    private void OnCollisionEnter(Collision hit)
    {
        Debug.Log($"Name: {hit.gameObject.name}");
        if(hit.gameObject.transform.parent.gameObject.CompareTag("Tower"))
        {
            Object.Instantiate(explosion, transform.position, Quaternion.Euler(0, 0, 0));
            hit.gameObject.transform.parent.gameObject.GetComponent<TowerAttackScript>().destroyMe();
            Destroy(hit.gameObject.transform.parent.gameObject);
        }
        Destroy(this.gameObject);
        
        /*Collider[] destroyCheck = Physics.OverlapSphere(new Vector3(transform.position.x, 0, transform.position.y), 0.5F);
        List<GameObject> markedForDestroy = new List<GameObject>();
        for(int i = 0; i < destroyCheck.Length; i++)
        {
            Collider current = destroyCheck[i];

            if(current.gameObject.CompareTag("Tower"))
            {
                markedForDestroy.Add(current.gameObject);
            }
        }

        while(markedForDestroy.Count > 0)
        {
            Destroy(markedForDestroy[0]);
            markedForDestroy.RemoveAt(0);
        }

        Destroy(this.gameObject);*/
    }
}
