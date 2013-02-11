// Utils.h

#ifndef _UTILS_h
#define _UTILS_h

#if defined(ARDUINO) && ARDUINO >= 100
	#include "Arduino.h"
#else
	#include "WProgram.h"
#endif

class Utils
{
 private:


 public:
	void init();
	int stringToInt(String decstring);
};

extern Utils UTILS;

#endif

