/**change quantity of product first load*/
var quantityOfProduct = $("tbody").children().length
$(".quantity-product").text(quantityOfProduct)

/**remove item trong cart */
$(".cart-item__remove").click((e)=>{
    let item = $(e.target).parent().parent()
    $(item).remove()
    //update quantity
    var quantityOfProduct = $("tbody").children().length
    $(".quantity-product").text(quantityOfProduct)
})
