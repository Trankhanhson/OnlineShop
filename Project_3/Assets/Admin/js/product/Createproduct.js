function SaveData() {

    //lấy dữ liệu của product
    var ProName = $("#name").val()
    var Material = $("#material").val()
    var Description = $("#description").val()
    var ProCatId = $("#ProCatId").val()
    var ImportPrice = $("#importPrice").val()
    var Price = $("#Price").val()
    var PricePromotion = $("#pricePromotion").val()
    var StartPromotion = $("#startPromotion").val()
    var StopPromotion = $("#stopPromotion").val()
    var product = {
        ProName: ProName,
        Material: Material,
        Description: Description,
        ProCatId: ProCatId,
        ImportPrice: ImportPrice,
        Price: Price,
        PromotionPrice: PricePromotion,
        StartPromotion: StartPromotion,
        StopPromotion: StopPromotion
    }

    var listVariation = [] //danh sách biến thể
    var variationWrap = $(".table-create tbody tr")

    variationWrap.each((index, value) => {
        let colorId = $($(value).find(".colorOption")).attr("data-idColor")
        let variation = {
            ProId: null,
            ProColorID: colorId,
            ProSizeID: $($(value).find(".sizeOption")).attr("data-idSize"),
            Quantity: $($(value).find(".input-create")).val()
        }
        listVariation.push(variation)
    })

    //Lấy list Color
    var listColorId = []
    let listSpanColor = $(".wrap-color").children("span")
    listSpanColor.each((index, value) => {
        listColorId.push($(value).attr("data-idColor"))
    })

    //thêm sản phẩm và danh sách biến thể bằng ajax và lấy về idproduct mới
    $.ajax({
        url: "/Admin/Product/Create",
        data: { product: product, listVariation: listVariation },
        dataType: "json",
        type: "POST",
        success: function (response) {
            
            for (var i = 0; i < listColorId.length; i++) {
                UploadImgToServer(listColorId[i], response.Proid)
            }
            $("#successToast .text-toast").text("Đã thêm sản phẩm thành công")
            $("#successToast").toast("show") //hiển thị thông báo thành công
        }
    })
}

function UploadImgToServer(idColor, idProduct) {
    //upload img
    //kiểm tra trình duyệt có hỗ trợ FormData oject không
    if (window.FormData != undefined) {
        //Lấy dữ liệu trên file upload
        //lấy thẻ input theo idcolor
        var imgItem = $(`.imgItem[data-idColor=${idColor}]`)
        var fileMain = $(imgItem).find('.input-file__main').get(0).files;
        var fileDetail1 = $(imgItem).find('.input-file__detail1').get(0).files;
        var fileDetail2 = $(imgItem).find('.input-file__detail2').get(0).files;
        var fileDetail3 = $(imgItem).find('.input-file__detail3').get(0).files;
        var fileDetail4 = $(imgItem).find('.input-file__detail4').get(0).files;
        var fileDetail5 = $(imgItem).find('.input-file__detail5').get(0).files;
        //Tạo đối trượng formData
        var formData = new FormData();
        //tạo các key,value cho data
        formData.append("file", fileMain[0])
        formData.append("file1", fileDetail1[0])
        formData.append("file2", fileDetail2[0])
        formData.append("file3", fileDetail3[0])
        formData.append("file4", fileDetail4[0])
        formData.append("file5", fileDetail5[0])
        formData.append("ProId", idProduct)
        formData.append("ProColorId", idColor)
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/Product/UploadImg',
            contentType: false, //Không có header
            processData: false, //không xử lý dữ liệu
            data: formData,
            success: function (urlImage) {
            },
            error: function (err) {
                alert('có lỗi khi upload: ' + err.statusText);
            }
        })
    }
}

$(".btn-create").click((e) => {
    //kiểm tra giá khuyến mại phải nhỏ hơn giá bán
    var Price = $("#Price").val()
    var PricePromotion = $("#pricePromotion").val()
    if (PricePromotion <= Price) {
        var variationWrap = $(".table-create tbody tr")
        //kiểm tra xem đã thêm variation chưa
        if (variationWrap.length > 0) {
            if ($('#formProduct').valid()) {
                SaveData()
            }
        }
        else {
            $("#errorToast .text-toast").text("Bạn chưa thêm biến thể nào")
            $("#errorToast").toast("show")
        }
    }
    else {
        $("#errorToast .text-toast").text("Giá khuyến mại phải nhỏ hơn giá bán")
        $("#errorToast").toast("show")
    }
})