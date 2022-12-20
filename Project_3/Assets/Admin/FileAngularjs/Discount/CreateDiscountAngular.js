var discountApp = angular.module("discountApp", ['angularUtils.directives.dirPagination']);

discountApp.controller("discountController", function ($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = '5'

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách loại sản phẩm*/
        $http.get("/Admin/Discount/getProductOnly", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data.result)
            $scope.productList = pageData.Data
            //kiểm tra các sản phẩm đã được check khi trả về
            $.each($scope.productList, (index, value) => {
                let checkExist = $scope.listProConfirmed.some((v, i) => {
                    return value.ProId == v.ProId
                })
                if (checkExist) {
                    value.Check = true
                }
            })
            $scope.totalCount = pageData.TotalCount
        })
    }
    $scope.getPage(1)

    $scope.listProConfirmed = []
    $scope.selectProduct = function (value) {
        let indexExist = 0
        let checkExist = $scope.listProConfirmed.some((v, index) => {
            indexExist = index
            return value.ProId == v.ProId
        })
        if (checkExist == false) {
            let ProConfirm = {
                ProId: value.ProId,
                firstImage: value.firstImage,
                ProName: value.ProName,
                Price: value.Price,
                priceAfter: value.Price,
                Amount: 1,
                TypeAmount: "0",
                MaxQuantityPerUser: 1
            }
            $scope.listProConfirmed.push(ProConfirm)
            $scope.checkProduct = true
        }
        else {
            if (value.Check) {
                $scope.listProConfirmed.splice(indexExist, 1)
                if ($scope.listProConfirmed.length == 0) {
                    $scope.checkProduct = false
                }
            }
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

    $scope.changePriceAfter = function (item) {

        if (item.TypeAmount == "1") //trường hợp giảm giá bằng %
        {
            if (item.Amount > 100) {
                item.Amount = 100
                $("#errorToast .text-toast").text("Bạn không thể giảm giá lớn hơn 100%")
                $("#errorToast").toast("show")
            }
            item.priceAfter = (item.Price - (item.Price * (item.Amount / 100)))
        }
        else {
            if (item.Amount > item.Price) {
                item.Amount = item.Price
                $("#errorToast .text-toast").text("Bạn không thể giảm giá lớn hơn giá bán")
                $("#errorToast").toast("show")
            }
            item.priceAfter = item.Price - item.Amount;
        }
    }

    $scope.ChangeDiscountPrice = function () {
        let item = $scope.generalSetting
        if (item.TypeAmount == "1") //trường hợp giảm giá bằng %
        {
            if (item.Amount > 100) {
                item.Amount = 100
                $("#errorToast .text-toast").text("Bạn không thể giảm giá lớn hơn 100%")
                $("#errorToast").toast("show")
            }
        }
        else {
            //kiểm tra giá genaralSetting có lớn hơn giá của sản phẩm nào đã đc chọn không
            let min_val = $scope.listProConfirmed.reduce(function (accumulator, element) {
                return (accumulator > element.Price) ? element.Price : accumulator
            });

            if (item.Amount > min_val.Price) {
                item.Amount = min_val.Price
                $("#errorToast .text-toast").text("Giảm giá đã vượt qua giá của một số sản phẩm")
                $("#errorToast").toast("show")
            }
        }
    }

    //check startDate và endDate
    let checkDate = true
    $scope.checkDate = function (startDate, endDate) {
        checkDate = true
        $scope.errMessage = '';
        var curDate = new Date();

        if (new Date(startDate) > new Date(endDate)) {
            $("#errorToast .text-toast").text("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc")
            $("#errorToast").toast("show")
            checkDate = false
            return false;
        }
        if (new Date(startDate) < curDate) {
            $("#errorToast .text-toast").text("Thời gian bắt đầu phải lớn hơn thời gian hiện tại")
            $("#errorToast").toast("show")
            checkDate = false
            return false;
        }
    }

    $scope.submit = function () {
        if ($scope.listProConfirmed.length > 0) {
            if ($scope.discountForm.$valid) {
                $scope.checkDate($scope.discountPro.StartDate, $scope.discountPro.EndDate)
                if (checkDate) {
                    let listDiscountDetail = []
                    $.each($scope.listProConfirmed, (index, value) => {
                        let discountDetail = {
                            ProId: value.ProId,
                            Amount: value.Amount,
                            TypeAmount: value.TypeAmount
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
                            location.href = "/Admin/Discount/Index"

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
        }
        else {
            $("#errorToast .text-toast").text("Bạn chưa thêm sản phẩm nào")
            $("#errorToast").toast("show")
        }
    }

    $scope.generalSetting = { TypeAmount: "0" }
    $scope.setAll = function () {
        if ($scope.generalSetting.Amount != null || $scope.generalSetting.MaxQuantityPerUser != null) {
            if (confirm("Bạn có muốn thiết lập tất cả")) {
                $.each($scope.listProConfirmed, (index, value) => {
                    if ($scope.generalSetting.Amount != null) {
                        value.Amount = $scope.generalSetting.Amount
                        value.TypeAmount = $scope.generalSetting.TypeAmount

                        if (value.TypeAmount == "1") {
                            value.priceAfter = (value.Price - (value.Price * (value.Amount / 100)))
                        }
                        else {
                            value.priceAfter = value.Price - value.Amount;
                        }
                    }
                })
            }
        }
    }
});