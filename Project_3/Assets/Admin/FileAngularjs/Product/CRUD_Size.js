var productApp = angular.module("ProductApp", ['ngFileUpload']);

productApp.controller("SizeController", SizeController)
function SizeController($scope, $http) {


    /** Lấy danh sách category*/
    $http.get("/Admin/Size/getAllData").then(function (res) {
        $scope.ListSize = res.data
    }, function (error) {
        alert("Lỗi khi tải dữ liệu")
    })

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

productApp.controller("ProCatController", ProCatController);

function ProCatController($scope, Upload, $http) {

    //lưu file người dùng upload
    $scope.SelectImage = function (file) {
        $scope.fileImage = file
    }

    ///**Thêm loại sản phẩm */
    //hàm upload ảnh trả về true nếu thành công
    let checkUpload = true
    $scope.UploadFiles = function (file, proCatId) {
        $scope.SelectFiles = file;
        if ($scope.SelectFiles && $scope.SelectFiles.length) {
            Upload.upload({
                url: '/ProductCat/Upload',
                data: { file: $scope.SelectFiles, ProCatId: proCatId }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    $scope.errorImage = false
    $scope.SaveAdd = function () {
        if ($scope.createForm.$valid) {
            if ($scope.fileImage !== undefined) {
                $scope.proCat.Image = $scope.fileImage[0].name
                $http({
                    method: "POST",
                    url: "/Admin/ProductCat/Create",
                    datatype: 'Json',
                    data: { proCat: $scope.proCat }
                }).then(function (res) {
                    if (res.data.check) //tạo mới thành công
                    {
                        var productCat = JSON.parse(res.data.pc)
                        //upload ảnh khi thêm đối tượng thành công
                        $scope.UploadFiles($scope.fileImage, productCat.ProCatId)
                        if (checkUpload === false) {
                            //khi upload fail thì xóa đối tượng vừa tạo
                            $http({
                                method: 'Post',
                                url: '/Admin/ProductCat/Delete',
                                data: { id: productCat.ProCatId }
                            })
                        }
                        else {

                        }

                        $scope.errorImage = false

                        //nếu người dùng chỉ nhấn lưu
                        $(".btn-close").trigger('click') //đóng modal thêm

                        //hiển thị thông báo thành công
                        $("#successToast .text-toast").text("Thêm loại sản phẩm thành công")
                        $("#successToast").toast("show")
                    }
                    else {
                        $("#errorToast .text-toast").text("Thêm thất bại")
                        $("#errorToast").toast("show")
                    }
                })
            }
            else {
                $scope.errorImage = true
            }
        }
    }
}

productApp.controller("ColorController", ColorController)

function ColorController(Upload, $scope, $http) {
    $http.get("/Admin/Color/getAllData").then(function (res) {
        $scope.ColorList = res.data
    }, function (error) {
        alert("Có lỗi khi lấy dữ liệu")
    })

    //lưu file người dùng upload
    $scope.SelectImage = function (file) {
        $scope.fileImage = file
    }

    ///**Thêm loại sản phẩm */
    //hàm upload ảnh trả về true nếu thành công
    let checkUpload = true
    $scope.UploadFiles = function (file, proCatId) {
        $scope.SelectFiles = file;
        if ($scope.SelectFiles && $scope.SelectFiles.length) {
            Upload.upload({
                url: '/ProductCat/Upload',
                data: { file: $scope.SelectFiles, ProCatId: proCatId }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    $scope.errorImage = false
    $scope.SaveAdd = function () {
        if ($scope.createForm.$valid) {
            if ($scope.fileImage !== undefined) {
                $scope.proCat.Image = $scope.fileImage[0].name
                $http({
                    method: "POST",
                    url: "/Admin/ProductCat/Create",
                    datatype: 'Json',
                    data: { proCat: $scope.proCat }
                }).then(function (res) {
                    if (res.data.check) //tạo mới thành công
                    {
                        var productCat = JSON.parse(res.data.pc)
                        //upload ảnh khi thêm đối tượng thành công
                        $scope.UploadFiles($scope.fileImage, productCat.ProCatId)
                        if (checkUpload === false) {
                            //khi upload fail thì xóa đối tượng vừa tạo
                            $http({
                                method: 'Post',
                                url: '/Admin/ProductCat/Delete',
                                data: { id: productCat.ProCatId }
                            })
                        }
                        else {

                        }

                        $scope.errorImage = false

                        //nếu người dùng chỉ nhấn lưu
                        $(".btn-close").trigger('click') //đóng modal thêm

                        //hiển thị thông báo thành công
                        $("#successToast .text-toast").text("Thêm loại sản phẩm thành công")
                        $("#successToast").toast("show")
                    }
                    else {
                        $("#errorToast .text-toast").text("Thêm thất bại")
                        $("#errorToast").toast("show")
                    }
                })
            }
            else {
                $scope.errorImage = true
            }
        }
    }
}