var importBillApp = angular.module("importBillApp", [])
importBillApp.controller("importBillController", importBillController)

function importBillController($scope, $http) {

    $scope.searchText = ""
    $scope.productImport = []

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        $http.get("/Admin/Product/getSearchData", {
            params: { searchText: $scope.searchText }
        }).then(function (res) {
            if (res.data.check) {
                $scope.products = JSON.parse(res.data.result)
            }
            else {
                $scope.products = []
            }
        }, function (error) {
            alert("failed")
        })
    }
    $scope.saveClick = function () {

        if ($scope.productImport.length > 0) {
            let ImportBill = {
                StaffId: $("#StaffId").val(),
                MoneyTotal: $scope.totalBill
            }

            let listProduct = []
            let billDetails = []
            $.each($scope.productImport, (index, value) => {
                let Product = {
                    ProId: value.ProId,
                    Price: value.Price,
                    ImportPrice: value.ImportPrice
                }
                listProduct.push(Product)

                //lấy billdetail
                $.each(value.listVariation, (i, v) => {
                    let detail = {
                        ProVariationID: v.ProVariationID,
                        Quantity: v.Quantity,
                        ImportPrice: value.ImportPrice
                    }
                    billDetails.push(detail)
                })
            })

            $http({
                method: "Post",
                url: "/Admin/ImportBill/Create",
                datatype: 'Json',
                data: { ImportBill: ImportBill, listProduct: listProduct, billDetails: billDetails }
            }).then(function (res) {
                if (res.data) {
                    location.reload()
                    //hiển thị thông báo thành công
                    $("#successToast .text-toast").text("Thêm hóa đơn nhập thành công")
                    $("#successToast").toast("show")
                }
                else {
                    $("#errorToast .text-toast").text("Thêm thất bại")
                    $("#errorToast").toast("show")
                }
            }), function (error) {
                $("#errorToast .text-toast").text("Thêm thất bại")
                $("#errorToast").toast("show")
            }
        }
        else {
            $("#errorToast .text-toast").text("Vui lòng chọn sản phẩm")
            $("#errorToast").toast("show")
        }
    }

    $scope.selectProduct = function (p) {

        let listElement = $($(`.row-variation-${p.ProId}`)).find(".variation-item.active")
        if (listElement.length > 0) {
            //chuyển các product và variation thành productDetail
            let listDetail = []
            listElement.each((index, value) => {
                let variationSelected = $(value).attr("data-idVariation")
                for (let i = 0; i < p.ProductVariations.length; i++) {
                    if (p.ProductVariations[i].ProVariationID == variationSelected) {
                        let detail = {
                            ProId: p.ProId,
                            ProVariationID: p.ProductVariations[i].ProVariationID,
                            DisplayImage: p.ProductVariations[i].DisplayImage,
                            NameSize: p.ProductVariations[i].ProductSize.NameSize,
                            NameColor: p.ProductVariations[i].ProductColor.NameColor,
                            Quantity: 1,
                            SubTotal: p.Price
                        }
                        listDetail.push(detail)
                    }
                }
            })

            let productDetail = {
                ProId: p.ProId,
                ProName: p.ProName,
                ImportPrice: p.ImportPrice,
                Price: p.Price,
                listVariation: listDetail
            }

            //kiểm tra sản phẩm vừa chọn đã đc chọn chưa
            let pResult = $scope.productImport.find(pi => pi.ProId == p.ProId)

            if (pResult !== undefined) {
                let length = productDetail.listVariation.length
                for (let i = 0; i < length; i++) {
                    let exist = pResult.listVariation.some(p => p.ProVariationID == productDetail.listVariation[i].ProVariationID)
                    if (exist == false) {
                        pResult.listVariation.push(productDetail.listVariation[i])
                    }
                }
            }
            else {

                $scope.productImport.push(productDetail)
            }
            //bỏ active của sp đã select
            $(listElement).removeClass("active")
            $scope.countTotalBill()
        }
    }

    $scope.totalBill = 0
    $scope.countTotalBill = function () {
        let totalBill = 0
        for (let i = 0; i < $scope.productImport.length; i++) {
            let p = $scope.productImport[i]
            for (let j = 0; j < p.listVariation.length; j++) {
                totalBill += (p.ImportPrice * p.listVariation[j].Quantity)
            }
        }

        $scope.totalBill = totalBill
    }

    $scope.DeleteProImport = function (index) {
        $scope.productImport.splice(index, 1)
        $scope.countTotalBill()
    }

    $scope.DeleteDetail = function (indexPro, indexDetail) {
        let p = $scope.productImport[indexPro]
        if (p.listVariation.length === 1) {
            $scope.productImport.splice(indexPro, 1)
        }
        else {
            p.listVariation.splice(indexDetail, 1)
        }
        $scope.countTotalBill()
    }
}