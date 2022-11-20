var importBillApp = angular.module("importBillApp", ['angularUtils.directives.dirPagination'])
importBillApp.controller("importBillController", importBillController)

function importBillController($scope, $http) {
    $http.get("/Admin/Product/getAllData").then(function (res) {
        $scope.products = JSON.parse(res.data)
    }, function (error) {
        alert("failed")
    })

    $scope.saveClick = function () {

        let ImportBill = {
            StaffId: $("#StaffId").val(),
            ImpDate: $("#ImpDate").val(),
            MoneyTotal: JSON.parse($("#totalImportBill").text())
        }

        let listProduct = []
        let rowProductElement = $(".table-importBill .row-product")
        rowProductElement.each((index, value) => {
            let proId = $(value).attr("data-proId")
            let price = $($(value).find(".col-price input")).val()
            let importPrice = $($(value).find(".col-importPrice input")).val()
            let Product = {
                ProId: proId,
                Price: price,
                ImportPrice: importPrice
            }
            listProduct.push(Product)
        })

        if (listProduct.length > 0) {
            /*let ProVariationID Quantity*/
            let billDetails = []
            $(".importDetail-item").each((index, value) => {
                let ProVariationID = $(value).attr("data-idVariation")
                let Quantity = $($(value).find(".input-quantity")).val()
                let detail = {
                    ProVariationID: ProVariationID,
                    Quantity: Quantity
                }
                billDetails.push(detail)
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


    }
}