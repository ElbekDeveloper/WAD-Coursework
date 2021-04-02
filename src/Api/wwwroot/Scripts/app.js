var penApp = angular.module('penApp', [ "ngRoute" ]);
penApp.config(function($routeProvider) {
  $routeProvider
      .when("/home", {
        templateUrl : "../Views/main.htm",
        controller : "ArticlesController"
      })
      .when("/", {
        templateUrl : "../Views/main.htm",
        controller : "ArticlesController"
      })
      .when("/404", {templateUrl : "../Views/404.htm"})
      .when("/articles", {
        templateUrl : "../Views/main.htm",
        controller : "ArticlesController"
      })
      .when("/articles/:id", {
        templateUrl : "../Views/articleDetails.htm",
        controller : "ArticleDetailsController"
      })
      .otherwise({redirectTo : "/404"});
});

penApp.controller('ArticlesController', function($scope, $http, $log) {
  $http({

    method : 'GET',

    url : '/api/v1/Articles',

    data : 'articles'

  })
      .then(
          function success(response) {
            // this function will be called when the request is success
            $scope.articles = response.data;
          },
          function error(response) {
            // this function will be called when the request returned error
            // status
            $scope.error = response.error;
            $log.log($scope.error);
          });
});

penApp.controller('ArticleDetailsController',
                  function($scope, $http, $log, $routeParams) {
                    $http({

                      method : 'GET',

                      url : '/api/v1/Articles/' + $routeParams.id,

                      data : 'article'

                    })
                        .then(
                            function success(response) {
                              // this function will be called when the request
                              // is success
                              $scope.article = response.data;
                              $log.log($scope.article);
                            },
                            function error(response) {
                              // this function will be called when the request
                              // returned error status
                              $scope.error = response.error;
                              $log.log($scope.error);
                            });
                  });