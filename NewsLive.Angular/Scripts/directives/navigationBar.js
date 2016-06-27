(function (angular) {
    angular.module('app')
           .directive('navigationBar', [function ($http) {
               return {
                   restrict: 'E',
                   replace: true,
                   scope: {
                       membership: '='
                   },
                   templateUrl: 'partials/navigation-bar',
                   link: function (scope) {
                   }
               };
           }]);
})(angular);
