// 
// 
// 

#include "Rgb.h"
#include "Utils.h"

void Rgb::init(System &system, int pinRed, int pinGreen, int pinBlue, String name)
{
	_system = system;
	_pinRed = pinRed;
	_pinGreen = pinGreen;
	_pinBlue = pinBlue;
	_name = name;
	_isOn = false;
	
	_brightnessRed = 255;
	_brightnessGreen = 255;
	_brightnessBlue = 255;

	pinMode(_pinRed, OUTPUT);
	pinMode(_pinGreen, OUTPUT);
	pinMode(_pinBlue, OUTPUT);
}



void Rgb::sendStatus()
{
	//state
	Serial.print(_name);
	Serial.print(" STATE ");
	if(_isOn){
		Serial.println("ON");
	}else{
		Serial.println("OFF");
	}

	//Color
	Serial.print(_name);
	Serial.print(" COLOR ");
	
	Serial.print(_brightnessRed);
	Serial.print(" ");
	Serial.print(_brightnessGreen);
	Serial.print(" ");
	Serial.print(_brightnessBlue);

	Serial.println();
	
}

void Rgb::recieveCommand(String name, String prop, String value){
	if(name==_name){
		if( prop == "STATE"){
		_isOn = value == "ON";
		}else if(prop == "COLOR"){
			int firstSpace = value.indexOf(' ', 0);
			if(firstSpace == -1)
				firstSpace = value.length();
		
			String red = value.substring(0, firstSpace);
			String leftovervalue = value.substring(firstSpace+1, value.length());

			 firstSpace = leftovervalue.indexOf(' ', 0);
			if(firstSpace == -1)
				firstSpace = leftovervalue.length();
		
			String green = leftovervalue.substring(0, firstSpace);
			String blue = leftovervalue.substring(firstSpace+1, leftovervalue.length());
		
			_brightnessRed = UTILS.stringToInt(red);
			_brightnessGreen = UTILS.stringToInt(green);
			_brightnessBlue = UTILS.stringToInt(blue);

		}
		sendStatus();
	}
}

void Rgb::loop(){
	if(_isOn){
		 analogWrite(_pinRed, _brightnessRed);
		 analogWrite(_pinGreen, _brightnessGreen);
		 analogWrite(_pinBlue, _brightnessBlue);
	}else{
		 analogWrite(_pinRed, 0);
		 analogWrite(_pinGreen, 0);
		 analogWrite(_pinBlue, 0);
	}
}

Rgb RGB;

