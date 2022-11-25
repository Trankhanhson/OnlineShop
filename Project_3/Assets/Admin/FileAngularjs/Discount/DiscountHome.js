var discountApp = angular.module("discountApp", ['angularUtils.directives.dirPagination']);

discountApp.controller("discountController", function ($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = '5'

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách loại sản phẩm*/
        $http.get("/Admin/Discount/getPageData", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data.result)
            $scope.discountList = pageData.Data
            $scope.totalCount = pageData.TotalCount
        })
    }
    $scope.getPage(1)

    $scope.showDetail = function (id) {
        $http({
            method: "GET",
            url: "/Admin/Discount/getById/" + id,
            datatype: 'Json'
        }).then(function (res) {
            $scope.Discount = JSON.parse(res.data)

            for (let i = 0; i < $scope.Discount.DiscountDetails.length; i++) {
                $scope.Discount.DiscountDetails[i].TypeAmount = $scope.Discount.DiscountDetails[i].TypeAmount == "0" ? "đ" : "%"
            }
        })
    }

    $scope.Delete = function (index, item) {
        if (confirm(`Bạn có muốn hủy chương trình ${item.Name}`)) {
            $http({
                method: "GET",
                url: "/Admin/Discount/Delete/" + item.DiscountProductId,
                dataType: 'Json'
            }).then(function (res) {
                if (res.data) {
                    $scope.discountList.splice(index, 1)
                    $("#successToast .text-toast").text(`Chương trình ${item.Name} đã được hủy`)
                    $("#successToast").toast("show")
                }
                else {
                    $("#erorrToast .text-toast").text(`Không thể hủy `)
                    $("#erorrToast").toast("show")
                }
            })
        }
    }
})