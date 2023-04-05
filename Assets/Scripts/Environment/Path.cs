using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
    [DisallowMultipleComponent]
    public class Path : MonoBehaviour
    {
        public void Setup(Vector3 _startPos, Vector3 _endPos)
        {
            Vector3 dir = _endPos - _startPos;
            transform.forward = dir;
            float length = dir.magnitude;
            transform.position = _startPos + dir.normalized * length * 0.5f;
            transform.localScale = new Vector3(0.5f, 1, length);

            Material material = GetComponentInChildren<Renderer>().material;
            material.SetFloat("_Scale", length);
        }
    }
}
