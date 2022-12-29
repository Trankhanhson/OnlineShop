var OrderApp = angular.module("OrderApp", ['angularUtils.directives.dirPagination']);

OrderApp.controller("OrderController", function ($scope, $http) {
    let statusId = $("main").attr("data-statusId")
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách đơn hàng*/
        $http.get("/Admin/Order/getPageData", {
            params: { statusId: statusId, searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.OrderList = pageData.Data
            $scope.totalCount = pageData.TotalCount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getPage(1)

    let indexChange = 0
    $scope.showDetail = function (index, id) {
        indexChange = index
        $http({
            method: "GET",
            url: "/Admin/Order/getOrderById/" + id,
            datatype: 'Json'
        }).then(function (res) {
            $scope.Order = JSON.parse(res.data)
        })
    }

    $scope.ChangeStatus = function (id) {
        if (confirm(`Bạn có muốn duyệt đơn hàng mã ${id}`)) {
            $http({
                method: "GET",
                url: "/Admin/Order/ChangeStatus/" + id,
                dataType: 'Json'
            }).then(function (res) {
                $scope.OrderList.splice(indexChange, 1)
                $(".modal-footer .close-modal").trigger("click")
                $("#successToast .text-toast").text(`Đơn hàng ${id} đã được chuyển sang trạng thái ${res.data}`)
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

    $scope.Delete = function (index, o) {
        if (confirm(`Bạn có muốn xóa đơn hàng mã ${o.OrdID}`)) {
            $http({
                method: "GET",
                url: "/Admin/Order/Delete/" + o.OrdID,
                dataType: 'Json'
            }).then(function (res) {
                if (res.data) {
                    $scope.OrderList.splice(index, 1)
                    $("#successToast .text-toast").text(`Đơn hàng ${o.OrdID} đã được xóa`)
                    $("#successToast").toast("show")
                }
                else {
                    $("#erorrToast .text-toast").text(`Không thể xóa đơn hàng ${o.OrdID}`)
                    $("#erorrToast").toast("show")
                }
            })
        }
    }

});