using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ModelReferences : ScriptableObject
{
    [Header("Roof 1x1")]
    public GameObject sprinklesRoof1x1;
    public GameObject peppermintRoof1x1;
    public GameObject laserRoof1x1;
    public GameObject frostingRoof1x1;

    [Header("Roof 2x2")]
    public GameObject sprinklesRoof2x2;
    public GameObject peppermintRoof2x2;
    public GameObject laserRoof2x2;
    public GameObject frostingRoof2x2;

    [Header("Bases")]
    public GameObject base1x1;
    public GameObject base2x2;

    [Header("Segments")]
    public GameObject segment1x1;

    public GameObject segment2x2;

    [Header("Decor")]
    public List<GameObject> siding1x1;
    public List<GameObject> siding2x2;
}