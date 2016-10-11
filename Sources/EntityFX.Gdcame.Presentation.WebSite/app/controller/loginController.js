angular.module("gdCameApp").controller('LoginController', ["$location", "$rootScope", "$scope", "$localStorage", "authenticationService",
    function ($location, $rootScope, $scope, $localStorage, authenticationService) {

        if ($localStorage.global && $localStorage.global.auth) {
            $location.path('/');
        }

        $scope.user = { login: 'admin', password: "P@ssw0rd" };

        $scope.login = function () {
            authenticationService.login($scope.user.login, $scope.user.password)
            .then(function (value) {
                authenticationService.setCredentials(value.data, $scope.user.login);
                $location.path('/');
            })
            .catch(function (reason) {
                alert(reason);
            });
        }
    }]);