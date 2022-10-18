$(".btn-deleteColor").off("click").on("click",function(e) {
    e.preventDefault();

    if (confirm("Bạn có muốn xóa màu này không")) {
        let id = $(e.target).attr("id")
        $.ajax({
            url: "/Admin/Color/Delete",
            data: { id: id },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.result == false) {
                    alert("Màu này đang được dùng ở sản phẩm")
                }
                else {
                    $(`#row_${id}`).remove()
                }
            }
        })
    }
} )