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
 * @fileoverview Generating CSharp for math blocks.
 */
'use strict';

goog.provide('Blockly.CSharp.math');

goog.require('Blockly.CSharp');

Blockly.CSharp.math_number = function() {
  // Numeric value.
  var code = window.parseFloat(this.getTitleValue('NUM'));
  return [code, Blockly.CSharp.ORDER_ATOMIC];
};

Blockly.CSharp.math_arithmetic = function() {
  // Basic arithmetic operators, and power.
  var mode = this.getTitleValue('OP');
  var tuple = Blockly.CSharp.math_arithmetic.OPERATORS[mode];
  var operator = tuple[0];
  var order = tuple[1];
  var argument0 = Blockly.CSharp.valueToCode(this, 'A', order) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'B', order) || '0';
  var code;
  // Power in CSharp requires a special case since it has no operator.
  if (!operator) {
    code = 'Math.pow(' + argument0 + ', ' + argument1 + ')';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  }
  code = argument0 + operator + argument1;
  return [code, order];
};

Blockly.CSharp.math_arithmetic.OPERATORS = {
  ADD: [' + ', Blockly.CSharp.ORDER_ADDITION],
  MINUS: [' - ', Blockly.CSharp.ORDER_SUBTRACTION],
  MULTIPLY: [' * ', Blockly.CSharp.ORDER_MULTIPLICATION],
  DIVIDE: [' / ', Blockly.CSharp.ORDER_DIVISION],
  POWER: [null, Blockly.CSharp.ORDER_COMMA]  // Handle power separately.
};

Blockly.CSharp.math_single = function() {
  // Math operators with single operand.
  var operator = this.getTitleValue('OP');
  var code;
  var arg;
  if (operator == 'NEG') {
    // Negation is a special case given its different operator precedence.
    arg = Blockly.CSharp.valueToCode(this, 'NUM',
        Blockly.CSharp.ORDER_UNARY_NEGATION) || '0';
    if (arg[0] == '-') {
      // --3 is not legal in JS.
      arg = ' ' + arg;
    }
    code = '-' + arg;
    return [code, Blockly.CSharp.ORDER_UNARY_NEGATION];
  }
  if (operator == 'SIN' || operator == 'COS' || operator == 'TAN') {
    arg = Blockly.CSharp.valueToCode(this, 'NUM',
        Blockly.CSharp.ORDER_DIVISION) || '0';
  } else {
    arg = Blockly.CSharp.valueToCode(this, 'NUM',
        Blockly.CSharp.ORDER_NONE) || '0';
  }
  // First, handle cases which generate values that don't need parentheses
  // wrapping the code.
  switch (operator) {
    case 'ABS':
      code = 'Math.abs(' + arg + ')';
      break;
    case 'ROOT':
      code = 'Math.sqrt(' + arg + ')';
      break;
    case 'LN':
      code = 'Math.log(' + arg + ')';
      break;
    case 'EXP':
      code = 'Math.exp(' + arg + ')';
      break;
    case 'POW10':
      code = 'Math.pow(10,' + arg + ')';
      break;
    case 'ROUND':
      code = 'Math.round(' + arg + ')';
      break;
    case 'ROUNDUP':
      code = 'Math.ceil(' + arg + ')';
      break;
    case 'ROUNDDOWN':
      code = 'Math.floor(' + arg + ')';
      break;
    case 'SIN':
      code = 'Math.sin(' + arg + ' / 180 * Math.PI)';
      break;
    case 'COS':
      code = 'Math.cos(' + arg + ' / 180 * Math.PI)';
      break;
    case 'TAN':
      code = 'Math.tan(' + arg + ' / 180 * Math.PI)';
      break;
  }
  if (code) {
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  }
  // Second, handle cases which generate values that may need parentheses
  // wrapping the code.
  switch (operator) {
    case 'LOG10':
      code = 'Math.log(' + arg + ') / Math.log(10)';
      break;
    case 'ASIN':
      code = 'Math.asin(' + arg + ') / Math.PI * 180';
      break;
    case 'ACOS':
      code = 'Math.acos(' + arg + ') / Math.PI * 180';
      break;
    case 'ATAN':
      code = 'Math.atan(' + arg + ') / Math.PI * 180';
      break;
    default:
      throw 'Unknown math operator: ' + operator;
  }
  return [code, Blockly.CSharp.ORDER_DIVISION];
};

Blockly.CSharp.math_constant = function() {
  // Constants: PI, E, the Golden Ratio, sqrt(2), 1/sqrt(2), INFINITY.
  var constant = this.getTitleValue('CONSTANT');
  return Blockly.CSharp.math_constant.CONSTANTS[constant];
};

Blockly.CSharp.math_constant.CONSTANTS = {
  PI: ['Math.PI', Blockly.CSharp.ORDER_MEMBER],
  E: ['Math.E', Blockly.CSharp.ORDER_MEMBER],
  GOLDEN_RATIO: ['(1 + Math.sqrt(5)) / 2', Blockly.CSharp.ORDER_DIVISION],
  SQRT2: ['Math.SQRT2', Blockly.CSharp.ORDER_MEMBER],
  SQRT1_2: ['Math.SQRT1_2', Blockly.CSharp.ORDER_MEMBER],
  INFINITY: ['Infinity', Blockly.CSharp.ORDER_ATOMIC]
};

Blockly.CSharp.math_number_property = function() {
  // Check if a number is even, odd, prime, whole, positive, or negative
  // or if it is divisible by certain number. Returns true or false.
  var number_to_check = Blockly.CSharp.valueToCode(this, 'NUMBER_TO_CHECK',
      Blockly.CSharp.ORDER_MODULUS) || 'NaN';
  var dropdown_property = this.getTitleValue('PROPERTY');
  var code;
  if (dropdown_property == 'PRIME') {
    // Prime is a special case as it is not a one-liner test.
    if (!Blockly.CSharp.definitions_['isPrime']) {
      var functionName = Blockly.CSharp.variableDB_.getDistinctName(
          'isPrime', Blockly.Generator.NAME_TYPE);
      Blockly.CSharp.logic_prime= functionName;
      var func = [];
      func.push('function ' + functionName + '(n) {');
      func.push('  // http://en.wikipedia.org/wiki/Primality_test#Naive_methods');
      func.push('  if (n == 2 || n == 3) {');
      func.push('    return true;');
      func.push('  }');
      func.push('  // False if n is NaN, negative, is 1, or not whole.');
      func.push('  // And false if n is divisible by 2 or 3.');
      func.push('  if (isNaN(n) || n <= 1 || n % 1 != 0 || n % 2 == 0 ||' +
                ' n % 3 == 0) {');
      func.push('    return false;');
      func.push('  }');
      func.push('  // Check all the numbers of form 6k +/- 1, up to sqrt(n).');
      func.push('  for (var x = 6; x <= Math.sqrt(n) + 1; x += 6) {');
      func.push('    if (n % (x - 1) == 0 || n % (x + 1) == 0) {');
      func.push('      return false;');
      func.push('    }');
      func.push('  }');
      func.push('  return true;');
      func.push('}');
      Blockly.CSharp.definitions_['isPrime'] = func.join('\n');
    }
    code = Blockly.CSharp.logic_prime + '(' + number_to_check + ')';
    return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
  }
  switch (dropdown_property) {
    case 'EVEN':
      code = number_to_check + ' % 2 == 0';
      break;
    case 'ODD':
      code = number_to_check + ' % 2 == 1';
      break;
    case 'WHOLE':
      code = number_to_check + ' % 1 == 0';
      break;
    case 'POSITIVE':
      code = number_to_check + ' > 0';
      break;
    case 'NEGATIVE':
      code = number_to_check + ' < 0';
      break;
    case 'DIVISIBLE_BY':
      var divisor = Blockly.CSharp.valueToCode(this, 'DIVISOR',
          Blockly.CSharp.ORDER_MODULUS) || 'NaN';
      code = number_to_check + ' % ' + divisor + ' == 0';
      break;
  }
  return [code, Blockly.CSharp.ORDER_EQUALITY];
};

Blockly.CSharp.math_change = function() {
  // Add to a variable in place.
  var argument0 = Blockly.CSharp.valueToCode(this, 'DELTA',
      Blockly.CSharp.ORDER_ADDITION) || '0';
  var varName = Blockly.CSharp.variableDB_.getName(
      this.getTitleValue('VAR'), Blockly.Variables.NAME_TYPE);
  return varName + ' = (typeof ' + varName + ' == \'number\' ? ' + varName +
      ' : 0) + ' + argument0 + ';\n';
};

// Rounding functions have a single operand.
Blockly.CSharp.math_round = Blockly.CSharp.math_single;
// Trigonometry functions have a single operand.
Blockly.CSharp.math_trig = Blockly.CSharp.math_single;

Blockly.CSharp.math_on_list = function() {
  // Math functions for lists.
  var func = this.getTitleValue('OP');
  var list, code;
  switch (func) {
    case 'SUM':
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_MEMBER) || '[]';
      code = list + '.reduce(function(x, y) {return x + y;})';
      break;
    case 'MIN':
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_COMMA) || '[]';
      code = 'Math.min.apply(null, ' + list + ')';
      break;
    case 'MAX':
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_COMMA) || '[]';
      code = 'Math.max.apply(null, ' + list + ')';
      break;
    case 'AVERAGE':
      // math_median([null,null,1,3]) == 2.0.
      if (!Blockly.CSharp.definitions_['math_mean']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'math_mean', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.math_on_list.math_mean = functionName;
        var func = [];
        func.push('function ' + functionName + '(myList) {');
        func.push('  return myList.reduce(function(x, y) {return x + y;}) / ' +
                  'myList.length;');
        func.push('}');
        Blockly.CSharp.definitions_['math_mean'] = func.join('\n');
      }
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_NONE) || '[]';
      code = Blockly.CSharp.math_on_list.math_mean + '(' + list + ')';
      break;
    case 'MEDIAN':
      // math_median([null,null,1,3]) == 2.0.
      if (!Blockly.CSharp.definitions_['math_median']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'math_median', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.math_on_list.math_median = functionName;
        var func = [];
        func.push('function ' + functionName + '(myList) {');
        func.push('  var localList = myList.filter(function (x) ' +
                  '{return typeof x == \'number\';});');
        func.push('  if (!localList.length) return null;');
        func.push('  localList.sort(function(a, b) {return b - a;});');
        func.push('  if (localList.length % 2 == 0) {');
        func.push('    return (localList[localList.length / 2 - 1] + ' +
                  'localList[localList.length / 2]) / 2;');
        func.push('  } else {');
        func.push('    return localList[(localList.length - 1) / 2];');
        func.push('  }');
        func.push('}');
        Blockly.CSharp.definitions_['math_median'] = func.join('\n');
      }
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_NONE) || '[]';
      code = Blockly.CSharp.math_on_list.math_median + '(' + list + ')';
      break;
    case 'MODE':
      if (!Blockly.CSharp.definitions_['math_modes']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'math_modes', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.math_on_list.math_modes = functionName;
        // As a list of numbers can contain more than one mode,
        // the returned result is provided as an array.
        // Mode of [3, 'x', 'x', 1, 1, 2, '3'] -> ['x', 1].
        var func = [];
        func.push('function ' + functionName + '(values) {');
        func.push('  var modes = [];');
        func.push('  var counts = [];');
        func.push('  var maxCount = 0;');
        func.push('  for (var i = 0; i < values.length; i++) {');
        func.push('    var value = values[i];');
        func.push('    var found = false;');
        func.push('    var thisCount;');
        func.push('    for (var j = 0; j < counts.length; j++) {');
        func.push('      if (counts[j][0] === value) {');
        func.push('        thisCount = ++counts[j][1];');
        func.push('        found = true;');
        func.push('        break;');
        func.push('      }');
        func.push('    }');
        func.push('    if (!found) {');
        func.push('      counts.push([value, 1]);');
        func.push('      thisCount = 1;');
        func.push('    }');
        func.push('    maxCount = Math.max(thisCount, maxCount);');
        func.push('  }');
        func.push('  for (var j = 0; j < counts.length; j++) {');
        func.push('    if (counts[j][1] == maxCount) {');
        func.push('        modes.push(counts[j][0]);');
        func.push('    }');
        func.push('  }');
        func.push('  return modes;');
        func.push('}');
        Blockly.CSharp.definitions_['math_modes'] = func.join('\n');
      }
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_NONE) || '[]';
      code = Blockly.CSharp.math_on_list.math_modes + '(' + list + ')';
      break;
    case 'STD_DEV':
      if (!Blockly.CSharp.definitions_['math_standard_deviation']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'math_standard_deviation', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.math_on_list.math_standard_deviation = functionName;
        var func = [];
        func.push('function ' + functionName + '(numbers) {');
        func.push('  var n = numbers.length;');
        func.push('  if (!n) return null;');
        func.push('  var mean = numbers.reduce(function(x, y) ' +
                  '{return x + y;}) / n;');
        func.push('  var variance = 0;');
        func.push('  for (var j = 0; j < n; j++) {');
        func.push('    variance += Math.pow(numbers[j] - mean, 2);');
        func.push('  }');
        func.push('  variance = variance / n;');
        func.push('  return Math.sqrt(variance);');
        func.push('}');
        Blockly.CSharp.definitions_['math_standard_deviation'] =
            func.join('\n');
      }
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_NONE) || '[]';
      code = Blockly.CSharp.math_on_list.math_standard_deviation +
          '(' + list + ')';
      break;
    case 'RANDOM':
      if (!Blockly.CSharp.definitions_['math_random_item']) {
        var functionName = Blockly.CSharp.variableDB_.getDistinctName(
            'math_random_item', Blockly.Generator.NAME_TYPE);
        Blockly.CSharp.math_on_list.math_random_item = functionName;
        var func = [];
        func.push('function ' + functionName + '(list) {');
        func.push('  var x = Math.floor(Math.random() * list.length);');
        func.push('  return list[x];');
        func.push('}');
        Blockly.CSharp.definitions_['math_random_item'] = func.join('\n');
      }
      list = Blockly.CSharp.valueToCode(this, 'LIST',
          Blockly.CSharp.ORDER_NONE) || '[]';
      code = Blockly.CSharp.math_on_list.math_random_item +
          '(' + list + ')';
      break;
    default:
      throw 'Unknown operator: ' + func;
  }
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.math_modulo = function() {
  // Remainder computation.
  var argument0 = Blockly.CSharp.valueToCode(this, 'DIVIDEND',
      Blockly.CSharp.ORDER_MODULUS) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'DIVISOR',
      Blockly.CSharp.ORDER_MODULUS) || '0';
  var code = argument0 + ' % ' + argument1;
  return [code, Blockly.CSharp.ORDER_MODULUS];
};

Blockly.CSharp.math_constrain = function() {
  // Constrain a number between two limits.
  var argument0 = Blockly.CSharp.valueToCode(this, 'VALUE',
      Blockly.CSharp.ORDER_COMMA) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'LOW',
      Blockly.CSharp.ORDER_COMMA) || '0';
  var argument2 = Blockly.CSharp.valueToCode(this, 'HIGH',
      Blockly.CSharp.ORDER_COMMA) || 'Infinity';
  var code = 'Math.min(Math.max(' + argument0 + ', ' + argument1 + '), ' +
      argument2 + ')';
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.math_random_int = function() {
  // Random integer between [X] and [Y].
  var argument0 = Blockly.CSharp.valueToCode(this, 'FROM',
      Blockly.CSharp.ORDER_COMMA) || '0';
  var argument1 = Blockly.CSharp.valueToCode(this, 'TO',
      Blockly.CSharp.ORDER_COMMA) || '0';
  if (!Blockly.CSharp.definitions_['math_random_int']) {
    var functionName = Blockly.CSharp.variableDB_.getDistinctName(
        'math_random_int', Blockly.Generator.NAME_TYPE);
    Blockly.CSharp.math_random_int.random_function = functionName;
    var func = [];
    func.push('function ' + functionName + '(a, b) {');
    func.push('  if (a > b) {');
    func.push('    // Swap a and b to ensure a is smaller.');
    func.push('    var c = a;');
    func.push('    a = b;');
    func.push('    b = c;');
    func.push('  }');
    func.push('  return Math.floor(Math.random() * (b - a + 1) + a);');
    func.push('}');
    Blockly.CSharp.definitions_['math_random_int'] = func.join('\n');
  }
  var code = Blockly.CSharp.math_random_int.random_function +
      '(' + argument0 + ', ' + argument1 + ')';
  return [code, Blockly.CSharp.ORDER_FUNCTION_CALL];
};

Blockly.CSharp.math_random_float = function() {
  // Random fraction between 0 and 1.
  return ['Math.random()', Blockly.CSharp.ORDER_FUNCTION_CALL];
};
