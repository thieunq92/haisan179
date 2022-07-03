moduleBookingViewing.controller("globalVariableDeclareController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $rootScope.totalServiceOutside = 0.0;
    $rootScope.totalPriceOfSet = 0.0;
    $rootScope.totalCommission = 0.0;
}])
moduleBookingViewing.controller("menuController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $scope.menuGetById = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/MenuGetById",
            data: {
                "menuId": $scope.menuId,
            },
        }).then(function (response) {
            var menu = JSON.parse(response.data.d);
            $rootScope.costPerPerson.Adult = menu.costOfAdult.toString();
            $rootScope.costPerPerson.Child = menu.costOfChild.toString();
            $rootScope.costPerPerson.Baby = menu.costOfBaby.toString();
            $("[data-id='txtMenuDetail']").val(menu.details)
            $rootScope.calculateTotalPrice();
        }, function (response) {
            $rootScope.costPerPerson.Adult = "0";
            $rootScope.costPerPerson.Child = "0";
            $rootScope.costPerPerson.Baby = "0";
            $("[data-id='txtMenuDetail']").val("");
        })
    }
}])
moduleBookingViewing.controller("priceController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
}])
moduleBookingViewing.controller("numberOfPaxController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
}])
moduleBookingViewing.controller("numberOfDiscountedPaxController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
}])
moduleBookingViewing.controller("totalPriceOfSetController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $rootScope.calculatePriceOfSet = function () {
        var numberOfPaxAdult = 0;
        try {
            numberOfPaxAdult = parseInt($rootScope.numberOfPax.Adult.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var numberOfDiscountedPaxAdult = 0;
        try {
            numberOfDiscountedPaxAdult = parseInt($rootScope.numberOfDiscountedPax.Adult.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var costPerPersonAdult = 0.0;
        try {
            costPerPersonAdult = parseFloat($rootScope.costPerPerson.Adult.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var numberOfPaxChild = 0;
        try {
            numberOfPaxChild = parseInt($rootScope.numberOfPax.Child.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var numberOfDiscountedPaxChild = 0;
        try {
            numberOfDiscountedPaxChild = parseInt($rootScope.numberOfDiscountedPax.Child.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var costPerPersonChild = 0.0;
        try {
            costPerPersonChild = parseFloat($rootScope.costPerPerson.Child.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var numberOfPaxBaby = 0;
        try {
            numberOfPaxBaby = parseInt($rootScope.numberOfPax.Baby.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var numberOfDiscountedPaxBaby = 0;
        try {
            numberOfDiscountedPaxBaby = parseInt($rootScope.numberOfDiscountedPax.Baby.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var costPerPersonBaby = 0.0;
        try {
            costPerPersonBaby = parseFloat($rootScope.costPerPerson.Baby.toString().replace(/,/g, ''));
        } catch (Exception) { }
        $rootScope.totalPriceOfSet =
          (numberOfPaxAdult - numberOfDiscountedPaxAdult) * costPerPersonAdult
        + (numberOfPaxChild - numberOfDiscountedPaxChild) * costPerPersonChild
        + (numberOfPaxBaby - numberOfDiscountedPaxBaby) * costPerPersonBaby;
        $rootScope.priceOfSet = $rootScope.totalPriceOfSet.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    $scope.$watch("$root.totalPriceOfSet", function () {
        $rootScope.calculateTotalPrice();
    })
}])
moduleBookingViewing.controller("totalPriceController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $rootScope.calculateTotalPrice = function () {
        var totalServiceOutside = 0.0;
        try {
            totalServiceOutside = parseFloat($rootScope.totalServiceOutside.toString().replace(/,/g, ''));
        } catch (Exception) { }
        var totalPriceOfSet = 0.0;
        try {
            totalPriceOfSet = parseFloat($rootScope.totalPriceOfSet.toString().replace(/,/g, ''));
        } catch (Exception) { }
        $rootScope.totalPrice = totalServiceOutside + totalPriceOfSet;
        $rootScope.totalPrice = $rootScope.totalPrice.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
}])
moduleBookingViewing.controller("bookerController", ["$rootScope", "$scope", "$http", "$timeout", function ($rootScope, $scope, $http, $timeout) {
    $scope.bookerGetAllByAgencyId = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/BookerGetAllByAgencyId",
            data: {
                "agencyId": $scope.agencyId,
            },
        }).then(function (response) {
            $scope.listBooker = JSON.parse(response.data.d);
        }, function (response) {
        })
    }
    $scope.fillBookerPhoneNumber = function () {
        $scope.bookerPhoneNumber = $("#ddlBooker option:selected").attr("data-phonenumber");
    }
    $timeout(function () {
        $scope.fillBookerPhoneNumber();
    }, 0);
}])
moduleBookingViewing.controller("commissionController", ["$rootScope", "$scope", "$http", "$timeout", function ($rootScope, $scope, $http, $timeout) {
    $rootScope.listCommission = []
    $scope.loadCommission = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/CommissionGetAllByBookingId",
            data: {
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $rootScope.listCommission = JSON.parse(response.data.d);
        }, function (response) {
        })
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'placeholder': '0',
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }

    $scope.addCommission = function () {
        $rootScope.listCommission.push({ id: -1, payFor: "", amount: 0, paymentVoucher: "", transfer: false, restaurantBookingId: $rootScope.restaurantBookingId })
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'placeholder': '0',
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
    $scope.removeCommission = function (index) {
        $rootScope.listCommission.splice(index, 1);
    }
    $scope.calculateTotalCommission = function () {
        var total = 0;
        for (var i = 0; i < $rootScope.listCommission.length; i++) {
            var commission = $rootScope.listCommission[i];
            total += parseFloat(commission.amount.toString().replace(/,/g, ''));
        }
        $rootScope.totalCommission = total;
        return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫";
    }

}])
moduleBookingViewing.controller("serviceOutsideController", ["$rootScope", "$scope", "$http", "$timeout", function ($rootScope, $scope, $http, $timeout) {
    $rootScope.listServiceOutside = []
    $scope.loadServiceOutside = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/ServiceOutsideGetAllByBookingId",
            data: {
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $rootScope.listServiceOutside = JSON.parse(response.data.d);
        }, function (response) {
        })
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'placeholder': '0',
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
    var id = -1;
    $scope.addServiceOutside = function () {
        $rootScope.listServiceOutside.push({ id: id, service: "", unitPrice: 0, quantity: 0, totalPrice: 0, restaurantBookingId: $rootScope.restaurantBookingId, vat: $rootScope.bookingVAT, listServiceOutsideDetailDTO: [] })
        id += -1;
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'placeholder': '0',
                'rightAlign': false
            })
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
    $scope.removeServiceOutside = function (index) {
        $rootScope.listServiceOutside.splice(index, 1);
    }
    $scope.calculateServiceOutside = function (index, unitPrice, quantity) {
        $rootScope.listServiceOutside[index].totalPrice = parseFloat(unitPrice.replace(/,/g, '')) * parseInt(quantity);
        $rootScope.listServiceOutside[index].totalPrice = $rootScope.listServiceOutside[index].totalPrice.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    $rootScope.totalServiceOutside = 0;
    $rootScope.$watch('$root.totalServiceOutside', function () {
        $rootScope.calculateTotalPrice();
    })
    $rootScope.calculateTotalServiceOutside = function () {
        var total = 0;
        for (var i = 0; i < $rootScope.listServiceOutside.length; i++) {
            var serviceOutside = $rootScope.listServiceOutside[i];
            total += parseFloat(serviceOutside.totalPrice.toString().replace(/,/g, ''));
        }
        $rootScope.totalServiceOutside = total;
        return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫";
    }
}])
moduleBookingViewing.controller("guideController", ["$rootScope", "$scope", "$http", "$timeout", function ($rootScope, $scope, $http, $timeout) {
    $rootScope.listGuide = []
    $scope.loadGuide = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/GuideGetAllByBookingId",
            data: {
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $rootScope.listGuide = JSON.parse(response.data.d);
        }, function (response) {
        })
    }

    $scope.addGuide = function () {
        $rootScope.listGuide.push({ id: -1, name: "", phone: "", restaurantBookingId: $rootScope.restaurantBookingId })
    }
    $scope.removeGuide = function (index) {
        $rootScope.listGuide.splice(index, 1);
    }
}])
moduleBookingViewing.controller("actuallyCollectedController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $rootScope.actuallyCollected = 0;
    $rootScope.totalCommission = 0;
    $rootScope.calculateActuallyCollected = function () {
        $rootScope.actuallyCollected = parseFloat($rootScope.totalPrice.toString().replace(/,/g, '')) - parseFloat($rootScope.totalCommission.toString().replace(/,/g, ''));
        $rootScope.actuallyCollected = $rootScope.actuallyCollected.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",")
    }
    $rootScope.$watch('$root.totalPrice', function () {
        $rootScope.calculateActuallyCollected();
    })
    $rootScope.$watch('$root.totalCommission', function () {
        $rootScope.calculateActuallyCollected();
    })
}])
moduleBookingViewing.controller("vatController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $scope.serviceOutsideChangeValueVAT = function () {
        for (var i = 0; i < $rootScope.listServiceOutside.length; i++) {
            $rootScope.listServiceOutside[i].vat = $rootScope.bookingVAT;
        }
    }
}])
moduleBookingViewing.controller("serviceOutsideDetailController", ["$rootScope", "$scope", "$http", "$timeout", function ($rootScope, $scope, $http, $timeout) {
    $scope.loadServiceOutsideDetail = function (serviceOutsideId) {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/ServiceOutsideDetailGetAllByServiceOutsideId",
            data: {
                "serviceOutsideId": serviceOutsideId,
            },
        }).then(function (response) {
            for (var i = 0; i < $rootScope.listServiceOutside.length; i++) {
                if ($rootScope.listServiceOutside[i].id == serviceOutsideId) {                  
                    $rootScope.listServiceOutside[i].listServiceOutsideDetailDTO = JSON.parse(response.data.d);
                }
            }
        }, function (response) {
        })
        $timeout(function () {
            $("[data-control='inputmask']").inputmask({
                'alias': 'numeric',
                'groupSeparator': ',',
                'autoGroup': true,
                'digits': 2,
                'digitsOptional': true,
                'placeholder': '0',
                'rightAlign': false
            });
            $('input[type="text"]').keydown(function () {
                $(this).trigger('input');
                $(this).trigger('change');
            });
        }, 0);
    }
    $scope.addServiceOutsideDetail = function (serviceOutsideId) {
        for (var i = 0; i < $rootScope.listServiceOutside.length; i++) {
            if ($rootScope.listServiceOutside[i].id == serviceOutsideId) {
                $rootScope.listServiceOutside[i].listServiceOutsideDetailDTO.push({ id: -1, name: "", unitPrice: 0, quantity: 0, totalPrice: 0 })
                $timeout(function () {
                    $("[data-control='inputmask']").inputmask({
                        'alias': 'numeric',
                        'groupSeparator': ',',
                        'autoGroup': true,
                        'digits': 2,
                        'digitsOptional': true,
                        'placeholder': '0',
                        'rightAlign': false
                    });
                    $('input[type="text"]').keydown(function () {
                        $(this).trigger('input');
                        $(this).trigger('change');
                    });
                }, 0);
            }
        }
    }
    $scope.removeServiceOutsideDetail = function (index, serviceOutsideId) {
        for (var i = 0; i < $rootScope.listServiceOutside.length; i++) {
            if ($rootScope.listServiceOutside[i].id == serviceOutsideId) {
                $rootScope.listServiceOutside[i].listServiceOutsideDetailDTO.splice(index, 1);
            }
        }

    }
    $scope.calculateServiceOutsideDetail = function (serviceOutside, index, unitPrice, quantity) {
        serviceOutside.listServiceOutsideDetailDTO[index].totalPrice = parseFloat(unitPrice.replace(/,/g, '')) * parseInt(quantity);
        serviceOutside.listServiceOutsideDetailDTO[index].totalPrice = serviceOutside.listServiceOutsideDetailDTO[index].totalPrice.replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
    $rootScope.calculateTotalServiceOutsideDetail = function (serviceOutside) {
        var total = 0;
        if (serviceOutside.listServiceOutsideDetailDTO == null) {
            return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫";
        }
        for (var i = 0; i < serviceOutside.listServiceOutsideDetailDTO.length; i++) {
            var serviceOutsideDetail = serviceOutside.listServiceOutsideDetailDTO[i];
            total += parseFloat(serviceOutsideDetail.totalPrice.toString().replace(/,/g, ''));
        }
        return total.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",") + "₫";
    }
}])
moduleBookingViewing.controller("saveController", ["$rootScope", "$scope", "$http", function ($rootScope, $scope, $http) {
    $scope.commissionSaveState = "undone";
    $scope.serviceOutsideSaveState = "undone";
    $scope.guideSaveState = "undone";
    $scope.bookerSaveState = "undone";
    $scope.save = function () {
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/CommissionSave",
            data: {
                "listCommissionDTO": $rootScope.listCommission,
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $scope.commissionSaveState = "done";
        }, function (response) {
        })
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/ServiceOutsideSave",
            data: {
                "listServiceOutsideDTO": $rootScope.listServiceOutside,
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $scope.serviceOutsideSaveState = "done";
        }, function (response) {
        })
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/GuideSave",
            data: {
                "listGuideDTO": $rootScope.listGuide,
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $scope.guideSaveState = "done"
        }, function (response) {
        })
        $http({
            method: "POST",
            url: "WebMethod/BookingViewingWebMethod.asmx/BookerSave",
            data: {
                "bookerId": $rootScope.bookerId,
                "restaurantBookingId": $rootScope.restaurantBookingId,
            },
        }).then(function (response) {
            $scope.bookerSaveState = "done"
        }, function (response) {
        })
        $scope.$watchGroup(["commissionSaveState", "serviceOutsideSaveState", "guideSaveState", "bookerSaveState"], function (newValues, oldValues, scope) {
            if ($scope.commissionSaveState == "done"
                && $scope.serviceOutsideSaveState == "done"
                && $scope.guideSaveState == "done"
                && $scope.bookerSaveState == "done") {
                setTimeout(function () { __doPostBack($("#btnSave").attr("data-uniqueId"), "OnClick"); }, 1);
            }
        })
    };
}])

