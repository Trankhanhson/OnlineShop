$(".btn-ChangeStatusUser").on('click', function (e) {
    e.preventDefault();
    var id = $(e.target).data("id")
    $.ajax({
        url: "/Admin/User/ChangeStatus",
        data: { id: id },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.status ==true) {
                $(e.target).text("Kích hoạt")
            }
            else {
                $(e.target).text("Khóa")
            }
        }
    })
})