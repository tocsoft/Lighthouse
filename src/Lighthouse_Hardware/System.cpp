// 
// 
// 

#include "System.h"
#include "Button.h"
#include "Rgb.h"
#include "Led.h"
#include "VariableResistor.h"

Button switch1;
Button button1;
Led led1;
Led led2;
Rgb rgb;
VariableResistor pot;
VariableResistor ldr;

void System::init(String id)
{
	Serial.begin(57600);   
	//setup button on pin2
	switch1.init(*this, 2, "SWITCH1");
	button1.init(*this, 7, "BUTTON1");
	led1.init(*this, 4, "LED1");
	led2.init(*this, 1, "LED2");
	rgb.init(*this,3,5,6, "RGB");
	pot.init(*this, 0, 5, 0, 1023, "KNOB");
	ldr.init(*this, 1, 10, 135, 955, "LIGHT");
	_id = id;
}
bool _requiresSendStatus = true;;


void System::loop()
{
	if (Serial.available() > 0) {
		// read the incoming byte:
		String cmdBuffer = Serial.readString();

		int posOfNewline = cmdBuffer.indexOf(13, 0);
			if(posOfNewline == -1)
				posOfNewline = cmdBuffer.length();

		while(posOfNewline > 0){
			String cmd = cmdBuffer.substring(0, posOfNewline);
			
			cmdBuffer = cmdBuffer.substring(posOfNewline+1, cmdBuffer.length());
			posOfNewline = cmdBuffer.indexOf(13, 0);

			if(posOfNewline == -1)
				posOfNewline = cmdBuffer.length();
			cmd.trim();
			if(cmd.length() > 0){
				int firstSpace = cmd.indexOf(' ', 0);
				if(firstSpace == -1)
					firstSpace = cmd.length();
		
				String device = cmd.substring(0, firstSpace);
				cmd = cmd.substring(firstSpace+1, cmd.length());

				 firstSpace = cmd.indexOf(' ', 0);
				if(firstSpace == -1)
					firstSpace = cmd.length();
		
				String prop = cmd.substring(0, firstSpace);
				String value = cmd.substring(firstSpace+1, cmd.length());
				/*
				Serial.print(device);
				Serial.print(".");
				Serial.print(prop);
				Serial.print("(");
				Serial.print(value);
				Serial.println(")");*/
		
				switch1.recieveCommand(device, prop, value);
				button1.recieveCommand(device, prop, value);
				rgb.recieveCommand(device, prop, value);
				led1.recieveCommand(device, prop, value);
				led2.recieveCommand(device, prop, value);
				pot.recieveCommand(device, prop, value);
				ldr.recieveCommand(device, prop, value);
				if(device == "STATUS"){
					
					sendStatus();
				}
			}
		}
		
		//sendStatus();
	}
	
	switch1.loop();
	button1.loop();
	rgb.loop();
	led1.loop();
	led2.loop();
	pot.loop();
	ldr.loop();
	/*
	if(_requiresSendStatus){
		
		switch1.sendStatus();
		button1.sendStatus();
		rgb.sendStatus();
		led1.sendStatus();
		led2.sendStatus();


		Serial.println("SYSTEM STATUS COMPLETE");

		_requiresSendStatus = false;
	}*/
}

void System::sendStatus()
{
		switch1.sendStatus();
		button1.sendStatus();
		rgb.sendStatus();
		led1.sendStatus();
		led2.sendStatus();
		pot.sendStatus();
		ldr.sendStatus();

		Serial.print("SYSTEM ID ");
		Serial.println(_id);
}


System SYSTEM;

