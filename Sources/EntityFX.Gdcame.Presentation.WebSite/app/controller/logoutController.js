angular.module("gdCameApp").controller('LogoutController', [
    "$rootScope", "$scope", "$location", "authenticationService",
    function ($rootScope, $scope, $location, authenticationService) {
        authenticationService.logout()
        .then(function (result) {
            authenticationService.clearCredentials();
            alert('1');
            $location.path('/login');
        });
    }]);