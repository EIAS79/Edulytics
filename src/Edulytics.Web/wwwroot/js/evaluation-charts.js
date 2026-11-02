(() => {
  "use strict";

  const clamp = (value, min, max) =>
    Math.min(max, Math.max(min, Number.isFinite(value) ? value : min));

  const reducedMotion = window.matchMedia &&
    window.matchMedia("(prefers-reduced-motion: reduce)").matches;

  const parseSeries = (raw) =>
    (raw || "")
      .split(",")
      .map((value) => Number.parseFloat(value.trim()))
      .filter((value) => Number.isFinite(value));

  const parseLabels = (raw) =>
    (raw || "")
      .split("|")
      .map((value) => value.trim());

  const createSvgElement = (name, attributes = {}) => {
    const element = document.createElementNS(
      "http://www.w3.org/2000/svg",
      name
    );

    Object.entries(attributes).forEach(([key, value]) =>
      element.setAttribute(key, String(value))
    );

    return element;
  };

  const animateNumber = (element, target, suffix = "", decimals = 1) => {
    if (!element || !Number.isFinite(target)) return;

    if (reducedMotion) {
      element.textContent = target.toFixed(decimals).replace(/\.0$/, "") + suffix;
      return;
    }

    const start = performance.now();
    const duration = 700;

    const tick = (now) => {
      const progress = clamp((now - start) / duration, 0, 1);
      const eased = 1 - Math.pow(1 - progress, 3);
      const current = target * eased;
      element.textContent =
        current.toFixed(decimals).replace(/\.0$/, "") + suffix;

      if (progress < 1) requestAnimationFrame(tick);
    };

    requestAnimationFrame(tick);
  };

  const initDonut = (root) => {
    const parsed = Number.parseFloat(root.dataset.value || "");

    root.style.setProperty("--chart-value", "0");

    if (!Number.isFinite(parsed)) {
      root.classList.add("is-chart-missing");
      return;
    }

    const value = clamp(parsed, 0, 100);

    requestAnimationFrame(() => {
      root.style.setProperty("--chart-value", String(value));
      root.classList.add("is-chart-ready");
    });

    const number = root.querySelector("[data-chart-number]");
    if (number) {
      animateNumber(
        number,
        value,
        root.dataset.suffix || "%",
        Number.parseInt(root.dataset.decimals || "1", 10)
      );
    }
  };

  const initBars = (root) => {
    root.querySelectorAll("[data-chart-value]").forEach((bar, index) => {
      const parsed = Number.parseFloat(bar.dataset.chartValue || "");

      bar.style.setProperty("--bar-value", "0");

      if (!Number.isFinite(parsed)) {
        bar.classList.add("is-chart-missing");
        return;
      }

      const value = clamp(parsed, 0, 100);

      window.setTimeout(() => {
        bar.style.setProperty("--bar-value", String(value));
        bar.classList.add("is-chart-ready");
      }, reducedMotion ? 0 : index * 55);
    });
  };

  const initLine = (root) => {
    const values = parseSeries(root.dataset.values);
    const labels = parseLabels(root.dataset.labels);

    if (values.length < 2) {
      root.classList.add("is-chart-empty");
      return;
    }

    const width = 640;
    const height = 220;
    const padX = 36;
    const padTop = 22;
    const padBottom = 42;
    const usableWidth = width - padX * 2;
    const usableHeight = height - padTop - padBottom;

    const svg = createSvgElement("svg", {
      viewBox: `0 0 ${width} ${height}`,
      role: "img",
      "aria-label": root.dataset.ariaLabel || "Progress chart"
    });

    [0, 25, 50, 75, 100].forEach((value) => {
      const y = padTop + usableHeight * (1 - value / 100);
      const line = createSvgElement("line", {
        x1: padX,
        x2: width - padX,
        y1: y,
        y2: y,
        class: "evaluation-chart-gridline"
      });
      svg.appendChild(line);

      const label = createSvgElement("text", {
        x: 4,
        y: y + 4,
        class: "evaluation-chart-axis-label"
      });
      label.textContent = String(value);
      svg.appendChild(label);
    });

    const points = values.map((value, index) => {
      const x =
        padX +
        (usableWidth * index) /
          Math.max(1, values.length - 1);
      const y =
        padTop +
        usableHeight *
          (1 - clamp(value, 0, 100) / 100);
      return { x, y, value };
    });

    const polyline = createSvgElement("polyline", {
      points: points.map((point) => `${point.x},${point.y}`).join(" "),
      class: "evaluation-chart-line"
    });
    svg.appendChild(polyline);

    points.forEach((point, index) => {
      const circle = createSvgElement("circle", {
        cx: point.x,
        cy: point.y,
        r: 5,
        class: "evaluation-chart-point"
      });
      svg.appendChild(circle);

      const valueLabel = createSvgElement("text", {
        x: point.x,
        y: point.y - 10,
        "text-anchor": "middle",
        class: "evaluation-chart-value-label"
      });
      valueLabel.textContent =
        point.value.toFixed(1).replace(/\.0$/, "") + "%";
      svg.appendChild(valueLabel);

      if (labels[index]) {
        const xLabel = createSvgElement("text", {
          x: point.x,
          y: height - 14,
          "text-anchor": "middle",
          class: "evaluation-chart-axis-label"
        });
        xLabel.textContent = labels[index].slice(0, 16);
        svg.appendChild(xLabel);
      }
    });

    root.appendChild(svg);

    if (!reducedMotion) {
      const totalLength = polyline.getTotalLength();
      polyline.style.strokeDasharray = String(totalLength);
      polyline.style.strokeDashoffset = String(totalLength);
      requestAnimationFrame(() => {
        polyline.style.strokeDashoffset = "0";
      });
    }
  };

  const initWaterfall = (root) => {
    const start = clamp(
      Number.parseFloat(root.dataset.start || "0"),
      0,
      100
    );
    const deltas = parseSeries(root.dataset.deltas);
    const labels = parseLabels(root.dataset.labels);

    if (!Number.isFinite(start) || deltas.length === 0) {
      root.classList.add("is-chart-empty");
      return;
    }

    const stages = [{ label: labels[0] || "Start", from: 0, to: start, kind: "start" }];
    let current = start;

    deltas.forEach((delta, index) => {
      const next = clamp(current + delta, 0, 100);
      stages.push({
        label: labels[index + 1] || `Step ${index + 1}`,
        from: current,
        to: next,
        kind: delta >= 0 ? "up" : "down",
        delta
      });
      current = next;
    });

    const chart = document.createElement("div");
    chart.className = "evaluation-waterfall-stage-grid";

    stages.forEach((stage) => {
      const column = document.createElement("div");
      column.className = `evaluation-waterfall-stage is-${stage.kind}`;

      const plot = document.createElement("div");
      plot.className = "evaluation-waterfall-plot";

      const lower = Math.min(stage.from, stage.to);
      const upper = Math.max(stage.from, stage.to);
      const span = Math.max(2, upper - lower);

      const bar = document.createElement("div");
      bar.className = "evaluation-waterfall-bar";
      bar.style.setProperty("--waterfall-bottom", String(lower));
      bar.style.setProperty("--waterfall-height", String(span));
      plot.appendChild(bar);

      const value = document.createElement("strong");
      value.textContent =
        stage.kind === "start"
          ? `${stage.to.toFixed(1).replace(/\.0$/, "")}%`
          : `${stage.delta >= 0 ? "+" : ""}${stage.delta
              .toFixed(1)
              .replace(/\.0$/, "")} pts`;

      const label = document.createElement("span");
      label.textContent = stage.label;

      column.appendChild(value);
      column.appendChild(plot);
      column.appendChild(label);
      chart.appendChild(column);
    });

    root.appendChild(chart);
  };

  const reveal = (root) => {
    const type = root.dataset.evaluationChart;

    switch (type) {
      case "donut":
        initDonut(root);
        break;
      case "bars":
      case "lollipop":
        initBars(root);
        break;
      case "line":
        initLine(root);
        break;
      case "waterfall":
        initWaterfall(root);
        break;
      default:
        break;
    }
  };

  document
    .querySelectorAll("[data-report-autosubmit]")
    .forEach((element) => {
      element.addEventListener("change", () => {
        element.form?.requestSubmit();
      });
    });

  document
    .querySelectorAll("[data-report-print]")
    .forEach((element) => {
      element.addEventListener("click", () => window.print());
    });

  const roots = Array.from(
    document.querySelectorAll("[data-evaluation-chart]")
  );

  if (roots.length === 0) return;

  if (reducedMotion || !("IntersectionObserver" in window)) {
    roots.forEach(reveal);
    return;
  }

  const observer = new IntersectionObserver(
    (entries) => {
      entries.forEach((entry) => {
        if (!entry.isIntersecting) return;
        observer.unobserve(entry.target);
        reveal(entry.target);
      });
    },
    { rootMargin: "80px 0px", threshold: 0.12 }
  );

  roots.forEach((root) => observer.observe(root));
})();
