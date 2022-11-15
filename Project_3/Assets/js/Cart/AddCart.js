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
                                            <a href="/vay-lien-be-gai-1ds22w012" class="router-link-exact-active router-link-active">${list[i].ProName}</a>
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

loadMiniCart()

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
    let parent = $(input).parents(".product")
    let dataProduct = JSON.parse($(parent).attr("data"))
    let CartItem = {
        ProId: dataProduct.ProId,
        ProName: dataProduct.ProName,
        Price: dataProduct.Price,
        DiscountPrice: dataProduct.DiscountPrice,
        Percent: dataProduct.Percent,
        proSizeId: $($(parent).find(".product-size.active")).data("idsize"),
        proSizeName: $($(parent).find(".product-size.active")).text(),
        proColorId: $($(parent).find(".product-color.active")).data("idcolor"),
        srcColor: $($(parent).find(".product-color.active span")).data("src"),
        Image: $($(parent).find(".product-img img")).attr("src"),
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
                            $("#successToast .text-toast").text("Đã thêm sản phẩm vào giỏ hàng")
                            $("#successToast").toast("show")
                        }
                        else {
                            $("#errorToast .text-toast").text("Đã vượt quá số lượng tồn kho của sản phẩm này")
                            $("#errorToast").toast("show")
                        }
                    }
                })
            }
            else {
                listCartItem.push(CartItem)
                localStorage.setItem("Cart", JSON.stringify(listCartItem))
                $("#successToast .text-toast").text("Đã thêm sản phẩm vào giỏ hàng")
                $("#successToast").toast("show")
            }
        }
        else {
            listCartItem = [CartItem]
            localStorage.setItem("Cart", JSON.stringify(listCartItem))
        }
        loadMiniCart()
    }
    else {
        $("#errorToast .text-toast").text("Bạn chưa chọn kích thước")
        $("#errorToast").toast("show")
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
        proSizeId: $(".product-size.active").attr("data-idSize"),
        proSizeName: $(".product-size.active").text(),
        proColorId: $(".product-color.active").attr("data-idColor"),
        srcColor: $($(parent).find(".product-color.active")).attr("data-srcColor"),
        Image: $($(parent).find(".product-img img")).attr("src"),
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
                            $("#successToast .text-toast").text("Đã thêm sản phẩm vào giỏ hàng")
                            $("#successToast").toast("show")
                        }
                        else {
                            $("#errorToast .text-toast").text("Đã vượt quá số lượng tồn kho của sản phẩm này")
                            $("#errorToast").toast("show")
                        }
                    }
                })
            }
            else {
                listCartItem.push(CartItem)
                localStorage.setItem("Cart", JSON.stringify(listCartItem))
                $("#successToast .text-toast").text("Đã thêm sản phẩm vào giỏ hàng")
                $("#successToast").toast("show")
            }
        }
        else {
            listCartItem = [CartItem]
            localStorage.setItem("Cart", JSON.stringify(listCartItem))
        }
        loadMiniCart()
    }
    else {
        $("#errorToast .text-toast").text("Bạn chưa chọn kích thước")
        $("#errorToast").toast("show")
    }

}