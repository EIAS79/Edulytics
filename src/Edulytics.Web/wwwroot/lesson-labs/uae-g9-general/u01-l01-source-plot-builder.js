(function (root) {
  'use strict';

  // Author-created diagram workbenches. They only render student-entered data.
  // No expected answers, grades, remote scripts or background submissions.
  const definitions = Object.freeze({
    'U01L01-Q47': {
      kind: 'bar', name: 'مخطط الأعمدة',
      labels: ['الكرة', 'القراءة', 'السباحة', 'الشطرنج', 'الرسم', 'الجري', 'الموسيقى'],
      unit: 'عدد الأصوات', titleRequired: true
    },
    'U01L01-Q48': {
      kind: 'box', name: 'مخطط الصندوق والشاربين',
      labels: ['الصغرى', 'الربيع الأول', 'الوسيط', 'الربيع الثالث', 'العظمى'],
      unit: 'دقيقة', titleRequired: false
    }
  });

  function parseNumber(raw) {
    if (typeof raw !== 'string' && typeof raw !== 'number') return NaN;
    const normalized = String(raw).trim()
      .replace(/[٠-٩]/g, ch => String(ch.charCodeAt(0) - 0x0660))
      .replace(/[۰-۹]/g, ch => String(ch.charCodeAt(0) - 0x06f0))
      .replace(/٫/g, '.').replace(/−/g, '-');
    if (!/^-?\d+(?:\.\d+)?$/.test(normalized)) return NaN;
    return Number(normalized);
  }

  // Running times are durations (mm:ss), not decimal minutes.
  function parseMinutesSeconds(raw) {
    const normalized = String(raw).trim()
      .replace(/[\u0660-\u0669]/g, ch => String(ch.charCodeAt(0) - 0x0660))
      .replace(/[\u06f0-\u06f9]/g, ch => String(ch.charCodeAt(0) - 0x06f0));
    if (!/^\d{1,3}:[0-5]\d$/.test(normalized)) return NaN;
    const [minutes, seconds] = normalized.split(':').map(Number);
    return minutes + seconds / 60;
  }

  function formatMinutesSeconds(value) {
    const totalSeconds = Math.round(value * 60);
    return Math.floor(totalSeconds / 60) + ':' +
      String(totalSeconds % 60).padStart(2, '0');
  }

  function validate(sourceItem, rawValues, title) {
    const definition = definitions[sourceItem];
    if (!definition) return { ok: false, error: 'هذه المهمة لا تحتاج إلى مخطط.' };
    if (!Array.isArray(rawValues) || rawValues.length !== definition.labels.length)
      return { ok: false, error: 'عدد حقول المخطط غير صحيح.' };
    const timed = definition.kind === 'box' && rawValues.some(value =>
      typeof value === 'string' && value.includes(':'));
    if (timed && rawValues.some(value => !String(value).includes(':')))
      return { ok: false, error: '\u0627\u0643\u062a\u0628 \u062c\u0645\u064a\u0639 \u0627\u0644\u0623\u0632\u0645\u0646\u0629 \u0628\u0635\u064a\u063a\u0629 \u062f\u0642\u064a\u0642\u0629:\u062b\u0627\u0646\u064a\u0629 \u0623\u0648 \u062c\u0645\u064a\u0639\u0647\u0627 \u0628\u0627\u0644\u062f\u0642\u0627\u0626\u0642.' };
    const values = rawValues.map(value =>
      definition.kind === 'box' && typeof value === 'string' && value.includes(':')
        ? parseMinutesSeconds(value)
        : parseNumber(value));
    if (values.some(v => !Number.isFinite(v) || v < 0 || v > 1000))
      return { ok: false, error: 'أدخل قيمًا رقمية غير سالبة لا تتجاوز ١٠٠٠.' };
    if (definition.kind === 'bar' && values.some(v => !Number.isInteger(v)))
      return { ok: false, error: 'عدد الأصوات يجب أن يكون عددًا صحيحًا.' };
    if (definition.kind === 'box' && values.some((value, i) => i > 0 && value < values[i - 1]))
      return { ok: false, error: 'رتّب القيم الخمس من الصغرى إلى العظمى.' };
    const normalizedTitle = String(title || '').trim();
    if (definition.titleRequired && normalizedTitle.length < 5)
      return { ok: false, error: 'اكتب عنوانًا واضحًا للمخطط أولًا.' };
    if (normalizedTitle.length > 90)
      return { ok: false, error: 'عنوان المخطط طويل جدًا.' };
    return {
      ok: true, taskId: sourceItem, kind: definition.kind,
      title: normalizedTitle || definition.name, labels: definition.labels.slice(),
      unit: timed ? '\u062f\u0642\u064a\u0642\u0629:\u062b\u0627\u0646\u064a\u0629' : definition.unit,
      timeFormat: timed ? 'mm:ss' : 'decimal-minutes',
      rawEntries: rawValues.map(value => String(value)),
      values, review: 'TEACHER_REVIEW_REQUIRED',
      graded: false
    };
  }

  function svgElement(documentRef, tag, attrs, text) {
    const element = documentRef.createElementNS('http://www.w3.org/2000/svg', tag);
    Object.entries(attrs || {}).forEach(([name, value]) => element.setAttribute(name, String(value)));
    if (text !== undefined) element.textContent = String(text);
    return element;
  }

  function draw(documentRef, result) {
    const svg = svgElement(documentRef, 'svg', {
      viewBox: '0 0 780 380', role: 'img', 'aria-label': result.title + ' — ' + result.unit,
      width: '100%'
    });
    svg.append(svgElement(documentRef, 'title', {}, result.title));
    svg.append(svgElement(documentRef, 'desc', {},
      result.kind === 'bar' ? 'مخطط من القيم السبعة التي أدخلها الطالب.'
        : 'مخطط صندوق وشاربين من ملخص الطالب ذي القيم الخمس.'));
    svg.append(svgElement(documentRef, 'text', {
      x: 390, y: 30, 'text-anchor': 'middle', fill: 'currentColor', 'font-size': 19
    }, result.title));

    const append = (tag, attrs, text) => svg.append(svgElement(documentRef, tag, attrs, text));
    const axis = '#365872', mark = '#2563a8';
    if (result.kind === 'bar') {
      const max = Math.max(10, Math.ceil(Math.max(...result.values) / 5) * 5);
      for (let tick = 0; tick <= 5; tick++) {
        const y = 315 - 245 * tick / 5;
        append('line', { x1: 80, y1: y, x2: 735, y2: y,
          stroke: axis, 'stroke-opacity': tick ? '.15' : '.8' });
        append('text', { x: 68, y: y + 5, 'text-anchor': 'end',
          fill: 'currentColor', 'font-size': 14 }, max * tick / 5);
      }
      append('line', { x1: 80, y1: 55, x2: 80, y2: 315, stroke: axis, 'stroke-width': 2 });
      append('text', { x: 27, y: 190, fill: 'currentColor', 'font-size': 13,
        transform: 'rotate(-90 27 190)', 'text-anchor': 'middle' }, result.unit);
      result.values.forEach((value, i) => {
        const x = 106 + i * 92, height = value / max * 245;
        append('rect', { x, y: 315 - height, width: 58, height,
          fill: mark, rx: 2 });
        append('text', { x: x + 29, y: 309 - height, 'text-anchor': 'middle',
          fill: 'currentColor', 'font-size': 14 }, value);
        append('text', { x: x + 29, y: 342, 'text-anchor': 'middle',
          fill: 'currentColor', 'font-size': 12 }, result.labels[i]);
      });
    } else {
      const a = result.values;
      const axisMin = Math.max(0, Math.floor((a[0] - 2) / 5) * 5);
      const axisMax = Math.max(axisMin + 5, Math.ceil((a[4] + 1) / 5) * 5);
      const x = value => 105 + 590 * (value - axisMin) / (axisMax - axisMin);
      append('line', { x1: 85, y1: 260, x2: 715, y2: 260, stroke: axis, 'stroke-width': 2 });
      for (let tick = 0; tick <= 5; tick++) {
        const value = axisMin + tick * (axisMax - axisMin) / 5, px = x(value);
        append('line', { x1: px, y1: 251, x2: px, y2: 270, stroke: axis });
        append('text', { x: px, y: 290, 'text-anchor': 'middle',
          fill: 'currentColor', 'font-size': 13 },
          result.timeFormat === 'mm:ss' ? formatMinutesSeconds(value) : Number(value.toFixed(1)));
      }
      append('line', { x1: x(a[0]), y1: 178, x2: x(a[4]), y2: 178,
        stroke: mark, 'stroke-width': 3 });
      append('line', { x1: x(a[0]), y1: 152, x2: x(a[0]), y2: 204,
        stroke: mark, 'stroke-width': 3 });
      append('line', { x1: x(a[4]), y1: 152, x2: x(a[4]), y2: 204,
        stroke: mark, 'stroke-width': 3 });
      append('rect', { x: x(a[1]), y: 125, width: Math.max(0, x(a[3]) - x(a[1])),
        height: 106, fill: '#dbeafe', stroke: mark, 'stroke-width': 3 });
      append('line', { x1: x(a[2]), y1: 125, x2: x(a[2]), y2: 231,
        stroke: '#bd3e45', 'stroke-width': 4 });
      append('text', { x: 390, y: 337, 'text-anchor': 'middle',
        fill: 'currentColor', 'font-size': 15 }, result.unit);
      append('text', { x: 390, y: 370, 'text-anchor': 'middle',
        fill: 'currentColor', 'font-size': 13 },
        'الصغرى • الربيع الأول • الوسيط • الربيع الثالث • العظمى');
    }
    return svg;
  }

  function attach(task, listItem, answer) {
    const definition = definitions[task.sourceItem];
    if (!definition) return;
    const d = listItem.ownerDocument;
    const fieldset = d.createElement('fieldset');
    fieldset.className = 'plot-builder';
    fieldset.setAttribute('aria-label', definition.name + ' — ' + task.sourceItem);
    const legend = d.createElement('legend');
    legend.textContent = 'إنشاء ' + definition.name + ' (عمل الطالب)';
    fieldset.append(legend);
    const info = d.createElement('p');
    info.textContent = 'أدخل القيم التي استنتجتها بنفسك، ثم اضغط معاينة. الرسم وصفٌ لإجابتك وليس تصحيحًا آليًا.';
    if (definition.kind === 'box')
      info.textContent += ' \u0644\u0623\u0632\u0645\u0646\u0629 \u0627\u0644\u0633\u0628\u0627\u0642 \u0627\u0643\u062a\u0628 \u062f\u0642\u064a\u0642\u0629:\u062b\u0627\u0646\u064a\u0629 \u0645\u062b\u0644 17:30 \u0623\u0648 \u0642\u064a\u0645\u0629 \u0628\u0627\u0644\u062f\u0642\u0627\u0626\u0642.';
    fieldset.append(info);
    const titleLabel = d.createElement('label');
    titleLabel.textContent = 'عنوان المخطط';
    const titleInput = d.createElement('input');
    titleInput.type = 'text'; titleInput.maxLength = 90;
    titleInput.setAttribute('aria-label', 'عنوان مخطط المهمة ' + task.sourceItem);
    titleLabel.append(titleInput);
    fieldset.append(titleLabel);
    const grid = d.createElement('div');
    grid.className = 'plot-inputs';
    const inputs = definition.labels.map((label, index) => {
      const parent = d.createElement('label');
      parent.textContent = label;
      const input = d.createElement('input');
      // Chromium number fields discard Arabic-Indic and Persian digits.
      // Text fields preserve the original digits; validate() checks values.
      input.type = 'text';
      input.inputMode = definition.kind === 'box' ? 'text' : 'decimal';
      input.maxLength = 18;
      input.autocomplete = 'off';
      input.setAttribute('data-plot-number', 'true');
      input.setAttribute('dir', 'ltr');
      input.setAttribute('aria-label', label + ' — ' + task.sourceItem);
      parent.append(input); grid.append(parent);
      return input;
    });
    fieldset.append(grid);
    const previewButton = d.createElement('button');
    previewButton.type = 'button';
    previewButton.textContent = 'ارسم من القيم المدخلة';
    fieldset.append(previewButton);
    const error = d.createElement('p');
    error.setAttribute('role', 'alert');
    fieldset.append(error);
    const preview = d.createElement('div');
    preview.className = 'plot-preview';
    fieldset.append(preview);

    const clear = () => {
      delete listItem.dataset.plotDraft;
      preview.replaceChildren();
      error.textContent = '';
    };
    [titleInput, ...inputs].forEach(input => input.addEventListener('input', clear));
    previewButton.addEventListener('click', () => {
      const model = validate(task.sourceItem, inputs.map(input => input.value), titleInput.value);
      preview.replaceChildren();
      if (!model.ok) {
        error.textContent = model.error;
        delete listItem.dataset.plotDraft;
        return;
      }
      error.textContent = '';
      preview.append(draw(d, model));
      listItem.dataset.plotDraft = JSON.stringify(model);
    });
    listItem.append(fieldset);
    // The answer remains available for the student's explanation of the diagram.
    answer.setAttribute('aria-describedby', 'chart-help-' + task.independentTaskId);
    info.id = 'chart-help-' + task.independentTaskId;
  }

  const api = Object.freeze({ definitions, validate, draw, attach });
  if (typeof module !== 'undefined' && module.exports) module.exports = api;
  if (root) root.Grade9SourcePlot = api;
})(typeof window !== 'undefined' ? window : null);
