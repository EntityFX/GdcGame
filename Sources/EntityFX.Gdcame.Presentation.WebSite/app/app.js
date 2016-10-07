var app = angular.module("gdCameApp", ["ngRoute", "ngStorage"])

app.constant('apiUri', 'gdcame.local');

app.config(function ($routeProvider, $locationProvider) {
    $routeProvider
    .when("/login", {
        templateUrl: "app/views/loginView.html",
        controller: "LoginController"
    })
    .when("/", {
        templateUrl: "app/views/gameView.html",
        controller: "GameController"
    })
    .when("/logout", {
        controller: "LogoutController",
        template: ""
    })
    .when("/register", {
        templateUrl: "app/views/registerView.html",
        controller: "RegisterController"
    })
    .when("/rating", {
        templateUrl: "app/views/reatingView.html",
    })
    .otherwise({ redirectTo: '/login' });
});

app.factory("gameData",
    function ($q, gdCameApiService) {
        var deferred = $q.defer();
        gdCameApiService.getGameData().then(function (value) {
            deferred.resolve(value.data);
        });
        return deferred.promise;
    });


app.run(['$rootScope', '$location', '$localStorage', '$http', function ($rootScope, $location, $localStorage, $http) {
    $rootScope.$storage = $localStorage;
    if ($localStorage.globals && $localStorage.globals.auth) {
        $http.defaults.headers.common['Authorization'] = $localStorage.globals.auth.authData.token_type + ' ' + $localStorage.globals.auth.authData.access_token; // jshint ignore:line
    }


    $rootScope.$on('$locationChangeStart', function (event, next, current) {
        // redirect to login page if not logged in and trying to access a restricted page
        var restrictedPage = $.inArray($location.path(), ['/login', '/register']) === -1;
        var loggedIn = $localStorage.globals && $localStorage.globals.auth;

        if (restrictedPage && !loggedIn) {
            $location.path('/login');
        }
    });
}]);
