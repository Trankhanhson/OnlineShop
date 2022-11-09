var userApp = angular.module("UserApp", ['angularUtils.directives.dirPagination']);

userApp.controller("UserController", UserController);

function UserController($scope, $http) {


    /** Lấy danh sách category*/
    $http.get("/Admin/User/getAllData").then(function (res) {
        $scope.listUser = JSON.parse(res.data)
    }, function (error) {
        alert("failed")
    })

    /**Thêm danh mục */

    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.user = null
        
    }

    //khi người dung nhấn lưu thêm mới danh mục
    $scope.SaveAdd = function (closeOrNew) {
        if ($scope.createForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/User/Create",
                datatype: 'Json',
                data: { user: $scope.user }
            }).then(function (res) {
                if (res.data.message) {
                    $scope.listUser.push(res.data.u) //hiển thị thêm đối tượng vừa thêm

                    //nếu người dùng chỉ nhấn lưu
                    if (closeOrNew) {
                        $(".btn-close").trigger('click') //đóng modal thêm
                    }
                    else //nếu người dùng nhấn lưu và thêm mới
                    {
                        $scope.user = null;
                    }

                    //hiển thị thông báo thành công
                    $("#successToast .text-toast").text("Thêm nhân viên thành công")
                    $("#successToast").toast("show")
                }
                else {
                    $("#errorToast .text-toast").text("Thêm thất bại")
                    $("#errorToast").toast("show")
                }
            })
        }

    }

    /** Sửa danh mục*/

    let indexEdit = 1 //biến chứa vị trí vừa sửa

    $scope.Edit = function (u, index) {
        //nếu gán thẳng thì nó sẽ thay đổi luôn ở view trong khi chưa sửa
        $scope.user = { UserID: u.UserID, Name: u.Name, UserName: u.UserName, Password: u.Password, UserAdress: u.UserAdress, UserPhone: u.UserPhone, Status: u.Status }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        if ($scope.editForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/User/Edit",
                datatype: 'Json',
                data: { user: $scope.user }
            }).then(function (res) {
                if (res.data) {
                    //Tìm phần tử vừa được sửa trong danh sách
                    var newUser = $scope.user
                    $scope.listUser.splice(indexEdit, 1, newUser)
                    $("#successToast .text-toast").text("Sửa nhân viên thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text("Sửa thất bại")
                    $("#errorToast").toast("show") //hiển thị thông báo thành công
                }
                $(".btn-close").trigger('click') //đóng modal sửa
            })
        }

    }

    /**Xóa danh mục*/
    $scope.Delete = function (u) {
        if (confirm(`Bạn có chắc chắn muốn xóa nhân viên này không`)) {
            $http({
                method: 'Post',
                url: '/Admin/User/Delete',
                dataType: 'Json',
                data: { id: u.UserID }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.listUser.indexOf(u);
                    $scope.listUser.splice(c, 1);
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
    $scope.ChangeStatus = function (idUser) {
        if (confirm("Bạn có chắc chắn muốn thay đổi trạng thái")) {
            $http({
                method: "Post",
                url: "/Admin/User/ChangeStatus",
                dataType: 'Json',
                data: { id: idUser }
            }).then(function (res) {
                if (res.data) {
                    $("#successToast .text-toast").text("Đã cập nhật trạng thái thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text("Cập nhật trạng thái thất bại")
                    $("#errorToast").toast("show")
                }
            })
        }
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
}