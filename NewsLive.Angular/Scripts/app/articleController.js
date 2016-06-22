(function (angular) {
    angular.module('app')
           .controller('articleController', ['$scope', '$http', '$routeParams', '$location', '$httpArticleService', '$memberSessionService', function ($scope, $http, $routeParams, $location, $httpArticleService, $memberSessionService) {
               $scope.membership = $memberSessionService.getMembership();
               if ($scope.membership == null)
                   return $location.path('/index');

               // note: ( only use $scope.membership from here )

               $scope.input = {
                   comment: {
                       value: null,
                       config: {
                           type: 'textarea',
                           label: 'Comment',
                           id:'article-comment',
                           class: 'form-control',
                           rows: 2
                       }
                   }
               };

               $scope.personId = $scope.membership.person.personId;
               $scope.total = 100;
               $scope.heading = "";
               $scope.numPages = 0;
               $scope.nextPageNum = $routeParams.lastPageNum || 1;
               $scope.numResultsPerPage = 3;

               getArticles();

               $scope.incrementPageNum = function () {
                   if ($scope.nextPageNum < $scope.numPages.length) {
                       $scope.nextPageNum++;
                       getArticles();
                   }
               };

               $scope.decrementPageNum = function () {
                   if ($scope.nextPageNum > 1) {    
                       $scope.nextPageNum--;
                       getArticles();
                   }
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
               };

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
                   var promise = $httpArticleService.allArticles($scope.nextPageNum, $scope.numResultsPerPage);
                    promise.then(function (articles) {
                        $scope.numPages = $httpArticleService.numberOfPages(articles);
                        $httpArticleService.parseLikedByAuthor(articles, $scope.personId);
                        $scope.articles = articles;
                    }, function (error) {
                        // TODO: log error
                    });
               };

           }]);
})(angular);
