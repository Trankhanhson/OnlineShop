$('.list-video').slick({
    slidesToShow: 4,
    slidesToScroll: 4,
    infinite:true,
    prevArrow:"<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [
        {
          breakpoint:768,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2
          }
        }
      ]
  });
  $('.list-news').slick({
    slidesToShow: 3,
    slidesToScroll: 3,
    infinite:true,
    prevArrow:"<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow:"<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [
        {
          breakpoint:768,
          settings: {
            slidesToShow: 2,
            slidesToScroll: 2
          }
        }
      ]
  });