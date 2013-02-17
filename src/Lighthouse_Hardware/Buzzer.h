// Buzzer.h
#include "System.h"

#ifndef _BUZZER_h
#define _BUZZER_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Buzzer
{
 
 private:
	int _pin;
	byte _tone;
	bool _isOn;
	uint8_t _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pin, uint8_t name);
	void recieveCommand(uint8_t name, uint8_t prop, byte value);
};

extern Buzzer BUZZER;

#endif

