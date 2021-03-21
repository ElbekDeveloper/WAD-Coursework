var penApp = angular.module('penApp', []);

penApp.controller('ArticlesController', function ($scope) {
   
    $scope.articles = [
      {
    "id": 1017,
    "title": "some title",
    "createdDate": "2021-03-02T14:06:02.5519197",
    "updatedDate": null,
    "body": "string",
    "author": {
      "id": "e1f2c2f6-7554-46b5-aaf3-551967060aa7",
      "name": "n@yahoo.com",
      "email": "n@yahoo.com"
    }
  },
  {
    "id": 1018,
    "title": "Some new Post",
    "createdDate": "2021-03-17T11:29:47.8661595",
    "updatedDate": null,
    "body": "Some Lorem lOerm asldfja sldf jlkasdfjkl asd;lfksdlkj fal;ksdfja;l kjdal;sk j",
    "author": {
      "id": "d9aed2ff-79c0-48d5-9dfa-ebcbdcb50ff6",
      "name": "user2@test.com",
      "email": "user2@test.com"
    }
  }
    ]

});