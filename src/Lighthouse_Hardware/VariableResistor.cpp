// 
// 
// 

#include "VariableResistor.h"


void VariableResistor::init(System &system, int pin, int stutterFix, int low, int high, String name)
{
	_system = system;
	_pin = pin;
	_stutterFix = stutterFix;
	_name = name;
	_low = low;
	_high = high;
	sendStatus();
	
}

void VariableResistor::sendStatus()
{
	Serial.print(_name);
	Serial.print(" VALUE ");
	Serial.println(map(_value, _low, _high, 0, 255));
	/*
	Serial.print(_name);
	Serial.print(" MIN ");
	Serial.println(_low);
	
	Serial.print(_name);
	Serial.print(" MAX ");
	Serial.println(_high);*/
	
}

void VariableResistor::loop(){


	int val = analogRead(_pin);
	if(val != _value){
		
		if(val > _high)
			_high = val;

		if(val < _low)
			_low = val;

		if(abs( val - _value) > _stutterFix)
		{
			_value = val;
			sendStatus();
		}
	}

}

void VariableResistor::recieveCommand(String name, String prop, String value){
	if(name == _name){
		sendStatus();
	}
}

VariableResistor VARIABLERESISTOR;

