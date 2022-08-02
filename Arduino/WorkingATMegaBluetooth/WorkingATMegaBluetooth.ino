// Example code from https://www.arnabkumardas.com/arduino-tutorial/usart-programming/

/*
 * 
 * -- BLUETOOTH
* usart.c
*
* Created : 15-08-2020 07:24:45 PM
* Author  : Arnab Kumar Das
* Website : www.ArnabKumarDas.com
*/

// Unedited constants from Arnab Kumar Das (-Group 7) 
#ifndef F_CPU
#define F_CPU 16000000UL // Defining the CPU Frequency
#endif

#include <avr/io.h>      // Contains all the I/O Register Macros
#include <util/delay.h>  // Generates a Blocking Delay

//#define USART_BAUDRATE 9600 // Desired Baud Rate
#define USART_BAUDRATE 9600 // Desired Baud Rate
#define BAUD_PRESCALER (((F_CPU / (USART_BAUDRATE * 16UL))) - 1)

#define ASYNCHRONOUS (0<<UMSEL00) // USART Mode Selection

#define DISABLED    (0<<UPM00)
#define EVEN_PARITY (2<<UPM00)
#define ODD_PARITY  (3<<UPM00)
#define PARITY_MODE  DISABLED // USART Parity Bit Selection

#define ONE_BIT (0<<USBS0)
#define TWO_BIT (1<<USBS0)
#define STOP_BIT ONE_BIT      // USART Stop Bit Selection

#define FIVE_BIT  (0<<UCSZ00)
#define SIX_BIT   (1<<UCSZ00)
#define SEVEN_BIT (2<<UCSZ00)
#define EIGHT_BIT (3<<UCSZ00)
#define DATA_BIT   EIGHT_BIT  // USART Data Bit Selection


/*
 * PINS 
 * 
 * 
 */
#define input0 A5
#define input1 A4


/*
    Initializing Functions 

    USART - for bluetooth 
    ADC - for analog measurements
*/
// Partially edited intitialization function from Arnab Kumar Das (-Group 7) 
void USART_Init()
{
  // Set Baud Rate
  UBRR0H = BAUD_PRESCALER >> 8;
  UBRR0L = BAUD_PRESCALER;
  
  // Set Frame Format
  UCSR0C = ASYNCHRONOUS | DISABLED | STOP_BIT | DATA_BIT;
  
  // Enable Receiver and Transmitter
  UCSR0B = (1<<RXEN0) | (1<<TXEN0);
}

// Hereon is original code  (-Group 7) 
void ADC_Init()
{
  ADMUX = 0x00;
  //ADMUX |= (1 << ADLAR); // Left Justify 

  ADCSRA = 0x00;
  ADCSRA |= (1 << ADEN); // ADC Enable 
  ADCSRA |= (1 << ADPS0) | (1 << ADPS1) | (1 << ADPS2); // ADC prescalers (max value) 
  //ADCSRA |= (1 << ADIE); // interrupter 

  // https://ww1.microchip.com/downloads/en/DeviceDoc/Atmel-7810-Automotive-Microcontrollers-ATmega328P_Datasheet.pdf page 220 
  DIDR0 = (1 << ADC5D); // Digital Input Disable Register 
}


/*
    Helper Functions 

    transmit - sent one char across bluetooth USART
    readADC - measure analog voltage (given binary for which analog input pin (select[3...0])) 
*/

void transmit(uint8_t DataByte)
{
  while (( UCSR0A & (1<<UDRE0)) == 0) {}; // Do nothing until UDR is ready
  UDR0 = DataByte;
}


int readADC(int s3, int s2, int s1, int s0) {
  // Clear
  ADMUX &= (0 << MUX3);
  ADMUX &= (0 << MUX2);
  ADMUX &= (0 << MUX1);
  ADMUX &= (0 << MUX0);
  // Set
  ADMUX |= (s3 << MUX3);
  ADMUX |= (s2 << MUX2);
  ADMUX |= (s1 << MUX1);
  ADMUX |= (s0 << MUX0);

  // Begin measurement by setting ADSC bit 
  // Hardware automatically resets ADSC
  ADCSRA |= (1 << ADSC);
  bool waiting;
  do 
  {
    waiting = (ADCSRA & (1 << ADSC));
  } while(waiting);

  // MEASURE TWICE to allow ADC value to catch up
  ADCSRA |= (1 << ADSC);
  do 
  {
    waiting = (ADCSRA & (1 << ADSC));
  } while(waiting);

  // Read output 
  uint8_t lower = ADCL;
  uint8_t upper = ADCH; 
  uint16_t value = upper<<8 | lower<<0;
  return value;
}


/*



      Main Function 



*/
//unsigned char message[] = "Hello from arduino\n";
String message = "Hello from arduino\n";
unsigned int i;
unsigned int histLength = 5; //Incease for more value smoothing, decrease for more responsiveness
unsigned int memIndex = 0;
int main()
{
  //Initialize Measurement Storage variables
  unsigned int V[3];                    // running sum
  unsigned int V_avg[3];                // average of values 
  unsigned int V_Hist[3][histLength];   // most recently measured values 
  unsigned int A[3];                    // Angle which will be returned 

  // Initialize to zero 
  for(int m = 0; m < 3; m++){
    for(int n = 0; n < histLength; n++){
        V_Hist[m][n] = 0;
      }
      V[m] = 0;
  }

  // Initialize hardware 
  USART_Init();
  ADC_Init(); 
  
  while (1)
  {
    
    // Remove old value
    for(int m = 0; m < 3; m++){
        V[m] -= V_Hist[m][memIndex];
      }

      
      
    // Get new analog measurements 
    V_Hist[0][memIndex] = readADC(0,0,0,0); //between
    V_Hist[1][memIndex] = readADC(0,0,0,1); //joint
    V_Hist[2][memIndex] = readADC(0,0,1,0); //knuckle

    // Replace value in memory with new reading
    for(int m = 0; m < 3; m++){
        V[m] += V_Hist[m][memIndex];
        V_avg[m] = V[m] / histLength;
      }


    

    // Convert averages to angles 
    A[2] = 133.04 - .31*V_avg[2];  //knuckle model
    A[1] = 125.29 - .1883*V_avg[1]; //joint model

    if(A[2] < 0 or A[2] > 60000){
        A[2] = 0;
    }else if(A[2] > 99){
        A[2] = 99;
    }
    if(A[1] < 0 or A[1] > 60000){
        A[1] = 0;
    }else if(A[1] > 99){
        A[1] = 99;
    }

//    if(A[2] < 10 and A[1] < 10){
//      A[0] = -36.792 + .1265*V_avg[0]; //between model
//      }else{
//        A[0] = 15;
//      }
    A[0] = -36.792 + .1265*V_avg[0]; //between model
    
    if(A[0] < 0 or A[0] > 60000){
        A[0] = 0;
    }else if(A[0] > 99){
        A[0] = 99;
      }
    

    

    
    
    message = (String(A[0]) + "." + String(A[1]) + "." + String(A[2]) + "\n\0"); 

    // Send Message
    i = 0;
    while(message[i] != '\0') {
      transmit(message[i++]);
    }

    memIndex++;
    if(memIndex == histLength){
      memIndex = 0;
    }
  }
    return 0;
  }
  
