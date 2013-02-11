
'use strict';

// Extensions to Blockly's language and JavaScript generator.

Blockly.JavaScript = Blockly.Generator.get('JavaScript');


Blockly.Language.button_isOn = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {
        this.setColour(120);
        this.setOutput(true, Boolean);
        this.appendDummyInput()
            .appendTitle('Is')
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle('on')

        this.setTooltip('Returns true if the button is switched on');
    }
};

Blockly.Language.button_isOn.DEVICE = [['Switch 1', 'Switch1'], ['Button 1', 'Button1']];


Blockly.Language.led_turnOn = {
    // Block for moving forward or backwards.
    helpUrl: '',
    init: function () {
        this.setColour(290);

        this.appendDummyInput()
            .appendTitle("Turn ")
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle(" ")
            .appendTitle(new Blockly.FieldDropdown(this.STATE), 'STATE');
        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setTooltip('Moves Pegman forward or backward one space.');
    }
};


Blockly.Language.led_turnOn.STATE = [['on', 'true'], ['off', 'false']];
Blockly.Language.led_turnOn.DEVICE = [['Multi-colour', 'Rgb'], ['Green', 'Led1'], ['Red', 'Led2']];


Blockly.JavaScript.button_isOn = function () {

    var code = 'device.' + this.getTitleValue('DEVICE') + '.IsOn';
  return [code, Blockly.JavaScript.ORDER_NONE];
};


Blockly.JavaScript.led_turnOn = function () {

    return 'device.' + this.getTitleValue('DEVICE') + '.IsOn = ' + this.getTitleValue('STATE') + ';\n';
};
