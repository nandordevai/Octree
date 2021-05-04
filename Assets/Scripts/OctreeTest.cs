using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeTest : MonoBehaviour
{
    public float size = 5;
    public Material foundMaterial;

    void Start()
    {
        Octree ot = new Octree(new BoundingBox(Vector3.zero, size, size, size));
        for (int i = 0; i < 300; i++)
        {
            ot.Insert(new Vector3(
                Random.Range(-size, size),
                Random.Range(-size, size),
                Random.Range(-size, size)
            ));
        }
        ot.Show();
        BoundingBox b = new BoundingBox(Vector3.zero, size / 2, size / 2, size / 2);
        List<Vector3> found = new List<Vector3>();
        ot.Query(b, ref found);
        foreach (var f in found)
        {
            GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.GetComponent<MeshRenderer>().material = foundMaterial;
            s.transform.localScale = new Vector3(.1f, .1f, .1f);
            s.transform.position = f;
        }
    }
}
