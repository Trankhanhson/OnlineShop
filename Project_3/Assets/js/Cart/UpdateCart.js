/*hiển thị giá*/
function convertPrice(priceInput) {
    let price = ""
    if (typeof priceInput == "number") {
        price = JSON.stringify(priceInput)
    }
    else {
        price = priceInput
    }
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

/*Load cart*/
function loadCart() {
    if (localStorage.getItem("Cart") != null) {
        let list = JSON.parse(localStorage.getItem("Cart"))
        let string = ""
        for (let i = 0; i < list.length; i++) {
            let subtotal = 0;
            let disPlay = ""
            if (list[i].DiscountPrice != 0) {
                subtotal = list[i].DiscountPrice * list[i].Quantity
                disPlay = `<span class="price" data="${list[i].DiscountPrice}">${convertPrice(list[i].DiscountPrice)}</span><span class="old-price">${convertPrice(list[i].Price)}</span>`
            }
            else {
                subtotal = list[i].Price * list[i].Quantity
                disPlay = `<span class="price" data="${list[i].Price}">${convertPrice(list[i].Price)}</span>`
            }

            string += `<tr id="proVariation_${i}">
                                    <td class="col item">
                                        <div class="d-flex">
                                            <a href="#">
                                                <img class="img-item" src="${list[i].Image}" alt="">
                                            </a>
                                            <div style="width:100%" class="flex-grow-1">
                                                <a href="" class="item-name">${list[i].ProName}</a>
                                                <div class="d-flex align-items-center">
                                                    <span class="item-size" data-sizeid="${list[i].proSizeId}" >${list[i].proSizeName}</span>
                                                    <span>&nbsp;/&nbsp;</span>
                                                    <span data-colorid="${list[i].proColorId}" class="item-color rounded-circle d-inline-block" style="width:14px; height:14px;background-image:url(${list[i].srcColor}) ;"></span>
                                                </div>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="col priceCartItem">
                                       
                                        ${disPlay}
                                    </td>
                                    <td class="col qty">
                                        <div class="d-flex justify-content-start align-items-center quantity-wrap">
                                            <span class="btn-minus" onclick="decreaseQuantity(event,${i})"></span>
                                            <input type="text" readonly="readonly" class="input-cart-qty" value="${list[i].Quantity}">
                                            <span class="btn-plus" onclick="increaseQuantity(event,${i})"></span>
                                        </div>
                                    </td>
                                    <td class="col subtotal">
                                        <span class="price" data="${subtotal}">${convertPrice(JSON.stringify(subtotal))}</span>
                                    </td>
                                    <td class="actions">
                                        <a class="cart-item__remove" onclick="deleteCartItem(event,${i})"></a>
                                    </td>
                                </tr>`
        }

        $(".table-cart tbody").html(string) //thêm các phần tử vào table
        $(".quantity-product").text(list.length) //hiển thị số lượng phần tử
    }
    else {
        let btnOrder = $(".btn-order")
        $(btnOrder).css({ "opacity": "0.7", "pointerEvents": "none" })
        $(".table-cart tbody").html('') //thêm các phần tử vào table
        $(".quantity-product").text(0) //hiển thị số lượng phần tử
    }
}

/**remove item trong cart */
function deleteCartItem(event, index) {
    event.preventDefault()
    let list = JSON.parse(localStorage.getItem("Cart"))
    if (list.length == 1) {
        localStorage.removeItem("Cart") //xóa luôn Cart tránh trường hợp lưu mảng rỗng vào Cart gây khó check
    }
    else {
        list.splice(index, 1)
        localStorage.setItem("Cart", JSON.stringify(list))
    }
    loadCart()
    updateTotalPrice()
}

function updateTotalPrice() {
    let originPrice = 0;
    let totalPrice = 0
    if (localStorage.getItem("Cart") != null) {
        let list = JSON.parse(localStorage.getItem("Cart"))
        for (let i = 0; i < list.length; i++) {
            let Price = JSON.parse(list[i].Price)
            let DiscountPrice = JSON.parse(list[i].DiscountPrice)
            let Quantity = JSON.parse(list[i].Quantity)
            originPrice += (Price * Quantity)

            if (DiscountPrice != 0) {
                totalPrice += (DiscountPrice * Quantity)
            }
            else {
                totalPrice += (Price * Quantity)
            }
        }
    }

    let totaldisCountPrice = originPrice - totalPrice;
    let textOriTotal = convertPrice(JSON.stringify(originPrice))
    let textDiscountTotal = convertPrice(JSON.stringify(totaldisCountPrice))
    let textTotalPrice = convertPrice(JSON.stringify(totalPrice))
    //update gia tri cho gia goc
    $(".original-price").text(textOriTotal)
    $(".discount-total").text("-" + textDiscountTotal)
    $(".total-price").text(textTotalPrice)

    $(".discount-total").attr("data", totaldisCountPrice)
    $(".total-price").attr("data", totalPrice)
}

function decreaseQuantity(event, index) {
    const input = $($(event.target).parent()).children("input")
    let quantityCurrent = JSON.parse($(input).val())
    if (quantityCurrent > 1) {
        //lấy thông tin giỏ hàng và thay đổi
        let list = JSON.parse(localStorage.getItem("Cart"))
        list[index].Quantity = quantityCurrent - 1
        localStorage.setItem("Cart", JSON.stringify(list))

        //load lại subtotal
        const span = $($(`#proVariation_${index}`).find(".subtotal span"))
        let newSubtotal = 0;
        if (list[index].DiscountPrice != 0) {
            newSubtotal = list[index].DiscountPrice * list[index].Quantity
        }
        else {
            newSubtotal = list[index].Quantity * list[index].Price
        }

        $(span).text(convertPrice(JSON.stringify(newSubtotal)))
        $(span).attr("data", newSubtotal)
        $(input).val(quantityCurrent - 1) //thay dổi ở thẻ input

        updateTotalPrice()
    }
}

function increaseQuantity(event, index) {
    const input = $($(event.target).parent()).children("input")
    let quantityCurrent = JSON.parse($(input).val())
    let newQuantity = quantityCurrent + 1

    //Lấy thông tin từ local
    let list = JSON.parse(localStorage.getItem("Cart"))
    $.ajax({
        url: "/Cart/CheckQuantity",
        type: "POST",
        data: { ProId: list[index].ProId, ProColorId: list[index].proColorId, ProSizeId: list[index].proSizeId, newQuantity: newQuantity },
        dataType: "Json",
        success: function (res) {
            if (res) {
                //khi trong kho vẫn còn
                list[index].Quantity = newQuantity
                localStorage.setItem("Cart", JSON.stringify(list))

                //load lại subtotal
                const span = $($(`#proVariation_${index}`).find(".subtotal span"))
                let newSubtotal = 0;
                if (list[index].DiscountPrice != 0) {
                    newSubtotal = list[index].DiscountPrice * list[index].Quantity
                }
                else {
                    newSubtotal = list[index].Quantity * list[index].Price
                }

                $(span).text(convertPrice(JSON.stringify(newSubtotal)))
                $(span).attr("data", newSubtotal)
                $(input).val(newQuantity) //thay dổi ở thẻ input

                updateTotalPrice()
            }
            else {
                $("#errorToast .text-toast").text("Đã vượt quá số lượng tồn kho của sản phẩm này")
                $("#errorToast").toast("show")
            }
        }
    })
}

//đồng bộ cart ở local và ở serverside
function updateNewCart() {
    if (localStorage.getItem("Cart") != null) {
        let list = JSON.parse(localStorage.getItem("Cart"))
        let variationCarts = []
        $.each(list, (index, value) => {
            let v = {
                ProId: value.ProId,
                proSizeId: value.proSizeId,
                proColorId: value.proColorId,
                Quantity: value.Quantity
            }
            variationCarts.push(v)
        })

        $.ajax({
            url: "/Cart/getNewCart",
            dataType: "Json",
            type: "Post",
            data: { variationCarts: variationCarts },
            success: function (res) {
                localStorage.setItem("Cart", res)
                loadCart()
                updateTotalPrice()
            }
        })
    }
    else {
        loadCart()
    }
}
updateNewCart()