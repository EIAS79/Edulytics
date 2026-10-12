'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {clean,tokens,notationTokens,sameExtractedMathText}=require('../tools/us-ccss-official-text-audit.cjs');

test('Phase 02: official exponent HTML remains visible in mathematics comparisons',()=>{
  const html='3<sup>2</sup> &times; 3<sup>-5</sup> = 3<sup>-3</sup> = 1/3<sup>3</sup>';
  assert.equal(clean(html),'3^{2} × 3^{-5} = 3^{-3} = 1/3^{3}');
  assert.deepEqual(notationTokens(html),['^{2}','×','^{-5}','=', '^{-3}','=', '^{3}']);
  assert.equal(sameExtractedMathText(html,'3 × 3 = 3 = 1/3'),false);
});

test('Phase 02: mathematical signs and relations cannot silently pass lexical exact match',()=>{
  const comparisons=[
    ['x + y','x - y'],
    ['x < 2','x > 2'],
    ['x = 2','x ≠ 2'],
    ['5<sup>1/3</sup>','5<sup>-1/3</sup>'],
  ];
  for(const [a,b] of comparisons){
    assert.equal(tokens(a).join(' '),tokens(b).join(' '),
      'Regression fixture must exercise the previously unsafe lexical equality: '+a);
    assert.equal(sameExtractedMathText(a,b),false,
      'Mathematical notation divergence must require review: '+a+' vs '+b);
  }
  assert.equal(sameExtractedMathText('x<sup>2</sup>','x<sup>3</sup>'),false);
  assert.equal(sameExtractedMathText('A<sub>1</sub>','A<sub>2</sub>'),false);
});

test('Phase 02: the literal math symbols and HTML entities are retained',()=>{
  assert.equal(clean('x &ne; 0 &amp; y &le; 3 &times; 2'),'x ≠ 0 & y ≤ 3 × 2');
  assert.equal(clean('5<sup>&#45;2</sup>'),'5^{-2}');
  assert.equal(clean('a<sub>n</sub>'),'a_{n}');
  assert.equal(sameExtractedMathText('a = b','a = b'),true);
});

test('Phase 02: footnote-like superscript markers remain reviewable, not automatically removed',()=>{
  assert.equal(clean('Explain why.<sup><a href="#footnote-1">1</a></sup>'),
    'Explain why.^{1}');
  assert.equal(sameExtractedMathText('Explain why.<sup>1</sup>','Explain why.'),false);
});
