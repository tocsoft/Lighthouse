// 
// 
// 

#include "Led.h"

void Led::init(System &system, int pin, uint8_t name)
{
	_system = system;
	_pin = pin;
	_name = name;
	_isOn = false;
	
	_flashingSpeed = 100;
	_isFlashing = false;


	pinMode(_pin, OUTPUT);
}



void Led::sendStatus()
{
	if(_isOn){
		_system.sendPacket(_name, 0x01, 0x01);
	}else{
		_system.sendPacket(_name, 0x01, 0x00);
	}
}

void Led::recieveCommand(uint8_t name, uint8_t prop, byte value){
	if(name==_name){
		if(prop == 0x01){
			_isOn = (value == 0x01);
		}
		sendStatus();
	}
}

void Led::loop(){
		
		if(_isOn){
		

				digitalWrite(_pin, HIGH);
			}else{
				

				 digitalWrite(_pin, LOW);
			}
}



Led LED;

