// VariableResistor.h
#include "System.h"

#ifndef _VARIABLERESISTOR_h
#define _VARIABLERESISTOR_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class VariableResistor
{
 private:
	int _pin;
	int _value;
	int _stutterFix;
	
	int _low;
	int _high;
	uint8_t _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pin, int stutterFix, int low, int high, uint8_t name);
	void recieveCommand(uint8_t name, uint8_t prop, byte value);
};

extern VariableResistor VARIABLERESISTOR;

#endif

