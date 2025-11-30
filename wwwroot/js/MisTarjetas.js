document.addEventListener('DOMContentLoaded', () => {
    const toggle = document.querySelector('.credit-toggle');
    const panel = document.querySelector('.credit-panel-body');
    const amexCard = document.querySelector('[data-card]');
    const eyeToggle = document.querySelector('[data-eye-toggle]');

    if (toggle && panel) {
        toggle.addEventListener('click', () => {
            const expanded = toggle.getAttribute('aria-expanded') === 'true';
            toggle.setAttribute('aria-expanded', String(!expanded));
            panel.hidden = expanded;
        });
    }

    if (amexCard) {
    const numberEl = amexCard.querySelector('[data-field="number"]');
    const expiryEl = amexCard.querySelector('[data-field="expiry"]');
    const cvvEl = amexCard.querySelector('[data-field="cvv"]');
    const frontFace = amexCard.querySelector('.card-face-front');
    const backFace = amexCard.querySelector('.card-face-back');
        const eyeIcon = eyeToggle ? eyeToggle.querySelector('[data-eye-icon]') : null;

        const setSensitiveState = (revealed) => {
            numberEl.textContent = revealed
                ? amexCard.dataset.numberFull
                : amexCard.dataset.numberMask;
            expiryEl.textContent = revealed
                ? amexCard.dataset.expFull
                : amexCard.dataset.expMask;
            cvvEl.textContent = revealed
                ? amexCard.dataset.cvvFull
                : amexCard.dataset.cvvMask;

            amexCard.classList.toggle('is-revealed', revealed);

            if (eyeToggle) {
                eyeToggle.setAttribute('aria-pressed', String(revealed));
                eyeToggle.setAttribute('title', revealed ? 'Ocultar datos sensibles' : 'Mostrar datos sensibles');
            }

            if (eyeIcon) {
                eyeIcon.classList.remove('ri-eye-line', 'ri-eye-off-line');
                eyeIcon.classList.add(revealed ? 'ri-eye-line' : 'ri-eye-off-line');
            }
        };

        if (eyeToggle) {
            eyeToggle.addEventListener('click', () => {
                const revealed = eyeToggle.getAttribute('aria-pressed') === 'true';
                setSensitiveState(!revealed);
            });
        }

        const setFlipState = (flipped) => {
            amexCard.classList.toggle('is-flipped', flipped);
            amexCard.setAttribute('aria-pressed', String(flipped));

            if (frontFace) {
                frontFace.setAttribute('aria-hidden', String(flipped));
            }
            if (backFace) {
                backFace.setAttribute('aria-hidden', String(!flipped));
            }
        };

        const handleFlip = () => {
            const flipped = !amexCard.classList.contains('is-flipped');
            setFlipState(flipped);
        };

        amexCard.addEventListener('click', handleFlip);
        amexCard.addEventListener('keypress', (event) => {
            if (event.key === 'Enter' || event.key === ' ') {
                event.preventDefault();
                handleFlip();
            }
        });

        setSensitiveState(false);
        setFlipState(false);
    }
});
