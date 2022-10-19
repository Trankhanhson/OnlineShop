$(".method-header").click((e) => {
    $(".method-header").toggleClass("active")
})


/**handle method */
/**add value */
$('.method-dropdown__item-link').click(function (e) {
    e.preventDefault();  //Không cho submit from bạn có thể bỏ nều k cần
    //lấy giá trị vừa nhập
    let valueInput = $($(e.target).children("p")).text()
    let parentColor = $(e.target).parents(".wrap-color")
    let parentSize = $(e.target).parents(".wrap-size")
    if ($(parentColor).hasClass("wrap-color")) {
        let listColor = $(".wrap-color").children("span") //danh sách size
        let checkExist = false
        let idColor = $(e.target).attr("data-idColor")
        listColor.each(function (index, value) {
            let sizeValue = $(value).text()
            if (sizeValue.trim() == valueInput.trim()) {
                checkExist = true
                return
            }
        })
        if (checkExist != true) {
            //thêm 1 size vào box nếu value chưa tồn tại
            $(".wrap-color").prepend(`<span data-idColor="${idColor}" class="method-values">${valueInput}<span class="method-remove" ><i class="fa-solid fa-xmark"></i></span></span>`)
            addImgBox(idColor, valueInput) //khi thêm 1 màu mưới thì sẽ thêm hình ảnh cho màu đó
        }
        addVariation(idColor, valueInput, "color")
    }
    else if ($(parentSize).hasClass("wrap-size")) {
        let listSize = $(".wrap-size").children("span") //danh sách size
        let checkExist = false
        let idSize = $(e.target).attr("data-idSize")
        listSize.each(function (index, value) {
            let sizeValue = $(value).text()
            if (sizeValue.trim() == valueInput.trim()) {
                checkExist = true
                return
            }
        })
        if (checkExist != true) {
            //thêm 1 size vào box nếu value chưa tồn tại
            $(".wrap-size").prepend(`<span class="method-values" data-idSize="${idSize}">${valueInput}<span class="method-remove" ><i class="fa-solid fa-xmark"></i></span></sp>`)
        }
        addVariation(idSize, valueInput, "size")
    }

});

//remove size or 
//phải gọi từ cha mới có thể thực hiện click

$(".wrap-color").click(deleteValue)
$(".wrap-size").click(deleteValue)

function deleteImgBox(idColor) {
    let boxImg = $(`div[data-idColor="${idColor}"]`)
    boxImg.remove()
}

function deleteValue(e) {

    let item = $(e.target)
    if (item.hasClass("method-remove")) {

        //remove options
        let listColor = $(".colorOption") //take option from table
        let listSize = $(".sizeOption")
        let wrapColor = $(e.target).parents(".wrap-color")
        let wrapSize = $(e.target).parents(".wrap-size")
        let textClicked = $(e.target).parent().text()
        let idColor = $($(item).parent()).attr("data-idColor")
        if ($(wrapColor).children().length == 2 || $(wrapSize).children().length == 2) {
            //khi xóa phần tử cuối cùng của method thì sẽ không thể tạo ra product
            $(".box-options").html('')
            deleteImgBox(idColor)
        }
        else if (wrapColor.hasClass("wrap-color")) { //khi nhấn xóa giá trị ở color
            listColor.each((index, value) => {
                if ($(value).text() == textClicked) {
                    $(value).parents('tr').remove()
                }
            })
            deleteImgBox(idColor)

        }
        else { //khi nhấn xóa giá trị ở size
            listSize.each((index, value) => {
                if ($(value).text() == textClicked) {

                    $(value).parents('tr').remove()
                }
            })
        }

        $(e.target).parent().remove() //remove item clicked
    }

}

function addImgBox(idColor, colorValue) {
    let result = `<div class="d-flex justify-content-between mb-4" data-idColor="${idColor}">                                  
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file__main input-file" type='file' " accept="image/*" onchange="uploadImg(this)"/>
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>  
                    
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file__detail1 input-file" type='file' accept="image/*" onchange="uploadImg(this)" />
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file__detail2 input-file" type='file' accept="image/*" onchange="uploadImg(this)" />
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file input-file__detail3" type='file' accept="image/*" onchange="uploadImg(this)" />
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file input-file__detail4" type='file' accept="image/*" onchange="uploadImg(this)" />
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>    
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap">
                        <input class="input-file input-file__detail5" type='file' accept="image/*" onchange="uploadImg(this)" />
                        <div class="drag-text">
                        <p>Ảnh ${colorValue}</p>
                        </div>
                    </div>
                    <div class="file-upload-content">
                        <img class="file-upload-image" src="#" alt="your image" />
                        <div class="image-title-wrap">
                        <button type="button" class="remove-image" onclick="removeImg(this)">Xóa</button>
                        </div>
                    </div>   
                </div>            
            </div>`
    $(".wrap-imgItem").append(result)
}

function addVariation(valueId, valueInput, sizeOrColor) {
    //lấy danh sách màu
    let listSpanColor = $(".wrap-color").children("span")
    let listColor = []
    listSpanColor.each((index, value) => {
        var color = {
            textColor: $(value).text(),
            idColor: $(value).attr("data-idColor")
        }

        listColor.push(color)
    })
    //lấy danh sách size
    let listSize = []
    let listSpanSize = $(".wrap-size").children("span")
    listSpanSize.each((index, value) => {
        var size = {
            textSize: $(value).text(),
            idSize: $(value).attr("data-idSize")
        }
        listSize.push(size)
    })

    let listColorLength = listColor.length
    let listSizeLength = listSize.length
    if ((listColorLength > 0 && listSizeLength == 1) || (listColorLength == 1 && listSizeLength > 0)) {
        let result = `<div class=" px-3 py-2  text-primary font-weight-bold options-header" >
            <span>Danh sách các hàng hóa cùng loại</span>
            </div>
            <div class="options-content px-4">
                <table class="table table-create table-borderless">
                    <thead>
                        <tr class="text-primary">
                            <th style="width:10%">Màu</th>
                            <th>Kích thước</th>
                            <th>Tồn kho</th>
                            <th></th>
                        </tr>                                    
                    </thead>
                    <tbody>
                            `
        //render value
        let str = ""
        for (let j = 0; j < listColorLength; j++) {

            for (let i = 0; i < listSizeLength; i++) {
                str += `<tr>
                            <td>
                                <p class="colorOption text-dark" data-idColor="${listColor[j].idColor}">${listColor[j].textColor}</p>
                            </td>
                            <td>
                                <p class="sizeOption text-dark" data-idSize="${listSize[i].idSize}">${listSize[i].textSize}</p>
                            </td>
                            <td>
                                <div class="input-wrap d-flex">
                                    <input type="text"  class="input-create input-quantity text-dark">
                                </div>
                            </td >
                            <td class="text-end"><span class="delete-option"><i class="fa-solid fa-trash-can"></i></span></td>
                        </tr>    `
            }
        }

        result += str + '</tbody></table><div class="text-end mb-4 pe-4"></div>'
        $(".box-options").html(result)
    }
    else {
        let tbody = $(".table tbody")
        let result = ""
        if (sizeOrColor == "color") {
            for (let i = 0; i < listSizeLength; i++) {
                let str = `<tr>
                            <td>
                                <p class="colorOption text-dark" data-idColor="${valueId}">${valueInput}</p>
                            </td>
                            <td>
                                <p class="sizeOption text-dark" data-idSize="${listSize[i].idSize}">${listSize[i].textSize}</p>
                            </td>
                        <td>
                            <div class="input-wrap d-flex">
                                <input type="text"  class="input-create input-quantity text-dark">
                            </div>
                        </td>
                        <td class="text-end"><span class="delete-option"><i class="fa-solid fa-trash-can"></i></span></td>
                    </tr>    `
                result += str
            }
            tbody.prepend(result)
        }
        else {
            for (let i = 0; i < listColorLength; i++) {
                let str = `<tr>
                            <td>
                                <p class="colorOption text-dark" data-idColor="${listColor[i].idColor}">${listColor[i].textColor}</p>
                            </td>
                            <td>
                                <p class="sizeOption text-dark" data-idSize="${valueId}">${valueInput}</p>
                            </td>
                        <td>
                            <div class="input-wrap d-flex">
                                <input type="text"  class="input-create input-quantity text-dark">
                            </div>
                        </td>
                        <td class="text-end"><span class="delete-option"><i class="fa-solid fa-trash-can"></i></span></td>
                    </tr>    `
                result += str
            }
            tbody.append(result)
        }
    }
}

$(".box-options").click((e) => {
    let item = $(e.target)
    if (item.hasClass("delete-option")) {

        if ($(".table-create tbody tr").length == 1) {
            $(".box-options").html('')
        }
        else {
            item.parents("tr").remove()
        }

        //remove from method
        let listColor = $(".colorOption") //take option from table
        let listSize = $(".sizeOption")
        let wrapSpanColor = $(".wrap-color > span")
        let wrapSpanSize = $(".wrap-size > span")

        //check color exist
        wrapSpanColor.each((index2, valueMethod) => {
            let checkColor = false
            listColor.each((index, value) => {
                if ($(value).text().trim() == $(valueMethod).text().trim()) {
                    checkColor = true
                    return false
                }
            })
            if (!checkColor) {
                $(valueMethod).remove()
            }
        })

        wrapSpanSize.each((index2, valueMethod) => {
            let checkSize = false
            listSize.each((index, value) => {
                if ($(value).text().trim() == $(valueMethod).text().trim()) {
                    checkSize = true
                    return false
                }
            })
            if (!checkSize) {
                $(valueMethod).remove()
            }
        })
    }

})


/**uppload file */
var clickFile = document.createElement('div') //lưu thẻ bọc 1 ảnh khi click để truy xuất đến các phẩn tử con
function fileBoxClick(input) {
    clickFile = input
}
function uploadImg(input) {
    const reader = new FileReader()

    // Lấy thông tin tập tin được đăng tải
    const file = input.files

    // Đọc thông tin tập tin đã được đăng tải
    reader.readAsDataURL(file[0])
    // Lắng nghe quá trình đọc tập tin hoàn thành
    reader.addEventListener("load", (event) => {
        // Lấy chuỗi Binary thông tin hình ảnh
        const img = event.target.result;
        // Thực hiện hành động thêm chuỗi giá trị này vào thẻ IMG
        $(clickFile).find('.file-upload-image').attr('src', img);
        $(clickFile).find('.file-upload-content').show();
        $(clickFile).find('.image-upload-wrap').hide();
    })
}


function removeImg(input) {
    let parentBox = $(input).parents(".wrap-file-box")
    $(parentBox).find('.input-file').replaceWith($('.input-file').clone());
    $(parentBox).find('.file-upload-content').hide();
    $(parentBox).find('.image-upload-wrap').show();
}

$('.image-upload-wrap').bind('dragover', function () {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function () {
    $('.image-upload-wrap').removeClass('image-dropping');
});

//submit form
function SaveClick() {
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
        PrPricePromotion: PricePromotion,
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
        var fileMain = $(`div[data-idColor=${idColor}]`).find('.input-file__main').get(0).files;
        var fileDetail1 = $(`div[data-idColor=${idColor}]`).find('.input-file__detail1').get(0).files;
        var fileDetail2 = $(`div[data-idColor=${idColor}]`).find('.input-file__detail2').get(0).files;
        var fileDetail3 = $(`div[data-idColor=${idColor}]`).find('.input-file__detail3').get(0).files;
        var fileDetail4 = $(`div[data-idColor=${idColor}]`).find('.input-file__detail4').get(0).files;
        var fileDetail5 = $(`div[data-idColor=${idColor}]`).find('.input-file__detail5').get(0).files;
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
            type: 'POST',
            url: '/Admin/Product/UploadImg',
            contentType: false, //Không có header
            processData: false, //không xử lý dữ liệu
            data: formData,
            success: function (urlImage) {
                $.ajax({
                    type: 'GET',
                    url: '/Admin/Product/Index'
                })
            },
            error: function (err) {
                alert('có lỗi khi upload: ' + err.statusText);
            }
        })
    }
}

//handle list product
$(".row-product").click((e) => {
    let rowProduct = $(e.target).parents(".row-product");
    let id = $(rowProduct).attr("id")
    $(`.row-variation-${id}`).toggle()
    rowProduct.toggleClass("active")
})

