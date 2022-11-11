//check radio
$(".payment-method").click((e) => {
    $(".payment-method").removeClass("active")
    $(e.target).addClass("active")
    let radioInput = $($(e.target).find(".radioPay"))
    $(radioInput).trigger("checked")
})

/*render city district*/
var listResult = []
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
    listResult = result.data
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

function payment() {
    const CusID = $(".form-Payment").attr("data-cusid")
    const Phone = $("#phone").val().trim()
    const Note = $("#note").val().trim()
    const city = $("#city").val()
    const district = $("#district").val()
    const ward = $("#ward").val()
    if (city == NaN) {
        alert("Bạn chưa chọn Tỉnh / Thành phố")
    }
    else if (district == NaN) {
        alert("Bạn chưa chọn Quận / Huyện")
    }
    else if (ward == NaN) {
        alert("Bạn chưa chọn Phường / Xã")
    }
    else if($(".form-Payment").valid()) {
        let order = {
            CusID: CusID,

        }
    }
}