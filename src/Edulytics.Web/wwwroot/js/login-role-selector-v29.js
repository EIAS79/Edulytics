(() => {
  const form = document.querySelector('[data-role-gated-login]');
  const roleInputs = [...document.querySelectorAll('.ed-login-v28-role-input')];
  if (!form || roleInputs.length === 0) return;

  const controls = [...form.querySelectorAll('[data-role-gated-control]')];
  const helper = document.querySelector('[data-login-gated-copy]');

  const syncState = ({ focusEmail = false } = {}) => {
    const selected = roleInputs.find(input => input.checked);
    const hasSchoolRole = Boolean(selected);

    document.querySelectorAll('.ed-login-v28-audience').forEach(card => {
      const input = document.getElementById(card.htmlFor);
      const active = Boolean(input?.checked);
      card.classList.toggle('is-selected', active);
      card.setAttribute('aria-pressed', String(active));
    });

    if (helper) {
      helper.textContent = hasSchoolRole
        ? helper.dataset.readyText || helper.textContent
        : helper.dataset.lockedText || helper.textContent;
    }

    if (hasSchoolRole && focusEmail) {
      controls[0]?.focus();
    }
  };

  roleInputs.forEach(input => {
    input.addEventListener('change', () => syncState({ focusEmail: true }));
  });

  // Credentials stay available even when no public school role is selected.
  // The backend permits that path only for the internal platform administrator;
  // every school user is still required to select a matching public role.
  syncState();
})();
