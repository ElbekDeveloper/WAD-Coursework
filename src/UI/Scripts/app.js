var penApp = angular.module('penApp', []);
penApp.controller('ArticlesController', function($scope, $http, $log) {
  $http({

    method : 'GET',

    url : 'http://localhost:5000/api/v1/Articles',

    data : 'articles'

  })
      .then(
          function success(response) {
            // this function will be called when the request is success
            $scope.articles = response.data;
            $log.log($scope.articles);
          },
          function error(response) {
            // this function will be called when the request returned error
            // status
            $scope.error = response.error;
            $log.log($scope.error);
          });
});