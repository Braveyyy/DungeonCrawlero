using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasureRoomSize : MonoBehaviour
{
    void Start()
    {
        // Initialize bounds based on the first MeshRenderer found
        Bounds combinedBounds = new Bounds(transform.position, Vector3.zero);

        // Find all MeshRenderer components in the room and combine their bounds
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            combinedBounds.Encapsulate(meshRenderer.bounds);
        }

        // Log the total dimensions of the room
        Debug.Log("ROOMSIZE: " + combinedBounds.size);
        Debug.Log("Room Total Size (Width): " + combinedBounds.size.x + ", Height: " + combinedBounds.size.y + ", Depth: " + combinedBounds.size.z);
    }
}
