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
	
	_brightness = 255;

	pinMode(_pin, OUTPUT);
}



void Led::sendStatus()
{
	//state
	Serial.print(_name);
	Serial.print(" STATE ");
	if(_isOn){
		Serial.println("ON");
	}else{
		Serial.println("OFF");
	}

	//Color
	Serial.print(_name);
	Serial.print(" BRIGHTNESS ");
	
	Serial.print(_brightness);

	Serial.println();
	
}

void Led::recieveCommand(String name, String prop, String value){
	if(name==_name){
		if( prop == "STATE"){
		_isOn = value == "ON";
		}else if(prop == "BRIGHTNESS"){
			
			_brightness = UTILS.stringToInt(value);

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

