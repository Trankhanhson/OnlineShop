
/**product slider home*/
$('.product-slider').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    infinite: true,
    prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow: "<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 2
            }
        }
    ]
});

/**product slider */
$('.product-banner-slider').slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 2000,
    arrows: false,
    dots: true
});

/**topbar  */
$(".topbar").slick({
    slidesToShow: 1,
    slidesToScroll: 1,
    infinite: true,
    arrows: false,
    vertical: true,
    autoplay: true,
    autoplaySpeed: 2500
})

/**product-related */
$('.product-related').slick({
    slidesToShow: 4,
    slidesToScroll: 1,
    infinite: true,
    prevArrow: "<button type='button' class='slick-prev pull-left'><i class='fa fa-angle-left' aria-hidden='true'></i></button>",
    nextArrow: "<button type='button' class='slick-next pull-right'><i class='fa fa-angle-right' aria-hidden='true'></i></button>",
    responsive: [
        {
            breakpoint: 1200,
            settings: {
                slidesToShow: 3,
            }
        },
        {
            breakpoint: 768,
            settings: {
                slidesToShow: 1,
                arrows: false,
            }
        }
    ]
});

/** element muốn thêm active khi khi click và remove khi click ra ngoài*/

$(`.header-form-wrap`).click((e) => {
    $(`.header-form-wrap`).addClass("active")
    e.stopPropagation()
})

$(`.sidebar-form-wrap`).click((e) => {
    $(`.sidebar-form-wrap`).addClass("active")
    e.stopPropagation()
})

$("body").click((e) => {
    $(`.header-form-wrap`).removeClass("active")
    $(`.sidebar-form-wrap`).removeClass("active")
})

function changeProduct(e) {
    let parent = $(e.target).parents(".product")
    let colorId = $(e.target).attr("data-idColor")
    let listImage = $(parent).find(".wrap-image")
    $(listImage).removeClass("active")
    listImage.each((index, value) => {
        let id = JSON.parse($(value).attr("data-id"))
        if (id == colorId) {
            $(value).addClass("active")
        }
    })
}

function addActive(e) {
    let parent = $(e.target).parent()
    let listItem = $(parent).children()
    $(listItem).removeClass("active")
    $(e.target).addClass("active")
}


/**toast */

function alertSuccess(message) {
    var toastElList = [].slice.call(document.querySelectorAll('.toast-successClient'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })
    $(".toast-successClient p").text(message)
    toastList.forEach(toast => toast.show())
}
function alertError(message) {
    var toastElList = [].slice.call(document.querySelectorAll('.toast-errorClient'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })
    $(".toast-errorClient p").text(message)
    toastList.forEach(toast => toast.show())
}


$(".product-like").click((e) => {
    var toastElList = [].slice.call(document.querySelectorAll('.toast-like'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })

    if ($(e.target).hasClass("active")) {
        $(".toast-like p").text("Sản phẩm đã được xóa khỏi danh sách yêu thích!")
    }
    else {
        $(".toast-like p").text("Sản phẩm đã được thêm vào danh sách yêu thích!")
    }

    toastList.forEach(toast => toast.show())

})


/**product-like */
$(".product-like").click((e) => {
    $(e.target).toggleClass("active")
})

/*hiển thị giá*/
$(".price").each((index, value) => {
    let tg = "";
    let textPrice = $(value).text();
    let length = textPrice.length;
    let count = 0;
    for (var i = length - 1; i >= 0; --i) {
        if (count % 3 == 0 && count != 0) {
            tg = textPrice[i] + '.' + tg;
        }
        else {
            tg = textPrice[i] + tg;
        }
        count++;
    }
    $(value).text(tg + "đ")
})

/**loại các size bị trung trên giao diện*/
/**cách dùng : đưa class filter-sameValue và thẻ bọc các size và class filter-value vào các thẻ có data-idSize*/
var parentSizes = $(".filter-sameValue") //danh sách các element chứa các size
parentSizes.each((index, wrapSize) => {
    let listSize = $(wrapSize).find(".filter-value") //danh sách size
    let newList = [] //chứa list idSize đã được filter
    listSize.each((index1, sizeElement) => {
        let idSize = $(sizeElement).attr("data-idSize")

        //nếu idSize chưa tồn tại trong newList
        if (newList.indexOf(idSize) == -1) {
            newList.push(idSize)
        }
        else {
            $(sizeElement).remove() //xóa phần element nếu đã tồn tại
        }
    })
})


//Add class active vào phần tử đầu tiền
var parentAddActives = $(".wrapActiveToFirstChild")
parentAddActives.each((index, wrapItem) => {
    let listValue = $(wrapItem).find(".addActiveItem")
    $(listValue[0]).addClass("active")
})



