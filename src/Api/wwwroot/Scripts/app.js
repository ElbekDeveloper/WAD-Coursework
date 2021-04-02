var penApp = angular.module('penApp', ["ngRoute", "angular-jwt"]);
penApp.config(function ($routeProvider,$httpProvider, jwtOptionsProvider) {
  $routeProvider
    .when("/home", {
      templateUrl: "../Views/main.htm",
      controller: "ArticlesController"
    })
    .when("/", {
      templateUrl: "../Views/main.htm",
      controller: "ArticlesController"
    })
    .when("/404", {
      templateUrl: "../Views/404.htm"
    })
    .when("/articles", {
      templateUrl: "../Views/main.htm",
      controller: "ArticlesController"
    })
    .when("/articles/:id", {
      templateUrl: "../Views/articleDetails.htm",
      controller: "ArticleDetailsController"
    })
    .when("/login", {
      templateUrl: "../Views/login.htm",
      controller: "LoginController"
    })
    .otherwise({
      redirectTo: "/404"
    });
  //route config ends here
   // Please note we're annotating the function so that the $injector works when the file is minified
    jwtOptionsProvider.config({
      tokenGetter: ['', function() {
        return localStorage.getItem('token');
      }]
    });
    //$httpProvider.interceptors.push('jwtInterceptor');
});


penApp.controller('ArticlesController', function ($scope, $http, $log) {
  
  $http({

    method: 'GET',

    skipAuthorization: true,

    url: '/api/v1/Articles',

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.articles = response.data;
  
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
  
});

penApp.controller('ArticleDetailsController', function ($scope, $http, $log, $routeParams) {
    $http({

      method: 'GET',

      skipAuthorization: true,

    url: '/api/v1/Articles/' + $routeParams.id,

    data: 'article'

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.article = response.data;
  $log.log($scope.article);

  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
});

penApp.controller('LoginController', function ($scope, $http, $log, $routeParams, jwtHelper) {
  $scope.email = '';
  $scope.password = '';
  $scope.login = function () {
    $http({

    method: 'POST',

    url: '/api/v1/Identity/Login',

    data: {
    "email": $scope.email,
    "password": $scope.password
}

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.userData = response.data;
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });

  }
});