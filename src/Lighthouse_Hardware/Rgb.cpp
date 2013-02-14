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
	Serial.print(" ON ");
	if(_isOn){
		Serial.println("1");
	}else{
		Serial.println("0");
	}

	//Color
	Serial.print(_name);
	Serial.print(" RED ");
	Serial.println(_brightnessRed);
	
	Serial.print(_name);
	Serial.print(" GREEN ");
	Serial.println(_brightnessGreen);
	
	Serial.print(_name);
	Serial.print(" BLUE ");
	Serial.println(_brightnessBlue);

	
}

void Rgb::recieveCommand(String name, String prop, String value){
	if(name==_name){
		if( prop == "ON"){
			_isOn = value == "1";
		}else if(prop == "RED"){
			_brightnessRed = UTILS.stringToInt(value);

		}else if(prop == "GREEN"){
			_brightnessGreen = UTILS.stringToInt(value);

		}else if(prop == "BLUE"){
			_brightnessBlue = UTILS.stringToInt(value);
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

