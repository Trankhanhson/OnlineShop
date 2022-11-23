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
})