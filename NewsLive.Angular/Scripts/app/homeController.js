(function (angular) {
    angular.module('app')
           .controller('homeController', ['$scope', '$location', '$routeParams', '$httpArticleService', '$memberSessionService', function ($scope, $location, $routeParams, $httpArticleService, $memberSessionService) {
               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership === null)
                   return $location.path('/index');

               // note: ( only use $scope.membership from here )

               // TODO: ( Refactor this See articleEditController )
               if ($scope.membership.person.isPublisher) {
                   $location.path('/publisher-home-view');
               }
               else {
                   $location.path('/employee-home-view');
               }

               $scope.input = {
                   comment: {
                       value: null,
                       config: {
                           type: 'textarea',
                           label: 'Comment',
                           class: 'form-control',
                           rows: 2
                       }
                   }
               };

               $scope.personId = $scope.membership.person.personId;
               $scope.heading = ""; 
               $scope.numPages = 0;
               $scope.nextPageNum = $routeParams.lastPageNum || 1;
               $scope.numResultsPerPage = 1;
               $scope.myFullName = $scope.membership.person.firstName + ' ' + $scope.membership.person.lastName;

               getArticles();

               $scope.incrementPageNum = function () {
                   if ($scope.nextPageNum < $scope.numPages.length) {
                       $scope.nextPageNum++;
                       getArticles();
                   };
               };

               $scope.decrementPageNum = function () {
                   if ($scope.nextPageNum > 1) {
                       $scope.nextPageNum--;
                       getArticles();
                   };
               };

               $scope.toggleLike = function (articleId) {
                   var promise = $httpArticleService.toggleLike(articleId, $scope.personId);
                   promise.then(function (liked) {
                       console.log(liked.articleId);
                       console.log(liked.personId);
                   },
                   function (error) {
                       // TODO: log error
                   });
               }

               $scope.addComment = function (article) {
                   var comment = $scope.input.comment.value;
                   $scope.article = article;
                   // TODO: add client side validation
                   if (comment !== null || comment !== null) {
                       var promise = $httpArticleService.addComment(article.articleId, $scope.personId, comment);
                       promise.then(function (comment) {
                           $scope.input.comment.value = "";
                           $scope.article.comments.push(comment);
                       },
                       function (error) {
                           // TODO: log error
                       });
                   };
               };

               $scope.deleteArticle = function (article) {
                   $scope.article = article;
                   var promise = $httpArticleService.deleteArticle(article.articleId);
                   promise.then(function (deleted) {
                       if (deleted) {
                           var index = $scope.articles.map(function (x) {
                               return x.articleId;
                           }).indexOf(article.articleId);
                           $scope.articles.splice(index, 1);
                           // last article on page, page back
                           if ($scope.articles.length <= 0) {
                               $scope.nextPageNum--;
                               getArticles();
                           };
                       };
                   },
                   function (error) {
                       // TODO: log error
                   });

               };

               function getArticles() {
                   var path = $location.path();
                   if (path == '/publisher-home-view') {
                       var authorId = $scope.membership.person.personId;
                       var promise = $httpArticleService.allArticlesByAuthor(
                           authorId,
                           $scope.nextPageNum,
                           $scope.numResultsPerPage);
                       promise.then(function (articles) {
                           $scope.numPages = $httpArticleService.numberOfPages(articles);
                           $httpArticleService.parseLikedByAuthor(
                               articles,
                               $scope.membership.person.personId);
                           $scope.articles = articles;
                       }, function (error) {
                           // log error
                       });
                   }
               };
           }]);
})(angular);
