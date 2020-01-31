using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRotate : MonoBehaviour
{
    public Vector3 rotateVector;

    private void Update()
    {
        transform.Rotate(rotateVector * Time.deltaTime);
    }

}
