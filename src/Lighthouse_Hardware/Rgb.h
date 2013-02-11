// Rgb.h
#include "System.h"

#ifndef _RGB_h
#define _RGB_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Rgb
{
 private:
	int _pinRed;
	int _pinGreen;
	int _pinBlue;
	int _brightnessRed;
	int _brightnessGreen;
	int _brightnessBlue;
	bool _isOn;
	String _name;
System _system;
 public:
	void loop();
	void sendStatus();
	void init(System &system, int pinRed, int pinGreen, int pinBlue, String name);
	void recieveCommand(String name, String prop, String value);
};

extern Rgb RGB;

#endif

