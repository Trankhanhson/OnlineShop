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
            url: "/LoginClient/Register",
            type: "POST",
            dataType: "Json",
            data: { cus: cus },
            success: function (res) {
                if (res.message == "ExistEmail") {
                    addError("Email", "Email này đã tồn tại")
                }
                if (res.message == "ExistPhone") {
                    addError("Phone", "SĐT này đã tồn tại")
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

$("#formLogin").validate({
    rules: {
        userName: "required",
        passwordLogin: "required"
    },
    messages: {
        userName: "Bạn chưa nhập tên tài khoản",
        passwordLogin: "Bạn chưa nhập mật khẩu"
    }
})

function Login() {
    if ($("#formLogin").valid()) {
        const username = $("#userName").val()
        const password = $("#passwordLogin").val()
        $.ajax({
            url: "/LoginClient/Login",
            type: "Post",
            dataType: "Json",
            data: { username: username, password: password },
            success: function (res) {
                if (res.message == "usename") {
                    addError("userName", "Tại khoản không tồn tại")
                }
                else if (res.message == "password") {
                    addError("passwordLogin", "Mật khẩu không đúng")
                }
                else if (res.message == "fail") {
                    $("#errorToast .text-toast").text("Đăng nhập thất bại")
                    $("#errorToast").toast("show")
                }
                else {
                    location.reload();
                    $("#successToast .text-toast").text("Đã đăng nhập thành công")
                    $("#successToast").toast("show")
                }
            }
        })
    }
}

$("#formConfirmEmail").validate({
    rules: {
        EmailConfirm: {
            required: true,
            isEmail: true
        }
    },
    messages: {
        EmailConfirm: {
            required: "Bạn cần nhập Email"
        }
    }
})

function ConfirmEmail() {

    if ($("#formConfirmEmail").valid()) {
        const EmailConfirm = $("#EmailConfirm").val()
        $.ajax({
            url: "/LoginClient/ForgotPassword",
            type: "Post",
            dataType: "Json",
            data: { Email: EmailConfirm },
            success: function (res) {

                if (res) //nếu Email tồn tại
                {
                    $("#ConfirmedEmail").modal("show")
                    $("#ModalGetPassword").modal("hide")
                }
                else //nếu không thì sẻ thông báo
                {
                    addError("EmailConfirm", "Email không đúng")
                }
            }
        })
    }
}

$("#resetPassword").validate({
    rules: {
        NewPassword: {
            required: true,
            minlength: 8,
            letterOnly: true
        },
        ConfirmPass: {
            equalTo: "#NewPassword"
        }
    },
    messages: {
        NewPassword: {
            required: "Bạn cần nhập mật khẩu mới",
            minlength: "Mật khẩu tối thiểu 8 ký tự"
        },
        ConfirmPass: {
            equalTo: "Xác nhận mật khẩu không trùng khớp"
        }
    }
})

function ResetPassword() {
    if ($("#resetPassword").valid()) {
        const NewPassword = $("#NewPassword").val()
        const ResetCode = $("#ResetCode").val()
        const model = {
            NewPassword: NewPassword,
            ResetCode: ResetCode
        }
        $.ajax({
            url: "/LoginClient/ResetPassword",
            type: "Post",
            dataType: "Json",
            data: { model: model },
            success: function (res) {

                if (res.check) //nếu Email tồn tại
                {
                    $("#successToast .text-toast").text("Thay đổi mật khẩu thành công, bạn có thể dùng mật khẩu này để đăng nhập bây giờ")
                    $("#successToast").toast("show")

                    let url = "/LoginClient/LoginView/" + res.id
                    window.location.href = url;
                }
                else //nếu không thì sẻ thông báo
                {
                    $("#errorToast .text-toast").text("Thay đổi mật khẩu thất bại")
                    $("#errorToast").toast("show")
                }
            }
        })
    }
}

$("#LoginView").validate({
    rules: {
        Email: "required",
        Password: "required"
    },
    messages: {
        Email: "Bạn chưa nhập tên tài khoản",
        Password: "Bạn chưa nhập mật khẩu"
    }
})

function LoginView() {
    console.log($("#LoginView"))
    if ($("#LoginView").valid()) {
        const username = $("#Email").val()
        const password = $("#Password").val()
        $.ajax({
            url: "/LoginClient/Login",
            type: "Post",
            dataType: "Json",
            data: { username: username, password: password },
            success: function (res) {
                if (res.message == "usename") {
                    addError("userName", "Tại khoản không tồn tại")
                }
                else if (res.message == "password") {
                    addError("passwordLogin", "Mật khẩu không đúng")
                }
                else if (res.message == "fail") {
                    $("#errorToast .text-toast").text("Đăng nhập thất bại")
                    $("#errorToast").toast("show")
                }
                else {
                    let url = "/Home/HomePage"
                    window.location.href = url;
                }
            }
        })
    }
}