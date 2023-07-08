using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stepParticles : MonoBehaviour

{
    public ParticleSystem stepsBase;

    // Start is called before the first frame update
    void Start()
    {
        stepsBase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
