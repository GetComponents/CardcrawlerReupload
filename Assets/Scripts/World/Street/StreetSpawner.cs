using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform rightPoint;
    [SerializeField]
    private Transform leftPoint;

    [SerializeField]
    private List<Material> streetMaterial = new List<Material>();

    [SerializeField]
    private List<GameObject> streetDeco = new List<GameObject>();

    [ContextMenu("Spawn Street")]
    public void SpawnStreet()
    {
        GetComponentInChildren<Renderer>().material = (streetMaterial.Count > 1) ? streetMaterial[Random.Range(0, streetMaterial.Count-1)] : streetMaterial[0];

        if((int)(Random.Range(1, 10)) > 5)
        {
            Instantiate((streetDeco.Count > 1) ? streetDeco[Random.Range(0, streetDeco.Count - 1)] : streetDeco[0], rightPoint);
        }
        else
        {
            Instantiate((streetDeco.Count > 1) ? streetDeco[Random.Range(0, streetDeco.Count - 1)] : streetDeco[0], leftPoint);
        }
    } 
}
