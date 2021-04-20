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
    .when("/update/:id", {
      templateUrl: "../Views/updateArticle.htm",
      controller: "UpdateArticleController",
      resolve: requireAuthentication()
    })
    .when("/manage", {
      templateUrl: "../Views/manageUsers.htm",
      controller: "ManageUsersController",
      resolve: requireAuthentication()
    })
    .when("/profile", {
      templateUrl: "../Views/profile.htm",
      controller: "ProfileController"
    })
    .otherwise({
      redirectTo: "/404"
    });
  //route config ends here
   // Please note we're annotating the function so that the $injector works when the file is minified
    jwtOptionsProvider.config({
      tokenGetter: ['options', function () {
        return localStorage.getItem('id_token');
      }],
      unauthenticatedRedirectPath: '/login'
    });

    $httpProvider.interceptors.push('jwtInterceptor');
});
penApp.run(function (authManager, $rootScope, jwtHelper) {
  authManager.redirectWhenUnauthenticated();
  $rootScope.isAuthenticated = localStorage.getItem("id_token") !== null;
  if ($rootScope.isAuthenticated) {
    var tokenPayLoad = jwtHelper.decodeToken(localStorage.getItem("id_token"));
    $rootScope.CanWriteArticle = tokenPayLoad.role.includes("CanWriteArticle");
    $rootScope.CanManageUsers = tokenPayLoad.sub == "admin@pen.com";
  }
});
penApp.controller('NavController', function ($rootScope,$scope, $log, jwtHelper) {
  $scope.isAuthenticated = $rootScope.isAuthenticated;
  $scope.logout = function () {
    $log.log("Logged out...")
    localStorage.clear();
  };
  $scope.canManageUsers = function () {
    if (localStorage.getItem("id_token") !== null) {
      var tokenPayLoad = jwtHelper.decodeToken(localStorage.getItem("id_token"));
      return tokenPayLoad.sub == "admin@pen.com";
    }
    else {
      return false;
    }
    
    
  }
  $scope.canWriteArticle = function () {
    if (localStorage.getItem("id_token") !== null) {
      var tokenPayLoad = jwtHelper.decodeToken(localStorage.getItem("id_token"));
      return tokenPayLoad.role.includes("CanWriteArticle");
    }
    else {
      return false;
    }

  }
  $scope.checkAuthState = function(){
      return localStorage.getItem("id_token") != null;
    }
 
})
penApp.controller('ArticlesController', function ($scope, $http, $log, customAuthManager) {
  //$log.log(customAuthManager.CheckAuthState());

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
penApp.controller('ManageUsersController', function ($scope, $http, $log, customAuthManager) {
  //$log.log(customAuthManager.CheckAuthState());

  $http({

    method: 'GET',

    url: '/api/v1/Author/GetAll',

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.users = response.data;
  
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
  
});

penApp.controller('ProfileController', function ($scope, $http, $log, $location) {
  $http({

    method: 'GET',

    url: '/api/v1/Author/GetArticles',

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.articles = response.data;
  
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
  
  $scope.delete = function (id) {
     check = confirm("Are you sure to delete this article?")
        if(check){
          $log.log("Yes, OK Pressed");
          $http({

    method: 'DELETE',

    url: '/api/v1/Articles/' + id,

}).then(function success(response) {
  // this function will be called when the request is success
  $scope.users = response.data;
  
  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
        }else{
            $log.log("No, Cancel Pressed")

        }
  }
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


penApp.controller('UpdateArticleController', function ($scope, $http, $log, $routeParams, $location) {
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
  
  $scope.update = function () {
    $http({

    method: 'PUT',

    url: '/api/v1/Articles/' + $scope.article.id,

    data: {
    "title": $scope.article.title,
    "body": $scope.article.body
}

}).then(function success(response) {
  // this function will be called when the request is success
  
  $log.log('Article Updated...');
  
  $location.path("/articles/" + response.data.id);

  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });
  } 
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
  
  $log.log('New Article Saved...');
  
  $location.path("/articles/" + response.data.id);

  }, function error(response) { 
    // this function will be called when the request returned error status
    $scope.error = response.error;
    $log.log($scope.error);
  });

  } 
});