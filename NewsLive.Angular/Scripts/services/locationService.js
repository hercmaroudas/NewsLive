(function (angular) {
    angular.module('app')
        .service('$locationService', ['$location', function ($location) {
            var scope = this;
            scope.virtUrl = function () {
                var absUrl = $location.absUrl();
                var path = '/#' + $location.path();
                var virtualDirectory = absUrl.replace(path, '');
                return virtualDirectory;
            };
            scope.path = function (path) {
                if (path === undefined) {
                    return $location.path();
                }
                else {
                    $location.path(path);
                }
            };
        }]);
})(angular);