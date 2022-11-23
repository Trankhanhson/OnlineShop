var ProductApp = angular.module("ProductApp", ['angularUtils.directives.dirPagination'])
ProductApp.controller("ProductController", ProductController)

function ProductController($scope, $http) {

    $scope.maxSize = 5;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = '5'

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách loại sản phẩm*/
        $http.get("/Admin/Product/getPageData",
            { params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize } }).then(function (res) {
                let pageData = JSON.parse(res.data.result)
                $scope.products = pageData.Data
                $scope.totalCount = pageData.TotalCount
            }, function (error) {
                alert("failed")
            })
    }

    $scope.getPage(1)

    $scope.showVariation = function (proId) {
        $(`.row-variation-${proId}`).toggle()
        $(`.row-product-${proId}`).toggleClass("active")
    }
}