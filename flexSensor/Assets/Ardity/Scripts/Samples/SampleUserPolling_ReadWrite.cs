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
public class SampleUserPolling_ReadWrite : MonoBehaviour
{


    static public float FS_A, FS_B, FS_C, FS_D, FS_E, FS_F;
    public SerialController serialController;

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
            changeBend(message);
    }

    public void changeBend(string dataString)
    {
        char bristleID = dataString[0];
        string degree = dataString.Remove(0, 1);
        switch (bristleID)
        {
            case 'A':
                FS_A = int.Parse(degree);
                bendPointA.changep1Pos = FS_A;
                break;
            case 'B':
                FS_B = int.Parse(degree);
                bendPointB.changep1Pos = FS_B;
                break;
            case 'C':
                FS_C = int.Parse(degree);
                bendPointC.changep1Pos = FS_C;
                break;
            case 'D':
                FS_D = int.Parse(degree);
                bendPointD.changep1Pos = FS_D;
                break;
            case 'E':
                FS_E = int.Parse(degree);
                bendPointE.changep1Pos = FS_E;
                break;
            case 'F':
                FS_F = int.Parse(degree);
                bendPointF.changep1Pos = FS_F;
                break;
        }
    }
}
