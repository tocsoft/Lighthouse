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
	 
 public:

	void init(String id);
	void loop();
	//marks the the system requires that is needs to send a status update
	void sendStatus();
};

extern System SYSTEM;

#endif

