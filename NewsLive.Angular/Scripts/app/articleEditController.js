(function (angular) {
    angular.module('app')
           .controller('articleEditController', ['$scope', '$http', '$routeParams', '$locationService', '$httpArticleService', '$memberSessionService', function ($scope, $http, $routeParams, $locationService, $httpArticleService, $memberSessionService) {
               var routed = $routeParams;
               $scope.articleId = $routeParams.articleId;
               $scope.lastPageNum = $routeParams.lastPageNum;

               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership == null)
                   return $locationService.path('/index');

               // TODO: ( Put this in a service class and save URL history )
               if ($scope.membership.person.isPublisher) {
                   $scope.returnUrl = '/publisher-home-view';
               }
               else {
                   $scope.returnUrl = '/employee-home-view';
               }

               // note: ( only use $scope.membership from here )
             
               $scope.articleSaved = false;
               getArticle();

               $scope.input = {
                   title: {
                       value: null,
                       config: {
                           type: 'textarea',
                           label: 'Title',
                           id: 'article-title',
                           class: 'news-post-edit-title form-control',
                           rows: 1
                       }
                   },
                   body: {
                       value: null,
                       config: {
                           type: 'textarea',
                           label: 'Body',
                           id: 'article-body',
                           class: 'news-post-edit-body form-control',
                           rows: 8
                       }
                   },
                   publish: {
                       value: null,
                       config: {
                           type: 'checkbox',
                           label: 'Published',
                           id: 'news-post-edit-publish',
                           checked: false
                       }
                   },
                   comment: {
                       value: null,
                       config: {
                           type: 'textarea',
                           label: 'Comment',
                           id: 'article-comment',
                           class: 'news-post-edit-comment form-control',
                           rows: 2
                       }
                   }
               };

               $scope.updateArticle = function () {
                   var promise = $httpArticleService.updateArticle(
                       $scope.authorId,
                       $scope.articleId,
                       $scope.input.title.value,
                       $scope.input.body.value,
                       $scope.input.publish.value,
                       $scope.input.comment.value);
                   promise.then(function (updated) {
                       $scope.articleSaved = updated;
                   }, function (error) {
                       // TODO: log error
                   });
               };

               function getArticle() {
                   var promise = $httpArticleService.articleById($scope.articleId);
                   promise.then(function (article) {
                       $scope.input.title.value = article.title;
                       $scope.input.body.value = article.body;
                       $scope.input.publish.value = (article.isPublished);
                       $scope.authorId = article.author.personId;
                       $scope.authorName = article.author.firstName + ' ' + article.author.lastName;
                       $scope.publishDate = new Date(article.publishDate)
                       $scope.publishDate || $scope.publishDate.toDateString();
                   }, function (error) {
                       // TODO: log error
                   });
               };
           }]);
})(angular);
