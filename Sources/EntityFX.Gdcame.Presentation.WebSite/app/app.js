var app = angular.module("gdCameApp", ["ngRoute", "ngCookies"])

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


app.run(['$rootScope', '$location', '$cookieStore', '$http', function ($rootScope, $location, $cookieStore, $http) {
    $rootScope.globals = $cookieStore.get('globals') || {};
    if ($rootScope.globals.auth) {
        $http.defaults.headers.common['Authorization'] = $rootScope.globals.auth.authData.token_type + ' ' + $rootScope.globals.auth.authData.access_token; // jshint ignore:line
    }


    $rootScope.$on('$locationChangeStart', function (event, next, current) {
        // redirect to login page if not logged in and trying to access a restricted page
        var restrictedPage = $.inArray($location.path(), ['/login', '/register']) === -1;
        var loggedIn = $rootScope.globals.auth;
        if (restrictedPage && !loggedIn) {
            $location.path('/login');
        }
    });
}]);
