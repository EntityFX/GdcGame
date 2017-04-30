angular.module("gdCameApp").controller('LoginController', ["$location", "$rootScope", "$scope", "$localStorage", "authenticationService",
    function ($location, $rootScope, $scope, $localStorage, authenticationService) {
        var _this = this;
        if ($localStorage.global && $localStorage.global.auth) {
            $location.path('/');
        }

        this.user = { login: 'admin', password: "P@ssw0rd" };

        this.login = function () {
            authenticationService.login(_this.user.login, _this.user.password)
            .then(function (value) {
                    authenticationService.setCredentials(value.data, _this.user.login);
                    $location.path('/');
                    _this.error = false;
                }, function (reason) {
                    _this.error = reason.data;
                });
        }
    }]);