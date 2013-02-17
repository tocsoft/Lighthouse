// System.h

#ifndef _SYSTEM_h
#define _SYSTEM_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class System
{
 private:
	 String _id;
	 bool readCmd();
 public:

	void init(String id);
	void loop();
	//marks the the system requires that is needs to send a status update
	void sendStatus();
	void sendPacket(uint8_t componentAddress, uint8_t propertyAddess, uint8_t value);
};

extern System SYSTEM;

#endif

