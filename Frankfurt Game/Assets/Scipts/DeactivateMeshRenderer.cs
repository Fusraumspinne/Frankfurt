using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMeshRenderer : MonoBehaviour
{
    public string[] tags;
    void Start()
    {
        foreach (string tag in tags)
        {
            // Alle GameObjects mit dem aktuellen Tag finden
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objectsWithTag)
            {
                // Versuchen, den MeshRenderer zu bekommen
                MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

                if (meshRenderer != null)
                {
                    // MeshRenderer deaktivieren
                    meshRenderer.enabled = false;
                }
            }
        }
    }
}
