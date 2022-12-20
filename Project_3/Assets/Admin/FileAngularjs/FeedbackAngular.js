var FeedbackApp = angular.module("FeedbackApp", ['angularUtils.directives.dirPagination']);

FeedbackApp.controller("FeedbackController", FeedbackController);

function FeedbackController($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"
    $scope.StatusFeedback = "true"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách category*/
        $http.get("/Admin/Feedback/getPageData", {
            params: { StatusFeedback: $scope.StatusFeedback, searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.feedbacks = pageData.Data
            $scope.totalCount = pageData.TotalCount
        }, function (error) {
            alert("failed")
        })
    }

    $scope.getPage(1)

    let indexFeedback = 0
    $scope.showDetail = function (index, id) {
        indexFeedback = index
        $http.get("/Admin/Feedback/getById/" + id).then(function (res) {
            $scope.Feedback = JSON.parse(res.data)
        })
    }

    $scope.ChangeStatus = function (id) {
        if (confirm("Bạn cso chắc chắn muốn đổi trạng thái bình luận này")) {
            $http.get("/Admin/Feedback/ChangeStatus/" + id).then(function (res) {
                if (res.data) {
                    $scope.feedbacks.splice(indexFeedback, 1)
                    $("#successToast .text-toast").text("Cập nhật thành công")
                    $("#successToast").toast("show")
                    $("btn-close").trigger("click")
                }
                else {
                    $("#errorToast .text-toast").text("Đã có lỗi xảy ra")
                    $("#errorToast").toast("show")
                }
            })
        }
    }
}