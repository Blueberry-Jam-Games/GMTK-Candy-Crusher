using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class godrayParticleStart : MonoBehaviour

{
    public ParticleSystem godraysBase;

    // Start is called before the first frame update
    //This is temporary, tweak and such as needed
    void Start()
    {
        godraysBase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
