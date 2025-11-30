(function () {
	const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

	const navToggle = document.querySelector('[data-nav-toggle]');
	const navLinks = document.querySelector('[data-nav-links]');

	if (navToggle && navLinks) {
		navToggle.addEventListener('click', () => {
			const expanded = navToggle.getAttribute('aria-expanded') === 'true';
			navToggle.setAttribute('aria-expanded', (!expanded).toString());
			navLinks.classList.toggle('open');
		});
	}

	const revealTargets = document.querySelectorAll('.reveal-on-scroll');
	if (revealTargets.length && !prefersReducedMotion) {
		const observer = new IntersectionObserver(entries => {
			entries.forEach(entry => {
				if (entry.isIntersecting) {
					entry.target.classList.add('is-visible');
					observer.unobserve(entry.target);
				}
			});
		}, {
			threshold: 0.15,
			rootMargin: '0px 0px -40px 0px'
		});

		revealTargets.forEach(target => observer.observe(target));
	}

	const parallaxNodes = document.querySelectorAll('[data-parallax]');
	if (parallaxNodes.length && !prefersReducedMotion) {
		const parallax = () => {
			const offset = window.scrollY * 0.08;
			parallaxNodes.forEach(node => {
				node.style.transform = `translate3d(0, ${offset * (node.dataset.parallax || 1)}px, 0)`;
			});
		};
		window.addEventListener('scroll', parallax, { passive: true });
	}

	const tiltCards = document.querySelectorAll('[data-tilt]');
	if (tiltCards.length && !prefersReducedMotion) {
		tiltCards.forEach(card => {
			card.addEventListener('mousemove', (event) => {
				const rect = card.getBoundingClientRect();
				const x = event.clientX - rect.left;
				const y = event.clientY - rect.top;
				const rotateY = ((x / rect.width) - 0.5) * 8;
				const rotateX = ((y / rect.height) - 0.5) * -8;
				card.style.transform = `perspective(900px) rotateX(${rotateX}deg) rotateY(${rotateY}deg)`;
			});

			card.addEventListener('mouseleave', () => {
				card.style.transform = 'perspective(900px) rotateX(0deg) rotateY(0deg)';
			});
		});
	}

	let hue = 0;
	if (!prefersReducedMotion) {
		setInterval(() => {
			hue = (hue + 1) % 360;
			document.documentElement.style.setProperty('--dynamic-hue', hue.toString());
		}, 120);
	}

	const backButton = document.querySelector('[data-back-button]');
	if (backButton) {
		backButton.addEventListener('click', () => {
			if (window.history.length > 1) {
				window.history.back();
			} else {
				window.location.href = '/';
			}
		});
	}
})();
