var penApp = angular.module('penApp', ["ngRoute", "angular-jwt", "ngCookies"]);
penApp.factory('customAuthManager', function ($rootScope) {
  
  return {
    CheckAuthState: function(){
      return localStorage.getItem("id_token") !== null;
    }
  }
});

var requireAuthentication = function () {
    return {
        load: function ($q, $location) {
            console.log('Can user access route?');
            var deferred = $q.defer();
        deferred.resolve();
        console.log(localStorage.getItem("id_token") !== null)
            if (localStorage.getItem("id_token") !== null) { // fire $routeChangeSuccess
                console.log('Yes they can!');
                return deferred.promise;
            } else { // fire $routeChangeError
                console.log('No they cant!');
                $location.path('/login');

                // I don't think this is still necessary:
                return $q.reject("'/login'");
            }
        }
    };
}
penApp.config(function ($routeProvider, $httpProvider, jwtOptionsProvider) {

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
    .when("/write", {
      templateUrl: "../Views/writeArticle.htm",
      controller: "WriteArticleController",
      resolve: requireAuthentication()
    })
    .otherwise({
      redirectTo: "/404"
    });
  //route config ends here
   // Please note we're annotating the function so that the $injector works when the file is minified
    jwtOptionsProvider.config({
      tokenGetter: ['options', function (options) {
        console.log(options);
        return localStorage.getItem('id_token');
      }],
      unauthenticatedRedirectPath: '/login'
    });

    $httpProvider.interceptors.push('jwtInterceptor');
});
penApp.run(function (authManager, $rootScope){
  authManager.redirectWhenUnauthenticated();
  $rootScope.isAuthenticated = localStorage.getItem("id_token") !== null;
})
penApp.controller('NavController', function ($rootScope,$scope, $log, customAuthManager) {
  $scope.isAuthenticated = $rootScope.isAuthenticated;
  $log.log($scope.isAuthenticated);
  $scope.logout = function () {
    $log.log("Logged out...")
    localStorage.clear();
    $rootScope.isAuthenticated = false;
    $scope.isAuthenticated = false;
  }
})
penApp.controller('ArticlesController', function ($scope, $http, $log, customAuthManager) {
  $log.log(customAuthManager.CheckAuthState());

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

      url: '/api/v1/Articles/' + $routeParams.id,

      data: 'article'

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.article = response.data;

  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
});

penApp.controller('LoginController', function ($rootScope,$scope, $http, $log, $routeParams, jwtHelper, $location) {
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
  $log.log('Logged in');
  localStorage.setItem('id_token', $scope.userData.token);
  $rootScope.isAuthenticated = true;
  $location.path("/home");
  
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });

  } 
});

penApp.controller('WriteArticleController', function ($scope, $http, $log, $routeParams, jwtHelper, $location) {
  $scope.title = '';
  $scope.body = '';
  $scope.write = function () {
    $http({

    method: 'POST',

    url: '/api/v1/Articles',

    data: {
    "title": $scope.title,
    "body": $scope.body
}

}).then(function success(response) {
  // this function will be called when the request is success
  
  $log.log('new article saved');
  
  $location.path("/articles/" + response.data.id);

  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });

  } 
});