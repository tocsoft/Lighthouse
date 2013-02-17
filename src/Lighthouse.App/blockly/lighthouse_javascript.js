

Blockly.JavaScript = Blockly.Generator.get('JavaScript');

Blockly.JavaScript.button_isOn = function () {

    var code = 'device.Input("' + this.getTitleValue('DEVICE') + '").IsOn';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript.led_turnOn = function () {
    return 'device.Led("' + this.getTitleValue('DEVICE') + '").IsOn = ' + this.getTitleValue('STATE') + ';\n';
};

Blockly.JavaScript.variableinput_value = function () {
    var code = 'device.Range("' + this.getTitleValue('DEVICE') + '").Value';
    return [code, Blockly.JavaScript.ORDER_FUNCTION_CALL];
};


Blockly.JavaScript.system_sleep = function () {
    //function actualy takes in milliseconds;
    var millis = parseInt(this.getTitleValue('SECS')) * 1000;

    return 'system.Sleep(' + millis + ');'
};

Blockly.JavaScript.repeat_forever = function()
{ 
    
    var branch = Blockly.JavaScript.statementToCode(this, 'DO');
    return "while(true){ "+ branch + " }";
}

Blockly.JavaScript.DEBUGGER_CODE_ENTER = "__debugger__.Enter(%ID%);\n"
Blockly.JavaScript.DEBUGGER_CODE_EXIT = "__debugger__.Exit(%ID%);\n"

//Blockly.JavaScript.rgb_setcolour = function () {
//    var code = 'device.' + this.getTitleValue('DEVICE') + '.Color = ' + this.getValue('COLOR') + ';';
//    return code;
//};


Blockly.JavaScript.buzzer_turnOn = function(){

    return 'device.Buzzer("' + this.getTitleValue('DEVICE') + '").IsOn = ' + this.getTitleValue('STATE') + ';\n';

};



Blockly.JavaScript.buzzer_turnOn = function(){

    return 'device.Buzzer("' + this.getTitleValue('DEVICE') + '").IsOn = ' + this.getTitleValue('STATE') + ';\n';

};
Blockly.JavaScript.buzzer_setTone = function () {


    var tone = Blockly.JavaScript.valueToCode(this, 'TONE',
         Blockly.JavaScript.ORDER_NONE) || '0';

    return 'device.Buzzer("' + this.getTitleValue('DEVICE') + '").Tone = ' + tone + ';\n';

}