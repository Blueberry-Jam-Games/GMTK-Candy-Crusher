using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackScript : BlockingObject
{
    [SerializeField]
    public TowerType attackState;
    [SerializeField]
    public TowerMaterials attackMaterials;
    public int floorCount = 1;

    public ModelReferences towerPieces;
    
    [SerializeField]
    private float fireRate = 1.0f;
    private float nextFire = 0.0f;

    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    private int maxTargets;
    private int currTargets = 0;

    Dictionary<int, bool> playerTracking;

    private void Start()
    {
        playerTracking = new Dictionary<int, bool>();
    }

    private void FixedUpdate()
    {
        List<int> destroyList = new List<int>();
        List<int> inventoryList = new List<int>(playerTracking.Keys);
        foreach(int key in inventoryList)
        {
            if(playerTracking[key])
            {
               playerTracking[key] = false; 
            }
            else
            {
                destroyList.Add(key);
            }
        }
        foreach(int i in destroyList)
        {
            playerTracking.Remove(i);
        }
    }

    public void ReloadAmmo(int reloadQty)
    {
        // TODO
    }

    // void OnTriggerEnter(Collider collision)
    // {
    //     if (collision.gameObject.CompareTag("Soldier") && maxTargets > currTargets)
    //     {
    //         Debug.Log("hi");
    //         collision.gameObject.GetComponent<MediumPlayer>().SetDamage(true);
    //         currTargets++;
    //     }
    // }

    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            MediumPlayer player = collision.gameObject.GetComponent<MediumPlayer>();
            if(!playerTracking.ContainsKey(player.id) && playerTracking.Count < maxTargets)
            {
                playerTracking.Add(player.id, true);
            }

            if(playerTracking.ContainsKey(player.id))
            {
                playerTracking[player.id] = true;
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Projectile rocket = Instantiate(bullet, transform.position, transform.rotation).GetComponent<Projectile>();

                    rocket.velocity = collision.transform.position - transform.position;
                    Debug.Log("Name: " + collision.gameObject.name + " health: " + player.healthPoints);
                    // do damage
                    player.DoDamage(5f);

                    // if (player.healthPoints <= 0.0f)
                    // {
                    //     Debug.Log("destroy is getting called");
                    //     // currTargets--;
                    //     Destroy(collision.gameObject);
                    // }
                }
            }
        }
    }

    // void OnTriggerExit(Collider collision)
    // {
    //     if (collision.gameObject.CompareTag("Soldier"))
    //     {
    //         collision.gameObject.GetComponent<MediumPlayer>().SetDamage(false);
    //         currTargets--;
    //     }
    // }

    void OnValidate() {
        if (floorCount > 5)
        {
            floorCount = 5;
        }
        else if (floorCount < 1)
        {
            floorCount = 1;
        }
    }

    public override void UpdateGeometry()
    {
        float segmentHeight = 0.57F;
        //Destroy child objects to prep for rebuild
        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        Vector3 basePosition = transform.position;

        //1x1
        if (width == 1 && height == 1)
        {
            Quaternion forward = new Quaternion();
            forward = Quaternion.Euler(0, 0, 0);

            Quaternion segmentForward = new Quaternion();
            segmentForward = Quaternion.Euler(-90, 0, 0);

            Instantiate(towerPieces.base1x1, this.transform);

            for(int i = 0; i < floorCount; i++)
            {
                Vector3 position = new Vector3(0, 0.1F + (i * segmentHeight), 0);
                Instantiate(towerPieces.segment1x1, position + basePosition, segmentForward, this.transform);
                GameObject pickedWrap = towerPieces.siding1x1[Random.Range(0, towerPieces.siding1x1.Count)];
                Instantiate(pickedWrap, position + basePosition, segmentForward, this.transform);
            }

            Vector3 roofPosition = new Vector3(0, 0.1F + floorCount * segmentHeight);
            roofPosition += basePosition;

            if (attackState == TowerType.PEPPERMINT)
            {
                Instantiate(towerPieces.peppermintRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.SPRINKLES)
            {
                Instantiate(towerPieces.sprinklesRoof1x1, roofPosition, forward, this.transform);
            }
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
