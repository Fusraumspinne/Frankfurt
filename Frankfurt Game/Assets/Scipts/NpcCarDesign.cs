using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCarDesign : MonoBehaviour
{
    public Material[] materials;

    public GameObject[] objects;

    void Start()
    {
        Material selectedMaterial = materials[Random.Range(0, materials.Length)];

        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = selectedMaterial;
        }

        foreach (GameObject obj in objects)
        {
            obj.SetActive(false);
        }

        GameObject selectedObject = objects[Random.Range(0, objects.Length)];
        selectedObject.SetActive(true);
    }
}
