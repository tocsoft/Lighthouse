// 
// 
// 

#include "Buzzer.h"

void Buzzer::init(System &system, int pin, uint8_t name)
{
	_system = system;
	_pin = pin;
	_name = name;
	_isOn = false;
	
	_tone = 10;


	pinMode(_pin, OUTPUT);
}



void Buzzer::sendStatus()
{
	if(_isOn){
		_system.sendPacket(_name, 0x01, 0x01);
	}else{
		_system.sendPacket(_name, 0x01, 0x00);
	}
	
	_system.sendPacket(_name, 0x11, _tone);
}

void Buzzer::recieveCommand(uint8_t name, uint8_t prop, byte value){
	if(name==_name){
		if(prop == 0x01){
			_isOn = (value == 0x01);
		}else 
		if(prop == 0x11){
			_tone = value;
		}
		sendStatus();
	}
}

void Buzzer::loop(){
		
		if(_isOn){
			tone(_pin, map(_tone, 0, 255, 200, 4978/4));
		}else{
			noTone(_pin);
		}
}

Buzzer BUZZER;

