const int FLEX_0 = A0;
const int FLEX_1 = A1;
const int FLEX_2 = A2;
const int FLEX_3 = A3;
const int FLEX_4 = A4;
const int FLEX_5 = A5;

const float VCC = 4.98; 
const float R = 56000.0; 
int flexADC;
float flexV;
float flexR;
float angle;

const float STRAIGHT_RESISTANCE = 30000.0; // resistance when straight
const float BEND_RESISTANCE = 90000.0; // resistance at 90 deg

unsigned long last_time = 0;

void setup() 
{
  Serial.begin(9600);
  pinMode(FLEX_0, INPUT);
  pinMode(FLEX_1, INPUT);
  pinMode(FLEX_2, INPUT);
  pinMode(FLEX_3, INPUT);
  pinMode(FLEX_4, INPUT);
  pinMode(FLEX_5, INPUT);
}

void loop()
{
    // Print a heartbeat
    if (millis() > last_time + 2000)
    {
        //Serial.println("Arduino is alive!!");
        last_time = millis();
    }

    // Send some message when I receive an 'A' or a 'Z'.
    switch (Serial.read())
    {
        case 'A':
            Serial.println("A" + String(analogRead(FLEX_0))); 
            break;
            
        case 'B':
            Serial.println("B" + String(analogRead(FLEX_1))); 
            break;

        case 'C':
            Serial.println("C" + String(analogRead(FLEX_2))); 
            break;

        case 'D':
            Serial.println("D" + String(analogRead(FLEX_3))); 
            break;
            
        case 'E':
            Serial.println("E" + String(analogRead(FLEX_4))); 
            break;
            
        case 'F':
            Serial.println("F" + String(analogRead(FLEX_5))); 
            break;
    }
}


/*
void flexAngle(int sensor, char letter){
  flexADC = analogRead(sensor);
  flexV = flexADC * VCC / 1023.0;
  flexR = R * (VCC / flexV - 1.0);
  angle = map(flexR, STRAIGHT_RESISTANCE, BEND_RESISTANCE,0, 90.0);
  Serial.println(letter + String(angle)); 
}*/
