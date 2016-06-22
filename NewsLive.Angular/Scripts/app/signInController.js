(function (angular) {
    angular.module('app')
           .controller('signInController', ['$scope', '$http', '$location', '$memberSessionService', function ($scope, $http, $location, $memberSessionService) {
               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership !== null)
                   return $location.path('/home-view');

           }]);
})(angular);
