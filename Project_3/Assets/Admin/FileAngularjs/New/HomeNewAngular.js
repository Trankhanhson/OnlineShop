var NewApp = angular.module("NewApp", ['angularUtils.directives.dirPagination']);

NewApp.controller("NewController", function ($scope, $http) {

    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách category*/
        $http.get("/Admin/New/getPageData", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.NewList = pageData.Data
            $scope.totalCount = pageData.TotalCount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getPage(1)

    /**Xóa danh mục*/
    $scope.Delete = function (n) {
        if (confirm(`Bạn có chắc chắn muốn xóa bài viết ${n.Title}`)) {
            $http({
                method: 'Post',
                url: '/Admin/New/Delete',
                dataType: 'Json',
                data: { n: n }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.NewList.indexOf(n);
                    $scope.NewList.splice(c, 1);
                    $("#successToast .text-toast").text(res.data.message)
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text(res.data.message)
                    $("#errorToast").toast("show")
                }
            })
        }
    }

    /** Change status*/
    $scope.ChangeStatusNew = function (newId) {
        $http({
            method: "Post",
            url: "/Admin/New/ChangeStatus",
            dataType: 'Json',
            data: { id: newId }
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

});