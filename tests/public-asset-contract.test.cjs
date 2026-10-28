'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const path = require('node:path');
const { inspect, readCsharpArray, readBashArray } = require('../tools/public-asset-contract.cjs');

test('static and ASP.NET public assets remain ordered identically and exist on disk', () => {
  const counts = inspect(path.resolve(__dirname, '..'));
  assert.deepEqual(counts, { CSS: 24, JavaScript: 19, 'Content JavaScript': 4 });
});

test('array parsers reject omitted and malformed manifests', () => {
  assert.throws(() => readCsharpArray('private string[] Unrelated = [];', 'CssFiles'), /Expected one C# array/);
  assert.throws(() => readBashArray('PUBLIC_CSS_FILES=(\n  "css/main.css"\n  bad\n)', 'PUBLIC_CSS_FILES'), /Unexpected Bash array syntax/);
});

test('array parsers keep declaration order rather than sorting', () => {
  const cs = 'private static readonly string[] CssFiles =\n[\n "css/a.css",\n "css/b.css"\n];';
  const sh = 'PUBLIC_CSS_FILES=(\n  "css/a.css"\n  "css/b.css"\n)';
  assert.deepEqual(readCsharpArray(cs, 'CssFiles'), ['css/a.css', 'css/b.css']);
  assert.deepEqual(readBashArray(sh, 'PUBLIC_CSS_FILES'), ['css/a.css', 'css/b.css']);
});
