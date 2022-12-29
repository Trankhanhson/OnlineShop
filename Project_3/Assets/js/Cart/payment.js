

//handle countdown timer
function checkSecond(sec) {
    if (sec < 10 && sec >= 0) { sec = "0" + sec }; // add zero in front of numbers < 10
    if (sec < 0) { sec = "59" };
    return sec;
}

function startTimer() {
    var presentTime = document.getElementById('timer').innerHTML;
    var timeArray = presentTime.split(/[:]+/);
    var m = timeArray[0];
    var s = checkSecond((timeArray[1] - 1));
    if (s == 59) { m = m - 1 }
    if (m < 0) {
        $(".btn-confirm").addClass("disabled")
        return
    }

    document.getElementById('timer').innerHTML = m + ":" + s;
    setTimeout(startTimer, 1000);

}

//check radio
$(".payment-method").click((e) => {
    $(".payment-method").removeClass("active")
    $(e.target).addClass("active")
    let radioInput = $($(e.target).find(".radioPay"))
    $(radioInput).trigger("checked")
})


/*render city district*/
var citis = document.getElementById("city");
var districts = document.getElementById("district");
var wards = document.getElementById("ward");
var Parameter = {
    url: "https://raw.githubusercontent.com/kenzouno1/DiaGioiHanhChinhVN/master/data.json",
    method: "GET",
    responseType: "application/json",
};
var promise = axios(Parameter);
promise.then(function (result) {
    listLocation = result.data
    renderCity(result.data);
});

function renderCity(data) {
    for (const x of data) {
        citis.options[citis.options.length] = new Option(x.Name, x.Id);
    }
    citis.onchange = function () {
        district.length = 1;
        ward.length = 1;
        if (this.value != "") {
            const result = data.filter(n => n.Id === this.value);

            for (const k of result[0].Districts) {
                district.options[district.options.length] = new Option(k.Name, k.Id);
            }
        }
    };
    district.onchange = function () {
        ward.length = 1;
        const dataCity = data.filter((n) => n.Id === citis.value);
        if (this.value != "") {
            const dataWards = dataCity[0].Districts.filter(n => n.Id === this.value)[0].Wards;

            for (const w of dataWards) {
                wards.options[wards.options.length] = new Option(w.Name, w.Id);
            }
        }
    };
}


/*Post info bill*/
function addError(id, textError) {
    //thay đổi thuộc tính của thẻ input
    const element = $(`#${id}`) //element input
    $(element).removeClass("valid")
    $(element).addClass("error")
    $(element).attr("aria-invalid", true)

    //thay đổi thẻ label error
    const erorrLabel = $(`#${id}-error`)
    $(erorrLabel).css("display", "inline-block")
    $(erorrLabel).text(textError)
}


$.validator.addMethod('PhoneVN', function (value) {
    return /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(value);
}, "Số điện thoại không hợp lệ")

$.validator.addMethod('letterOnly', function (value) {
    return /^[a-zA-Z0-9]*$/.test(value);
}, "Mật khẩu không được có dấu hoặc kí tự đặc biệt")

$.validator.addMethod('isEmail', function (value) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(value);
}, "Email không hợp lệ")


$(".form-Payment").validate({
    rules: {
        name: {
            required: true
        },
        address: {
            required: true,
            maxlength: 100
        },
        phone: {
            required: true,
            PhoneVN: true
        },
        city: "required",
        district: "required",
        ward: "required"
    },
    messages: {
        name: {
            required: "Họ tên không được để trống",
        },
        address: {
            required: "Địa chỉ không được để trống",
            maxlength: "Địa chỉ khống quá 100 ký tự"
        },
        phone: {
            required: "Số điện thoại không được để trống"
        },
        city: "Bạn cần chọn trường này",
        district: "Bạn cần chọn trường này",
        ward: "Bạn cần chọn trường này"
    }
})

//khi người dùng muốn đặt hàng
function listCartItem() {
    let listResult = []
    if (localStorage.getItem("Cart") != null) {
        let list = JSON.parse(localStorage.getItem("Cart"))

        for (let i = 0; i < list.length; i++) {
            let cartItem = {
                ProId: list[i].ProId,
                ProSizeID: list[i].proSizeId,
                ProColorID: list[i].proColorId,
                Quantity: list[i].Quantity,
                Price: list[i].Price,
                DiscountPrice: list[i].DiscountPrice
            }
            listResult.push(cartItem)
        }
    }
    return listResult
}

//get voucher
let checkVoucher = false
function getVoucher(input, VoucherId, Name, MiximumMoney, Amount, TypeAmount) {
    $(".voucher-error").text('')
    //Add voucher to input
    if ($(input).hasClass("active") == false) {
        $(".coupon.active").removeClass("active")
        $(input).addClass("active")
        $(".checkout-coupon-form .voucherId").val(VoucherId)
        $(".checkout-coupon-form .voucher-title").val(Name)

        //check condition to apply
        updateTotalPrice() //update lại giá cũa
        let totalBill = JSON.parse($(".total-price").attr("data"))
        if (MiximumMoney > totalBill) {
            let textMiximumMoney = convertPrice(JSON.stringify(MiximumMoney))
            $(".voucher-error").text("Mã giảm giá chỉ áp dụng cho đơn hàng tối thiểu " + textMiximumMoney)
            $("#errorToast .text-toast").text("Mã giảm giá chỉ áp dụng cho đơn hàng tối thiểu " + textMiximumMoney)
            $("#errorToast").toast("show")
            checkVoucher = false
        }
        else {
            checkVoucher = true
            let discountExtra = 0
            if (TypeAmount == "0") //giảm giá theo đ
            {
                discountExtra = Amount
                totalBill -= discountExtra
            }
            else //giảm theo %
            {
                discountExtra = (totalBill * (Amount / 100))
                totalBill -= discountExtra
            }
            //update gia tri cho gia goc
            let discountTotal = JSON.parse($(".discount-total").attr("data"))
            discountTotal += discountExtra //trừ thêm tiền được giảm giá
            let textDiscountTotal = convertPrice(JSON.stringify(discountTotal))

            $(".discount-total").text("-" + textDiscountTotal)
            $(".total-price").text(convertPrice(JSON.stringify(totalBill)))

            $(".discount-total").attr("data", discountTotal)
            $(".total-price").attr("data", totalBill)
        }
    }
    else {
        $(".coupon.active").removeClass("active")
        $(".checkout-coupon-form .voucherId").val('')
        $(".checkout-coupon-form .voucher-title").val('')
        updateTotalPrice() //update lại giá cũa
        checkVoucher = false
    }
}

function payment() {

    const CusID = $(".form-Payment").attr("data-cusid")
    const Phone = $("#phone").val().trim()
    const Name = $("#name").val().trim()
    const Note = $("#note").val().trim()
    const city = $("#city option:selected").text()
    const district = $("#district option:selected").text()
    const ward = $("#ward option:selected").text()
    const address = $("#address").val()
    const PaymentType = $('.payment-method.active input').val()
    const MoneyTotal = $(".total-price").attr("data")
    const VoucherId = checkVoucher == true ? $("input.voucherId").val() : null //nếu checkVoucher = false thì sẽ không lấy cvoucher
    if ($(".form-Payment").valid()) {
        let order = {
            CusID: CusID,
            ReceivingPhone: Phone,
            ReceivingName: Name,
            ReceivingCity: city,
            ReceivingDistrict: district,
            ReceivingWard: ward,
            ReceivingAddress: address,
            PaymentType: PaymentType,
            Note: Note,
            MoneyTotal: MoneyTotal,
            VoucherId: VoucherId
        }

        let cartItems = listCartItem()
        $.ajax({
            url: "/Cart/Order",
            type: "POST",
            data: { order: order, cartItems: cartItems},
            dataType: "Json",
            success: function (res) {
                if (res != 0) {
                    localStorage.removeItem("Cart")
                    location.href = '/Cart/InfoBill/' + res;
                }
                else {
                    alertError("Dã có lỗi xảy ra")
                }
            }
        })
    }
}

function sendOtp() {
    const firebaseConfig = {
        apiKey: "AIzaSyB1FuGkBfcS7oHWqFGLtHSUxY3btvXiWaM",
        authDomain: "otpconfirm-de0b8.firebaseapp.com",
        projectId: "otpconfirm-de0b8",
        storageBucket: "otpconfirm-de0b8.appspot.com",
        messagingSenderId: "1551170791",
        appId: "1:1551170791:web:9cc9ae02118876e6f8b642",
        measurementId: "G-QV2VCK2XHB"
    };
    firebase.initializeApp(firebaseConfig);

    var a = document.getElementById('phone').value;
    var b = "+84";
    var number = b + a.slice(-9);

    const appVerifier = new firebase.auth.RecaptchaVerifier('Confirm', { size: 'invisible' });
    firebase.auth().signInWithPhoneNumber(number, appVerifier).then(function (confirmationResult) {
        window.confirmationResult = confirmationResult;
        coderesult = confirmationResult;
        appVerifier.reset();
    }).catch(function (error) {
        alert(error.message);
        appVerifier.reset();
    });
}

function Resend() {
    event.preventDefault()
    $(".btn-confirm").removeClass("disabled")
    const firebaseConfig = {
        apiKey: "AIzaSyB1FuGkBfcS7oHWqFGLtHSUxY3btvXiWaM",
        authDomain: "otpconfirm-de0b8.firebaseapp.com",
        projectId: "otpconfirm-de0b8",
        storageBucket: "otpconfirm-de0b8.appspot.com",
        messagingSenderId: "1551170791",
        appId: "1:1551170791:web:9cc9ae02118876e6f8b642",
        measurementId: "G-QV2VCK2XHB"
    };
    if (!firebase.apps.length) {
        firebase.initializeApp(firebaseConfig);
    } else {
        firebase.app(); // if already initialized, use that one
    }

    var a = document.getElementById('phone').value;
    var b = "+84";
    var number = b + a.slice(-9);

    if (!window.recaptchaVerifier) {
        window.recaptchaVerifier = new firebase.auth.RecaptchaVerifier('Confirm', {
            'size': 'invisible',
            'callback': (response) => {
                console.log(response)
            },
            'expired-callback': () => {
            }
        });
    }
    firebase.auth().signInWithPhoneNumber(number, window.recaptchaVerifier).then(function (confirmationResult) {
        window.confirmationResult = confirmationResult;
        coderesult = confirmationResult;
    }).catch(function (error) {
        alert(error.message);
    });
}

function codeverity() {
    var code = document.getElementById('partitioned').value;
    coderesult.confirm(code).then(function () {
        payment()
    }).catch(function (error) {
        alertError("Mã OTP không hợp lệ")
    })
}

function confirmPhone() {
    if ($(".form-Payment").valid()) {
        //show modal end countdown
        const Phone = $("#phone").val()
        $(".toPhone").text(Phone)
        $("#partitioned").val('')
        document.getElementById('timer').innerHTML = 03 + ":" + 00;
        $("#ConfirmOrder").modal("show")
        startTimer();
        sendOtp()
    }
}
