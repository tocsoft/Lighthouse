

Blockly.CSharp = Blockly.Generator.get('CSharp');

Blockly.CSharp.button_isOn = function () {

    var code = 'device.' + this.getTitleValue('DEVICE') + '.IsOn';
    return [code, Blockly.CSharp.ORDER_NONE];
};

Blockly.CSharp.led_turnOn = function () {
    return 'device.' + this.getTitleValue('DEVICE') + '.IsOn = ' + this.getTitleValue('STATE') + ';\n';
};

Blockly.CSharp.variableinput_value = function () {
    var code = 'device.' + this.getTitleValue('DEVICE') + '.Value';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};


Blockly.CSharp.rgb_setcolour = function () {
    var code = 'device.' + this.getTitleValue('DEVICE') + '.Color = ' + this.getValue('COLOR') + ';';
    return code;
};
