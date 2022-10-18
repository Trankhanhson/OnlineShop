$(".btn-deleteSize").off("click").on("click", function (e) {
    e.preventDefault();

    if (confirm("Bạn có muốn xóa kích thước này không")) {
        let id = $(e.target).attr("id")
        $.ajax({
            url: "/Admin/Size/Delete",
            data: { id: id },
            dataType: "json",
            type: "POST",
            success: function (response) {
                if (response.result == false) {
                    alert("kích thước này đang được dùng ở sản phẩm")
                }
                else {
                    $(`#row_${id}`).remove()
                }
            }
        })
    }
})