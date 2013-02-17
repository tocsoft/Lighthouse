// 
// 
// 

#include "System.h"
#include "Button.h"
#include "Rgb.h"
#include "Led.h"
#include "VariableResistor.h"
#include "Buzzer.h"

Button switch1;
Button button1;
Led led1;
Led led2;
Rgb rgb;
VariableResistor pot;

Buzzer buzzer;

VariableResistor ldr;

void System::init(String id)
{
	Serial.begin(57600);   

	//setup button on pin2
	switch1.init(*this, 2, 0x11);
	button1.init(*this, 7, 0x12);

	led1.init(*this, 4, 0x21);
	led2.init(*this, 1, 0x22);

	//rgb.init(*this,3,5,6, 0x15);
	pot.init(*this, 0, 5, 0, 1023, 0x31);
	ldr.init(*this, 1, 10, 135, 955, 0x32);


	buzzer.init(*this, 9, 0x41);

	_id = id;
}
bool _requiresSendStatus = true;;


void System::loop()
{
	readCmd();

	switch1.loop();
	button1.loop();
	led1.loop();
	led2.loop();
	pot.loop();
	ldr.loop();
	buzzer.loop();
}

void System::sendStatus()
{
		switch1.sendStatus();
		button1.sendStatus();
		//rgb.sendStatus();
		led1.sendStatus();
		led2.sendStatus();
		pot.sendStatus();
		ldr.sendStatus();
		buzzer.sendStatus();

		sendPacket(0x01, 0x01, 0x01);
}


uint8_t rx_len;		//RX packet length according to the packet

// example packet 
// [0x06] start bit
// [0x10 - 0x1F] address of component
// [0x20 - 0x2F] property address 
// [0x00 - 0xFF] value
// [0x00 - 0xFF] checksum
bool System::readCmd()
{
	if(Serial.available() < 5){
		return false;
		digitalWrite(1, HIGH);
		digitalWrite(4, LOW);
	}else{
	int count = 0;
		while(Serial.read() != 0x06) 
		{
			if(count++ > 100){
				
				return false;
			}
			//This will trash any preamble junk in the serial buffer
			//but we need to make sure there is enough in the buffer to process while we trash the rest
			//if the buffer becomes too empty, we will escape and try again on the next call
			if(Serial.available() < 4){
				return false;
			}
		}
		
		if(Serial.available() >= 4){
			byte componentAddress = Serial.read();
			byte propertyAddress = Serial.read();
			byte value = Serial.read();
			byte cs = Serial.read();

			byte calc_CS = 0;
			calc_CS^=componentAddress;
			calc_CS^=propertyAddress;
			calc_CS^=value;
			if(calc_CS == cs){
				

				if(componentAddress == 0x01){
					sendStatus();
				}else{
					led1.recieveCommand(componentAddress, propertyAddress, value);
					led2.recieveCommand(componentAddress, propertyAddress, value);
					switch1.recieveCommand(componentAddress, propertyAddress, value);
					button1.recieveCommand(componentAddress, propertyAddress, value);
					pot.recieveCommand(componentAddress, propertyAddress, value);
					ldr.recieveCommand(componentAddress, propertyAddress, value);
					buzzer.recieveCommand(componentAddress, propertyAddress, value);
				}

				return true;
			}
		}
		
	}

	

  return false;
}

void System::sendPacket(uint8_t componentAddress, uint8_t propertyAddess, uint8_t value){
	Serial.write(0x06);
	Serial.write(componentAddress);
	Serial.write(propertyAddess);
	Serial.write(value);
	uint8_t calc_CS = 0;
    calc_CS^=componentAddress;
    calc_CS^=propertyAddess;
    calc_CS^=value;
	Serial.write(calc_CS);
}

System SYSTEM;

