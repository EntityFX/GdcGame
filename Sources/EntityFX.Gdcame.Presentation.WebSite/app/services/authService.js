angular
    .module("gdCameApp")
    .factory("authenticationService",
        function ($rootScope, $localStorage, $http, $location, apiUri) {
            var apiServer = "ns1";
            var authBaseUri = $location.protocol() + "://" + apiServer + '.' + apiUri + '/';

            var authServiceFactory = {

                login: function (login, password) {
                    return $http({
                        method: 'POST',
                        url: authBaseUri + "token",
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
                    return $http.post(authBaseUri + "api/auth/logout/")
                        .then(function (result) {
                            return result;
                        });
                },

                register: function (user) {
                    return $http.post(authBaseUri + "api/auth/register/", user)
                        .then(function (result) {
                            return result;
                        });
                },

                setCredentials: function(authData, login) {
                    $http.defaults.headers.common['Authorization'] = authData.token_type + ' ' + authData.access_token; // jshint ignore:line
                    var auth = {
                        login: login,
                        authData: authData
                    }
                    $localStorage.globals = {
                        auth: auth
                    };
                },

                clearCredentials: function () {
                    delete $localStorage.globals;
                    $http.defaults.headers.common.Authorization = 'Basic';
                }
            };
            return authServiceFactory;
        });