(function (angular) {
    angular.module('app')
           .directive('like', [function () {
               return {
                   restrict: 'E',
                   replace: true,
                   scope: {
                       // ( initial value from controller )
                       liked: '=',
                       likes: '=',
                       toggle: '&'
                   },
                   templateUrl: 'partials/like',
                   link: function (scope, element, attrs, ctrl) {
                       // ( setup when directive loads )
                       scope.unliked = !scope.liked;
                       scope.total = scope.likes.filter(function (x) {
                           return x.isLiked;
                       }).length;

                       element.bind("click", function () {
                           scope.$apply(function () {
                               // ( change when clicked - see template ) 
                               scope.liked = !scope.liked;
                               scope.unliked = !scope.unliked;
                               // update total likes
                               if (scope.liked == true) {scope.total++; }
                               else { scope.total--; }
                           });
                       });
                       scope.$watch('liked', function (value) {
                           scope.liked = Boolean(value);
                       });
                   }
               };
           }]);
})(angular);

