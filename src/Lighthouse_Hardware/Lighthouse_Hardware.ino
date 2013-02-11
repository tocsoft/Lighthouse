
#include "Led.h"
#include "Utils.h"
#include "Rgb.h"
#include "Button.h"
#include "System.h"


System system;
void setup(){
	system.init( "9e0694dd-30c0-42c9-8278-e61a9d769202");
}
void loop(){
	system.loop();
}

//#include "Button.h"
//#include "System.h"
//#include "Host.h"
//
//
//#include <Servo.h> 
// 
// // create servo object to control a servo 
//                // a maximum of eight servo objects can be created 
//int pos = 0;    // variable to store the servo position 
// 
//const int redPin = 3;
//const int greenPin = 5;
//const int bluePin = 6;
//const String uid =
// 
//void setup() 
//{ 
////	pinMode(13, OUTPUT);
// //  // attaches the servo on pin 9 to the servo object 
//
//  Serial.begin(9600);   
//  servoSetup();
//  rgbSetup();
//  buttonSetup();
//} 
// 
// 
//void SendId(){
//	Serial.print("SYSTEM ID ");
//	Serial.println(uid);
//}
//
//
//void loop() 
//{ 
//	  
//
//	if (Serial.available() > 0) {
//                // read the incoming byte:
//		String cmd = Serial.readString();	
//
//		int firstSpace = cmd.indexOf(' ', 0);
//		if(firstSpace == -1)
//			firstSpace = cmd.length();
//
//		String part = cmd.substring(0, firstSpace);
//		cmd = cmd.substring(firstSpace+1, cmd.length());
//		part.toUpperCase();
//		
//
//		if(part == "ID")
//			SendId();
//
//		if(part == "RGB")
//			rgbCmd(cmd);
//		
//		
//		if(part == "SERVO")
//			servoCmd(cmd);
//		
//		StatusUpdate();
//	}
//	
//	rgbLoop();
//	servoLoop();
//	buttonLoop();
//}
//
//void StatusUpdate(){
//	SendId();
//	SendButtonState();
//	SendRgbColor();
//	SendRgbMode();
//	SendServoAngle();
//	SendServoMaxAngle();
//	SendServoMinAngle();
//	SendServoMode();
//	SendServoSpeed();
//			
//	Serial.println("SYSTEM STATUS COMPLETE");
//}
//
//
//const int buttonPin =2; 
//bool isButtonOn = true;
//
//void buttonSetup(){
//  pinMode(buttonPin, INPUT);
//}
//
//void buttonLoop(){
//	
//	bool state = (getButtonState(buttonPin) == HIGH);
//
//	if(isButtonOn != state){
//		isButtonOn = state;
//		StatusUpdate();
//	}
//}
//
//void SendButtonState()
//{
//	if(getButtonState(buttonPin)== HIGH){
//		Serial.println("BUTTON STATE ON");
//	}else{
//		Serial.println("BUTTON STATE OFF");
//	}
//}
//
//
//void buttonCmd(String cmd){
//	//ignored as buttons are get only
//	/*bool isSet = cmd.startsWith("SET");
//	
//	String subcmd = cmd.substring(4, cmd.length());
//
//	if(subcmd.startsWith("STATE")){
//		SendButtonState();
//	}*/
//}
//
//int getButtonState(int pin){
//	int read1 = digitalRead(pin);
//	delay(10);
//	int read2 = digitalRead(pin);
//
//	while(read1 != read2){
//		read1 = read2;
//		delay(10);
//		read2 = digitalRead(pin);
//	}
//	return read1;
//}
//
//Servo myservo; 
//void servoSetup(){
//	myservo.attach(9); 
//}
//
//int setAngle = 0;
//int minAngle = 0;
//int maxAngle = 170;
//int speedModifier = 10;
//bool isSweeping = false;
//int deviceMax = 175;
//int oldAngle = -1;
//int sweepOffset = 0;
//void servoLoop(){
//
//	if(isSweeping){
//		int range = maxAngle - minAngle;
//		setAngle = range - ((millis() / speedModifier) % (range * 2));
//		if(setAngle < 0)
//			setAngle *=  -1;
//		setAngle += minAngle;
//		
//	}
//	if(setAngle > deviceMax)
//		setAngle = deviceMax;
//	if(oldAngle != setAngle){
//		myservo.write(setAngle);
//		oldAngle = setAngle;
//	}
//	
//}
//
//void servoCmd(String cmd){
//
//	//actions =
//	//sweep min max
//	//set ang
//	bool isSet = cmd.startsWith("SET");
//	
//		String subcmd = cmd.substring(4, cmd.length());
//	if(isSet){
//		if(subcmd.startsWith("MIN")){
//			String minAngString = subcmd.substring(4, subcmd.length());
//			minAngle = stringToInt(	minAngString);
//			if(minAngle > deviceMax)
//				minAngle = deviceMax;
//		}else 
//		if(subcmd.startsWith("MAX")){
//			String maxAngString = subcmd.substring(4, subcmd.length());
//			maxAngle = stringToInt(	maxAngString);
//			if(maxAngle > deviceMax)
//				maxAngle = deviceMax;
//		}else 
//		if(subcmd.startsWith("SPEED")){
//			String maxAngString = subcmd.substring(6, subcmd.length());
//			speedModifier = stringToInt(	maxAngString);
//		} else
//		if(subcmd.startsWith("ANGLE")){
//			String maxAngString = subcmd.substring(6, subcmd.length());
//			
//			setAngle = stringToInt(	maxAngString);
//			if(setAngle > deviceMax)
//				setAngle = deviceMax;
//			
//		} else
//		if(subcmd.startsWith("MODE")){
//			String mode = subcmd.substring(5, subcmd.length());
//			mode.toUpperCase();
//			if(mode == "SWEEP"){
//				sweepOffset = setAngle;
//				isSweeping = true;
//			}
//			else if(mode == "STATIC"){
//				isSweeping = false;
//			}
//
//		}
//	}
///*
//	if(subcmd.startsWith("MIN")){
//			SendServoMinAngle();
//		}else 
//		if(subcmd.startsWith("MAX")){
//			SendServoMaxAngle();
//		}else 
//		if(subcmd.startsWith("SPEED")){
//			SendServoSpeed();
//		} else
//		if(subcmd.startsWith("ANGLE")){
//			SendServoAngle();
//		} else
//		if(subcmd.startsWith("MODE")){
//			SendServoMode();
//		}*/
//}
//void SendServoMinAngle(){
//	Serial.print("SERVO MIN ");
//	Serial.println(minAngle);
//}
//void SendServoMaxAngle(){
//	Serial.print("SERVO MAX ");
//	Serial.println(maxAngle);
//}
//void SendServoSpeed(){
//	Serial.print("SERVO SPEED ");
//	Serial.println(speedModifier);
//}
//void SendServoAngle(){
//	Serial.print("SERVO ANGLE ");
//	Serial.println(setAngle);
//}
//void SendServoMode(){
//	Serial.print("SERVO MODE ");
//
//	if(isSweeping)
//		Serial.println("SWEEP");
//	else
//		Serial.println("STATIC");
//}
//
//byte currentColorValueRed = 255;
//byte currentColorValueGreen = 255;
//byte currentColorValueBlue = 255;
//
//bool isLEDOn = false;
//
//void rgbSetup(){
//  pinMode(redPin, OUTPUT);
//  pinMode(greenPin, OUTPUT);
//  pinMode(bluePin, OUTPUT);
//}
//
//void rgbLoop(){
//	if(isLEDOn){
//		  analogWrite(redPin, currentColorValueRed);
//		  analogWrite(bluePin, currentColorValueBlue);
//		  analogWrite(greenPin, currentColorValueGreen);
//	}else{
//		  analogWrite(redPin, 0);
//		  analogWrite(bluePin, 0);
//		  analogWrite(greenPin, 0);
//	}
//}
//
//void SendRgbColor(){
//	Serial.print("RGB COLOR ");
//	Serial.print(currentColorValueRed);
//	Serial.print(" ");
//					
//	Serial.print(currentColorValueGreen);
//	Serial.print(" ");
//					
//	Serial.print(currentColorValueBlue);
//
//	Serial.println();
//}
//void SendRgbMode(){
//	Serial.print("RGB MODE ");
//	if(isLEDOn)
//		Serial.println("ON");
//	else
//		Serial.println("OFF");
//}
//
//void rgbCmd(String cmd){
//
//	bool doSet = cmd.startsWith("SET");
//	String subcmd = cmd.substring(4, cmd.length());
//
//	if(doSet)
//	{
//		if(subcmd.startsWith("COLOR"))
//		{
//			String color = subcmd.substring(6, subcmd.length());
//			
//			int nxtSpace = color.indexOf(" ", 0);
//			String colorInt = color.substring(0, nxtSpace);
//			
//			color = color.substring(nxtSpace+1, color.length());
//			currentColorValueRed = stringToInt(colorInt);
//			
//					
//			nxtSpace = color.indexOf(" ", 0);
//			colorInt = color.substring(0, nxtSpace);
//			color = color.substring(nxtSpace+1, color.length());
//			currentColorValueGreen = stringToInt(colorInt);
//		
//			colorInt = color;
//			color = "";
//			currentColorValueBlue = stringToInt(colorInt);
//			
//
//		}else if(subcmd.startsWith("MODE"))
//		{
//			String mode = subcmd.substring(5, subcmd.length());
//			mode.trim();
//			mode.toUpperCase();
//			isLEDOn = (mode == "ON");
//
//		}
//	} 
//	/*
//
//		
//	if(subcmd.startsWith("COLOR"))
//	{
//		SendRgbColor();
//				
//	}else if(subcmd.startsWith("MODE"))
//	{
//		SendRgbMode();
//	}
//	*/
//}
//
//static uint8_t
//nibbleFromChar(char c)
//{
//	if(c >= '0' && c <= '9') return c - '0';
//	if(c >= 'a' && c <= 'f') return c - 'a' + 10;
//	if(c >= 'A' && c <= 'F') return c - 'A' + 10;
//	return 255;
//}
//
////this is a string in HEX format
//byte stringToByte(String hexstr)
//{
//	return (nibbleFromChar(hexstr[0]) * 16) + nibbleFromChar(hexstr[1]);
//}
//
//
////this is a string in HEX format
//int stringToInt(String decstring)
//{
//	int val = 0;
//	int len = decstring.length();
//	int multi = 1;
//
//
//	for(int i = 0; i<len; i++){
//		if(i > 0){
//			multi *= 10;
//		}
//		
//		val += (decstring[(len - i) -1] - '0') * multi;
//		
//	}
//	return val;
//}
//
//
//
//
