(function (angular) {
    angular.module('app')
        .controller('membershipController', ['$scope', '$http', '$locationService', '$memberSessionService', '$stopwatchService', function ($scope, $http, $locationService, $memberSessionService, $stopwatchService) {
            $scope.membership = $memberSessionService.getMembership();
            $scope.input = {
                username: {
                    value: null,
                    config: {
                        type: 'email',
                        label: 'Email address',
                        id: 'email-input',
                        class: 'login-input form-control',
                        placeholder: 'Email'
                    }
                },
                password: {
                    value: null,
                    config: {
                        type: 'password',
                        label: 'Password',
                        id: 'password',
                        class: 'login-input form-control',
                        placeholder: "Password"
                    }
                }
            };
            $scope.showLogin = true;
            $scope.showWarning = false;
            $scope.attemptLogin = function () {
                var jsondata = { userName: $scope.input.username.value, password: $scope.input.password.value };
                $http.post($locationService.virtUrl() + '/api/membership/loginasync', jsondata).success(function (response, status, headers, config) {
                    var user = $memberSessionService.storeMembership(response);
                    if (user === null) {
                        $scope.showWarning = true;
                    }
                    else {
                        $scope.showLogin = false;
                        $scope.membership = response;
                        $locationService.path('/home-view');
                    }
                }).error(function (data, status, headers, config) {
                    // log error 
                });
            };
        }]);
})(angular);
