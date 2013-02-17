// Led.h
#include "System.h"

#ifndef _LED_h
#define _LED_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Led
{
 
 private:
	int _pin;
	int _brightness;
	bool _isOn;
	bool _isFlashing;
	int _flashingSpeed;
	int _lastFlash;
	uint8_t _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pin, uint8_t name);
	void recieveCommand(uint8_t name, uint8_t prop, byte value);
};

extern Led LED;

#endif

