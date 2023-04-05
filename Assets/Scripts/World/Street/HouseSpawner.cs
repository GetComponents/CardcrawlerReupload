using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject groundFloor;
    [SerializeField]
    private GameObject middleFloor;
    [SerializeField]
    private GameObject topFloor;

    [SerializeField]
    private List<Material> floorMaterials = new List<Material>();
    [SerializeField]
    private List<Material> decoMaterials = new List<Material>();

    [SerializeField]
    private List<GameObject> groundFloorDeco = new List<GameObject>();
    [SerializeField]
    private List<GameObject> topFloorDeco = new List<GameObject>();


    [ContextMenu("Spawn House")]
    public void SpawnHouse()
    {

        int midFloorCount = Random.Range(0, 4);
        Material floorMaterial = (floorMaterials.Count > 1) ? floorMaterials[Random.Range(0, floorMaterials.Count - 1)] : floorMaterials[0];
        Material windowMaterial = (decoMaterials.Count > 1) ? decoMaterials[Random.Range(0, decoMaterials.Count - 1)] : decoMaterials[0];    
 
        GameObject door = (groundFloorDeco.Count > 1) ? groundFloorDeco[Random.Range(0, groundFloorDeco.Count - 1)] : groundFloorDeco[0];
        GameObject window = (topFloorDeco.Count > 1) ? topFloorDeco[Random.Range(0, topFloorDeco.Count - 1)] : topFloorDeco[0];


        Instantiate(groundFloor, transform.position, transform.rotation, this.transform).GetComponent<FloorSegment>().Generate(door, floorMaterial, windowMaterial);
        if(midFloorCount > 0)
        {
            for(int x = 0; x < midFloorCount; x++)
            {
                Instantiate(middleFloor, transform.position + Vector3.up * ((x * 2.5f) + 2.45f) , transform.rotation, this.transform).GetComponent<FloorSegment>().Generate(window, floorMaterial, windowMaterial);
            }
        }
        Instantiate(topFloor, transform.position + Vector3.up * ((midFloorCount * 2.5f) + 2.45f), transform.rotation, this.transform).GetComponent<FloorSegment>().Generate(window, floorMaterial, windowMaterial);
    }
}
