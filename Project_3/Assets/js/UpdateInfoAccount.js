//Register
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

$.validator.addMethod('isEmail', function (value) {
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(value);
}, "Email không hợp lệ")

$.validator.addMethod('PhoneVN', function (value) {
    return /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(value);
}, "Số điện thoại không hợp lệ")

$.validator.addMethod('letterOnly', function (value) {
    return /^[a-zA-Z0-9]*$/.test(value);
}, "Mật khẩu không được có dấu hoặc kí tự đặc biệt")

$(".account-setting-form").validate({
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
        email: {
            required: true,
            isEmail: true
        }
    },
    messages: {
        name: {
            required: "Họ tên không được để trống",
        },
        address: {
            required: "Địa chỉ khổng được để trống",
            maxlength: "Địa chỉ khống quá 100 ký tự"
        },
        phone: {
            required: "Bạn cần nhập số điện thoại"
        },
        email: {
            required: "Bạn cần nhập Email"
        }
    }
})

function UpdateInfo() {
    if ($(".account-setting-form").valid()) {
        const CusID = $(".account-setting-form").attr("data-idcustomer")
        const Name = $("#name").val()
        const Address = $("#address").val()
        const Email = $("#email").val()
        const Phone = $("#phone").val().trim()
        const cus = {
            CusID: CusID,
            Name: Name,
            Address: Address,
            Email: Email,
            Phone: Phone
        }
        $.ajax({
            url: "/InfoCustomer/UpdateInfoCustomer",
            type: "Post",
            dataType: "Json",
            data: { cus: cus },
            success: function (res) {
                if (res.message == "ExistEmail") {
                    addError("email", "Email này đã tồn tại")
                }
                if (res.message == "ExistPhone") {
                    addError("phone", "SĐT này đã tồn tại")
                }
                else {
                    if (res.message == "success") {
                        location.reload()
                        $("#successToast .text-toast").text("Cập nhật thành công")
                        $("#successToast").toast("show")
                    }
                    else {
                        $("#errorToast .text-toast").text("Cập nhật thất bại")
                        $("#errorToast").toast("show")
                    }
                }
            }
        })
    }
}