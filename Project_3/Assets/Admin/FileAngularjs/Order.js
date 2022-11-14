var OrderApp = angular.module("OrderApp", ['angularUtils.directives.dirPagination']);

OrderApp.controller("OrderController", function ($scope, $http) {
    let statusId = $("main").attr("data-statusId")
    /** Lấy danh sách loại sản phẩm*/
    $http.get("/Admin/Order/getAllData/" + statusId).then(function (res) {
        $scope.OrderList = JSON.parse(res.data.result)
    }, function (error) {
        alert("failed")
    })

    $scope.showDetail = function (id) {
        $http({
            method: "GET",
            url: "/Admin/Order/getOrderById/" + id,
            datatype: 'Json'
        }).then(function (res) {
            $scope.Order = JSON.parse(res.data)
        })
    }

    $scope.getSortClass = function (column) {
        //khi reverse thay doi thi nd-class dc kich hoat
        if ($scope.sortColumn == column) {
            return $scope.reverse ? 'fa-solid fa-arrow-down' : 'fa-solid fa-arrow-up'
        }
        return ''
    }

    //paging
    $scope.pageSize = "5"
    $scope.getPageSize = function (pageSize) {
        $scope.pageSize = pageSize
    }

});