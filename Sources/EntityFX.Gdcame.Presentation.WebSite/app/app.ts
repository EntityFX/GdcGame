﻿var app = angular.module("gdCameApp", ['ui.router',"ngStorage"]);
app.constant('apiUri', 'gdcame.local');
app.constant('viewTheme', 'bootstrap4');
app.constant('serverIps', [
    'localhost:9001'
]);

app.config(function ($stateProvider, $urlRouterProvider, viewTheme) {

    $urlRouterProvider.otherwise("/login");

    $stateProvider.state('login', {
        url: '/login',
        component: 'login'
    });

    $stateProvider.state('game', {
        url: '/',
        component: 'game'
    });

    $stateProvider.state('register', {
        url: '/register',
        component: 'register'
    });

    /*
    $routeProvider
    .when("/login", {
        templateUrl: "app/views/" + viewTheme + "/loginView.html",
        controller: "LoginController"
    })
    .when("/", {
        templateUrl: "app/views/" + viewTheme + "/gameView.html",
        controller: "GameController"
    })
    .when("/logout", {
        controller: "LogoutController",
        template: ""
    })
    .when("/register", {
        templateUrl: "app/views/" + viewTheme + "/registerView.html",
        controller: "RegisterController"
    })
    .when("/rating", {
        templateUrl: "app/views/" + viewTheme + "/ratingView.html"
    })
    .when("/admin/statistics", {
        templateUrl: "app/views/" + viewTheme + "/adminStatisticsView.html",
        controller: "AdminStatisticsController"
    })
    .otherwise({ redirectTo: '/login' });*/
});

app.factory("gameData",
    function ($q, gdCameApiService) {
        var deferred = $q.defer();
        gdCameApiService.getGameData().then(function (value) {
            deferred.resolve(value.data);
        });
        return deferred.promise;
    });

angular
    .module("gdCameApp")
    .service("responseInterceptor", ['$q', function($q) {
        var interceptor = {
            responseError : response => {
                if (response.status === 500) {
                    alert("500!");
                }
                return $q.reject(response);
            }
        }
        return interceptor;
    }]);

app.config(function($httpProvider) {
    $httpProvider.interceptors.push("responseInterceptor");
});

angular
    .module("gdCameApp")
    .factory("apiServiceUri",
        function ($rootScope, apiUri, serverIps) {
            var useServerIps = serverIps != undefined && serverIps.length > 0;
            var gdCameApiServiceFactory = {
                getApiAddressByLogin: function (login) {
                    /*var userServerNumber = Utils.Utils.getModuloByUserIdHash(md5(login + '_gdcame'), serverIps.length);
                    return 'http://' + (useServerIps ? serverIps[userServerNumber] : 'ns' + (userServerNumber + 1) + '.' + apiUri);*/
                },
            };
            return gdCameApiServiceFactory;
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
