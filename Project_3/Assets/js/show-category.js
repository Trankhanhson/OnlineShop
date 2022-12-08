/**filter handle */
$(".block-price").click((e)=>{
    $(".block-price").removeClass("active")
    $(e.target).addClass("active")
    $(e.target).children("input").prop("checked", true);
})

//when click on default remove all check
$(".default-price").click((e)=>{
    $(".block-price").removeClass("active")
    $(".block-price").children("input").prop("checked", false);
})

$(".fillter-color").click((e)=>{
    $(".fillter-color").removeClass("active")
    $(e.target).addClass("active")
})

$(".default-color").click((e)=>{
    $(".fillter-color").removeClass("active")
})

$(".filter-size").click((e)=>{
    $(".filter-size").removeClass("active")
    $(e.target).addClass("active")
})

$(".default-size").click((e)=>{
    $(".filter-size").removeClass("active")

})

$(".fillter-btn").click((e)=>{
    $(".fillter-btn").hide()
    $(".fillter-content").show()
    //vì hai button phải dùng inline-block
    $(".fillter-close").css("display","inline-block")
})
$(".fillter-close").click((e)=>{
    $(".fillter-close").hide()
    $(".fillter-content").hide()
    $(".fillter-btn").css("display","inline-block")
})