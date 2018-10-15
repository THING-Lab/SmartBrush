const int FLEX_0 = A0;
const int FLEX_1 = A1;
const int FLEX_2 = A2;
const int FLEX_3 = A3;
const int FLEX_4 = A4;
const int FLEX_5 = A5;

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
