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



