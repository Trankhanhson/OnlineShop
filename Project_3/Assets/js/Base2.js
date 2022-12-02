
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


/**submit form header */
//var historys = JSON.parse(localStorage.getItem('historySearch')) || []

///**load history */
//function loadHistory() {
//    let bodyHistory = ""
//    for (var h of historys) {
//        bodyHistory += `<span>${h}</span>`
//    }
//    $(".search-history-body").html(bodyHistory)
//}
//loadHistory()

///**add history */
//function addHistory(item) {
//    historys.push(item)
//    localStorage.setItem('historySearch', JSON.stringify(historys))
//}

///*submit form header*/
//$(".sidebar-form").submit((e) => {
//    let info = $("#input-search").val()
//    addHistory(info)
//    loadHistory();
//})

///**delete history */
//$(".search-history__header span").click((e) => {
//    localStorage.clear()
//    historys = [];
//    loadHistory();
//})
///**click in history */
//$(".search-history-body").each(function (index, value) {
//    $(value).click((e) => {
//        let historyClick = $(this).text()
//        $("#input-search").val(historyClick)
//    })
//})

/**cart handle */


/**modal */
/**click login */
//$(".btn-login").click((e) => {
//    $(".btn-login").addClass("active")
//    $($(".btn-register")).removeClass("active")
//    $(".modal-title").text("Cảm ơn bạn đã trở lại.")
//})

///**click dang ky */
//$(".btn-register").click((e) => {
//    $(".modal-title").text("Đăng ký để Canifa có cơ hội phục vụ bạn tốt hơn.")
//    $(".btn-register").addClass("active")
//    $(".btn-login").removeClass("active")
//})


/**product handle */

//product list color active
$(".product-color").each(function (index, value) {
    $(value).click((e) => {

        let idGrandfather = $(value).parent().parent().attr("id")

        //change border
        $(`#${idGrandfather} .product-color`).removeClass("active")
        $(value).addClass("active")

        //change image of product
        let src = $(value).attr("data")
        $(`#${idGrandfather} .product-img img`).attr("src", src)
    })
})
function changeImgProduct(e) {
    if ($(e.target).hasClass("product-color")) {
        let parent = $(e.target).parents(".product")

        //change border
        $($(parent).find(".product-color")).removeClass("active")
        $(e.target).addClass("active")

        //change image of product
        let src = $(e.target).attr("data")
        $($(parent).find(".product-img img")).attr("src", src)
    }
}

/**product size click */
$(".product-size").click((e) => {
    const size = $(e.target).text()
    $(".product-size").removeClass("active")
    $(e.target).addClass("active")
})

/**product display size */
$(".product-open-size").click((e) => {
    $(".product-size-wrap").css("display", "block")
})


/**toast */

$(".product-btn-addCart").click((e) => {
    var toastElList = [].slice.call(document.querySelectorAll('.toast-addCart'))
    var toastList = toastElList.map(function (toastEl) {
        return new bootstrap.Toast(toastEl)
    })
    toastList.forEach(toast => toast.show())
})


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



