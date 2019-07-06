#include <ESP8266WiFi.h>        // Include the Wi-Fi library
#include <WiFiClient.h>

const char* ssid     = "UCB Wireless";         // The SSID (name) of the Wi-Fi network you want to connect to
//const char* password = "PASSWORD";     // The password of the Wi-Fi network

WiFiServer server(26);
WiFiClient client;


void setup() {
  Serial.begin(115200);         // Start the Serial communication to send messages to the computer
  delay(10);
  Serial.println('\n');
  
  WiFi.begin(ssid);             // Connect to the network
  Serial.print("Connecting to ");
  Serial.print(ssid); Serial.println(" ...");
  

  int i = 0;
  while (WiFi.status() != WL_CONNECTED) { // Wait for the Wi-Fi to connect
    delay(1000);
    Serial.print(++i); Serial.print(' ');
  }

  Serial.println('\n');
  Serial.println("Connection established!");  
  Serial.print("IP address:\t");
  Serial.println(WiFi.localIP());         // Send the IP address of the ESP8266 to the computer

  server.begin();
//  Serial.println("point a");
}

void loop() {
    client = server.available();
    if (client){
      Serial.println("Client connected");
      while (client.connected()){
        client.print(millis()%10);
        Serial.println(millis()%10);
      }
    }

    
  }
