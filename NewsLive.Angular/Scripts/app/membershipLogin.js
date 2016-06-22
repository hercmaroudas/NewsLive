(function (angular) {
    angular.module('app')
           .directive('membershipLogin', [function ($http) {
               return {
                   restrict: 'E',
                   replace: true,
                   scope: {
                       userName: '=',
                       password: '='
                   },
                   templateUrl: 'partials/membership-login',
                   link: function (scope) {
                   }
               };
           }]);
})(angular);
