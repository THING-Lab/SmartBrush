using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Used for intial implementation but code is not 
 * used nor necessary for current brush implementation
 * but will keep in case we wish is make the bristles 
 * have a sort of bend while painting */
public class lineBezier : MonoBehaviour {
    public LineRenderer lineRend;
    public Transform point0, point1, point2;

    static private int numPoint = 50;
    private Vector3[] pos = new Vector3[numPoint];
    // Use this for initialization
    void Start() {
        lineRend.positionCount = numPoint;
        //DrawLinearCurve();
        DrawQuadraticCurve();
    }

    // Update is called once per frame
    void Update() {
        DrawQuadraticCurve();
    }
    private void DrawLinearCurve()
    {
        for(int i=1; i<numPoint+1; i++)
        {
            float t = i / (float)numPoint;
            pos[i - 1] = calcLineBendPoint(t, point0.position, point1.position);

        }
        lineRend.SetPositions(pos);
    }
    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoint + 1; i++)
        {
            float t = i / (float)numPoint;
            pos[i - 1] = calcQuadBendPoint(t, point0.position, point1.position, point2.position);

        }
        lineRend.SetPositions(pos);
    }
    private Vector3 calcLineBendPoint(float t, Vector3 p0, Vector3 p1){
        return p0 + t * (p1-p0);
        }
    private Vector3 calcQuadBendPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = (uu*p0) + (2*u*t*p1) +(tt*p2);
        return p;
    }
}
