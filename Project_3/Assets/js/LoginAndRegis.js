//Register
function addErorr(id, textError) {
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

function RegisterSubmit() {
    if ($("#formRegister").valid()) {
        const Name = $("#Name").val()
        const Address = $("#Address").val()
        const Email = $("#Email").val()
        const Phone = $("#Phone").val().trim()
        const Password = $("#Password").val()
        const cus = {
            Name: Name,
            Address: Address,
            Email: Email,
            Phone: Phone,
            Password: Password
        }

        $.ajax({
            url: "/Home/Register",
            type: "POST",
            dataType: "Json",
            data: { cus: cus },
            success: function (res) {
                if (res.message == "ExistEmail") {
                    addErorr("Email", "Email này đã tồn tại")
                }
                if (res.message == "ExistPhone") {
                    addErorr("Phone", "SĐT này đã tồn tại")
                }
                else {
                    if (res.message == "success") {
                        //chuyển sang đăng nhập
                        $('#ModalRigister .btn-login').trigger("click")
                        $('#userName').val(Email) //gán email vừa đăng ký
                        $('#passwordLogin').val(Password) //gán sđt vừa đăng ký

                        $('#formRegister').trigger("reset");

                        $("#successToast .text-toast").text("Đăng ký thành công")
                        $("#successToast").toast("show")
                    }
                    else {
                        $("#errorToast .text-toast").text("Đăng ký thất bại")
                        $("#errorToast").toast("show")
                    }
                }
            }
        })
    }
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

$("#formRegister").validate({
    rules: {
        Name: {
            required: true
        },
        Address: {
            required: true,
            maxlength: 100
        },
        Phone: {
            required: true,
            PhoneVN: true
        },
        Email: {
            required: true,
            isEmail: true
        },
        Password: {
            required: true,
            minlength: 8,
            letterOnly: true
        },
        ConfirmPassword: {
            equalTo: "#Password"
        }
    },
    messages: {
        Name: {
            required: "Bạn cần nhập tên",
        },
        Address: {
            required: "Bạn cần nhập địa chỉ",
            maxlength: "Có thể bạn nhập sai địa chỉ"
        },
        Phone: {
            required: "Bạn cần nhập số điện thoại"
        },
        Email: {
            required: "Bạn cần nhập Email"
        },
        Password: {
            required: "Bạn cần nhập mật khẩu",
            minlength: "Mật khẩu tối thiểu 8 ký tự"
        },
        ConfirmPassword: {
            equalTo: "Xác nhận mật khẩu không trùng khớp"
        }
    }
})