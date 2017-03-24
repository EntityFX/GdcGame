angular.module("gdCameApp").controller('GameMenuController', ["$location", "$rootScope", "$scope", "$localStorage",
    function ($location, $rootScope, $scope, $localStorage) {
        $scope.isAuthenticated = $localStorage.globals && $localStorage.globals.auth;
        if ($scope.isAuthenticated) {
            $scope.login = $localStorage.globals.auth.login;
        }
    }]);