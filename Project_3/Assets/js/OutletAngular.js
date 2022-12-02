
ClientApp.controller("OutletController", OutletController);

function OutletController($scope, $http) {
    $scope.Object = 'All'
    $scope. MinMoney = 99
    $scope.MaxMoney = 149

    $scope.getData = function () {
        /** Lấy danh sách category*/
        $http.get("/Admin/Outlet/FilterOutlet", {
            params: { o: $scope.Object, minMoney: $scope.MinMoney, maxMoney: $scope.MaxMoney }
        }).then(function (res) {
            $scope.listOutlet = JSON.parse(res.data)
        }, function (error) {
            alert("failed")
        })
    }

    $scope.changeObject = function (o) {
        $scope.Object = o
        $scope.getData()
    }

    $scope.changeMoney = function (minMoney,maxMoney) {
        $scope.MinMoney = minMoney
        $scope.MaxMoney = maxMoney
        $scope.getData()
    }


}