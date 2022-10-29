var NewApp = angular.module("NewApp", ['ngFileUpload']);

NewApp.controller("NewController", function ($scope, Upload, $http) {
    let content = $("#Content-wrap p").text()
    CKEDITOR.instances['Content'].setData(content);
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
                url: '/New/Upload',
                data: { file: $scope.SelectFiles, ProCatId: proCatId }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    /** Sửa danh mục*/
    let nameOldImg = $("#formFile").attr("data-oldImg")
    $scope.SaveEdit = function () {
        if ($scope.editForm.$valid) {
            $scope.New.Content = CKEDITOR.instances['Content'].getData();
            //nếu upload file mới thì gán tên file mới vào proCat
            if ($scope.fileImage != undefined) {
                $scope.New.Image = $scope.fileImage[0].name
            }
            $http({
                method: "POST",
                url: "/Admin/New/Edit",
                datatype: 'Json',
                data: { n: $scope.New, nameOldImg: nameOldImg }
            }).then(function (res) {
                if (res.data.UpdateSuccess === true) //nếu update thành công
                {
                    //upload ảnh khi update thành công
                    $scope.UploadFiles($scope.fileImage, $scope.New.NewID)

                    $("#successToast .text-toast").text("Sửa bài viết thành công")
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


});