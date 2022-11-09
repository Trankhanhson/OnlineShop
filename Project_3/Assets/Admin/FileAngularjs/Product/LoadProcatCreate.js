

productApp.controller("ProductController", ProductController)
function ProductController($scope, $http) {
    $scope.Oject = "Nam"
    $scope.TopMenu = function (Oject) {
        $http.get("/Admin/Category/getByType/" + Oject).then(function (res) {
            $scope.listCategoryByType = JSON.parse(res.data)
            $scope.CategoryByType = JSON.stringify($scope.listCategoryByType[0])
            $scope.listProCatByCat = $scope.listCategoryByType[0].ProductCats
        }, function (error) {
            alert("Lỗi khi tải dữ liệu")
        })
    }

    $scope.renderProcat = function () {
        //vì đặt ở value nên giá trị truyền vào là chuỗi
        //lấy danh sách procat của cat vừa chọn
        $scope.listProCatByCat = JSON.parse($scope.CategoryByType).ProductCats
    }

    $scope.TopMenu("Nam")
}
