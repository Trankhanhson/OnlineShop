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
