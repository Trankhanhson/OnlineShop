$(".method-header").click((e) => {
    $(".method-header").toggleClass("active")
})

function ChangeStatus(event, id) {
    event.stopPropagation()
    if (confirm("Bạn có chắc chắn muốn đổi trạng thái")) {
        $.ajax({
            url: "/Admin/Product/ChangeStatus/" + id,
            dataType: "Json",
            type: "GET",
            success: function (res) {
                if (res) {
                    $("#successToast .text-toast").text("Đã cập nhật trạng thái thành công")
                    $("#successToast").toast("show")
                }
                else {
                    $("#errorToast .text-toast").text("Cập nhật trạng thái thất bại")
                    $("#errorToast").toast("show")
                }
            }
        })
    }
}
/**handle method */
/**add value */
function addValue(e) {
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
            addVariation(idSize, valueInput, "size")
        }
    }

}

//remove size or 
//phải gọi từ cha mới có thể thực hiện click

$(".wrap-color").click(deleteValue)
$(".wrap-size").click(deleteValue)

function deleteImgBox(idColor) {
    let boxImg = $(`.imgItem[data-idColor="${idColor}"]`)
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
        if ($(wrapColor).children(".method-values").length == 1 || $(wrapSize).children(".method-values").length == 1) {
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
    let result = `<div class="d-flex justify-content-between mb-4 imgItem" data-idColor="${idColor}">                                  
                <div class="wrap-file-box" onclick="fileBoxClick(this)">
                    <div class="image-upload-wrap" >
                        <input class="input-file__main input-file" type='file' " accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)"/>
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
                        <input class="input-file__detail1 input-file" type='file' accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)" />
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
                        <input class="input-file__detail2 input-file" type='file' accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)" />
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
                    <div class="image-upload-wrap" >
                        <input class="input-file input-file__detail3" type='file' accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)" />
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
                        <input class="input-file input-file__detail4" type='file' accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)" />
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
                        <input class="input-file input-file__detail5" type='file' accept="image/*" ondrop="dropImg(event)" ondragleave="dragLeaveBox(event)" ondragenter="DragEnter(event)" onchange="uploadImg(this)" />
                        <div class="drag-text">
                            <p class="text-nodragenter">Ảnh ${colorValue}</p>
                            <p class="text-dragenter"><i class="fa-solid fa-plus"></i> Thêm ảnh</p>
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
        let result = `<div class=" px-3 py-2  text-dark font-weight-bold options-header" >
            <span>Danh sách các hàng hóa cùng loại</span>
            </div>
            <div class="options-content px-4">
                <table class="table table-create table-borderless">
                    <thead>
                        <tr class="text-dark">
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
                                    <input type="number" value="0" readonly  class="input-create input-quantity text-dark">
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
                                <input type="number" value="0" readonly  class="input-create input-quantity text-dark">
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
                                <input type="number" value="0" readonly  class="input-create input-quantity text-dark">
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

function deleteVariation(item) {
    if (item.hasClass("delete-option")) {

        //trường hợp chỉ còn 1 variation
        if ($(".table-create tbody tr").length == 1) {
            $(".box-options").html('')
        }
        else {
            item.parents("tr").remove()
        }

        //remove from property
        let listColor = $(".colorOption") //take option from table
        let listSize = $(".sizeOption")
        let wrapSpanColor = $(".wrap-color > span")
        let wrapSpanSize = $(".wrap-size > span")

        //kiểm tra từng color ở property nếu vẫn tồn tại ở variation thì không xóa ngược lại sẽ xóa
        wrapSpanColor.each((index2, valueMethod) => {
            let checkColor = false
            listColor.each((index, value) => {
                if ($(value).text().trim() == $(valueMethod).text().trim()) {
                    checkColor = true
                    return false
                }
            })
            if (!checkColor) {
                $(valueMethod).remove() //xóa color trên property
                let idColor = $(valueMethod).attr("data-idColor")
                deleteImgBox(idColor) //xóa boximg có idColor
            }
        })

        //kiểm tra từng size ở property nếu vẫn tồn tại ở variation thì không xóa ngược lại sẽ xóa
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
}

$(".box-options").click((e) => {
    let item = $(e.target)
    deleteVariation(item)
})


/**uppload file */
var clickFile = document.createElement('div') //lưu thẻ bọc 1 ảnh khi click để truy xuất đến các phẩn tử con
function fileBoxClick(input) {
    clickFile = input
}

//Hiển thị hình ảnh vừa chọn lên view
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

function dropImg(event) {
    event.preventDefault();
    event.stopPropagation();
    this.file = event.dataTransfer.files[0];
    event.target.files = event.dataTransfer.files
    const reader = new FileReader();
    reader.onload = e => {
        this.url = reader.result;
        
        const dropDiv = $(event.target).parents(".wrap-file-box")
        $(dropDiv).find('.file-upload-image').attr('src', this.url);
        $(dropDiv).find('.file-upload-content').show();
        $(dropDiv).find('.image-upload-wrap').hide();
    };
    reader.readAsDataURL(this.file);
}

function DragEnter(event) {
    event.stopPropagation();
    event.preventDefault();
    const parent = $(event.target).parents(".image-upload-wrap")
    $(parent).find(".text-dragenter").show()
    $(parent).find(".text-nodragenter").hide()
    $(parent).css("border", "2px dashed #0a58ca")
}


function dragLeaveBox(event) {
    event.stopPropagation();
    event.preventDefault();
    const parent = $(event.target).parents(".image-upload-wrap")
    $(parent).find(".text-dragenter").hide()
    $(parent).find(".text-nodragenter").show()
    $(parent).css("border", "2px dashed #999")

}


function removeImg(input) {
    let parentBox = $(input).parents(".wrap-file-box")
    let file = $(parentBox).find('.input-file')
    $(file).val('')
    let Image = $(parentBox).find('.file-upload-image')
    $(Image).attr("src","")
    $(parentBox).find('.file-upload-content').hide();
    $(parentBox).find('.image-upload-wrap').show();
}

$('.image-upload-wrap').bind('dragover', function () {
    $('.image-upload-wrap').addClass('image-dropping');
});
$('.image-upload-wrap').bind('dragleave', function () {
    $('.image-upload-wrap').removeClass('image-dropping');
});



//validate form create and edit
$('#formProduct').validate({

    rules: {
        name: {
            required: true,
            maxlength: 300
        },
        importPrice: {
            min: 0
        },
        Price: {
            min: 0
        },
        ProCatId: {
            required: true
        }
    },
    messages: {
        name: {
            required: "Bạn chưa nhập tên sản phẩm",
            maxlength: "Không được nhập quá 300 kí tự"
        },
        importPrice: {
            min: "Giá nhập phải lớn hơn 0"
        },
        Price: {
            min: "Giá bán phải lớn hơn 0"
        },
        ProCatId: {
            required: "Bạn chưa chọn loại sản phẩm"
        }
    }
})