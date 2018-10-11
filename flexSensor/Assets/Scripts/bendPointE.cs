using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bendPointE : MonoBehaviour {

    public Transform point1, point2;
    static public float changep1Pos;
    private float changep2Pos;
    private Vector3 p1Pos, p2Pos;
    // Use this for initialization
    void Start()
    {

        changep1Pos = point1.transform.localPosition.z;
        changep2Pos = point2.transform.localPosition.y;

    }

    // Update is called once per frame
    void Update()
    {
        p1Pos.z = changep1Pos;

        p2Pos.y = changep2Pos - (changep1Pos / 10);
        point1.transform.localPosition = p1Pos;
        point2.transform.localPosition = p2Pos;
    }
}
