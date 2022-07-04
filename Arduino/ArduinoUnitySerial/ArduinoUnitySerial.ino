
// Pin Numbers 
// from https://forum.arduino.cc/t/trying-to-identify-pins-arduino-nano-3-0/497650
int LED = 2;    // D02 
int SENSOR = A0;// A00 

// Communication
int baud_rate = 9600;
int bitWidth = 12; // bits, number of bits for number formatting to Serial Port

// Other
int halfFreq = 100; // half of the total period in which the LED is on 

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


int V; 
String message;
void loop() {
  // Flash LED 
  flash();

  // Read Sensor
  V = analogRead(SENSOR);

  // Print to USB
  Serial.print( V );
//  Serial.print("\n");
  Serial.flush();

  //delay(200);
}
