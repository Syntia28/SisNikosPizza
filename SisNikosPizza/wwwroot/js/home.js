// Configuración y detección de características
const AppConfig = {
    supportsIntersectionObserver: 'IntersectionObserver' in window,
    supportsRequestIdleCallback: 'requestIdleCallback' in window,
    prefersReducedMotion: window.matchMedia('(prefers-reduced-motion: reduce)').matches
};

// Utilidades
const Utils = {
    debounce: function (func, wait) {
        let timeout;
        return function (...args) {
            clearTimeout(timeout);
            timeout = setTimeout(() => func(...args), wait);
        };
    },

    addClass: function (el, cls) {
        if (el && !el.classList.contains(cls)) el.classList.add(cls);
    },

    removeClass: function (el, cls) {
        if (el && el.classList.contains(cls)) el.classList.remove(cls);
    }
};

// Animaciones de scroll
const ScrollAnimations = {
    init: function () {
        if (!AppConfig.supportsIntersectionObserver || AppConfig.prefersReducedMotion) return;

        const observer = new IntersectionObserver((entries, observer) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    setTimeout(() => {
                        Utils.addClass(entry.target, 'animate-fade-in');
                    }, Math.random() * 300);
                    observer.unobserve(entry.target);
                }
            });
        }, {
            root: null,
            rootMargin: '0px 0px -10% 0px',
            threshold: 0.1
        });

        document.querySelectorAll('.pizza-card-nikos').forEach(el => observer.observe(el));
    }
};

// Navegación suave
const SmoothScroll = {
    init: function () {
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', e => {
                e.preventDefault();
                const target = document.querySelector(anchor.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({ behavior: 'smooth', block: 'start' });
                    target.setAttribute('tabindex', '-1');
                    target.focus();
                }
            });
        });
    }
};

// Interacción con tarjetas
const CardInteractivity = {
    init: function () {
        document.querySelectorAll('.pizza-card-nikos').forEach(card => {
            card.setAttribute('tabindex', '0');

            card.addEventListener('keydown', e => {
                if (e.key === 'Enter' || e.key === ' ') {
                    e.preventDefault();
                    card.click();
                }
            });

            card.addEventListener('click', () => {
                const pizzaType = card.getAttribute('data-pizza');
                const pizzaName = card.querySelector('h3')?.textContent;
                console.log('Pizza seleccionada:', pizzaName || pizzaType);

                // window.location.href = `/Producto/Details/${pizzaType}`;
                // o usar AJAX...
            });
        });
    }
};

// Optimización de imágenes
const ImageOptimization = {
    init: function () {
        document.addEventListener('error', function (e) {
            if (e.target.tagName === 'IMG') {
                //console.warn('Error al cargar imagen:', e.target.src);
                //e.target.src = '/Imagenes/placeholder.webp';
            }
        }, true);

        if (window.innerWidth > 768) {
            this.preloadCriticalImages();
        }
    },

    preloadCriticalImages: function () {
        const criticalImages = [
            '/Imagenes/nikos.webp',
            '/Imagenes/pepenori.webp'
        ];
        criticalImages.forEach(src => {
            const img = new Image();
            img.src = src;
        });
    }
};

// Animación del título
const TitleAnimation = {
    init: function () {
        if (AppConfig.prefersReducedMotion) return;

        const titleEl = document.querySelector('.bienvenida-letras');
        if (titleEl) {
            titleEl.style.opacity = '0';
            titleEl.style.transform = 'translateY(-20px)';
            titleEl.style.transition = 'opacity 0.6s ease, transform 0.6s ease';

            requestAnimationFrame(() => {
                titleEl.style.opacity = '1';
                titleEl.style.transform = 'translateY(0)';
            });
        }
    }
};

// Inicialización
const App = {
    init: function () {
        SmoothScroll.init();
        CardInteractivity.init();
        ImageOptimization.init();
        TitleAnimation.init();

        const startScrollAnimations = () => ScrollAnimations.init();

        if (AppConfig.supportsRequestIdleCallback) {
            requestIdleCallback(startScrollAnimations);
        } else {
            setTimeout(startScrollAnimations, 100);
        }
    }
};

// Iniciar al cargar
window.addEventListener('DOMContentLoaded', () => {
    App.init();
});
