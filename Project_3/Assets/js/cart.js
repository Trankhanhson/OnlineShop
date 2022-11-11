/*hiển thị giá*/
function convertPrice(price) {
    let tg = "";
    let length = price.length;
    let count = 0;
    for (var i = length - 1; i >= 0; --i) {
        if (count % 3 == 0 && count != 0) {
            tg = price[i] + '.' + tg;
        }
        else {
            tg = price[i] + tg;
        }
        count++;
    }
    return tg + "đ";
}

/**change quantity of product first load*/
var quantityOfProduct = $("tbody").children().length
$(".quantity-product").text(quantityOfProduct)

/**remove item trong cart */
function deleteCartItem(event, proVariationId) {
    event.preventDefault()
    $.ajax({
        url: "/Cart/deleteCartItem/" + proVariationId,
        type: "GET",
        dataType: "Json"
    })
    let parent = $(event.target).parents("tr")
    $(parent).remove();
    //update quantity
    var quantityOfProduct = $("tbody").children().length
    $(".quantity-product").text(quantityOfProduct)
    updateTotalPrice()
}

 function updateQuantity(id, newQuantity) {
    $.ajax({
        url: "/Cart/updateCart",
        type: "POST",
        data: { proVariationId: id, newQuantity: newQuantity },
        dataType: "Json",
        success: function (response) {
            if (response.checkQuantity) {
                let newPrice = JSON.stringify(response.newPrice)
                const span = $($(`#proVariation_${id}`).find(".subtotal span"))
                $(span).text(convertPrice(newPrice)) //thực hiện gán vào subtotal
                $(span).attr("data", newPrice)
                updateTotalPrice()
                checkQty(true)
            }
            else {
                $("#errorToast .text-toast").text("Đã vượt quá số lượng tồn kho của sản phẩm này")
                $("#errorToast").toast("show")
                checkQty(false)
            }
        }
    })
}


function updateTotalPrice() {
    let listSubTotal = $(".subtotal span")
    let oriTotal = 0
    listSubTotal.each((index, value) => {
        oriTotal += JSON.parse($(value).attr("data"))
    })

    let textOriTotal = convertPrice(JSON.stringify(oriTotal))
    //update gia tri cho gia goc
    $(".original-price").text(textOriTotal)

    //update tong tien bill
    $(".total-price").text(textOriTotal)
    $(".total-price").attr("data", oriTotal)
}
updateTotalPrice()

function decreaseQunatity(event, id) {
    const input = $($(event.target).parent()).children("input")
    let quantityCurrent = JSON.parse($(input).val())
    if (quantityCurrent != 1) {
        let newQuantity = quantityCurrent - 1
        $.ajax({
            url: "/Cart/decreaseQty",
            type: "POST",
            data: { proVariationId: id, newQuantity: newQuantity },
            dataType: "Json",
            success: function (response) {
                let newPrice = JSON.stringify(response.newPrice)
                const span = $($(`#proVariation_${id}`).find(".subtotal span"))
                $(span).text(convertPrice(newPrice)) //thực hiện gán vào subtotal
                $(span).attr("data", newPrice)
                updateTotalPrice()
                $(input).val(quantityCurrent - 1)
            }
        })
    }
}

function increaseQuantity(event, id) {
    const input = $($(event.target).parent()).children("input")
    let quantityCurrent = JSON.parse($(input).val())
    let newQuantity = quantityCurrent + 1

    $.ajax({
        url: "/Cart/increaseQty",
        type: "POST",
        data: { proVariationId: id, newQuantity: newQuantity },
        dataType: "Json",
        success: function (response) {
            if (response.checkQuantity) {
                let newPrice = JSON.stringify(response.newPrice)
                const span = $($(`#proVariation_${id}`).find(".subtotal span"))
                $(span).text(convertPrice(newPrice)) //thực hiện gán vào subtotal
                $(span).attr("data", newPrice)
                updateTotalPrice()
                $(input).val(quantityCurrent + 1)
            }
            else {
                $("#errorToast .text-toast").text("Đã vượt quá số lượng tồn kho của sản phẩm này")
                $("#errorToast").toast("show")
            }
        }
    })
}