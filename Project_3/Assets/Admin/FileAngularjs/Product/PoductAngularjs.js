var productApp = angular.module("ProductApp", ['ngFileUpload']);

productApp.controller("ProductController", ProductController)
function ProductController($scope, $http) {
    $scope.Oject = "Nam"
    $scope.TopMenu = function (Oject) {
        $http.get("/Admin/Category/getByType/" + Oject).then(function (res) {
            $scope.listCategoryByType = res.data
        }, function (error) {
            alert("Lỗi khi tải dữ liệu")
        })
    }

    $scope.TopMenu("Nam")
}


productApp.controller("SizeController", SizeController)
function SizeController($scope, $http) {

    $scope.addValue = function (e) {
        addValue(e) //hàm được viết ở file product.js
    }

    /** Lấy danh sách category*/
    $http.get("/Admin/Size/getAllData").then(function (res) {
        $scope.ListSize = res.data
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
                        $(".btn-closeModel").trigger('click') //đóng modal 

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

    //Thêm value size hoặc màu khi nhấn
    $scope.addValue = function (e) {
        addValue(e) //hàm được viết ở file product.js
    }

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
    $scope.UploadFiles = function (file, proColorId) {
        $scope.SelectFiles = file;
        if ($scope.SelectFiles && $scope.SelectFiles.length) {
            Upload.upload({
                url: '/Color/Upload',
                data: { file: $scope.SelectFiles, proColorId: proColorId }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.proColor = null
    }

    $scope.errorImage = false
    //thêm màu
    $scope.SaveAdd = function () {
        if ($scope.createColorForm.$valid) {
            if ($scope.fileImage !== undefined) {
                $scope.proColor.ImageColor = $scope.fileImage[0].name //gán bằng tên ảnh đã chọn
                $http({
                    method: "POST",
                    url: "/Admin/Color/Create",
                    datatype: 'Json',
                    data: { proColor: $scope.proColor }
                }).then(function (res) {
                    if (res.data.check) //tạo mới thành công
                    {
                        var pco = res.data.pco
                        $scope.ColorList.push(pco)

                        //upload ảnh khi thêm đối tượng thành công
                        $scope.UploadFiles($scope.fileImage, pco.ProColorID)

                        if (checkUpload === false) {
                            //khi upload fail thì xóa đối tượng vừa tạo
                            $http({
                                method: 'Post',
                                url: '/Admin/Color/Delete',
                                data: { id: pco.ProColorID }
                            })
                        }
                        else {

                        }

                        $scope.errorImage = false

                        //nếu người dùng chỉ nhấn lưu
                        $(".btn-closeModel").trigger('click') //đóng modal 

                        //hiển thị thông báo thành công
                        $("#successToast .text-toast").text("Thêm màu thành công")
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

    /** Sửa màu*/
    let indexEdit = 1 //biến chứa vị trí phần tử vừa sửa để thay thế lên view
    $scope.Edit = function (pc, index, e) {
        e.stopPropagation()
        $scope.proColor = { ProColorID: pc.ProColorID, NameColor: pc.NameColor, ImageColor: pc.ImageColor }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        if ($scope.editColorForm.$valid) {
            //nếu upload file mới thì gán tên file mới vào proCat
            if ($scope.fileImage != undefined) {
                $scope.proColor.ImageColor = $scope.fileImage[0].name
            }
            $http({
                method: "POST",
                url: "/Admin/Color/Edit",
                datatype: 'Json',
                data: { proColor: $scope.proColor }
            }).then(function (res) {
                if (res.data.UpdateSuccess === true) //nếu update thành công
                {
                    if (res.data.checkExistImg == false) //ảnh chưa tồn tại
                    {
                        //upload ảnh khi update thành công
                        $scope.UploadFiles($scope.fileImage, $scope.proColor.ProColorID)
                    }

                    //Tìm phần tử vừa được sửa trong danh sách và thay thế 
                    $scope.ColorList.splice(indexEdit, 1, $scope.proColor)

                    $("#successToast .text-toast").text("Sửa màu thành công")
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

    /**Xóa màu*/
    $scope.Delete = function (proColor) {
        if (confirm(`Bạn có chắc chắn muốn xóa màu ${proColor.NameColor}`)) {
            $http({
                method: 'Post',
                url: '/Admin/Color/Delete',
                dataType: 'Json',
                data: { proColor: proColor }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.ColorList.indexOf(proColor);
                    $scope.ColorList.splice(c, 1);
                    $("#successToast .text-toast").text("Đã xóa màu thành công")
                    $("#successToast").toast("show") //hiển thị thông báo thành công
                }
                else {
                    $("#errorToast .text-toast").text("Màu này đang được dùng ở sản phẩm")
                    $("#errorToast").toast("show")
                }
                $(".btn-closeModel").trigger('click') //đóng modal 

            })
        }
    }
}