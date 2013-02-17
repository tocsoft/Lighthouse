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
 * @fileoverview Generating CSharp for logic blocks.
 */
'use strict';

goog.provide('Blockly.CSharp.logic');

goog.require('Blockly.CSharp');

Blockly.CSharp.logic_compare = function() {
  // Comparison operator.
  var mode = this.getTitleValue('OP');
  var operator = Blockly.CSharp.logic_compare.OPERATORS[mode];
  var order = (operator == '==' || operator == '!=') ?
      Blockly.CSharp.ORDER_EQUALITY : Blockly.CSharp.ORDER_RELATIONAL;
  var argument0 = Blockly.CSharp.valueToCode(this, 'A', order) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'B', order) || '0';
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.CSharp.logic_compare.OPERATORS = {
  EQ: '==',
  NEQ: '!=',
  LT: '<',
  LTE: '<=',
  GT: '>',
  GTE: '>='
};

Blockly.CSharp.logic_operation = function() {
  // Operations 'and', 'or'.
  var operator = (this.getTitleValue('OP') == 'AND') ? '&&' : '||';
  var order = (operator == '&&') ? Blockly.CSharp.ORDER_LOGICAL_AND :
      Blockly.CSharp.ORDER_LOGICAL_OR;
  var argument0 = Blockly.CSharp.valueToCode(this, 'A', order) || 'false';
  var argument1 = Blockly.CSharp.valueToCode(this, 'B', order) || 'false';
  var code = argument0 + ' ' + operator + ' ' + argument1;
  return [code, order];
};

Blockly.CSharp.logic_negate = function() {
  // Negation.
  var order = Blockly.CSharp.ORDER_LOGICAL_NOT;
  var argument0 = Blockly.CSharp.valueToCode(this, 'BOOL', order) ||
      'false';
  var code = '!' + argument0;
  return [code, order];
};

Blockly.CSharp.logic_boolean = function() {
  // Boolean values true and false.
  var code = (this.getTitleValue('BOOL') == 'TRUE') ? 'true' : 'false';
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.logic_null = function() {
  // Null data type.
  return ['null', Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.logic_ternary = function() {
  // Ternary operator.
  var value_if = Blockly.CSharp.valueToCode(this, 'IF',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'false';
  var value_then = Blockly.CSharp.valueToCode(this, 'THEN',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'null';
  var value_else = Blockly.CSharp.valueToCode(this, 'ELSE',
      Blockly.CSharp.ORDER_CONDITIONAL) || 'null';
  var code = value_if + ' ? ' + value_then + ' : ' + value_else
  return [code, Blockly.CSharp.ORDER_CONDITIONAL];
};
