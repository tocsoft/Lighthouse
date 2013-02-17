// 
// 
// 

#include "VariableResistor.h"


void VariableResistor::init(System &system, int pin, int stutterFix, int low, int high, uint8_t name)
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

		_system.sendPacket(_name, 0x11, map(_value, _low, _high, 0, 255));

}

void VariableResistor::loop(){


	int val = analogRead(_pin);
	if(val != _value){
		
		

		if(abs( val - _value) > _stutterFix)
		{
			_value = val;
			
			sendStatus();
		}
		
	}

}


void VariableResistor::recieveCommand(uint8_t name, uint8_t prop, uint8_t value){
	if(name == _name){
		sendStatus();
	}
}


VariableResistor VARIABLERESISTOR;

