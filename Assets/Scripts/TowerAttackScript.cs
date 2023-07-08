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

            Instantiate(towerPieces.base1x1, basePosition + gridAlignment, segmentForward, this.transform);

            for(int i = 0; i < floorCount; i++)
            {
                Vector3 position = new Vector3(0, 0.1F + (i * segmentHeight), 0);
                Instantiate(towerPieces.segment1x1, position + basePosition + gridAlignment, segmentForward, this.transform);

                GameObject pickedWrap = towerPieces.siding1x1[Random.Range(0, towerPieces.siding1x1.Count)];
                Vector3 detailOffset = new Vector3(0.507859F, 0, 0.516459F);
                Instantiate(pickedWrap, position + basePosition - detailOffset + gridAlignment, segmentForward, this.transform);
            }

            Vector3 roofPosition = new Vector3(0, 0.1F + floorCount * segmentHeight);
            roofPosition += basePosition + gridAlignment;

            if (attackState == TowerType.PEPPERMINT)
            {
                Instantiate(towerPieces.peppermintRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.SPRINKLES)
            {
                Instantiate(towerPieces.sprinklesRoof1x1, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.LASER)
            {
                Instantiate(towerPieces.laserRoof1x1, roofPosition, forward, this.transform);
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

            Instantiate(towerPieces.base2x2,  basePosition + gridAlignment, segmentForward, this.transform);

            for(int i = 0; i < floorCount; i++)
            {
                Vector3 position = new Vector3(0, 0.15F + (i * segmentHeight), 0);
                Instantiate(towerPieces.segment2x2, position + basePosition + gridAlignment, segmentForward, this.transform);
                GameObject pickedWrap = towerPieces.siding2x2[Random.Range(0, towerPieces.siding2x2.Count)];
                Instantiate(pickedWrap, position + basePosition + gridAlignment, segmentForward, this.transform);
            }

            Vector3 roofPosition = new Vector3(0, 0.15F + floorCount * segmentHeight);
            roofPosition += basePosition + gridAlignment;

            if (attackState == TowerType.PEPPERMINT)
            {
                Instantiate(towerPieces.peppermintRoof2x2, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.SPRINKLES)
            {
                Instantiate(towerPieces.sprinklesRoof2x2, roofPosition, forward, this.transform);
            }
            else if (attackState == TowerType.LASER)
            {
                Instantiate(towerPieces.laserRoof2x2, roofPosition, forward, this.transform);
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
