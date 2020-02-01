using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SculptureChecker : MonoBehaviour
{

    public float  CalcScreenPercentage(Camera relevantCamera)
    {

        float minX = Mathf.Infinity;
        float minY = Mathf.Infinity;
        float maxX = -Mathf.Infinity;
        float maxY = -Mathf.Infinity;

        Bounds bounds = GetComponent<MeshFilter>().mesh.bounds;
        Vector3 v3Center = bounds.center;
        Vector3 v3Extents = bounds.extents;

        Vector3[] corners = new Vector3[8];

        corners[0] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top left corner
        corners[1] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z - v3Extents.z);  // Front top right corner
        corners[2] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom left corner
        corners[3] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z - v3Extents.z);  // Front bottom right corner
        corners[4] = new Vector3(v3Center.x - v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top left corner
        corners[5] = new Vector3(v3Center.x + v3Extents.x, v3Center.y + v3Extents.y, v3Center.z + v3Extents.z);  // Back top right corner
        corners[6] = new Vector3(v3Center.x - v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom left corner
        corners[7] = new Vector3(v3Center.x + v3Extents.x, v3Center.y - v3Extents.y, v3Center.z + v3Extents.z);  // Back bottom right corner

        for (int i = 0; i < corners.Length; i++)
        {
            Vector3 corner = transform.TransformPoint(corners[i]);
            corner = relevantCamera.WorldToScreenPoint(corner);
            if (corner.x > maxX) maxX = corner.x;
            if (corner.x < minX) minX = corner.x;
            if (corner.y > maxY) maxY = corner.y;
            if (corner.y < minY) minY = corner.y;
            minX = Mathf.Clamp(minX, 0, Screen.width);
            maxX = Mathf.Clamp(maxX, 0, Screen.width);
            minY = Mathf.Clamp(minY, 0, Screen.height);
            maxY = Mathf.Clamp(maxY, 0, Screen.height);
        }

        float width = maxX - minX;
        float height = maxY - minY;
        float area = width * height;
        float percentage = area / (Screen.width * Screen.height) * 100.0f;
        return percentage;
    }
}
