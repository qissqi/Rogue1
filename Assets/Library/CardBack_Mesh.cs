using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardBack_Mesh : MonoBehaviour
{
    RawImage ri;
    private Mesh mesh;
    private MeshFilter filter;

    private void Awake()
    {
        filter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        filter.mesh = mesh;


        Vector3[] vertices = new Vector3[]
        {
        new Vector3(-50,75,0),
        new Vector3(50,75,0),
        new Vector3(50,-75,0),
        new Vector3(-50,-75,0)

        };

        int[] triangles = new int[]
        {
            2,3,0,
            0,1,2
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0,1),
            new Vector2(1,1),
            new Vector2(1,0),
            new Vector2(0,0),
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

    }
}
