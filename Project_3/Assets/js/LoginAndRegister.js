//Register
function addErorr(id,textError){
    const element = $(`#${id}`) //element input
    $(element).removeClass("valid")
    $(element).attr("aria-invalid",true)

    const erorrLable = $(`#${id}-error`)
    $(erorrLable).css("display","inline-block")
    $(erorrLable).text(textError)
}
 
function RegisterSubmit() {
    if ($("#formRegister").valid()) {
        const Name = $("#Name").val()
        const Address = $("#Address").val()
        const Email = $("#Email").val()
        const Phone = $("#Phone").val()
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
                addErorr("Email","Email đã tồn tại")
            }
        })
    }
}


$.validator.addMethod('isEmail', function (value){
    var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
    return regex.test(value);
}, "Email không hợp lệ")

$.validator.addMethod('PhoneVN', function (value) {
    return /(03|05|07|08|09|01[2|6|8|9])+([0-9]{8})\b/.test(value);
}, "Số điện thoại không hợp lệ")

$.validator.addMethod('letterOnly', function (value) {
    return /^[a-zA-Z0-9]*$/.test(value);
},"Mật khẩu không được có dấu hoặc kí tự đặc biệt")

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