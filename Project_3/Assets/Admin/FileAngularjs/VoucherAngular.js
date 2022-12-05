var VoucherApp = angular.module("VoucherApp", ['angularUtils.directives.dirPagination']);

VoucherApp.controller("VoucherController", VoucherController);

function VoucherController($scope, $http) {
    $scope.maxSize = 3;
    $scope.totalCount = 0;
    $scope.searchText = ""
    $scope.pageSize = "5"

    $scope.getPage = function (newPage) {
        $scope.pageNumber = newPage
        /** Lấy danh sách category*/
        $http.get("/Admin/Voucher/getPageData", {
            params: { searchText: $scope.searchText, pageNumber: $scope.pageNumber, pageSize: $scope.pageSize }
        }).then(function (res) {
            let pageData = JSON.parse(res.data)
            $scope.listVoucher = pageData.Data
            $scope.totalCount = pageData.TotalCount
        })
    }

    $scope.getPage(1)

    //check startDate và endDate
    let checkDate = true
    $scope.checkDate = function (startDate, endDate, isEdit) {
        checkDate = true
        $scope.errMessage = '';
        var curDate = new Date();

        if (new Date(startDate) > new Date(endDate)) {
            $("#errorToast .text-toast").text("Thời gian bắt đầu phải nhỏ hơn thời gian kết thúc")
            $("#errorToast").toast("show")
            checkDate = false
            return false;
        }
        if (isEdit == false) {
            if (new Date(startDate) < curDate) {
                $("#errorToast .text-toast").text("Thời gian bắt đầu phải lớn hơn thời gian hiện tại")
                $("#errorToast").toast("show")
                checkDate = false
                return false;
            }
        }
    }

    /**Thêm danh mục */
    //khi người dùng nhấn thêm
    $scope.Add = function () {
        $scope.Voucher = { TypeAmount: "0" }
    }

    //khi người dung nhấn lưu thêm mới danh mục
    $scope.SaveAdd = function () {
        if ($scope.createForm.$valid) {
            $scope.checkDate($scope.Voucher.StartDate, $scope.Voucher.EndDate, false)
            if (checkDate) {
                $http({
                    method: "POST",
                    url: "/Admin/Voucher/Create",
                    datatype: 'Json',
                    data: { Voucher: $scope.Voucher }
                }).then(function (res) {
                    if (res.data.message) {
                        $scope.listVoucher.push(JSON.parse(res.data.v)) //hiển thị thêm đối tượng vừa thêm
                        location.reload()
                        //hiển thị thông báo thành công
                        $("#successToast .text-toast").text("Thêm voucher thành công")
                        $("#successToast").toast("show")
                    }
                    else {
                        $("#errorToast .text-toast").text("Thêm thất bại")
                        $("#errorToast").toast("show")
                    }
                })
            }
        }

    }

    /** Sửa danh mục*/

    let indexEdit = 1 //biến chứa vị trí vừa sửa

    $scope.Edit = function (v, index) {
        //nếu gán thẳng thì nó sẽ thay đổi luôn ở view trong khi chưa sửa
        $scope.Voucher = {
            VoucherId: v.VoucherId,
            Name: v.Name,
            StartDate: new Date(v.StartDate),
            EndDate: new Date(v.EndDate),
            Amount: v.Amount,
            TypeAmount: v.TypeAmount,
            MiximumMoney: v.MiximumMoney,
            Description: v.Description,
            MaxUses: v.MaxUses
        }
        indexEdit = index
    }

    $scope.SaveEdit = function () {
        if ($scope.editForm.$valid) {
            $scope.checkDate($scope.Voucher.StartDate, $scope.Voucher.EndDate, true)
            if (checkDate) {
                if (confirm("Bạn có chắc chắn muốn cập nhật mã giảm giá này")) {
                    $http({
                        method: "POST",
                        url: "/Admin/Voucher/Edit",
                        datatype: 'Json',
                        data: { Voucher: $scope.Voucher }
                    }).then(function (res) {
                        if (res.data) {
                            //Tìm phần tử vừa được sửa trong danh sách
                            $scope.listVoucher.splice(indexEdit, 1, $scope.Voucher)
                            $("#successToast .text-toast").text("Cập nhật thành công")
                            $("#successToast").toast("show") //hiển thị thông báo thành công
                        }
                        else {
                            $("#errorToast .text-toast").text("Cập nhật thất bại")
                            $("#errorToast").toast("show") //hiển thị thông báo thành công
                        }
                        $(".btn-close").trigger('click') //đóng modal sửa
                    })
                }
            }
        }

    }

    /**Xóa danh mục*/
    $scope.Delete = function (v) {
        if (confirm(`Bạn có chắc chắn muốn xóa Voucher ${v.Name}`)) {
            $http({
                method: 'Post',
                url: '/Admin/Voucher/Delete',
                dataType: 'Json',
                data: { id: v.VoucherId }
            }).then(function (res) {
                if (res.data.check) {
                    var c = $scope.listVoucher.indexOf(v);
                    $scope.listVoucher.splice(c, 1);
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