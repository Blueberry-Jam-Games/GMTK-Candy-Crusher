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

    public Turret turret;
    
    private float fireRate;
    public float damageDone;
    private float nextFire = 0.0f;

    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public int maxTargets;

    [SerializeField]
    public float sprinklesFireRate = 1.0f;
    [SerializeField]
    public float sprinklesDamageDone = 5.0f;

    [SerializeField]
    public float peppermintFireRate = 0.5f;
    [SerializeField]
    public float peppermintDamageDone = 7.5f;

    [SerializeField]
    public float laserFireRate = 2.5f;
    [SerializeField]
    public float laserDamageDone = 15.0f;

    [SerializeField]
    public int ammo;

    Dictionary<int, bool> playerTracking;

    private void Start()
    {
        if (attackState == TowerType.SPRINKLES)
        {
            fireRate = sprinklesFireRate;
            damageDone = sprinklesDamageDone;
            ammo = 5;
        }
        else if (attackState == TowerType.PEPPERMINT)
        {
            fireRate = peppermintFireRate;
            damageDone = peppermintDamageDone;
            ammo = 4;
        }
        else if (attackState == TowerType.LASER)
        {
            fireRate = laserFireRate;
            damageDone = laserDamageDone;
            ammo = 3;
        }
        else
        {
            fireRate = 0.1f;
        }

        playerTracking = new Dictionary<int, bool>();
        turret = GetComponentInChildren<Turret>();
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
        ammo += reloadQty;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier") && attackState == TowerType.FROSTING)
        {
            collision.gameObject.GetComponent<MediumPlayer>().SlowDown(0.5f);
        }
    }

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
                if (Time.time > nextFire && ammo > 0)
                {
                    if(attackState != TowerType.FROSTING)
                    {
                        nextFire = Time.time + fireRate;

                        if(turret.particles != true)
                        {
                            Projectile rocket = Instantiate(bullet, turret.transform.position, transform.rotation).GetComponent<Projectile>();
                            rocket.velocity = collision.transform.position - turret.transform.position;
                            if(attackState == TowerType.LASER)
                            {
                                TowerAudio.Instance.Play("laser");
                            }
                            else if(attackState == TowerType.PEPPERMINT)
                            {
                                TowerAudio.Instance.Play("fasttower");
                            }
                        }
                        else
                        {
                            turret.gun.time = 0;
                            turret.gun.Play();
                        }

                        //Debug.Log("Name: " + collision.gameObject.name + " health: " + player.healthPoints);
                        // do damage
                        player.DoDamage(damageDone);
                        ammo--;
                    }
                    else
                    {
                        nextFire = Time.time + fireRate;
                        turret.gun.time = 0;
                        turret.gun.Play();
                        TowerAudio.Instance.Play("mist");
                    }
                }

                //Aim at stuff
                Vector3 targetPosition = collision.transform.position;
                targetPosition.y = turret.transform.position.y;
                Quaternion aim = Quaternion.LookRotation(targetPosition - turret.transform.position);

                turret.transform.rotation = aim;

                turret.pointAtTarget(collision.gameObject.transform.position);
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Soldier") && attackState == TowerType.FROSTING)
        {
            collision.gameObject.GetComponent<MediumPlayer>().SlowDown(2.0f);
        }
    }

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
        
        //Destroy child objects to prep for rebuild
        for (int i = this.transform.childCount; i > 0; --i)
        {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }

        Vector3 basePosition = transform.position;

        //1x1
        if (width == 1 && height == 1)
        {
            Vector3 gridAlignment = new Vector3(0.5F, 0, 0.5F);
            float segmentHeight = 0.57F;
            Quaternion forward = new Quaternion();
            forward = Quaternion.Euler(0, 0, 0);

            Quaternion segmentForward = new Quaternion();
            segmentForward = Quaternion.Euler(-90, 0, 0);

            GameObject newBase = Instantiate(towerPieces.base1x1, basePosition + gridAlignment, segmentForward, this.transform);
            newBase.GetComponent<MeshRenderer>().staticShadowCaster = true;

            for(int i = 0; i < floorCount; i++)
            {
                Vector3 position = new Vector3(0, 0.1F + (i * segmentHeight), 0);
                GameObject newPiece = Instantiate(towerPieces.segment1x1, position + basePosition + gridAlignment, segmentForward, this.transform);
                newPiece.GetComponent<MeshRenderer>().staticShadowCaster = true;

                GameObject pickedWrap = towerPieces.siding1x1[Random.Range(0, towerPieces.siding1x1.Count)];
                Vector3 detailOffset = new Vector3(0.507859F, 0, 0.516459F);
                GameObject wrap = Instantiate(pickedWrap, position + basePosition - detailOffset + gridAlignment, segmentForward, this.transform);
                wrap.GetComponent<MeshRenderer>().staticShadowCaster = true;
            }

            Vector3 roofPosition = new Vector3(0, 0.1F + floorCount * segmentHeight);
            roofPosition += basePosition + gridAlignment;

            GameObject roof = null;

            if (attackState == TowerType.PEPPERMINT)
            {
                roof = Instantiate(towerPieces.peppermintRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.SPRINKLES)
            {
                roof = Instantiate(towerPieces.sprinklesRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.LASER)
            {
                roof = Instantiate(towerPieces.laserRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.FROSTING)
            {
                roof = Instantiate(towerPieces.frostingRoof1x1, roofPosition, forward, this.transform);
            }

            if(roof != null)
            {
                BoxCollider topCollider = roof.AddComponent<BoxCollider>();
                topCollider.size = Vector3.one;
                Vector3 adjust = new Vector3(0.5F, 0, 0.5F);
                topCollider.center = topCollider.center - adjust;

                Rigidbody rb = roof.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;

            }
        }
        else if(width == 2 && height == 2)
        {
            Vector3 gridAlignment = new Vector3(1.5F, 0, 1.5F);
            float segmentHeight = 0.855F;
            Quaternion forward = new Quaternion();
            forward = Quaternion.Euler(0, 0, 0);

            Quaternion segmentForward = new Quaternion();
            segmentForward = Quaternion.Euler(-90, 0, 0);

            GameObject newbase = Instantiate(towerPieces.base2x2,  basePosition + gridAlignment, segmentForward, this.transform);
            newbase.GetComponent<MeshRenderer>().staticShadowCaster = true;

            for(int i = 0; i < floorCount; i++)
            {
                Vector3 position = new Vector3(0, 0.15F + (i * segmentHeight), 0);
                GameObject towerPiece = Instantiate(towerPieces.segment2x2, position + basePosition + gridAlignment, segmentForward, this.transform);
                towerPiece.GetComponent<MeshRenderer>().staticShadowCaster = true;
                GameObject pickedWrap = towerPieces.siding2x2[Random.Range(0, towerPieces.siding2x2.Count)];
                GameObject newWrap = Instantiate(pickedWrap, position + basePosition + gridAlignment, segmentForward, this.transform);
                newWrap.GetComponent<MeshRenderer>().staticShadowCaster = true;
            }

            Vector3 roofPosition = new Vector3(0, 0.15F + floorCount * segmentHeight);
            roofPosition += basePosition + gridAlignment;

            GameObject roof = null;

            if (attackState == TowerType.PEPPERMINT)
            {
                roof = Instantiate(towerPieces.peppermintRoof2x2, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.SPRINKLES)
            {
                roof = Instantiate(towerPieces.sprinklesRoof2x2, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.LASER)
            {
                roof = Instantiate(towerPieces.laserRoof2x2, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.FROSTING)
            {
                roof = Instantiate(towerPieces.frostingRoof2x2, roofPosition, forward, this.transform);
            }

            if(roof != null)
            {
                BoxCollider topCollider = roof.AddComponent<BoxCollider>();
                topCollider.size = Vector3.one;
                Vector3 adjust = new Vector3(0.5F, 0, 0.5F);
                topCollider.center = topCollider.center - adjust;
                
                Rigidbody rb = roof.AddComponent<Rigidbody>();
                rb.isKinematic = true;
                rb.useGravity = false;
            }
            
        }
    }

    public void destroyMe()
    {
        TowerGrid grid = GameObject.FindGameObjectWithTag("TowerGrid").GetComponent<TowerGrid>();
        grid.removeBlocker(this);
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
