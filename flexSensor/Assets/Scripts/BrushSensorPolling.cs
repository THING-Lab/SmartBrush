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
 * Built of off of Arduino Serial Reading Code Created by Daniel Wilches
 */
public class BrushSensorPolling : MonoBehaviour
{
    public SerialController serialController;

    public GameObject Brush, bristle_A, bristle_B, bristle_C, bristle_D, bristle_E, bristle_F;
    
    private int [] bristleCalibrationBend = {600, 600, 600, 600, 600, 600 };
    private int [] currentBristleBend = { 600, 600, 600, 600, 600, 600 };
    public float bristleAngleScalefactor = 0.1f;
    public float bristleNegativeThreshold = 0.2f;
    public float bendThreshold = 2;

    // Initialization
    void Start()
    {
        //Make sure SerialContoller Game object is in Hirarchy and has SerialController Script attatched
        //Also make sure Port name on script is correct Com Port fro the arduino
        serialController = GameObject.Find("SerialController").GetComponent<SerialController>();


        Brush = GameObject.FindWithTag("Brush");
        //Find bristles to set location and rotation
        bristle_A = GameObject.FindWithTag("Bristle_A");
        bristle_B = GameObject.FindWithTag("Bristle_B");
        bristle_C = GameObject.FindWithTag("Bristle_C");
        bristle_D = GameObject.FindWithTag("Bristle_D");
        bristle_E = GameObject.FindWithTag("Bristle_E");
        bristle_F = GameObject.FindWithTag("Bristle_F");
        
        if(Brush == null)
            Debug.Log("Brush Not Found!");
    }

    // Executed each frame
    void Update()
    {
        //---------------------------------------------------------------------
        // Send data
        //---------------------------------------------------------------------
        if(Brush == null)
            Debug.Log("Brush Not Found!");
        if(bristle_A == null)
            Debug.Log("Brush Not Found!");
        
        //Set Burshs in the correct positin relative to their parent(Bristle_XP)
        bristle_A.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        bristle_B.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        bristle_C.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        bristle_D.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        bristle_E.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        bristle_F.transform.localPosition = new Vector3 (-1.3f, 0f, 0);
        
        //Set Bristles to Flat Position 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            setBristleCalibrationBend();
        }
        
        //Pulling Data From Arduino Serial Port
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
        //Pulls data from serial command line of arduino
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
        //Calculate how far to bent the Bristles based on Flexsensor values
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
        //Calibrating bristles to a set starting position
        for (int i = 0; i < 6; i++) {
            bristleCalibrationBend[i] = currentBristleBend[i];
        }
    }

    private void applyBendToBristles()
    {
        //Bends Bristles After all calculations are made
        GameObject bristle;

        for (int i = 0; i < 6; i++) {
            
            switch (i) // select the right game object bristle by it's name in the scene.
            {
                case 0: bristle = bristle_A;  break;
                case 1: bristle = bristle_B;  break;
                case 2: bristle = bristle_C;  break;
                case 3: bristle = bristle_D;  break;
                case 4: bristle = bristle_E;  break;
                case 5: bristle = bristle_F;  break;
                default: bristle = GameObject.FindWithTag("").gameObject; break;
            }
            if(bristle == null)
                Debug.Log("Bristle Not Found!");
            else{

                float angle = backBend(i); 
            //Rotates around parent pivot location Bristle_XP
            bristle.transform.parent.localRotation = Quaternion.Euler(new Vector3(0, angle, 0));
           
            }
        }
    }
    private float backBend (int bristleID) {
        //Allows bristle to bend backwards if bend back to a certain degree 
        //and neighbor bristle is also bending backwards 
        //Since flex sensors can only read values bending one way
        //Otherwise bends bristle forward to proper angle based off of flexsensor resistor value
        int neighborID = 0;
        if (bristleID % 2 == 0)
            neighborID = bristleID + 1;
        else
            neighborID = bristleID - 1;

        float bend = getBristleAngle(bristleID);
        float neighborBend = getBristleAngle(neighborID);

        if (bristleID % 2 == 0 && bend <= -bendThreshold && neighborBend <= -bendThreshold)
        {
             bend = neighborBend;
        }
         if (bristleID % 2 != 0 && bend >= bendThreshold && neighborBend >= bendThreshold)
        {
             bend = neighborBend;
        }

        return bend;
    }


    private void parseArduinoMessage(string dataString)
    {
        //Remove distinguishing character bit of flexsensor bristle
        //Take Bending value and apply is to the proper bristle to bend
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
