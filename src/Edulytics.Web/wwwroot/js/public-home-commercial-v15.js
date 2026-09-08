(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Remove the parent free-trial CTA from the teacher slide in every language.
     The second slide is the teacher slide for EN / PL / AR, so this remains copy-independent. */
  const teacherActions = root.querySelector('.ed-home-v12-slide:nth-child(2) .ed-home-v12-actions');
  if (teacherActions) {
    teacherActions.querySelector('.ed-home-v12-primary')?.remove();
    teacherActions.classList.add('is-v15-single');
  }
})();
