using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeRotate : MonoBehaviour
{
    private void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 100);
    }
}
