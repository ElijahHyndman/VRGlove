A Comm facilitates messages from Glove->desktop through USB and through Bluetooth serial communications.

- Comm object is independent of the platform it runs on (Mac, Windows)
  - Variations in mac and windows handled by (I)CommPort Object
- Comm object shall exist before and beyond physical connection of arduino device
- Comm object searches Com (windows) or /dev (mac) items for an arduino-hardware connection
