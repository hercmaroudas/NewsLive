(function (angular) {
    angular.module('app')
           .controller('dashboardController', ['$scope', '$http', '$locationService', '$memberSessionService', function ($scope, $http, $locationService, $memberSessionService) {
               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership === null)
                   return $locationService.path('/index');

           }]);
})(angular);
