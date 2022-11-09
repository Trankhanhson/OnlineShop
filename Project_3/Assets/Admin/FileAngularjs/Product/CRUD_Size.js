productApp.controller("SizeController", SizeController)
function SizeController($scope, $http) {

    $scope.addValue = function (e) {
        addValue(e) //hàm được viết ở file product.js
    }

    /** Lấy danh sách category*/
    $http.get("/Admin/Size/getAllData").then(function (res) {
        $scope.ListSize = JSON.parse(res.data)
    }, function (error) {
        alert("Lỗi khi tải dữ liệu")
    })

    /**Thêm kích thước */
    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.proSize = null
    }

    //khi người dung nhấn lưu thêm mới kích thước
    $scope.SaveAdd = function () {
        if ($scope.createSizeForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/Size/Create",
                datatype: 'Json',
                data: { proSize: $scope.proSize }
            }).then(function (res) {
                if (res.data.message) {
                    $scope.ListSize.push(res.data.newPz) //hiển thị thêm đối tượng vừa thêm

                    $(".btn-closeModel").trigger('click') //đóng modal

                    //hiển thị thông báo thành công
                    $("#successToast .text-toast").text("Thêm kích thước thành công")
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

    $scope.Edit = function (size, index, e) {
        e.stopPropagation()
        //nếu gán thẳng thì nó sẽ thay đổi luôn ở view trong khi chưa sửa
        $scope.proSize = { ProSizeID: size.ProSizeID, NameSize: size.NameSize }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        if ($scope.editSizeForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/Size/Edit",
                datatype: 'Json',
                data: { proSize: $scope.proSize }
            }).then(function (res) {
                if (res.data) {
                    //Tìm phần tử vừa được sửa trong danh sách
                    var newProSize = $scope.proSize
                    $scope.ListSize.splice(indexEdit, 1, newProSize)
                    $("#successToast .text-toast").text("Sửa kích thước thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text("Sửa thất bại")
                    $("#errorToast").toast("show") //hiển thị thông báo thành công
                }
                $(".btn-closeModel").trigger('click') //đóng modal
            })
        }

    }

    /**Xóa danh mục*/
    $scope.Delete = function (pz) {
        if (confirm(`Bạn có chắc chắn muốn xóa kích thước ${pz.NamSize}`)) {
            $http({
                method: 'Post',
                url: '/Admin/Size/Delete',
                dataType: 'Json',
                data: { id: pz.ProSizeID }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.ListSize.indexOf(pz);
                    $scope.ListSize.splice(c, 1);
                    $("#successToast .text-toast").text(res.data.message)
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text(res.data.message)
                    $("#errorToast").toast("show")
                }
                $(".btn-closeModel").trigger('click') //đóng modal
            })
        }
    }
}