var NewApp = angular.module("NewApp", [ 'angularUtils.directives.dirPagination']);

NewApp.controller("NewController", function ($scope, $http) {

    /** Lấy danh sách loại sản phẩm*/
    $http.get("/Admin/New/getAllData").then(function (res) {
        $scope.NewList = JSON.parse(res.data.result)
        $scope.firstUserID = res.data.firstUserID
    }, function (error) {
        alert("failed")
    })

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


    //sắp xếp
    $scope.sortColumn = 'Name'
    $scope.reverse = 'false'
    $scope.SortData = function (column) {
        if ($scope.sortColumn == column) {
            $scope.reverse = !$scope.reverse
        }
        else {
            $scope.reverse = false //sort increase
        }
        $scope.sortColumn = column
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