/**category slider */
$('.category-slider').slick({
    slidesToShow: 5,
    slidesToScroll: 1,
    infinite:true,
    prevArrow:"<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [
        {
          breakpoint:768,
          settings: {
            slidesToShow: 3
          }
        }
      ]
  });