using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackScript : BlockingObject
{
    [SerializeField]
    public TowerType attackState;
    [SerializeField]
    public TowerMaterials attackMaterials;


    [SerializeField]
    private float fireRate = 1.0f;
    private float nextFire = 0.0f;

    [SerializeField]
    public GameObject bullet;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            collision.gameObject.GetComponent<MediumPlayer>().SetDamage(true);
        }
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Projectile rocket = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Projectile>();

            rocket.velocity = collision.transform.position - transform.position;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            collision.gameObject.GetComponent<MediumPlayer>().SetDamage(false);
        }
    }
}

public enum TowerType
{
    SPRINKLES,
    PEPPERMINT,
    LASER,
    FROSTING
}

public enum TowerMaterials
{
    GRAHAMCRACKER,
    GINGERBREAD
}
