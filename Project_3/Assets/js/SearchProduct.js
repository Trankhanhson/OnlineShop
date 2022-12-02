
ClientApp.controller("searchProController", searchProController);

function searchProController($scope, $http) {
    $scope.searchText = ""
    $scope.emptyProduct = true;
    $scope.getData = function () {
        /** Lấy danh sách loại sản phẩm*/
        $http.get("/Admin/Product/getSearchDataClient", {
            params: { searchText: $scope.searchText }
        }).then(function (res) {
            if (res.data.check) {
                $scope.productResult = JSON.parse(res.data.result)
                
            }
            else {
                $scope.productResult = []
            }

            if ($scope.productResult.length == 0) {
                $scope.emptyProduct = true
            } else {
                $scope.emptyProduct = false
            }
        })
    }
}