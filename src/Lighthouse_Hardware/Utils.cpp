// 
// 
// 

#include "Utils.h"

void Utils::init()
{


}

int Utils::stringToInt(String decstring)
{
	int val = 0;
	int len = decstring.length();
	int multi = 1;


	for(int i = 0; i<len; i++){
		if(i > 0){
			multi *= 10;
		}
		
		val += (decstring[(len - i) -1] - '0') * multi;
		
	}
	return val;
}


Utils UTILS;

