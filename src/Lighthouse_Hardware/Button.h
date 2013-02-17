// Button.h
#include "System.h"

#ifndef _BUTTON_h
#define _BUTTON_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Button
{
 private:
	int _pin;
	bool _isTest1;
	bool _isTest2;
	bool _isOn;
	uint8_t _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pin, uint8_t name);
	void recieveCommand(uint8_t name, uint8_t prop, uint8_t value);
};

extern Button BUTTON;

#endif

