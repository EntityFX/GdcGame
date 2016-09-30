angular.module("gdCameApp").controller('LoginController', ["$location", "$scope", "authenticationService", function ($location, $scope, authenticationService) {
    $scope.user = { login: 'admin', password: "P@ssw0rd" };

    $scope.login = function () {
        authenticationService.login($scope.user.login, $scope.user.password)
        .then(function (value) {
            authenticationService.setCredentials(value.data, $scope.user.login);
            $location.path('/');
        })
        .catch(function (reason) {

        });
    }
}]);