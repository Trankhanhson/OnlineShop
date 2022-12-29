function SaveData() {

    //lấy dữ liệu của product
    var ProId = $("#main").attr("data-idPro")
    var ProName = $("#name").val()
    var Material = $("#material").val()
    var Description = $("#description").val()
    var ProCatId = $("#ProCatId").val()
    var ImportPrice = $("#importPrice").val()
    var Price = $("#Price").val()
    var product = {
        ProId: ProId,
        ProName: ProName,
        Material: Material,
        Description: Description,
        ProCatId: ProCatId,
        ImportPrice: ImportPrice,
        Price: Price
    }

    var listVariation = [] //danh sách biến thể
    var variationWrap = $(".table-create tbody tr")

    variationWrap.each((index, value) => {
        let colorId = $($(value).find(".colorOption")).attr("data-idColor")
        let variation = {
            ProId: ProId,
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
        url: "/Admin/Product/Edit",
        data: { product: product, listVariation: listVariation },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.check) {
                for (var i = 0; i < listColorId.length; i++) {
                    UploadImgToServer(listColorId[i], response.Proid) //upload ảnh mới của product
                }
                location.reload()
                $("#successToast .text-toast").text(response.message)
                $("#successToast").toast("show") //hiển thị thông báo thành công
            }
            else {
                $("#errorToast .text-toast").text(response.message)
                $("#errorToast").toast("show") //hiển thị thông báo thành công
            }
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
        var listImgElement = $(imgItem).find(".file-upload-image")
        var ListNoSelect = []
        listImgElement.each((index, value) => {
            //nếu có ảnh thì là chưa select ngược lại không có ảnh là đã
            let src = $(value).attr("src")
            if (src != "") {
                ListNoSelect.push("noSelect")
            }
            else {
                ListNoSelect.push("Selected")
            }
        })
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
        formData.append("ListNoSelect", ListNoSelect)
        formData.append("ProColorId", idColor)
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/Product/UploadImgEdit',
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

$(".btn-edit").click((e) => {

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
})


//hiển thị ảnh khi load
$('.file-upload-content').show();
$('.image-upload-wrap').hide();


/**loại các size bị trung trên giao diện*/
/**cách dùng : đưa class filter-sameValue và thẻ bọc các size và class filter-value vào các thẻ có data-idSize*/
var parentSizes = $(".filter-sameValue") //danh sách các element chứa các size
parentSizes.each((index, wrapSize) => {
    let listSize = $(wrapSize).find(".filter-value") //danh sách size
    let newList = [] //chứa list idSize đã được filter
    listSize.each((index1, sizeElement) => {
        let idSize = $(sizeElement).attr("data-idSize")

        //nếu idSize chưa tồn tại trong newList
        if (newList.indexOf(idSize) == -1) {
            newList.push(idSize)
        }
        else {
            $(sizeElement).remove() //xóa phần element nếu đã tồn tại
        }
    })
})        function SaveData() {

    //lấy dữ liệu của product
    var ProId = $("#main").attr("data-idPro")
    var ProName = $("#name").val()
    var Material = $("#material").val()
    var Description = $("#description").val()
    var ProCatId = $("#ProCatId").val()
    var ImportPrice = $("#importPrice").val()
    var Price = $("#Price").val()
    var product = {
        ProId: ProId,
        ProName: ProName,
        Material: Material,
        Description: Description,
        ProCatId: ProCatId,
        ImportPrice: ImportPrice,
        Price: Price
    }

    var listVariation = [] //danh sách biến thể
    var variationWrap = $(".table-create tbody tr")

    variationWrap.each((index, value) => {
        let colorId = $($(value).find(".colorOption")).attr("data-idColor")
        let variation = {
            ProId: ProId,
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
        url: "/Admin/Product/Edit",
        data: { product: product, listVariation: listVariation },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.check) {
                for (var i = 0; i < listColorId.length; i++) {
                    UploadImgToServer(listColorId[i], response.Proid) //upload ảnh mới của product
                }
                location.reload()
                $("#successToast .text-toast").text(response.message)
                $("#successToast").toast("show") //hiển thị thông báo thành công
            }
            else {
                $("#errorToast .text-toast").text(response.message)
                $("#errorToast").toast("show") //hiển thị thông báo thành công
            }
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
        var listImgElement = $(imgItem).find(".file-upload-image")
        var ListNoSelect = []
        listImgElement.each((index, value) => {
            //nếu có ảnh thì là chưa select ngược lại không có ảnh là đã
            let src = $(value).attr("src")
            if (src != "") {
                ListNoSelect.push("noSelect")
            }
            else {
                ListNoSelect.push("Selected")
            }
        })
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
        formData.append("ListNoSelect", ListNoSelect)
        formData.append("ProColorId", idColor)
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/Product/UploadImgEdit',
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

$(".btn-edit").click((e) => {

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
})


//hiển thị ảnh khi load
$('.file-upload-content').show();
$('.image-upload-wrap').hide();


/**loại các size bị trung trên giao diện*/
/**cách dùng : đưa class filter-sameValue và thẻ bọc các size và class filter-value vào các thẻ có data-idSize*/
var parentSizes = $(".filter-sameValue") //danh sách các element chứa các size
parentSizes.each((index, wrapSize) => {
    let listSize = $(wrapSize).find(".filter-value") //danh sách size
    let newList = [] //chứa list idSize đã được filter
    listSize.each((index1, sizeElement) => {
        let idSize = $(sizeElement).attr("data-idSize")

        //nếu idSize chưa tồn tại trong newList
        if (newList.indexOf(idSize) == -1) {
            newList.push(idSize)
        }
        else {
            $(sizeElement).remove() //xóa phần element nếu đã tồn tại
        }
    })
})        function SaveData() {

    //lấy dữ liệu của product
    var ProId = $("#main").attr("data-idPro")
    var ProName = $("#name").val()
    var Material = $("#material").val()
    var Description = $("#description").val()
    var ProCatId = $("#ProCatId").val()
    var ImportPrice = $("#importPrice").val()
    var Price = $("#Price").val()
    var product = {
        ProId: ProId,
        ProName: ProName,
        Material: Material,
        Description: Description,
        ProCatId: ProCatId,
        ImportPrice: ImportPrice,
        Price: Price
    }

    var listVariation = [] //danh sách biến thể
    var variationWrap = $(".table-create tbody tr")

    variationWrap.each((index, value) => {
        let colorId = $($(value).find(".colorOption")).attr("data-idColor")
        let variation = {
            ProId: ProId,
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
        url: "/Admin/Product/Edit",
        data: { product: product, listVariation: listVariation },
        dataType: "json",
        type: "POST",
        success: function (response) {
            if (response.check) {
                for (var i = 0; i < listColorId.length; i++) {
                    UploadImgToServer(listColorId[i], response.Proid) //upload ảnh mới của product
                }
                location.reload()
                $("#successToast .text-toast").text(response.message)
                $("#successToast").toast("show") //hiển thị thông báo thành công
            }
            else {
                $("#errorToast .text-toast").text(response.message)
                $("#errorToast").toast("show") //hiển thị thông báo thành công
            }
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
        var listImgElement = $(imgItem).find(".file-upload-image")
        var ListNoSelect = []
        listImgElement.each((index, value) => {
            //nếu có ảnh thì là chưa select ngược lại không có ảnh là đã
            let src = $(value).attr("src")
            if (src != "") {
                ListNoSelect.push("noSelect")
            }
            else {
                ListNoSelect.push("Selected")
            }
        })
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
        formData.append("ListNoSelect", ListNoSelect)
        formData.append("ProColorId", idColor)
        $.ajax({
            async: true,
            type: 'POST',
            url: '/Admin/Product/UploadImgEdit',
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

$(".btn-edit").click((e) => {

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
})


//hiển thị ảnh khi load
$('.file-upload-content').show();
$('.image-upload-wrap').hide();


/**loại các size bị trung trên giao diện*/
/**cách dùng : đưa class filter-sameValue và thẻ bọc các size và class filter-value vào các thẻ có data-idSize*/
var parentSizes = $(".filter-sameValue") //danh sách các element chứa các size
parentSizes.each((index, wrapSize) => {
    let listSize = $(wrapSize).find(".filter-value") //danh sách size
    let newList = [] //chứa list idSize đã được filter
    listSize.each((index1, sizeElement) => {
        let idSize = $(sizeElement).attr("data-idSize")

        //nếu idSize chưa tồn tại trong newList
        if (newList.indexOf(idSize) == -1) {
            newList.push(idSize)
        }
        else {
            $(sizeElement).remove() //xóa phần element nếu đã tồn tại
        }
    })
})