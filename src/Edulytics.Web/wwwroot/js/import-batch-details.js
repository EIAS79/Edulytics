(() => {
  const root = document.querySelector('[data-import-batch]');
  if (!root) return;

  const filter = root.querySelector('[data-import-row-filter]');
  const rows = Array.from(root.querySelectorAll('[data-import-row]'));
  const checkboxes = Array.from(root.querySelectorAll('input[name="rowNumbers"]'));
  const removeButton = root.querySelector('[data-remove-selected]');
  const count = root.querySelector('[data-import-selection-count]');

  const applyFilter = () => {
    const value = filter?.value || 'all';
    rows.forEach((row) => {
      const validation = row.dataset.validationState;
      const state = row.dataset.importState;
      row.hidden = value !== 'all' && value !== validation && value !== state;
    });
  };

  const updateSelection = () => {
    const selected = checkboxes.filter((box) => box.checked).length;
    if (removeButton) removeButton.disabled = selected === 0;
    if (count) {
      const suffix = count.textContent?.trim().split(' ').slice(1).join(' ') || 'selected';
      count.textContent = `${selected} ${suffix}`;
    }
  };

  filter?.addEventListener('change', applyFilter);
  checkboxes.forEach((box) => box.addEventListener('change', updateSelection));
  applyFilter();
  updateSelection();
})();
