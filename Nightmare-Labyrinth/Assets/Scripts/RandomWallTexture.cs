using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWallAndFloorTexture : MonoBehaviour
{
    public GameObject wallPrefab;
    public string wallTextureFolderPath = "Textures/Walls"; 
    public string floorTextureFolderPath = "Textures/Floors"; 

    private void Start()
    {
        // Load all wall textures from the folder
        Texture2D[] wallTextures = Resources.LoadAll<Texture2D>(wallTextureFolderPath);
        // Load all floor textures from the folder
        Texture2D[] floorTextures = Resources.LoadAll<Texture2D>(floorTextureFolderPath);

        //Check to see if textures are avaialable.
        if (wallTextures.Length == 0 || floorTextures.Length == 0)
        {
            Debug.LogError("No textures found in one or more specified folders.");
            return;
        }

        // Select one random texture for walls and one for floors
        Texture2D randomWallTexture = wallTextures[Random.Range(0, wallTextures.Length)];
        Texture2D randomFloorTexture = floorTextures[Random.Range(0, floorTextures.Length)];

        // Create materials for the selected textures
        Material wallMaterial = new Material(Shader.Find("Standard"));
        wallMaterial.mainTexture = randomWallTexture;

        Material floorMaterial = new Material(Shader.Find("Standard"));
        floorMaterial.mainTexture = randomFloorTexture;

        // Apply the wall material to all wall objects
        ApplyMaterialToObjectsWithTag("Wall", wallMaterial);

        // Apply the floor material to all maze cells
        ApplyFloorMaterialToMazeCells(floorMaterial);
    }

    private void ApplyMaterialToObjectsWithTag(string tag, Material material)
    {

        //Finds game objects with specific tags and adds those materials to the tag.
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);

        foreach (GameObject obj in objects)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                objRenderer.material = material;
            }
        }
    }

    private void ApplyFloorMaterialToMazeCells(Material material)
    {
        GameObject[] mazeCells = GameObject.FindGameObjectsWithTag("MazeCell");

        foreach (GameObject mazeCell in mazeCells)
        {
            // Find the plane inside the maze cell
            Transform floorPlaneTransform = mazeCell.transform.Find("Floor");
            if (floorPlaneTransform != null)
            {
                Renderer floorRenderer = floorPlaneTransform.GetComponent<Renderer>();
                if (floorRenderer != null)
                {
                    floorRenderer.material = material;
                }
            }
        }
    }
}
