ClientApp.controller("orderHistoryController", orderHistoryController);

function orderHistoryController($scope, Upload, $http) {
    /** Lấy danh sách Order*/

    let CusId = $("main").attr("data-id")

    $scope.getOrder = function (statusId) {
        //add active
        $(".account-page-filter li").removeClass("active")
        $(`.filer-order-${statusId}`).addClass("active")

        $http.get("/InfoCustomer/getOrderByCusId/", {
            params: { id: CusId, statusId: statusId }
        }).then(function (res) {
            $scope.OrderList = JSON.parse(res.data.result)
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getOrder(0)

    $scope.showDetail = function (idOrder) {
        $scope.clickDetail = true
        $http.get("/InfoCustomer/getOrderById/" + idOrder).then(function (res) {
            let result = res.data
            $scope.Order = JSON.parse(result.Order)
            $scope.originPrice = result.originPrice
            $scope.totalDiscount = result.totalDiscount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.OrderBackClick = function () {
        $scope.clickDetail = false
    }

    $scope.CancelOrder = function (id) {
        if (confirm("Bạn có chắc chắn muốn hủy đơn hàng này")) {
            $http.get("/InfoCustomer/CancelOrder/" + id).then(function (res) {
                if (res.data) {
                    $(".order-status").text("Đã hủy")
                    $(".detail strong").text("Đã hủy")
                    $(".btn-cancelOrder").hide()
                }
                else {

                }
            })
        }

    }

    /*add Feedback*/
    //lưu file người dùng upload
    $scope.SelectImage = function (file) {
        $scope.fileImage = file
    }

    $scope.removeImg = function () {
        $scope.fileImage = null
    }

    $scope.UploadFiles = function (file, FeedbackId, isEdit) {
        if (file && file.length) {
            Upload.upload({
                url: '/Feedback/Upload',
                data: { file: file, FeedbackId: FeedbackId, isEdit: isEdit }
            }).then(function (res) {

            }), function (error) {
                alert("Lỗi upload ảnh")
                checkUpload = false
            }
        }
    }

    $scope.ChangeStar = function (number) {
        $scope.Feedback.Stars = number
    }

    $scope.AddF = function (variationId) {
        $(".rating-0").trigger("click")
        $scope.fileImage = null
        $scope.Feedback = { FeedbackId: null, ProVariationID: variationId, CusID: CusId, Content: "", Stars: 0, Image: false }
        $(".file-upload-image").attr("src", "")
        $('.file-upload-content').hide();
        $('.image-upload-wrap').show();
    }

    $scope.AddFeedback = function () {
        if ($scope.Feedback.Stars == 0) {
            alertError("Nhấn vào hình sao để chọn số sao")
            return false
        }
        if ($scope.Feedback.Content == null || $scope.Feedback.Content.trim() == "") {
            alertError("Bạn chưa viết đánh giá")
            return false
        }
        if ($scope.fileImage !== undefined && $scope.fileImage !== null) {
            $scope.Feedback.Image = true
        }

        $http({
            method: "POST",
            url: "/Admin/Feedback/Create",
            datatype: 'Json',
            data: { feedback: $scope.Feedback }
        }).then(function (res) {
            if (res.data.check) //tạo mới thành công
            {
                if ($scope.fileImage !== undefined) {
                    $scope.UploadFiles($scope.fileImage, res.data.idFeedback, false)
                }
                $(".btn-close").trigger("click")
                alertSuccess("Đánh giá thành công")
            }
            else {
                alertError("Đã có lỗi xảy ra")
            }
        })
    }

    /*Edit feedback*/
    $scope.EditF = function (feedbackId) {
        $scope.fileImage = null
        $http.get("/Admin/Feedback/getById/" + feedbackId).then(function (res) {
            $scope.Feedback = JSON.parse(res.data)

            //click to input of star
            let inputStar = $(`.rating-${$scope.Feedback.Stars}`)
            $(inputStar).trigger("click")

            if ($scope.Feedback.Image) {
                $(".file-upload-image").attr("src", `/Upload/Feedback/${$scope.Feedback.FeedbackId}.jpg`)
                $('.file-upload-content').show();
                $('.image-upload-wrap').hide();
            }

        })
    }

    $scope.EditFeedback = function () {
        if ($scope.Feedback.Stars == 0) {
            alertError("Nhấn vào hình sao để chọn số sao")
            return false
        }
        if ($scope.Feedback.Content == null || $scope.Feedback.Content.trim() == "") {
            alertError("Bạn chưa viết đánh giá")
            return false
        }

        let srcImg = $(".file-upload-image").attr("src")
        //Deleted oldImage 
        if ($scope.fileImage == null && $scope.Feedback.Image == true && srcImg == "") {
            $scope.Feedback.Image = false
            $http.get("/Admin/Feedback/DeleteImgFeedback/" + $scope.Feedback.FeedbackId).then(function (res) {

            })
        }
        else if ($scope.Feedback.Image == true && $scope.fileImage != null) //delete oldImg and add new img
        {
            $scope.Feedback.Image = true
            $scope.UploadFiles($scope.fileImage, $scope.Feedback.FeedbackId, true) //add new img and delete old img
        }
        else if ($scope.Feedback.Image == false && $scope.fileImage != null) //add new img (imageOld is empty)
        {
            $scope.Feedback.Image = true
            $scope.UploadFiles($scope.fileImage, $scope.Feedback.FeedbackId, false) //add new img and don't delete old img
        }

        $http({
            method: "POST",
            url: "/Admin/Feedback/Edit",
            datatype: 'Json',
            data: { feedback: $scope.Feedback }
        }).then(function (res) {
            if (res.data) //tạo mới thành công
            {
                $(".btn-close").trigger("click")
                alertSuccess("Sửa đánh giá thành công")
            }
            else {
                alertError("Đã có lỗi xảy ra")
            }
        })
    }
}