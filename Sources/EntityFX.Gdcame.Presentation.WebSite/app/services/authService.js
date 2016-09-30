angular
    .module("gdCameApp")
    .factory("authenticationService",
        function ($rootScope, $cookieStore, $http, $location) {
            var authBaseUri = $location.protocol() + "://" + $location.host() + ":" + 8889 + "/token";

            var authServiceFactory = {

                login: function (login, password) {
                    return $http({
                        method: 'POST',
                        url: authBaseUri,
                        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                        transformRequest: function (obj) {
                            var str = [];
                            for (var p in obj)
                                str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                            return str.join("&");
                        },
                        data: {
                            grant_type: "password",
                            username: login,
                            password: password
                        }
                    });
                },

                logout: function () {
                    $http.post(authBaseUri + "FightAgainstInflation/");
                },

                setCredentials: function(authData, login) {
                    $rootScope.globals = {
                        auth: {
                            login: login,
                            authData: authData
                        }
                    };

                    $http.defaults.headers.common['Authorization'] = authData.token_type + ' ' + authData.access_token; // jshint ignore:line
                    $cookieStore.put('globals', $rootScope.globals);
                },

                clearCredentials: function () {
                    $rootScope.globals = {};
                    $cookieStore.remove('globals');
                    $http.defaults.headers.common.Authorization = 'Basic';
                }
            };
            return authServiceFactory;
        });