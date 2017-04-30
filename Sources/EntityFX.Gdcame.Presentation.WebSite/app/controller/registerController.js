angular.module("gdCameApp").controller('RegisterController', ["$location", "$rootScope", "$scope", "authenticationService",
    function ($location, $rootScope, $scope, authenticationService) {

        //$scope.user = { login: '', password: '', confirmPassword: '' };

        $scope.register = function () {
            authenticationService.register($scope.user)
                .then(function(value) {
                        $location.path('/login');
                    },
                    function(response) {
                        alert(JSON.stringify(reason.data.message));
                    });
        };
    }]);