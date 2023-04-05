using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSegment : MonoBehaviour
{
    [SerializeField]
    private HouseSpawner house;
    [SerializeField]
    private StreetSpawner street;

    [ContextMenu("Zauber")]
    public void GenerateSegment()
    {
        house.SpawnHouse();
        street.SpawnStreet();
    }
}
