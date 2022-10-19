var proCatApp = angular.module("ProCatApp", ['ngFileUpload']);

proCatApp.controller("ProCatController", function ($scope, Upload, $http) {

    /** Lấy danh sách loại sản phẩm*/
    $http.get("/Admin/ProductCat/getAllData").then(function (res) {
        $scope.ProCatList = JSON.parse(res.data)
    }, function (error) {
        alert("failed")
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

    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.proCat = null
    }

    //khi người dung nhấn lưu thêm mới danh mục
    $scope.SaveAdd = function (closeOrNew) {
        //upload ảnh trước khi thêm để chống lỗi đường dẫn ảnh
        $http({
            method: "POST",
            url: "/Admin/ProductCat/Create",
            datatype: 'Json',
            data: { proCat: $scope.proCat, nameImg: $scope.fileImage[0].name }
        }).then(function (res) {
            if (res.data.check) //tạo mới thành công
            {
                var productCat = res.data.pc
                $scope.ProCatList.push(productCat) //hiển thị thêm đối tượng vừa thêm
                //upload ảnh khi thêm đối tượng thành công
                $scope.UploadFiles($scope.fileImage, res.data.pc.ProCatId)
                if (checkUpload === false) {
                    //khi upload fail thì xóa đối tượng vừa tạo
                    $http({
                        method: 'Post',
                        url: '/Admin/ProductCat/Delete',
                        data: { id: res.data.pc.ProCatId }
                    })
                }
                else {

                }

                //nếu người dùng chỉ nhấn lưu
                if (closeOrNew) {
                    $(".btn-close").trigger('click') //đóng modal thêm
                }

                //reset lại dữ liệu vừa thêm
                $scope.proCat = null;

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

    /** Sửa danh mục*/
    let indexEdit = 1 //biến chứa vị trí phần tử vừa sửa để thay thế lên view
    let nameOldImg = "" //biến chứa tên Image cũ để xóa đi và thêm Image mới vào
    $scope.Edit = function (proCat, index) {
        nameOldImg = proCat.Image //image cũ
        $scope.proCat = { ProCatId: proCat.ProCatId, Name: proCat.Name, CatID: JSON.stringify(proCat.CatID), Status: proCat.Status, Image: $scope.fileImage[0].name }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        $http({
            method: "POST",
            url: "/Admin/ProductCat/Edit",
            datatype: 'Json',
            data: { proCat: $scope.proCat, nameOldImg: nameOldImg }
        }).then(function (res) {
            if (res.data.UpdateSuccess === true) //nếu update thành công
            {
                if (res.data.checkExistImg == false) //ảnh chưa tồn tại
                {
                    //upload ảnh khi update thành công
                    $scope.UploadFiles($scope.fileImage, $scope.proCat.ProCatId)
                }

                //Tìm phần tử vừa được sửa trong danh sách và thay thế 
                var newProCat = $scope.proCat
                $scope.ProCatList.splice(indexEdit, 1, newProCat)

                $("#successToast .text-toast").text("Sửa loại sản phẩm thành công")
                $("#successToast").toast("show") //hiển thị thông báo thành công
            }
            else {
                $("#errorToast .text-toast").text("Sửa thất bại")
                $("#errorToast").toast("show") //hiển thị thông báo thành công
            }
            $(".btn-close").trigger('click') //đóng modal sửa
        })
    }

    /**Xóa danh mục*/
    $scope.Delete = function (proCat) {
        if (confirm(`Bạn có chắc chắn muốn loại sản phẩm ${proCat.Name}`)) {
            $http({
                method: 'Post',
                url: '/Admin/ProductCat/Delete',
                dataType: 'Json',
                data: { proCat: proCat }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.ProCatList.indexOf(proCat);
                    $scope.ProCatList.splice(c, 1);
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
    $scope.ChangeStatusProCat = function (proCatId) {
        $http({
            method: "Post",
            url: "/Admin/ProductCat/ChangeStatus",
            dataType: 'Json',
            data: { id: proCatId }
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