using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sprinkleGunParticles : MonoBehaviour

{
    public ParticleSystem sprinklesBase;

    // Start is called before the first frame update
    //Change to keybind press? Wasn't sure what was wanted for this one
    void Start()
    {
        sprinklesBase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
