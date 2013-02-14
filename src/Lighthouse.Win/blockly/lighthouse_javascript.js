

Blockly.JavaScript = Blockly.Generator.get('JavaScript');

Blockly.JavaScript.button_isOn = function () {

    var code = 'device.' + this.getTitleValue('DEVICE') + '.IsOn';
    return [code, Blockly.JavaScript.ORDER_NONE];
};

Blockly.JavaScript.led_turnOn = function () {
    return 'device.' + this.getTitleValue('DEVICE') + '.IsOn = ' + this.getTitleValue('STATE') + ';\n';
};

Blockly.JavaScript.variableinput_value = function () {
    var code = 'device.' + this.getTitleValue('DEVICE') + '.Value';
    return [code, Blockly.JavaScript.ORDER_FUNCTION_CALL];
};



Blockly.JavaScript.system_sleep = function () {
    //function actualy takes in milliseconds;
    var millis = parseInt(this.getTitleValue('SECS')) * 1000;

    return 'system.Sleep(' + millis + ');'
};

//Blockly.JavaScript.rgb_setcolour = function () {
//    var code = 'device.' + this.getTitleValue('DEVICE') + '.Color = ' + this.getValue('COLOR') + ';';
//    return code;
//};
