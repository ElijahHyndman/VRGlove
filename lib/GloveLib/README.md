Requirements

Glove Object (GO)
- A GO exists independent of whether connection to the glove hardware is established
- A GO shall support multiple physical layouts of sensors on hand 

Connection
- shall connect through Bluetooth or through USB-Link
- shall transition bluetooth->USB automatically when USB wire connected
- Instantiated GO shall sit idle (default state) while listening for connection
- disconnecting (lost power, bluetooth->USB transition) returns glove to idle

Boards
- shall interact with Arduino Nano (breadboard) and ATMega chip (PCB)

Application
- launch-able GUI for representing glove
- shall provide raw character data for debugging
- shall provide individual sensor values (numbers)
- shall provide individual sensor value representation (graphical representation)
