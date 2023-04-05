using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class StreetWorldGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject segment;

    [SerializeField, Min(1)]
    private int length = 1;

    private void Start()
    {
        for(int x = 0; x < length; x++)
        {

            GameObject tempseg = Instantiate(segment, transform);
            tempseg.transform.localPosition += Vector3.right * (x * 3f);
            tempseg.GetComponent<StreetSegment>().GenerateSegment();
        }
    }
}
