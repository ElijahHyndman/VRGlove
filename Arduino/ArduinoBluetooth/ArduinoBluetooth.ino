
#include <SoftwareSerial.h> 
#include <string.h>

// Pin Numbers 
// from https://forum.arduino.cc/t/trying-to-identify-pins-arduino-nano-3-0/497650
int LED = 4;    // D04
int SENSOR = A0;// A00 

// Communication
int baud_rate = 9600;
// number of bits for number formatting to Serial Port
int bitWidth = 12; // bits
int txd = 3; // D02
int rxd = 2; // D03

// Other
// half of the total period in which the LED is on 
int halfFreq = 100; 

SoftwareSerial bluetoothSerial(rxd, txd);

void setup() {
  // put your setup code here, to run once:
  Serial.begin(baud_rate);
  pinMode(LED, OUTPUT);
  bluetoothSerial.begin(baud_rate);
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

int timer;
int V; 
int A;
int min = 0;
int max = 1024;

bool connected;
char inBT;

String str = "!";

void loop() {
  
  
  
  // Flash LED 
  flash();
  
  
  //Read bluetooth for calibrate signal
  inBT = '0';
  inBT = bluetoothSerial.read()
  if(inBT == '#'){
    timer = 5000;
  }
  
  //Calibration loop
  while(timer > 0){
    V = analogRead(SENSOR);
    if(V < min){
      min = V;
    }
    if(V > max){
      max = V;
    }
    timer--;
    delay(1);
  }
  

  // Read Sensor
  V = analogRead(SENSOR);
  
  //Convert to angle
  
  str = String(V);

  bluetoothSerial.print(str);
  Serial.print(str);

  delay(100);
}
