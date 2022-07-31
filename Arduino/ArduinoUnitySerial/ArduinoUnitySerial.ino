int baud_rate = 9600;

int halfFreq = 20; // half of the total period in which the LED is on 

int LED = 4;    // D02 

void setup() {
  // put your setup code here, to run once:
  Serial.begin(baud_rate);
  pinMode(LED, OUTPUT);
}

//
// Custom Functions
//
void flash() {
  digitalWrite(LED,HIGH);
  delay(halfFreq);
  digitalWrite(LED,LOW);
  delay(halfFreq);
}
//String intToFormatString(int num) {
//  // using solution from https://stackoverflow.com/questions/225362/convert-a-number-to-a-string-with-specified-length-in-c
//  char s[bitWidth];                 // Instantiate String
//  snprintf(s, bitWidth, "%0d", num);  // Fill String
//  return s;           
//}

int SENSOR1 = A0;// A00 
int SENSOR2 = A1;
int SENSOR3 = A2;
int V1, V2, V3; 
int A_1, A_2, A_3;
String message;
void loop() {
  // Flash LED 
  flash();

  // Read Sensor
  V1 = analogRead(SENSOR1);
  V2 = analogRead(SENSOR2);
  V3 = analogRead(SENSOR3);

  //Convert to Angle
//  A_1 = log((double)V1 / 810.54) / (-.027);
//  A_2 = log((double)V2 / 810.54) / (-.027);
//  A_3 = log((double)V3 / 810.54) / (-.027);
  // Print to USB
  Serial.print( String(V1) );
  Serial.print(".");
  Serial.print( String(V2) );
  Serial.print(".");
  Serial.print( String(V3) );
  Serial.print("\n");
  Serial.flush();
}
