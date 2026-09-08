(() => {
  const root = document.querySelector('.ed-home');
  if (!root) return;

  /* Remove only assistant-created public-home cartoon mascot artwork.
     Do not touch classroom photography or product/application visuals. */
  root.querySelectorAll('.ed-home-v12-mascot').forEach(node => node.remove());
})();
