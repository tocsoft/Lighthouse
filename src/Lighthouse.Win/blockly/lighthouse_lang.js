
'use strict';

// Extensions to Blockly's language and JavaScript generator.


Blockly.Language.button_isOn = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {
        this.setColour(120);
        this.setOutput(true, Boolean);
        this.appendDummyInput()
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle('is on')

        this.setTooltip('Returns true if the button/switch is switched on');
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
        this.setTooltip('Turns the light on.');
    }
};


Blockly.Language.led_turnOn.STATE = [['on', 'true'], ['off', 'false']];
Blockly.Language.led_turnOn.DEVICE = [['Multi-colour', 'Rgb'], ['Green', 'Led1'], ['Red', 'Led2']];



Blockly.Language.variableinput_value = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {
        this.setColour(120);
        this.setOutput(true, Number);
        this.appendDummyInput()
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle('is on')

        this.setTooltip('Returns number between 0 and 255 for the device');
    }
};

Blockly.Language.variableinput_value.DEVICE = [['Knob', 'Knob'], ['Light sensor', 'Light']];


Blockly.Language.rgb_setcolour = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {
        this.setColour(120);
        this.setOutput(true, Number);
        this.appendDummyInput()
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle('is on')

        this.appendValueInput('COLOR')
          .setCheck('Colour')
          .setAlign(Blockly.ALIGN_RIGHT)
          .appendTitle(Blockly.LANG_COLOUR_BLEND_TITLE);

        this.setTooltip('Sets the colour for the light');
    }
};
Blockly.Language.rgb_setcolour.DEVICE = [['Multi-colour', 'Rgb']];


