using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class adruinoConnetion : MonoBehaviour {

    public static SerialPort arduino = new SerialPort("COM7", 9600);
    private string dataString;
    private char[] myDisBit;

    static public float FS_A, FS_B, FS_C, FS_D, FS_E;

    // Use this for initialization
    private void Awake()
    {
        openPort();
    }
    void Start()
    {

        Debug.Log(arduino.PortName);
        transform.position = new Vector3(0, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
    

        if (arduino.IsOpen)
        {
            dataString = arduino.ReadLine();

            myDisBit = dataString.ToCharArray();
            changeBend(myDisBit);

            // Debug.Log("dataString: " + dataString);
            //Debug.Log("disbit0: " + disBit[0]);
        }

        // Debug.Log("transform pos x: " + transform.position.x);
        //arduino.ReadTimeout = 1000;

    }

    public void changeBend(char[] disBit)
    {
        string degree;
        switch (disBit[0])
        {
            case 'A':
                degree = dataString.Remove(0, 1);
                //Debug.Log("degreeA: " + degree);

                FS_A = float.Parse(degree);
                // Debug.Log(FS_A: " + FS_A);
                 bendPointA.changep1Pos = FS_A / 18;
                //Debug.Log("bendPointA.changep1Pos: " + bendPointA.changep1Pos);
                break;
            case 'B':
                degree = dataString.Remove(0, 1);
                //Debug.Log("degreeB: " + degree);

                FS_B = float.Parse(degree);
                // Debug.Log("FS_B: " + FS_B);

                bendPointB.changep1Pos = FS_B / 18;
                //Debug.Log("bendPointB.changep1Pos: " + bendPointB.changep1Pos);
                break;
            case 'C':
                degree = dataString.Remove(0, 1);
                // Debug.Log("degreeC: " + degree);

                FS_C = float.Parse(degree);
                // Debug.Log("FS_C: " + FS_C);

                bendPointC.changep1Pos = FS_C / 18;
                //Debug.Log("bendPointC.changep1Pos: " + bendPointC.changep1Pos);

                break;
            case 'D':
                degree = dataString.Remove(0, 1);
                // Debug.Log("degreeD: " + degree);

                FS_D = float.Parse(degree);
                // Debug.Log("FS_D: " + FS_D);

                bendPointD.changep1Pos = FS_D / 18;
                //Debug.Log("bendPointD.changep1Pos: " + bendPointD.changep1Pos);

                break;
            case 'E':
                degree = dataString.Remove(0, 1);
                // Debug.Log("degreeE: " + degree);

                FS_E = float.Parse(degree);
                // Debug.Log("FS_E: " + FS_E);

                bendPointE.changep1Pos = FS_E / 18;
                //Debug.Log("bendPointE.changep1Pos: " + bendPointE.changep1Pos);

                break;
        }


    }

    public void openPort()
    {
        if (arduino != null)
        {
            if (arduino.IsOpen)
            {
                arduino.Close();
                Debug.Log("Port already open, now closing.");
            }
            else
            {
                arduino.Open();
                arduino.ReadTimeout = 500;
                Debug.Log("Port Opened");
            }
        }
        else
        {
            if (arduino.IsOpen)
            {
                print("Port Open");
            }
            else
            {
                print("No port found.");
            }
        }
    }
}
