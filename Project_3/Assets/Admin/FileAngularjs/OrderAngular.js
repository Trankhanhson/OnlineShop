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

    $scope.ChangeStatus = function (index, o) {
        if (confirm(`Bạn có muốn thay đổi trạng thái đơn hàng mã ${o.OrdID}`)) {
            $http({
                method: "GET",
                url: "/Admin/Order/ChangeStatus/" + o.OrdID,
                dataType: 'Json'
            }).then(function (res) {
                $scope.OrderList.splice(index, 1)
                $("#successToast .text-toast").text(`Đơn hàng ${o.OrdID} đã được chuyển sang trạng thái ${res.data}`)
                $("#successToast").toast("show")
            })
        }
    }

    $scope.cancel = function (index, o) {
        if (confirm(`Bạn có muốn hủy đơn hàng mã ${o.OrdID}`)) {
            $http({
                method: "GET",
                url: "/Admin/Order/CancelOrder/" + o.OrdID,
                dataType: 'Json'
            }).then(function (res) {
                if (res.data) {
                    $scope.OrderList.splice(index, 1)
                    $("#successToast .text-toast").text(`Đơn hàng ${o.OrdID} đã được hủy`)
                    $("#successToast").toast("show")
                }
                else {
                    $("#erorrToast .text-toast").text(`Không thể hủy đơn hàng ${o.OrdID}`)
                    $("#erorrToast").toast("show")
                }
            })
        }
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