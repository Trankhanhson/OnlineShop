function showVariation(input) {
    let proId = $(input).attr("data-proId")
    $(`.row-variation-${proId}`).toggle()
}
function showImportDetail(idPro) {
    $(`.row-importDetail-${idPro}`).toggle()
}

function addActive(input) {
    $(input).toggleClass("active")
}

function selectProduct(input) {
    let idPro = $(input).attr("data-proId")
    let wrapVariations = $(`.row-variation-${idPro}`)
    let listSelected = $(wrapVariations).find(".variation-item.active") //các phần tử được chọn

    //lấy thông tin của product
    let proName = $(`.row-product-${idPro} .col-product-name`).text()
    let importPrice = $(`.row-product-${idPro} .col-product-importPrice`).attr("data-importPrice")
    let price = $(`.row-product-${idPro} .col-product-price`).attr("data-price")

    //tạo chuỗi để thêm vào importTable
    if (listSelected.length > 0) {
        //kiểm tra phần tử select đã tồn tại chưa
        let rowImportDetailId = $(`.row-importDetail-${idPro}`)

        if (rowImportDetailId.length == 0) //nếu sản phẩm này chưa tồn tại ở import table thì sẽ add mới
        {
            let listVariation = ``
            listSelected.each((index, value) => {
                let idVariation = $(value).attr("data-idVariation")
                let srcImg = $($(value).find(".variation-image")).attr("src")
                let sizeValue = $($(value).find(".sizeValue")).text()
                let colorValue = $($(value).find(".colorValue")).text()
                listVariation += `<ul class=" d-flex mb-0 importDetail-item" data-idVariation="${idVariation}">
                                <li class="col-name  ps-4">
                                    <div class="d-flex py-1 ps-3 align-items-start border-bottom border-start h-100">
                                        <img src="${srcImg}" class="me-2" alt="" width="30px">
                                        <div class="d-flex align-items-center">
                                            <span class="item-size">${sizeValue}</span>
                                            <span>&nbsp;/&nbsp;</span>
                                            <span >${colorValue}</span>
                                        </div>
                                    </div>
                                </li>
                                <li class="col-quantity p-2 d-flex align-items-center border-bottom"> <input type="number" class="input-quantity" onchange="countSubTotal(this,${importPrice})" value="1"></li>
                                <li class="col-importPrice p-2 border-bottom"></li>
                                <li class="col-price p-2 border-bottom"></li>
                                <li class="col-subtotal p-2 border-bottom" data-subTotal="${importPrice}">${importPrice}</li>
                                <li class="border-bottom col-action p-2"><a class="btn-delete" onclick="deleteImportDetail(this)"><i class="fa-solid fa-trash" style="pointer-events:none;"></i></a></li>
                            </ul>`
            })

            //chuỗi cần thêm vào import table
            let result = `<tr class="row-product row-product-import-${idPro}" data-proId="1" onclick="showImportDetail(${idPro})">
                            <td class="col-name">${proName}</td>
                            <td class="col-quantity"></td>
                            <td class="col-importPrice">
                                <input type="number" class="input-price" onclick="stopPropagation(event)" onchange="changeImportPrice(this,${idPro})" value="${importPrice}">
                            </td>
                            <td class="col-price">
                                <input type="number" class="input-price" onclick="stopPropagation(event)" value="${price}">
                            </td>
                            <td class="col-subtotal price"></td>
                            <td class="col-action">
                                <a class="btn-delete" onclick="deleteProductImport(${idPro})"><i class="fa-solid fa-trash" style="pointer-events:none;"></i></a>
                            </td>
                        </tr>
                        <tr class="row-importDetail-${idPro} row-importDetail">
                            <td colspan="10" class=" pb-4" style="padding: 0;">
                                    ${listVariation}
                            </td>
                        </tr>`

            //thêm sản phẩm vừa chọn vào import table
            $(".table-importBill tbody").append(result)
        }
        else {
            //lấy danh sách idVariation đã được thêm
            let listVariationImportId = []
            let listDetailElement = $(rowImportDetailId).find(".importDetail-item")
            listDetailElement.each((index, value) => {
                let id = $(value).attr("data-idVariation")
                listVariationImportId.push(id)
            })

            let result = ``
            listSelected.each((index, value) => {
                let idVariation = $(value).attr("data-idVariation")
                if (listVariationImportId.indexOf(idVariation) === -1) //nếu không tồn tại thì thêm sang import table
                {
                    let srcImg = $($(value).find(".variation-image")).attr("src")
                    let sizeValue = $($(value).find(".sizeValue")).text()
                    let colorValue = $($(value).find(".colorValue")).text()
                    result += `<ul class=" d-flex mb-0 importDetail-item " data-idVariation="${idVariation}">
                                    <li class="col-name  ps-4">
                                        <div class="d-flex py-1 ps-3 align-items-start border-bottom border-start h-100">
                                            <img src="${srcImg}" class="me-2" alt="" width="30px">
                                            <div class="d-flex align-items-center">
                                                <span class="item-size">${sizeValue}</span>
                                                <span>&nbsp;/&nbsp;</span>
                                                <span >${colorValue}</span>
                                            </div>
                                        </div>
                                    </li>
                                    <li class="col-quantity p-2 d-flex align-items-center border-bottom"> <input type="number" class="input-quantity" onchange="countSubTotal(this,${importPrice})" value="1"></li>
                                    <li class="col-importPrice p-2 price border-bottom"></li>
                                    <li class="col-price p-2 price border-bottom"></li>
                                    <li class="col-subtotal p-2 border-bottom" data-subTotal="${importPrice}">${importPrice}</li>
                                    <li class="border-bottom col-action p-2"><a class="btn-delete" onclick="deleteImportDetail(this)"><i class="fa-solid fa-trash" style="pointer-events:none;"></i></a></li>
                                </ul>`
                }
            })

            //them importdetail
            $($(rowImportDetailId).children("td")).append(result)
        }
    }
    $(`.row-variation-${idPro}`).find(".variation-item").removeClass("active")
}

//delete import item
function deleteImportDetail(input) {
    let parent = $(input).parents(".importDetail-item")
    $(parent).remove()
}
function deleteProductImport(idPro) {
    $(`.row-product-import-${idPro}`).remove()
    $(`.row-importDetail-${idPro}`).remove()
}

function countTotalBill() {
    let listSubtotal = $(".importDetail-item .col-subtotal")
    let toTalBill = 0
    listSubtotal.each((index, value) => {
        let subtotal = JSON.parse($(value).text())
        toTalBill += subtotal
    })
    $("#totalImportBill").text(toTalBill)
}
countTotalBill()

function countSubTotal(input,importPrice) {
    let quantity = $(input).val()
    let subtotal = importPrice * quantity
    let subTotalElement = $($(input).parents(".importDetail-item")).find(".col-subtotal")
    $(subTotalElement).text(subtotal) //gán text bằng subtotal
    countTotalBill()
}

function changeImportPrice(input, proId) {
    let newImportPrice = $(input).val()
    let listDetail = $(`.row-importDetail-${proId} .importDetail-item`)
    listDetail.each((index, value) => {
        let quantity = $($(value).find(".input-quantity")).val()
        $($(value).find(".col-subtotal")).text(quantity*newImportPrice)
    })
    countTotalBill()
}

function stopPropagation(event) {
    event.stopPropagation()
}