//
// Note: temporary method to store sessions that last for as long as you close a tab. 
// 
(function (angular) {
    angular.module('app')
        .service('$memberSessionService', ['$window', function ($window) {
            var scope = this;
            scope.key = 'membership';
            scope.storeMembership = function (membership) {
                if (membership === null)
                    return null;
                var memberUser = $window.JSON.stringify(membership);
                $window.sessionStorage.setItem(scope.key, memberUser);
                return memberUser;
            };
            scope.getMembership = function () {
                var membership = $window.sessionStorage.getItem(scope.key);
                if (membership === undefined)
                    return null;
                var membership = $window.JSON.parse(membership);
                return membership;
            };
            scope.deleteMembership = function () {
                var membership = this.getMembership();
                $window.sessionStorage.removeItem(scope.key);
                return (scope.getMembership() === null);
            }
        }]);
})(angular);