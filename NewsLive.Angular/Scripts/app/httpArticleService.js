(function (angular) {
    angular.module('app')
        .service('$httpArticleService', ['$http', '$q', function ($http, $q) {

            this.allArticlesByAuthor = function (authorId, nextPageNum, numResultsPerPage) {
                var deferred = $q.defer();
                var request = { params: { 'authorId': authorId, 'numResultsPerPage': numResultsPerPage, 'currentPageNum': nextPageNum } };
                $http.get('/api/article/getallarticlesbyauthorpaged', request).success(
                    function (response, status, headers, config) {
                    deferred.resolve(response);
                }).error(function (response, status, headers, config) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };

            this.allArticles = function (nextPageNum, numResultsPerPage) {
                var deferred = $q.defer();
                var request = { params: { 'numResultsPerPage': numResultsPerPage, 'currentPageNum': nextPageNum } };
                $http.get('/api/article/getallarticlespaged', request).success(
                    function (response, status, headers, config) {
                        deferred.resolve(response);
                }).error(function (response, status, headers, config) {
                    deferred.reject(response);
                });
                return deferred.promise;
            };

            this.articleById = function (articleId) {
                var deferred = $q.defer();
                var request = { params: { 'articleId': articleId } };
                $http.get('/api/article/getarticle', request).success(
                    function (response, status, headers, config) {
                        deferred.resolve(response);
                    }).error(function (response, status, headers, config) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };

            this.deleteArticle = function (articleId) {
                var deferred = $q.defer();
                var jsondata = { params: { 'articleId': articleId } };
                $http.delete('/api/article/deletepublishedarticle', jsondata).success(
                    function (response, status, headers, config) {
                        var data = response;
                        deferred.resolve(response);
                    }).error(function (data, status, headers, config) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };

            this.updateArticle = function (authorId, articleId, title, body, isPublished, comment) {
                var deferred = $q.defer();
                var comments = [];
                var jsondata = { 'authorId': authorId, 'articleId': articleId, 'title': title, 'body': body, 'isPublished': isPublished, 'comments': [{ commentContent: comment }] };
                $http.put('/api/article/updatepublishedarticle', jsondata).success(
                    function (response, status, headers, config) {
                        var data = response;
                        deferred.resolve(response);
                    }).error(function (data, status, headers, config) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };

            this.toggleLike = function (articleId, personId) {
                var deferred = $q.defer();
                var jsondata = { 'articleId': articleId, 'personId': personId };
                $http.post('/api/articlelike/togglelike', jsondata).success(
                    function (response, status, headers, config) {
                        var data = response;
                        deferred.resolve(response);
                    }).error(function (data, status, headers, config) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };

            this.addComment = function (articleId, personId, comment) {
                var deferred = $q.defer();
                var jsondata = { 'articleId': articleId, 'personId': personId, 'commentContent': comment };
                $http.put('/api/comment/addcomment', jsondata).success(
                    function (response, status, headers, config) {
                        var data = response;
                        deferred.resolve(response);
                    }).error(function (data, status, headers, config) {
                        deferred.reject(response);
                    });
                return deferred.promise;
            };

            this.parseLikedByAuthor = function (articles, personId) {
                for (var i = 0; i < articles.length; i++) {
                    var article = articles[i];
                    if (article.likes.length > 0) {
                        var index = article.likes.map(function (x) {
                            return x.personId;
                        }).indexOf(personId);
                        articles[i].liked = (index >= 0 && articles[i].likes[index].isLiked == true);
                    }
                };
            };

            this.numberOfPages = function(articles) {
                if (articles === null || articles === undefined)
                    return 0;
                if (articles.length <= 0)
                    return 0;
                return this.pageRange(articles[0].numberOfPages);
            };

            this.pageRange = function (max) {
                var input = [];
                for (var i = 1; i <= max; i += 1) input.push({ index: i });
                return input;
            };
        }]);
})(angular);