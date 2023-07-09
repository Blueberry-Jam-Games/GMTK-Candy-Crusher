using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogParticleStart : MonoBehaviour

{
    public ParticleSystem fogBase;

    // Start is called before the first frame update
    void Start()
    {
        fogBase.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
