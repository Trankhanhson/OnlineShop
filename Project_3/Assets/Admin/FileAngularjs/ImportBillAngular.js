var importBillApp = angular.module("importBillApp", ['angularUtils.directives.dirPagination'])
importBillApp.controller("importBillController", importBillController)

function importBillController($scope, $http) {
    $http.get("/Admin/Product/getAllData").then(function (res) {
        $scope.products = JSON.parse(res.data)
        console.log($scope.products)
    }, function (error) {
        alert("failed")
    })

    $scope.saveClick = function () {

    }
}