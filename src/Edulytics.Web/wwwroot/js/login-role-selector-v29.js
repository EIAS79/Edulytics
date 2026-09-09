(() => {
  const form = document.querySelector('[data-role-gated-login]');
  const roleInputs = [...document.querySelectorAll('.ed-login-v28-role-input')];
  if (!form || roleInputs.length === 0) return;

  const gatedControls = [...form.querySelectorAll('[data-role-gated-control]')];
  const submitButton = form.querySelector('[data-role-gated-submit]');
  const helper = document.querySelector('[data-login-gated-copy]');

  const syncState = ({ focusEmail = false } = {}) => {
    const selected = roleInputs.find(input => input.checked);
    const unlocked = Boolean(selected);

    gatedControls.forEach(control => {
      control.disabled = !unlocked;
      control.setAttribute('aria-disabled', String(!unlocked));
    });

    if (submitButton) {
      submitButton.disabled = !unlocked;
      submitButton.setAttribute('aria-disabled', String(!unlocked));
    }

    document.querySelectorAll('.ed-login-v28-audience').forEach(card => {
      const input = document.getElementById(card.htmlFor);
      const active = Boolean(input?.checked);
      card.classList.toggle('is-selected', active);
      card.setAttribute('aria-pressed', String(active));
    });

    if (helper) {
      helper.textContent = unlocked
        ? helper.dataset.readyText || helper.textContent
        : helper.dataset.lockedText || helper.textContent;
    }

    if (unlocked && focusEmail) {
      gatedControls[0]?.focus();
    }
  };

  roleInputs.forEach(input => {
    input.addEventListener('change', () => syncState({ focusEmail: true }));
  });

  syncState();
})();
