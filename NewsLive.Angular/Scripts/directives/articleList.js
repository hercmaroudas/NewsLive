(function (angular) {
    angular.module('app')
           .directive('articleList', [function ($http) {
               return {
                   restrict: 'E',
                   replace: true,
                   scope:false,
                   templateUrl: 'partials/article-list',
                   link: function (scope) {
                   }
               };
           }]);
})(angular);
