document.addEventListener('DOMContentLoaded', function () {
    var carousel = document.getElementById('carousel');
    var carouselItems = carousel.querySelectorAll('.carousel-item');
    var carouselPrev = carousel.querySelector('.carousel-prev');
    var carouselNext = carousel.querySelector('.carousel-next');

    carouselPrev.addEventListener('click', function (e) {
        e.preventDefault();
        var activeItem = carousel.querySelector('.carousel-item.active');
        activeItem.classList.remove('active');
        if (activeItem.previousElementSibling) {
            activeItem.previousElementSibling.classList.add('active');
        } else {
            carouselItems[carouselItems.length - 1].classList.add('active');
        }
    });

    carouselNext.addEventListener('click', function (e) {
        e.preventDefault();
        var activeItem = carousel.querySelector('.carousel-item.active');
        activeItem.classList.remove('active');
        if (activeItem.nextElementSibling) {
            activeItem.nextElementSibling.classList.add('active');
        } else {
            carouselItems[0].classList.add('active');
        }
    });
});