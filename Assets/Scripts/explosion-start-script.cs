using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class towerDestructionParticles : MonoBehaviour

{
    public ParticleSystem explosionBase;

    // Start is called before the first frame update
    void Start()
    {
        explosionBase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
