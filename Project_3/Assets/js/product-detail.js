//change img
$('.list-img-item').click((e)=>{
	var src = $(e.target).attr('src')
	$(document.querySelector(".main-img")).attr('src',src)
})

/**zoom img */
$(".main-img").mousemove((e)=>{
	const x = e.pageX - e.target.offsetLeft
	const y = e.pageY - e.target.offsetTop
	e.target.style.setProperty('--x', `${ x }px`)
	e.target.style.setProperty('--y', `${ y }px`)
})


/**product size click */
$(".product-size").click((e)=>{
	const size = $(e.target).text()
	$(".size-choose").text(size)
	$(".product-size").removeClass("active")
	$(e.target).addClass("active")
})

/**click on list img slide */
$(".list-img").slick({
	slidesToShow: 4,
    slidesToScroll: 1,
    infinite:true,
	arrows:false,
	vertical:true,
	focusOnSelect: true,
	topMode:true
})


//color click
let listColorOption = $(".detail-color")
listColorOption.click((e) => {
    $(listColorOption).removeClass("active")
    $(e.target).addClass("active")

    //??y ?nh lên
    let listImageClicked = JSON.parse($(e.target).attr("data-Images")) //danh sách link ?nh ? th? v?a ???c click

    //Vì dùng slick slider s? ren ra nhi?u th? img ch? không ph?i ch? 6 th?
    let listImage = $(".list-img-item")
    listImage.each((index, value) => {
        if ($(value).hasClass("MainImage")) {
            $(value).attr('src', listImageClicked.MainImage)
        }
        else if ($(value).hasClass("Detail1")) {
            $(value).attr('src', listImageClicked.Detail1)
        }
        else if ($(value).hasClass("Detail2")) {
            $(value).attr('src', listImageClicked.Detail2)
        }
        else if ($(value).hasClass("Detail3")) {
            $(value).attr('src', listImageClicked.Detail3)
        }
        else if ($(value).hasClass("Detail4")) {
            $(value).attr('src', listImageClicked.Detail4)
        }
        else if ($(value).hasClass("Detail5")) {
            $(value).attr('src', listImageClicked.Detail5)
        }
    })

    //Thay ??i ?nh to
    $(".main-img").attr('src', listImageClicked.MainImage)
})

function changeImageDetail(e, idColor) {
    $(" .product-size").removeClass("active")
    $(".detail-color").removeClass("active")
    $(e.target).addClass("active")
    $(".color-chose").text($(e.target).attr("data-nameColor"))

    //change size box
    let listSizeBox = $(".wrapSizeOfColor")
    $(listSizeBox).removeClass("active")
    $(`.wrapSizeOfColor-${idColor}`).addClass("active")

    //??y ?nh lên
    let listImageClicked = JSON.parse($(e.target).attr("data-Images")) //danh sách link ?nh ? th? v?a ???c click

    //Vì dùng slick slider s? ren ra nhi?u th? img ch? không ph?i ch? 6 th?
    let listImage = $(".list-img-item")
    listImage.each((index, value) => {
        if ($(value).hasClass("MainImage")) {
            $(value).attr('src', listImageClicked.MainImage)
        }
        else if ($(value).hasClass("Detail1")) {
            $(value).attr('src', listImageClicked.Detail1)
        }
        else if ($(value).hasClass("Detail2")) {
            $(value).attr('src', listImageClicked.Detail2)
        }
        else if ($(value).hasClass("Detail3")) {
            $(value).attr('src', listImageClicked.Detail3)
        }
        else if ($(value).hasClass("Detail4")) {
            $(value).attr('src', listImageClicked.Detail4)
        }
        else if ($(value).hasClass("Detail5")) {
            $(value).attr('src', listImageClicked.Detail5)
        }
    })

    //Thay ??i ?nh to
    $(".main-img").attr('src', listImageClicked.MainImage)
}