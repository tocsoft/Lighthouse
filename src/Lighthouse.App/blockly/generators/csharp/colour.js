/**
 * Visual Blocks Language
 *
 * Copyright 2012 Google Inc.
 * http://blockly.googlecode.com/
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * @fileoverview Generating CSharp for colour blocks.
 */
'use strict';

goog.provide('Blockly.CSharp.colour');

goog.require('Blockly.CSharp');

Blockly.CSharp.colour_picker = function() {
  // Colour picker.
  var code = '\'' + this.getTitleValue('COLOUR') + '\'';
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.colour_rgb = function() {
  // Compose a colour from RGB components.
  var red = Blockly.CSharp.valueToCode(this, 'RED',
      Blockly.CSharp.ORDER_COMMA) || 0;
  var green = Blockly.CSharp.valueToCode(this, 'GREEN',
      Blockly.CSharp.ORDER_COMMA) || 0;
  var blue = Blockly.CSharp.valueToCode(this, 'BLUE',
      Blockly.CSharp.ORDER_COMMA) || 0;

  if (!Blockly.CSharp.definitions_['colour_rgb']) {
    var functionName = Blockly.CSharp.variableDB_.getDistinctName(
        'colour_rgb', Blockly.Generator.NAME_TYPE);
    Blockly.CSharp.colour_rgb.functionName = functionName;
    var func = [];
    func.push('function ' + functionName + '(r, g, b) {');
    func.push('  r = Math.round(Math.max(Math.min(Number(r), 1), 0) * 255);');
    func.push('  g = Math.round(Math.max(Math.min(Number(g), 1), 0) * 255);');
    func.push('  b = Math.round(Math.max(Math.min(Number(b), 1), 0) * 255);');
    func.push('  r = (\'0\' + (r || 0).toString(16)).slice(-2);');
    func.push('  g = (\'0\' + (g || 0).toString(16)).slice(-2);');
    func.push('  b = (\'0\' + (b || 0).toString(16)).slice(-2);');
    func.push('  return \'#\' + r + g + b;');
    func.push('}');
    Blockly.CSharp.definitions_['colour_rgb'] = func.join('\n');
  }
  var code = Blockly.CSharp.colour_rgb.functionName +
      '(' + red + ', ' + green + ', ' + blue + ')';
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.colour_blend = function() {
  // Blend two colours together.
  var c1 = Blockly.CSharp.valueToCode(this, 'COLOUR1',
      Blockly.CSharp.ORDER_COMMA) || '\'#000000\'';
  var c2 = Blockly.CSharp.valueToCode(this, 'COLOUR2',
      Blockly.CSharp.ORDER_COMMA) || '\'#000000\'';
  var ratio = Blockly.CSharp.valueToCode(this, 'RATIO',
      Blockly.CSharp.ORDER_COMMA) || 0.5;

  if (!Blockly.CSharp.definitions_['colour_blend']) {
    var functionName = Blockly.CSharp.variableDB_.getDistinctName(
        'colour_blend', Blockly.Generator.NAME_TYPE);
    Blockly.CSharp.colour_blend.functionName = functionName;
    var func = [];
    func.push('function ' + functionName + '(c1, c2, ratio) {');
    func.push('  ratio = Math.max(Math.min(Number(ratio), 1), 0);');
    func.push('  var r1 = parseInt(c1.substring(1, 3), 16);');
    func.push('  var g1 = parseInt(c1.substring(3, 5), 16);');
    func.push('  var b1 = parseInt(c1.substring(5, 7), 16);');
    func.push('  var r2 = parseInt(c2.substring(1, 3), 16);');
    func.push('  var g2 = parseInt(c2.substring(3, 5), 16);');
    func.push('  var b2 = parseInt(c2.substring(5, 7), 16);');
    func.push('  var r = Math.round(r1 * (1 - ratio) + r2 * ratio);');
    func.push('  var g = Math.round(g1 * (1 - ratio) + g2 * ratio);');
    func.push('  var b = Math.round(b1 * (1 - ratio) + b2 * ratio);');
    func.push('  r = (\'0\' + (r || 0).toString(16)).slice(-2);');
    func.push('  g = (\'0\' + (g || 0).toString(16)).slice(-2);');
    func.push('  b = (\'0\' + (b || 0).toString(16)).slice(-2);');
    func.push('  return \'#\' + r + g + b;');
    func.push('}');
    Blockly.CSharp.definitions_['colour_blend'] = func.join('\n');
  }
  var code = Blockly.CSharp.colour_blend.functionName +
      '(' + c1 + ', ' + c2 + ', ' + ratio + ')';
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};
