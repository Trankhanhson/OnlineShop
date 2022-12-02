var CustomerApp = angular.module("CustomerApp", ['angularUtils.directives.dirPagination']);

CustomerApp.controller("CustomerController", CustomerController);

function CustomerController($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách category*/
        $http.get("/Admin/Customer/getPageData", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.customers = pageData.Data
            $scope.totalCount = pageData.TotalCount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getPage(1)

    /** Change status*/
    $scope.ChangeStatus = function (idCus) {
        if (confirm("Bạn có muốn thay đổi trạng thái của khách hàng này")) {
            $http({
                method: "Post",
                url: "/Admin/Customer/ChangeStatus",
                dataType: 'Json',
                data: { id: idCus }
            }).then(function (res) {
                if (res.data) {
                    $("#successToast .text-toast").text("Đã lưu thay đổi")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text("Không thể lưu thay đổi")
                    $("#errorToast").toast("show")
                }
            })
        }

    }

    
}