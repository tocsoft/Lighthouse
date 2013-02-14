// 
// 
// 

#include "Led.h"
#include "Utils.h"

void Led::init(System &system, int pin, String name)
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
	//state
	Serial.print(_name);
	Serial.print(" ON ");
	if(_isOn){
		Serial.println("1");
	}else{
		Serial.println("0");
	}

	Serial.print(_name);
	Serial.print(" FLASH ");
	if(_isFlashing){
		Serial.println("1");
	}else{
		Serial.println("0");
	}
	Serial.print(_name);
	Serial.print(" FLASH_SPEED ");
	Serial.println(_flashingSpeed);
}

void Led::recieveCommand(String name, String prop, String value){
	if(name==_name){
		if( prop == "ON"){
		_isOn = value == "1";
		}else if( prop == "FLASH"){
		_isFlashing = value == "1";
		if(_isFlashing)
			_isOn = true;
		}else if( prop == "FLASH_SPEED"){
			_flashingSpeed = UTILS.stringToInt(value);
		}

		sendStatus();
	}
}

void Led::loop(){
		if(_isFlashing){
			int n =  millis();
			if((n - _lastFlash) > (_flashingSpeed * 50))
			{
				_lastFlash = n;
				_isOn = !_isOn;
				sendStatus();
			}
		}

		if(_isOn){
				digitalWrite(_pin, HIGH);
			}else{
				 digitalWrite(_pin, LOW);
			}
}



Led LED;

