'use strict';

// Read-only check: both production targets must concatenate identical public
// assets in the same order. Never mutate or regenerate runtime bundle content.
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const spec = Object.freeze([
  { name: 'CSS', csharp: 'CssFiles', bash: 'PUBLIC_CSS_FILES', extension: '.css' },
  { name: 'JavaScript', csharp: 'JsFiles', bash: 'PUBLIC_JS_FILES', extension: '.js' },
  { name: 'Content JavaScript', csharp: 'ContentJsFiles', bash: 'PUBLIC_CONTENT_JS_FILES', extension: '.js' }
]);

function readCsharpArray(content, name) {
  const expression = new RegExp('\\b' + name + '\\s*=\\s*\\[([\\s\\S]*?)\\];', 'g');
  const matches = [...content.matchAll(expression)];
  if (matches.length !== 1) throw new Error('Expected one C# array: ' + name);
  const body = matches[0][1];
  const quoted = /"([^"\\\r\n]+)"/g;
  const files = [...body.matchAll(quoted)].map(item => item[1]);
  if (files.length === 0 || body.replace(quoted, '').replace(/[\s,]/g, '') !== '') {
    throw new Error('Unexpected C# array syntax in ' + name);
  }
  return files;
}

function readBashArray(content, name) {
  const expression = new RegExp('^' + name + '=\\(\\s*\\n([\\s\\S]*?)^\\)', 'gm');
  const matches = [...content.matchAll(expression)];
  if (matches.length !== 1) throw new Error('Expected one Bash array: ' + name);
  const body = matches[0][1];
  const quoted = /^\s*"([^"\r\n]+)"\s*$/gm;
  const files = [...body.matchAll(quoted)].map(item => item[1]);
  if (files.length === 0 || body.replace(quoted, '').trim() !== '') {
    throw new Error('Unexpected Bash array syntax in ' + name);
  }
  return files;
}

function inspect(repositoryRoot = root) {
  const csharp = fs.readFileSync(path.join(repositoryRoot, 'src/Edulytics.Web/Controllers/PublicAssetBundleController.cs'), 'utf8');
  const bash = fs.readFileSync(path.join(repositoryRoot, 'public-frontdoor/build.sh'), 'utf8');
  const webRoot = path.join(repositoryRoot, 'src/Edulytics.Web/wwwroot');
  const seen = new Set();
  const counts = {};

  for (const item of spec) {
    const backend = readCsharpArray(csharp, item.csharp);
    const staticSite = readBashArray(bash, item.bash);
    if (JSON.stringify(backend) !== JSON.stringify(staticSite)) {
      const mismatch = backend.findIndex((file, index) => file !== staticSite[index]);
      throw new Error('Public ' + item.name + ' order differs between ASP.NET and static frontdoor' +
        ' (first differing index ' + (mismatch < 0 ? Math.min(backend.length, staticSite.length) : mismatch) + ')');
    }
    for (const relative of backend) {
      if (!/^(css|js)\/[a-zA-Z0-9][a-zA-Z0-9._/-]*$/.test(relative) ||
          relative.includes('..') || !relative.endsWith(item.extension)) {
        throw new Error('Invalid bundle source path: ' + relative);
      }
      if (seen.has(relative)) throw new Error('Duplicate bundle entry: ' + relative);
      seen.add(relative);
      const absolute = path.join(webRoot, relative);
      if (!fs.statSync(absolute, { throwIfNoEntry: false })?.isFile()) {
        throw new Error('Missing bundle source file: ' + relative);
      }
    }
    counts[item.name] = backend.length;
  }
  return counts;
}

if (require.main === module) {
  try {
    const counts = inspect();
    process.stdout.write('Public bundle parity OK: ' + JSON.stringify(counts) + '\n');
  } catch (error) {
    process.stderr.write('Public bundle parity FAILED: ' + error.message + '\n');
    process.exitCode = 1;
  }
}
module.exports = { inspect, readCsharpArray, readBashArray };
