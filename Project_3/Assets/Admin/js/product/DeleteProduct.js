function deleteProduct(id) {
    if (confirm("Bạn có chắc chắn muốn xóa")) {
        $.ajax({
            type: 'Post',
            url: "/Admin/Product/Delete",
            dataType: "json",
            data: { id: id },
            success: function (res) {
                if (res.check) {
                    $("#successToast .text-toast").text("Đã xóa thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                    //remove khỏi view
                    $(`#${id}`).remove()
                    $(`.row-variation-${id}`).remove()
                }
                else {
                    $("#errorToast .text-toast").text("Sản phẩm này đang được dùng ở nơi khác")
                    $("#errorToast").toast("show")
                }
            },
            error: function (err) {
                $("#errorToast .text-toast").text("xóa thất bại")
                $("#errorToast").toast("show")
            }
        })
    }
}

function deleteVariation(input,idVariation) {
    if (confirm("Bạn có chắc chắn muốn xóa biến thể này")) {
        $.ajax({
            url: "/Admin/Product/DeleteVariation",
            type: 'POST',
            dataType: "json",
            data: { idVariation: idVariation },
            success: function (res) {
                if (res.check) {
                    $("#successToast .text-toast").text("Đã xóa thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                    //xóa trên view
                    let ulParent = $(input).parents("ul")
                    $(ulParent).remove()
                }
                else {
                    $("#errorToast .text-toast").text("Biến thể này đang được dùng ở nơi khác")
                    $("#errorToast").toast("show")
                }
            },
            error: function (err) {
                $("#errorToast .text-toast").text("xóa thất bại")
                $("#errorToast").toast("show")
            }
        })
    }
}