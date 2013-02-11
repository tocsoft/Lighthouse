// 
// 
// 

#include "Button.h"

void Button::init(System &system, int pin, String name)
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
	Serial.print(_name);
	Serial.print(" STATE ");
	if(!_isOn){
		Serial.println("ON");
	}else{
		Serial.println("OFF");
	}
}

void Button::loop(){
	if(millis() % 10 == 0){
		//make a check
		_isTest1 = _isTest2;
		_isTest2 = digitalRead(_pin);
		if(_isTest1 == _isTest2){
		bool newState = _isTest1;
			if(_isOn != newState){
				_isOn = newState;
				sendStatus();
			}
		}
		
	}
}

void Button::recieveCommand(String name, String prop, String value){
	if(name == _name){
		sendStatus();
	}
}

Button BUTTON;

