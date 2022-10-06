using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;
    public float meshResolution;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();
    public MeshFilter viewMeshFilter;
    public int edgeResolveIterations;
    public float edgeDstThreshold;
    Mesh viewMesh;

    public float maskcutawayDst = .1f;
    void Start() 
    {
        viewMesh = new Mesh();//intializing mesh
        viewMesh.name = "View Mesh";//intializing mesh name
        viewMeshFilter.mesh = viewMesh;//intializing mesh in meshfilter
        StartCoroutine ("FindTargetsWithDelay", .2f);//starting a couroutine
    }

    void LateUpdate()
    {
        DrawfieldOfView(); // calling DrawfieldOfView after Update()
    }
    
    IEnumerator FindTargetsWithDelay(float delay) {
        while(true) {
            yield return new WaitForSeconds (delay); // wait for 0.2 seconds
            FindVisibleTargets (); // call FindVisibleTargets function 
        }
    }

    void FindVisibleTargets() 
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

        for(int i=0; i<targetsInViewRadius.Length; i++) {
        Transform target = targetsInViewRadius [i].transform;//intializing target from targetInViewRadius array
        Vector3 dirToTarget = (target.position - transform.position).normalized;//direction from player to target
        
        if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) // if angle between player and target is less than viewangle - true
        {
        float distotarget = Vector3.Distance(transform.position, target.position);//distance from player to target
        if(!Physics.Raycast (transform.position, dirToTarget, distotarget, obstacleMask)) //Raycast(Vector3 origin, Vector3 direction, float maxDistance, int ignore layerMask) - false
        {
            visibleTargets.Add(target); // Add targets in visibleTargets List
        }
        }
    }
    }

    void DrawfieldOfView() 
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);// stepCount = Mathf.RoundToInt(viewAngle * meshResolution)
        float stepAngleSize = viewAngle / stepCount;// stepAngleSize = viewAngle / stepCount
        List<Vector3> viewPoints = new List<Vector3>();// viewPoints is a list of Vector3
        ViewCastInfo oldViewCast = new ViewCastInfo();// OldViewCast is a ViewCastInfo 
        for(int i=0; i<= stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;// angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i
            Debug.DrawLine(transform.position, transform.position + DirFormAngle(angle, true) * viewRadius, Color.red);// Draw redline from player to target
            ViewCastInfo newViewCast = ViewCast (angle); // call ViewCast function by sending angle

            if(i>0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst-newViewCast.dst) > edgeDstThreshold; // if edgeDstThreshold angle is lesser than Mathf.Abs(oldViewCast.dst-newViewCast.dst) - true
                if(oldViewCast.hit != newViewCast.hit || oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded) // -true
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast); // 
                    if (edge.pointA != Vector3.zero) // if edge.pointA != Vector3.zero
                    {
                        viewPoints.Add(edge.pointA); // edge.pointA adding in viewPoints list 
                    }
                    if (edge.pointB != Vector3.zero) // if edge.pointB != Vector3.zero
                    {
                        viewPoints.Add(edge.pointB); // edge.pointB adding in viewPoints list
                    }
                }
            }
             
            viewPoints.Add(newViewCast.points); // newViewCast points are adding in viewPoints list
            oldViewCast = newViewCast;          // oldViewCast = NewViewCast 
        }

            int vertexCount = viewPoints.Count + 1; // VertexCount is intialize by all viewPoints + 1
            Vector3[] vertices = new Vector3[vertexCount]; // Vector3 array name vertices is initialize with vertexCount size 
            int[] triangles = new int [(vertexCount - 2) * 3]; // int array name triangles is initialize with (vertexCount size - 2) * 3 

            vertices [0] = Vector3.zero; // vertices[0] = 0
            for(int i=0; i <  vertexCount - 1; i++) // loop till vertexCount
            {
                vertices[i + 1] = transform.InverseTransformPoint(viewPoints [i] + Vector3.forward * maskcutawayDst); // Transforms position from world space to local space.
                if( i < vertexCount-2) // if vertexCount - 2 > i
                {
                triangles[i * 3] = 0; // 0*3 - 0 = 0 ,1*3 - 3 = 0
                triangles[i* 3 + 1] = i+1; // 0*3+1 - 0+1 = 0+1 ,1*3+1 - 3+1 = 0+1 
                triangles[i* 3 + 2] = i+2; // 0*3+2 - 0+2 = 0+2 ,2*3+2 - 6+2 = 0+2
                }
            }

            viewMesh.Clear(); // clear all mesh
            viewMesh.vertices = vertices; // initializing new vertices in viewMesh.vertices
            viewMesh.triangles = triangles; // initializing new triangles in viewMesh.triangles
            viewMesh.RecalculateNormals(); // recalculating normal from new vertices and triangles
    }       


    public Vector3 DirFormAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)// - true
        {
            angleInDegrees += transform.eulerAngles.y; // // initializing angleInDegrees
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad)); // convert angle deg2Rad
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast) // bineary searching
    {
        float minAngle = minViewCast.angle; // initializing minviewcast angle
        float maxAngle = maxViewCast.angle; // initializing maxviewcast angle
        Vector3 minPoint = Vector3.zero; // minPoint = zero
        Vector3 maxPoint = Vector3.zero; // maxPoint = zero

        for(int i=0; i<edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle)/2;
            ViewCastInfo newViewCast = ViewCast(angle); // initializing new viewcast angle
 bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst-newViewCast.dst) > edgeDstThreshold; // if Mathf.Abs(minViewCast.dst-newViewCast.dst) > edgeDstThreshold - true
            if(newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) // if - true
            {
                minAngle = angle;
                minPoint = newViewCast.points;
            }
            else                                                                // else
            {
                maxAngle = angle;
                maxPoint = newViewCast.points;
            }
        }
        return new EdgeInfo(minPoint,maxPoint);                                 // recursive function call
    }

    ViewCastInfo ViewCast(float globalAngle) //ViewCastInfo function
    {
        Vector3 dir = DirFormAngle(globalAngle, true); // calling DirFromAngle
        RaycastHit hit;

        if (Physics.Raycast (transform.position, dir, out hit, viewRadius, obstacleMask)) { // if ray intersect with collider - true
            return new ViewCastInfo (true, hit.point, hit.distance, globalAngle); // return ViewCastInfo();
        }
        else
        {
            return new ViewCastInfo (false, transform.position + dir * viewRadius, viewRadius, globalAngle); // return ViewCastInfo();
        }
    }
    public struct ViewCastInfo { //ViewCastInfo structure with constructor
        public bool hit;
        public Vector3 points;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit,Vector3 _point, float _dst, float _angle) // construtor
        {
            hit = _hit;
            points = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo {  //EdgeInfo structure with constructor
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB) // construtor
        {
            pointA = _pointA;
            pointB = _pointB;
        }

    }
}

