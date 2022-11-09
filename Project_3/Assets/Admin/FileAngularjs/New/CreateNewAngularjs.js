var NewApp = angular.module("NewApp", ['ngFileUpload']);

NewApp.controller("NewController", function ($scope, Upload, $http) {

    $scope.New = {
        Title: '',
        UserID: '',
        Content: ''
    }
    //lưu file người dùng upload
    $scope.SelectImage = function (file) {
        $scope.fileImage = file
    }

    ///**Thêm loại sản phẩm */
    //hàm upload ảnh trả về true nếu thành công
    let checkUpload = true
    $scope.UploadFiles = function (file, NewID) {
        $scope.SelectFiles = file;
        if ($scope.SelectFiles && $scope.SelectFiles.length) {
            Upload.upload({
                url: '/New/Upload',
                data: { file: $scope.SelectFiles, NewID: NewID }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    //khi người dung nhấn lưu thêm mới danh mục
    $scope.SaveAdd = function (closeOrNew) {
        $scope.New.Content = CKEDITOR.instances['Content'].getData();
        if ($scope.fileImage !== undefined) {
            $scope.New.Image = $scope.fileImage[0].name
        }
        if ($scope.createForm.$valid) {
            $http({
                method: "POST",
                url: "/Admin/New/Create",
                datatype: 'Json',
                data: { n: $scope.New }
            }).then(function (res) {
                if (res.data.check) //tạo mới thành công
                {
                    //upload ảnh khi thêm đối tượng thành công
                    $scope.UploadFiles($scope.fileImage, res.data.newHasId.NewID)
                    if (checkUpload === false) {
                        //khi upload fail thì xóa đối tượng vừa tạo
                        $http({
                            method: 'Post',
                            url: '/Admin/New/Delete',
                            data: { id: res.data.newHasId.NewID }
                        })
                    }
                    else {

                    }

                    //hiển thị thông báo thành công
                    $("#successToast .text-toast").text("Thêm bài viết thành công")
                    $("#successToast").toast("show")
                }
                else {
                    $("#errorToast .text-toast").text("Thêm thất bại")
                    $("#errorToast").toast("show")
                }
            })
        }

    }

});