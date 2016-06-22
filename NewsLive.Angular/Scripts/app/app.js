(function (angular) {
    angular.module('app', ['ngRoute'])
           .config(['$routeProvider', '$locationProvider',
                 function ($routeProvider, $locationProvider) {
                     $routeProvider
                         .when('/', {
                             templateUrl: 'partials/index',
                             controller: 'signInController'
                         })
                         .when('/membership-login', {
                             templateUrl: 'partials/membership-login',
                             controller: 'membershipController'
                         })
                         .when('/home-view', {
                             templateUrl: 'partials/home-view',
                             controller: 'homeController'
                         })
                         .when('/publisher-home-view', {
                             templateUrl: 'partials/publisher-home-view',
                             controller: 'homeController'
                         })
                         .when('/employee-home-view', {
                             templateUrl: 'partials/employee-home-view',
                             controller: 'homeController'
                         })
                         .when('/article-view', {
                             templateUrl: 'partials/news-view',
                             controller: 'articleController'
                         })
                         .when('/article-edit-view/:articleId/:lastPageNum', {
                             templateUrl: 'partials/article-edit-view',
                             controller: 'articleEditController'
                         })
                         .when('/dashboard-view', {
                             templateUrl: 'partials/dashboard-view',
                             controller: 'dashboardController'
                         })
                         .otherwise({
                             redirectTo: '/'
                         });

                    // hash bang method e.g. #/partials/page
                    $locationProvider.html5Mode(false);
                }]);
})(angular);
