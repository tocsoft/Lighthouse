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
	String _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pin, String name);
	void recieveCommand(String name, String prop, String value);
};

extern Led LED;

#endif

