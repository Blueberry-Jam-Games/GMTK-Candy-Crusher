using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public ParticleSystem gun;

    public bool particles = false;

    public void pointAtTarget(Vector3 target)
    {
        if(particles)
        {
            Vector3 targetPosition = target;
            Quaternion aim = Quaternion.LookRotation(target - gun.transform.position);

            //aim = Quaternion.Euler(aim.eulerAngles - transform.rotation.eulerAngles);

            gun.transform.rotation = aim;
        }
    }
}
