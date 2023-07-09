using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public int timer = 250;

    public ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer -= 1;

        if(timer <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
