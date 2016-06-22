(function (angular) {
    angular.module('app')
           .directive('dynamicInput', [function () {
               return {
                   restrict: 'E',
                   replace: true,
                   scope: {
                       model: '=',
                       config: '=?'
                   },
                   templateUrl: 'partials/dynamic-input',
                   link: function (scope) {
                       scope.config = scope.config || {
                           label: 'Text',
                           id: 'fs9h32nm45nh4msdf0983dlj93',
                           type: 'text'
                       };
                   }
               };
           }]);
})(angular);