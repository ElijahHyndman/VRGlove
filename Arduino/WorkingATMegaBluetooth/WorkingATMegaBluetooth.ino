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

#ifndef F_CPU
#define F_CPU 16000000UL // Defining the CPU Frequency
#endif

#include <avr/io.h>      // Contains all the I/O Register Macros
#include <util/delay.h>  // Generates a Blocking Delay

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

  // Begin measurement, will automatically reset by hardware 
  ADCSRA |= (1 << ADSC);

  bool waiting;
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


//unsigned char message[] = "Hello from arduino\n";
String message = "Hello from arduino\n";
int i;


int main()
{
  int A0, A1, A2;
  
  USART_Init();
  ADC_Init(); 
  
  while (1)
  {
      // Get Message
    A0 = readADC(0,0,0,0);
    A1 = readADC(0,0,0,1);
    A2 = readADC(0,0,1,0);
        
    message = (String(A2) + "." + String(A1) + "." + String(A0) + "\n\0"); 

    // Send Message
    i = 0;
    while(message[i] != '\0') {
      transmit(message[i++]);
    }
    _delay_ms(100);
  }
  return 0;
}
