/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 */

using UnityEngine;
using System.Collections;

/**
 * Sample for reading using polling by yourself, and writing too.
 */
public class BrushSensorPolling : MonoBehaviour
{
    public SerialController serialController;
    
    private int [] bristleCalibrationBend = {600, 600, 600, 600, 600, 600 };
    private int [] currentBristleBend = { 600, 600, 600, 600, 600, 600 };
    public float bristleAngleScalefactor = 0.1f;
    public float bristleNegativeThreshold = 0.2f;

    // Initialization
    void Start()
    {
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------
        if (Input.GetKeyDown(KeyCode.C))
        {
            setBristleCalibrationBend();
        }

        serialController.SendSerialMessage("A");
        receiveMessage();
        serialController.SendSerialMessage("B");
        receiveMessage();
        serialController.SendSerialMessage("C");
        receiveMessage();
        serialController.SendSerialMessage("D");
        receiveMessage();
        serialController.SendSerialMessage("E");
        receiveMessage();
        serialController.SendSerialMessage("F");
        receiveMessage();
    }

    public void receiveMessage()
    {
        string message = serialController.ReadSerialMessage();
        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_CONNECTED))
            Debug.Log("Connection established");
        else if (ReferenceEquals(message, SerialController.SERIAL_DEVICE_DISCONNECTED))
            Debug.Log("Connection attempt failed or disconnection detected");
        else
            parseArduinoMessage(message);
    }


    private float getBristleAngle (int bristleID) {
        float bend = bristleCalibrationBend[bristleID] - currentBristleBend[bristleID];
        bend *= bristleAngleScalefactor; // multiply with the scale factor
        if (bristleID % 2 == 1)
        {
            return -bend;
        }
        return bend;
    }

    private void setBristleCalibrationBend()
    {
        for (int i = 0; i < 6; i++) {
            bristleCalibrationBend[i] = currentBristleBend[i];
        }
    }

    private void applyBendToBristles()
    {
        GameObject bristle;

        for (int i = 0; i < 6; i++) {
            
            switch (i) // select the right game object bristle by it's name in the scene.
            {
                case 0: bristle = GameObject.Find("/SmartBrush/Bristle_A"); break;
                case 1: bristle = GameObject.Find("/SmartBrush/Bristle_B"); break;
                case 2: bristle = GameObject.Find("/SmartBrush/Bristle_C"); break;
                case 3: bristle = GameObject.Find("/SmartBrush/Bristle_D"); break;
                case 4: bristle = GameObject.Find("/SmartBrush/Bristle_E"); break;
                case 5: bristle = GameObject.Find("/SmartBrush/Bristle_F"); break;
                default: bristle = GameObject.Find(""); break;
            }
            float angle = backBend(i);
            
            bristle.transform.Translate(2, 0, 0);
            bristle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            bristle.transform.Translate(-2, 0, 0);
//=======

//            float angle = getBristleAngle(i);
//            //Debug.Log(i +": Angle " + angle);
//            if (i % 2 == 1) // we treat even and odd brisles differently, as they are mounted flipped
//            {
//                if (angle > bristleNegativeThreshold) // if the bristle is bent in the direction that the sensor is not reacting to
//                {
//                    angle = getBristleAngle(i - 1); // just take the value of the neighboring paired bristle
//                }

//            }
//            else
//            {
//                if (angle < -bristleNegativeThreshold)  // if the bristle is bent in the direction that the sensor is not reacting to
//                {
//                    angle = getBristleAngle(i + 1); // just take the value of the neighboring paired bristle
//                }
//            }

//            bristle.transform.Translate(2, 0, 0); // transform so that it rotates around the right point
//            bristle.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // apply rotation
//            bristle.transform.Translate(-2, 0, 0); // transform back
//>>>>>>> origin/master
        }
    }
    private float backBend (int bristleID) {
        int neighborID = 0;
        if (bristleID % 2 == 0)
            neighborID = bristleID + 1;
        else
            neighborID = bristleID - 1;

        float bend = getBristleAngle(bristleID);
        float neighborBend = getBristleAngle(neighborID);

        if (bristleID % 2 == 0 && bend <= -2 && neighborBend <= -2)
        {
             bend = neighborBend;
        }
         if (bristleID % 2 != 0 && bend >= 2 && neighborBend >= 2)
        {
             bend = neighborBend;
        }

        return bend;
    }


    private void parseArduinoMessage(string dataString)
    {
        char bristleID = dataString[0];
        string degree = dataString.Remove(0, 1);
        int bristleBend = int.Parse(degree);

        switch (bristleID)
        {
            case 'A':
                currentBristleBend[0] = bristleBend;
                break;
            case 'B':
                currentBristleBend[1] = bristleBend;
                break;
            case 'C':
                currentBristleBend[2] = bristleBend;
                break;
            case 'D':
                currentBristleBend[3] = bristleBend;
                break;
            case 'E':
                currentBristleBend[4] = bristleBend;
                break;
            case 'F':
                currentBristleBend[5] = bristleBend;
                break;
        }
        applyBendToBristles();
    }
}
