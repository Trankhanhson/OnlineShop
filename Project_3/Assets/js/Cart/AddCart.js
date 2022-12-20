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

/*loadMiniCart*/
function loadMiniCart() {
    if (localStorage.getItem("Cart") != null) {
        let list = JSON.parse(localStorage.getItem("Cart"))
        let string = ""
        for (let i = 0; i < list.length; i++) {
            let disPlay = ""
            if (list[i].DiscountPrice != 0) {
                disPlay = `<div class="minicart-item-price"><span class="price">${convertPrice(list[i].DiscountPrice)}</span></div>`
            }
            else {
                disPlay = `<div class="minicart-item-price"><span class="price">${convertPrice(list[i].Price)}</span></div>`
            }

            let displayPercent = ""
            if (list[i].Percent != 0) {
                displayPercent = `<div class="minicart-item-label">-${list[i].Percent}%</div>`
            }
            string += `<li class="minicart-item-${i} minicart-item">
                                <div class="minicart-item-info">
                                    <div class="minicart-item-photo">
                                        <div class="minicart-item-photo">
                                            <a href="" class="router-link-exact-active router-link-active">
                                                <img src="${list[i].Image}" alt="">
                                            </a>
                                            ${displayPercent}
                                        </div>
                                    </div>
                                    <div class="minicart-item-details">
                                        <h3 class="minicart-item-name">
                                            <a href="/Home/Detail/${list[i].ProId}" class="router-link-exact-active router-link-active">${list[i].ProName}</a>
                                        </h3>
                                        <div class="minicart-item-options">
                                            <div class="minicart-item-option"><span class="value">${list[i].proSizeName}</span></div>
                                            <div class="minicart-item-option">
                                                <span class="swatch-option" style="background-image: url(${list[i].srcColor});"></span>
                                            </div>
                                        </div>
                                        <div class="minicart-item-action"><a class="minicart-item-remove" onclick="deleteMiniCart(event,${i})"></a></div>
                                        <div class="minicart-item-bottom">
                                            ${disPlay}
                                            <span style="font-size:13px;margin-right:30px;">x${list[i].Quantity}</div>
                                        </div>
                                    </div>
                                </div>
                            </li>`
        }

        $(".minicart-items").html(string)
        $(".block-minicart-heading span").text(list.length)
        $(".header-cart__quantity").text(list.length)

        let blockMiniCart = $(".header-cart__inner")
        $(blockMiniCart).removeClass("cart-empty")
    }
    else {
        let blockMiniCart = $(".header-cart__inner")
        $(blockMiniCart).addClass("cart-empty")
        $(".header-cart__quantity").text(0)
    }
}

//update new product in it is changed
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
                loadMiniCart()
            }
        })
    }
    else {
        loadMiniCart()
    }
}
updateNewCart()

function deleteMiniCart(event, index) {
    event.stopPropagation();
    let list = JSON.parse(localStorage.getItem("Cart"))
    if (list.length > 1) {
        list.splice(index, 1)
        localStorage.setItem("Cart", JSON.stringify(list))
        $(".block-minicart-heading span").text(list.length)
        $(".header-cart__quantity").text(list.length)
    }
    else {
        localStorage.removeItem("Cart") //xóa luôn Cart tránh trường hợp lưu mảng rỗng vào Cart gây khó check
        $(".block-minicart-heading span").text(0)
        $(".header-cart__quantity").text(0)
    }
    $(`.minicart-item-${index}`).remove()
}

/*ADD CART*/
function addCart(input) {
    let parentProduct = $(input).parents(".product")
    let dataProduct = JSON.parse($(parentProduct).attr("data"))
    let wrapImage = $(input).parents(".wrap-image")
    let CartItem = {
        ProId: dataProduct.ProId,
        ProName: dataProduct.ProName,
        Price: dataProduct.Price,
        DiscountPrice: dataProduct.DiscountPrice,
        Percent: dataProduct.Percent,
        proSizeId: $($(wrapImage).find(".product-size.active")).data("idsize"),
        proSizeName: $($(wrapImage).find(".product-size.active")).text(),
        proColorId: $($(wrapImage).find(".product-color.active")).data("idcolor"),
        srcColor: $($(wrapImage).find(".product-color.active span")).data("src"),
        Image: $($(wrapImage).find(".product-img img")).attr("src"),
        Quantity: 1
    }

    if (CartItem.proSizeId != undefined) {
        //lấy thông tin trong local
        let listCartItem = []
        listCartItem = JSON.parse(localStorage.getItem("Cart"))
        if (listCartItem != null) {
            let indexItem = 5
            let check = listCartItem.some(function (value, index, array) {
                indexItem = index
                return value.ProId == CartItem.ProId && value.proSizeId == CartItem.proSizeId && value.proColorId == CartItem.proColorId
            })

            //nếu sản phẩm đã tồn tại trong giỏ hàng thì cộng lên 1
            if (check) {
                $.ajax({
                    url: "/Cart/CheckQuantity",
                    type: "POST",
                    data: { ProId: CartItem.ProId, ProColorId: CartItem.proColorId, ProSizeId: CartItem.proSizeId, newQuantity: listCartItem[indexItem].Quantity + 1 },
                    dataType: "Json",
                    success: function (res) {
                        if (res) {
                            listCartItem[indexItem].Quantity += 1;
                            localStorage.setItem("Cart", JSON.stringify(listCartItem))
                            alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
                        }
                        else {
                            alertError("Đã vượt quá số lượng tồn kho của sản phẩm này")
                        }
                    }
                })
            }
            else {
                listCartItem.push(CartItem)
                localStorage.setItem("Cart", JSON.stringify(listCartItem))
                alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
            }
        }
        else {
            listCartItem = [CartItem]
            localStorage.setItem("Cart", JSON.stringify(listCartItem))
            alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
        }
        loadMiniCart()
    }
    else {
        alertError("Bạn chưa chọn kích thước")
    }
}

//add cart từ detail
function addCartFromDetail() {
    let parent = $("main")
    let dataProduct = JSON.parse($(parent).attr("data"))
    let CartItem = {
        ProId: dataProduct.ProId,
        ProName: dataProduct.ProName,
        Price: dataProduct.Price,
        DiscountPrice: dataProduct.DiscountPrice,
        Percent: dataProduct.Percent,
        proSizeId: $("#detail-section .product-size.active").attr("data-idSize"),
        proSizeName: $("#detail-section .product-size.active").text(),
        proColorId: $("#detail-section .product-color.active").attr("data-idColor"),
        srcColor: $($(parent).find("#detail-section .product-color.active")).attr("data-srcColor"),
        Image: $($(parent).find("#detail-section .product-img img")).attr("src"),
        Quantity: 1
    }

    if (CartItem.proSizeId != undefined) {
        //lấy thông tin trong local
        let listCartItem = []
        listCartItem = JSON.parse(localStorage.getItem("Cart"))
        if (listCartItem != null) {
            let indexItem = 5
            let check = listCartItem.some(function (value, index, array) {
                indexItem = index
                return value.ProId == CartItem.ProId && value.proSizeId == CartItem.proSizeId && value.proColorId == CartItem.proColorId
            })

            //nếu sản phẩm đã tồn tại trong giỏ hàng thì cộng lên 1
            if (check) {
                $.ajax({
                    url: "/Cart/CheckQuantity",
                    type: "POST",
                    data: { ProId: CartItem.ProId, ProColorId: CartItem.proColorId, ProSizeId: CartItem.proSizeId, newQuantity: listCartItem[indexItem].Quantity + 1 },
                    dataType: "Json",
                    success: function (res) {
                        if (res) {
                            listCartItem[indexItem].Quantity += 1;
                            localStorage.setItem("Cart", JSON.stringify(listCartItem))
                            alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
                        }
                        else {
                            alertError("Đã vượt quá số lượng tồn kho của sản phẩm này")
                        }
                    }
                })
            }
            else {
                listCartItem.push(CartItem)
                localStorage.setItem("Cart", JSON.stringify(listCartItem))
                alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
            }
        }
        else {
            listCartItem = [CartItem]
            localStorage.setItem("Cart", JSON.stringify(listCartItem))
            alertSuccess("Đã thêm sản phẩm vào giỏ hàng")
        }
        loadMiniCart()

    }
    else {
        alertError("Bạn chưa chọn kích thước")
    }

}