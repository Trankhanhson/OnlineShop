var PositionApp = angular.module("PositionApp", ['angularUtils.directives.dirPagination']);

PositionApp.controller("PositionController", PositionController);

function PositionController($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách category*/
        $http.get("/Admin/Position/getPageData", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.positions = pageData.Data
            $scope.totalCount = pageData.TotalCount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getPage(1)

    /**Thêm danh mục */

    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.category = null
        $scope.category = { type: "Nam", Status: true }  //Gán cho select bằng nam
    }

    //khi người dung nhấn lưu thêm mới danh mục
    $scope.SaveAdd = function (closeOrNew) {
        if ($scope.createForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/Category/Create",
                datatype: 'Json',
                data: { category: $scope.category }
            }).then(function (res) {
                if (res.data.message) {
                    $scope.categories.push(res.data.cat) //hiển thị thêm đối tượng vừa thêm

                    //nếu người dùng chỉ nhấn lưu
                    if (closeOrNew) {
                        $(".btn-close").trigger('click') //đóng modal thêm
                    }
                    else //nếu người dùng nhấn lưu và thêm mới
                    {
                        $scope.category = null;
                        $scope.category = { type: "Nam", Status: true }  //Gán cho select bằng nam
                    }

                    //hiển thị thông báo thành công
                    $("#successToast .text-toast").text("Thêm danh mục thành công")
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

    $scope.Edit = function (cat, index) {
        //nếu gán thẳng thì nó sẽ thay đổi luôn ở view trong khi chưa sửa
        $scope.category = { CatID: cat.CatID, Name: cat.Name, type: cat.type, Status: cat.Status }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        if ($scope.editForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/Category/Edit",
                datatype: 'Json',
                data: { category: $scope.category }
            }).then(function (res) {
                if (res.data) {
                    //Tìm phần tử vừa được sửa trong danh sách
                    var newCat = $scope.category
                    $scope.categories.splice(indexEdit, 1, newCat)
                    $("#successToast .text-toast").text("Sửa danh mục thành công")
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
    $scope.Delete = function (cat) {
        if (confirm(`Bạn có chắc chắn muốn xóa danh mục ${cat.Name}`)) {
            $http({
                method: 'Post',
                url: '/Admin/Category/Delete',
                dataType: 'Json',
                data: { id: cat.CatID }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.categories.indexOf(cat);
                    $scope.categories.splice(c, 1);
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


}