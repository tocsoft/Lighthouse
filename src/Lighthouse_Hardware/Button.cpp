// 
// 
// 

#include "Button.h"

void Button::init(System &system, int pin, uint8_t name)
{
	_system = system;
	_pin = pin;
	pinMode(_pin, INPUT);

	_isOn = (_isTest1 = _isTest2 = digitalRead(_pin));
	_name = name;
	
	sendStatus();
	
}

void Button::sendStatus()
{
	if(_isOn){
		_system.sendPacket(_name, 0x01, 0x01);
	}else{
		_system.sendPacket(_name, 0x01, 0x00);
	}
}

void Button::loop(){
	
	

	if(millis() % 10 == 0){
		//make a check
		_isTest1 = _isTest2;
		_isTest2 = digitalRead(_pin);
		if(_isTest1 == _isTest2){
		bool newState = _isTest1;
			if(_isOn == newState){
				_isOn = !newState;
				sendStatus();
			}
		}
		
	}
}

void Button::recieveCommand(uint8_t name, uint8_t prop, uint8_t value){
	if(name == _name){
		sendStatus();
	}
}

Button BUTTON;

