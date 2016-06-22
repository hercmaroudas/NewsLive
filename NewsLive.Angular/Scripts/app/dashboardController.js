(function (angular) {
    angular.module('app')
           .controller('dashboardController', ['$scope', '$http', '$location', '$memberSessionService', function ($scope, $http, $location, $memberSessionService) {
               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership === null)
                   return $location.path('/index');

           }]);
})(angular);
