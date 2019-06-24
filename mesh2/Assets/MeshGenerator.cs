using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {

    public GameObject player;
    bool playerSpawned;

    public NavMeshSurface surface;
    Mesh mesh;
    public float y;
    public Color pxlClr;
    private ProcessingToTexture depthMap;
    public GameObject kinectMap;

    Vector3[] vertices;
    int[] triangles;
    public int xSize = 48;
    public int zSize = 40;

    Color[] colors;
    public Gradient gradient;
    float minTerrainHeight;
    float maxTerrainHeight;

    void Awake()
    {
        depthMap = kinectMap.GetComponent<ProcessingToTexture>();
    }

    void Start () {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        depthMap.Start();
        surface.BuildNavMesh();
        SpawnPlayer();
    }

    void Update()
    {
        depthMap.Update();
        UpdateShape();
        UpdateMesh();
        surface.BuildNavMesh();
    }

    void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize +1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                y = 2;
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateShape()
    {
        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                
                pxlClr = depthMap.getPixel(x * 11, z * 11);

                y = 3f;
                if (pxlClr.r > 0.950 && pxlClr.g < 0.050)
                {
                    y = 10f;
                }
                if (pxlClr.r > 0.950 && pxlClr.g < 0.275 && pxlClr.g > 0.225)
                {
                    y = 9f;
                }
                if (pxlClr.r > 0.950 && pxlClr.g < 0.525 && pxlClr.g > 0.475)
                {
                    y = 8f;
                }
                if (pxlClr.r > 0.950 && pxlClr.g < 0.775 && pxlClr.g > 0.725)
                {
                    y = 7f;
                }
                if (pxlClr.r > 0.950 && pxlClr.g > 0.950)
                {
                    y = 6f;
                }
                if (pxlClr.r > 0.725 && pxlClr.r < 0.775 && pxlClr.g > 0.950)
                {
                    y = 5f;
                }
                if (pxlClr.r > 0.5 && pxlClr.r < 0.575 && pxlClr.g > 0.950)
                {
                    y = 4f;
                }
                if (pxlClr.r < 0.050 && pxlClr.g > 0.950)
                {
                    y = 3f;
                }
                vertices[i] = new Vector3(x, y, z);

                if (y > maxTerrainHeight)
                    maxTerrainHeight = y;
                if (y < minTerrainHeight)
                    minTerrainHeight = y;

                i++;
            }
        }

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }

        colors = new Color[vertices.Length];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float height = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y); 
                colors[i] = gradient.Evaluate(height);
                i++;
            }
        }
    }

    /*void OnDrawGizmos()
    {
        if (vertices == null)
            return;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }*/

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.colors = colors;

        mesh.RecalculateNormals();
    }

    void SpawnPlayer()
    {
        Debug.Log(playerSpawned);
        if (playerSpawned == false)
        {
            Vector3 pos = new Vector3(6f, 3f, 20f);
            Instantiate(player, pos, Quaternion.identity);
            playerSpawned = true;
        }

    }
}
