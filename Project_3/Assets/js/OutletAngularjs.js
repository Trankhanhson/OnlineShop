
ClientApp.controller("OutletController", OutletController);

function OutletController($scope, $http) {
    $scope.Object = 'All'
    $scope.MinMoney = 99000
    $scope.MaxMoney = 149000

    $scope.getData = function () {
        /** Lấy danh sách category*/
        $http.get("/Outlet/FilterOutlet", {
            params: { o: $scope.Object, minMoney: $scope.MinMoney, maxMoney: $scope.MaxMoney }
        }).then(function (res) {
            $scope.listOutlet = JSON.parse(res.data)
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getData()

    $scope.changeObject = function (o) {
        $scope.Object = o
        $scope.getData()
    }

    $scope.changeMoney = function (minMoney, maxMoney) {
        $scope.MinMoney = minMoney
        $scope.MaxMoney = maxMoney
        $scope.getData()
    }

    $scope.addActive = function (e) {
        let parent = $(e.target).parents(".product-size-wrap")
        let listItem = $(parent).find(".product-size")
        $(listItem).removeClass("active")
        $(e.target).addClass("active")
    }
    $scope.LikeProduct = function (e, ProId) {
        let CusId = readCookie("CustomerId")
        if (CusId != null) {
            $http({
                method: "POST",
                url: "/InfoCustomer/ProductLike",
                datatype: 'Json',
                data: { ProId: ProId, CusId: CusId }
            }).then(function (res) {
                if (res.data.check) {
                    if (res.data.IsAdd) {
                        alertSuccess("Đã thêm sản phẩm vào danh sách yêu thích")
                    }
                    else {
                        alertSuccess("Đã xóa sản phẩm vào danh sách yêu thích")
                    }
                    $(e.target).toggleClass("active")
                }
                else {
                    alertError("Đã có lỗi xảy ra")
                }
            })
        }
        else {
            alertError("Bạn chưa đăng nhập tài khoản")
        }
    }
}