

var discountApp = angular.module("discountApp", ['angularUtils.directives.dirPagination']);

discountApp.controller("productController", function ($scope, $http) {
    /** Lấy danh sách loại sản phẩm*/
    $http.get("/Admin/Product/getProductOnly").then(function (res) {
        $scope.productList = JSON.parse(res.data)
    }, function (error) {
        alert("failed")
    })

    $scope.productSelected = []
    $scope.selectProduct = function (item) {
        if ($scope.productSelected.indexOf(item) == -1) {
            $scope.productSelected.push(item)
        }
    }

    $scope.listProConfirmed = []
    $scope.confirmPro = function () {
        if ($scope.productSelected.length > 0) {
            $scope.listProConfirmed = $scope.productSelected
            $scope.checkProduct = true //display table
            $(".btn-close").trigger("click")
        }
        else {
            $("#errorToast .text-toast").text("Bạn chưa chọn sản phẩm nào")
            $("#errorToast").toast("show")
        }
    }

    $scope.deleteProductSelect = function (index) {
        if ($scope.listProConfirmed.length == 1) {
            $scope.listProConfirmed.splice(index, 1)
            $scope.checkProduct = false //display table
        }
        else {
            $scope.listProConfirmed.splice(index, 1)

        }
    }

    $scope.changePriceAfter = function (index,price) {
        let row = $(".product-" + index)
        let amountSale = JSON.parse($($(row).find(".amount-sale")).val())
        let typeSelect = $($(row).find(".TypeAmount")).val()
        let priceAfter = 0
        if (typeSelect == "1") {
            priceAfter = (price - (price * (amountSale/100)))
        }
        else {
            priceAfter = price - amountSale;
            
        }
        let result = convertPrice(JSON.stringify(priceAfter))
        $($(row).find(".priceAfter")).text(result)
    }

    $scope.submit = function () {
        if ($scope.discountForm.$valid) {
            let table = $(".table-product-selected tbody tr")
            let listDiscountDetail = []
            table.each((index, value) => {
                let discountDetail = {
                    ProId: $(value).attr("data-proid"),
                    Amount: $($(value).find(".amount-sale")).val(),
                    TypeAmount: $($(value).find(".TypeAmount")).val(),
                    MaxQuantityPerUser: $($(value).find(".maxOrder input")).val()
                }
                listDiscountDetail.push(discountDetail)
            })
            $http({
                method: "POST",
                url: "/Admin/Discount/Create",
                datatype: 'Json',
                data: { discountPro: $scope.discountPro, listDiscountDetail: listDiscountDetail }
            }).then(function (res) {
                if (res.data) {
                    $scope.discountPro = null;
                    $scope.listProConfirmed = null;
                    $scope.checkProduct = false;

                    $("#successToast .text-toast").text("Đã thêm chương tình khuyến mãi thành công")
                    $("#successToast").toast("show")
                }
                else {
                    $("#errorToast .text-toast").text("Thêm thất bại")
                    $("#errorToast").toast("show")
                }
            })
        }
    }

});


