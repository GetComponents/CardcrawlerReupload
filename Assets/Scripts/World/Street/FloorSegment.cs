using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSegment : MonoBehaviour
{
    [SerializeField]
    private Transform spawnPoint;

    public void Generate(GameObject _ornament, Material _floorMat, Material _decoMat)
    {
        GetComponentInChildren<Renderer>().material = _floorMat;

        Renderer[] render = Instantiate(_ornament, spawnPoint.position, spawnPoint.rotation, spawnPoint).GetComponentsInChildren<Renderer>();
        foreach (Renderer ren in render)
        {
            ren.material = _decoMat;
        }
    }
}
