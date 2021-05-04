using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octree
{
    BoundingBox boundary;
    int capacity;
    List<Vector3> points = new List<Vector3>();
    bool divided = false;
    Octree une = null;
    Octree unw = null;
    Octree use = null;
    Octree usw = null;
    Octree lne = null;
    Octree lnw = null;
    Octree lse = null;
    Octree lsw = null;

    public Octree(BoundingBox boundary, int capacity = 4)
    {
        this.boundary = boundary;
        this.capacity = capacity;
    }

    public bool Insert(Vector3 point)
    {
        if (!boundary.Contains(point)) return false;
        if (points.Count < capacity)
        {
            points.Add(point);
        }
        else
        {
            if (!divided)
            {
                Subdivide();
            }
            une.Insert(point);
            unw.Insert(point);
            use.Insert(point);
            usw.Insert(point);
            lne.Insert(point);
            lnw.Insert(point);
            lse.Insert(point);
            lsw.Insert(point);
        }
        return true;
    }

    void Subdivide()
    {
        float w = boundary.width;
        float h = boundary.height / 2;
        float d = boundary.depth;
        une = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x + w,
                    boundary.center.y + h,
                    boundary.center.z + d
                ),
                w, h, d
            ),
            capacity
        );
        unw = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x - w,
                    boundary.center.y + h,
                    boundary.center.z + d
                ),
                w, h, d
            ),
            capacity
        );
        use = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x + w,
                    boundary.center.y + h,
                    boundary.center.z - d
                ),
                w, h, d
            ),
            capacity
        );
        usw = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x - w,
                    boundary.center.y + h,
                    boundary.center.z - d
                ),
                w, h, d
            ),
            capacity
        );
        lne = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x + w,
                    boundary.center.y - h,
                    boundary.center.z + d
                ),
                w, h, d
            ),
            capacity
        );
        lnw = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x - w,
                    boundary.center.y - h,
                    boundary.center.z + d
                ),
                w, h, d
            ),
            capacity
        );
        lse = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x + w,
                    boundary.center.y - h,
                    boundary.center.z - d
                ),
                w, h, d
            ),
            capacity
        );
        lsw = new Octree(
            new BoundingBox(
                new Vector3(
                    boundary.center.x - w,
                    boundary.center.y - h,
                    boundary.center.z - d
                ),
                w, h, d
            ),
            capacity
        );
        divided = true;
    }

    public void Query(BoundingBox range, ref List<Vector3> found)
    {
        if (!boundary.Intersects(range)) return;
        foreach (var p in points)
        {
            if (range.Contains(p))
            {
                found.Add(p);
            }
        }
        if (divided)
        {
            une.Query(range, ref found);
            unw.Query(range, ref found);
            use.Query(range, ref found);
            usw.Query(range, ref found);
            lne.Query(range, ref found);
            lnw.Query(range, ref found);
            lse.Query(range, ref found);
            lsw.Query(range, ref found);
        }
    }

    public void Show()
    {
        foreach (var p in points)
        {
            GameObject s = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            s.transform.localScale = new Vector3(.05f, .05f, .05f);
            s.transform.position = p;
        }
        if (divided)
        {
            une.Show();
            unw.Show();
            use.Show();
            usw.Show();
            lne.Show();
            lnw.Show();
            lse.Show();
            lsw.Show();
        }
    }
}
