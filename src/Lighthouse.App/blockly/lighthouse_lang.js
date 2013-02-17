
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




Blockly.Language.button_isOn.DEVICE = [['INPUT1', 'INPUT1'], ['INPUT2', 'INPUT2']];




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
Blockly.Language.led_turnOn.DEVICE = [['LED1', 'LED1'], ['LED2', 'LED2']];



Blockly.Language.variableinput_value = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {
        this.setColour(230);//number colour
        this.setOutput(true, Number);
        this.appendDummyInput()
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
            .appendTitle('value')

        this.setTooltip('Returns number between 0 and 255 for the device');
    }
};

Blockly.Language.variableinput_value.DEVICE = [['RANGE1', 'RANGE1'], ['RANGE2', 'RANGE2']];






Blockly.Language.buzzer_turnOn = {
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
        this.setTooltip('Turns the buzzer on or off.');
    }
};


Blockly.Language.buzzer_turnOn.STATE = [['on', 'true'], ['off', 'false']];
Blockly.Language.buzzer_turnOn.DEVICE = [['BUZZER1', 'BUZZER1'], ['BUZZER2', 'BUZZER2']];

Blockly.Language.buzzer_setTone = {
    helpUrl: '',
    init: function () {
        this.setColour(290);

        this.appendValueInput('TONE')
            .setCheck(Number)
            .appendTitle("set tone for")
            .appendTitle(new Blockly.FieldDropdown(this.DEVICE), 'DEVICE')
        .appendTitle("to");

        this.setPreviousStatement(true);
        this.setNextStatement(true);

        this.setTooltip('Sets the tone for the buzzer');
    }
};
Blockly.Language.buzzer_setTone.DEVICE = [['BUZZER1', 'BUZZER1'], ['BUZZER2', 'BUZZER2']];




Blockly.Language.system_sleep = {
    // Block for moving forward or backwards.
    helpUrl: '',
    init: function () {
        this.setColour(290);

        this.appendDummyInput()
            .appendTitle("Wait")
            .appendTitle(new Blockly.FieldTextInput('1', Blockly.FieldTextInput.numberValidator), 'SECS')
            .appendTitle("second(s)")

        this.setPreviousStatement(true);
        this.setNextStatement(true);
        this.setTooltip('Wait a number of seconds before continuing');
    }
};






Blockly.Language.repeat_forever = {
    // Block for checking if there a wall.
    helpUrl: '',
    init: function () {



        this.setColour(120);
        this.appendDummyInput()
            .appendTitle('repeat forever');

        this.appendStatementInput('DO')
            .appendTitle(Blockly.LANG_CONTROLS_WHILEUNTIL_INPUT_DO);
        this.setPreviousStatement(true);
        this.setNextStatement(true);

        this.setTooltip('repeats forever');
    }
};