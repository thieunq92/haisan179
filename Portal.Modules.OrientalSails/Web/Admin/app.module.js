var moduleAddSeriesBookings = angular.module("moduleAddSeriesBookings", [])
    .directive('convertToNumber', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attrs, ngModel) {
                ngModel.$parsers.push(function (val) {
                    return parseInt(val, 10);
                });
                ngModel.$formatters.push(function (val) {
                    return '' + val;
                });
            }
        };
    });
var moduleViewActivities = angular.module("moduleViewActivities", []);
var moduleDocumentView = angular.module("moduleDocumentView", []);
var moduleContractCreate = angular.module("moduleContractCreate", [])
.directive('inputMask', function () {
    return {
        restrict: 'A',
        link: function (scope, el, attrs) {
            $(el).inputmask(scope.$eval(attrs.inputMask));
            $(el).on('change', function (e) {
                scope.application == scope.application || {}
                scope.application.phone = $(e.target).val();
            });
        }
    };
});
var moduleBookingViewing = angular.module("moduleBookingViewing", []);
angular.module("myApp",
    ["moduleAddSeriesBookings",
    "moduleViewActivities",
    "moduleDocumentView",
    "moduleContractCreate",
    "moduleBookingViewing"]);