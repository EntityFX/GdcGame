var app = angular.module("gdCameApp", []);

app.factory('gameData',
    function ($q, gdCameApiService) {
        var deferred = $q.defer();
        gdCameApiService.getGameData().then(function (value) {
            deferred.resolve(value.data);
        });
        return deferred.promise;
    });


app.controller('appGameController',
[
    '$rootScope', '$scope', '$interval', 'gameData', 'gdCameApiService',
    function ($rootScope, $scope, $interval, gameData, gdCameApiService) {
        gameData.then(function (data) {
            $rootScope.gameData = data;
        });

        $interval(function () {
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
        },
            1000);


        $scope.perfotmManulaStep = function () {
            gdCameApiService.performManualStep();
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
        }

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
        }
        //var clientPushHubProxy = signalRHubProxy(
        //    signalRHubProxy.defaultServer, 'gameDataHub');

        //$scope.getGameData = function () {
        //    clientPushHubProxy.invoke('getGameData', function (data) {
        //        $scope.currentServerTimeManually = data;
        //    });
        //};
    }]);


app.factory('signalRHubProxy', ['$rootScope', 'signalRServer',
    function ($rootScope, signalRServer) {
        function signalRHubProxyFactory(serverUrl, hubName, startOptions) {
            var connection = $.hubConnection(signalRServer);
            var proxy = connection.createHubProxy(hubName);
            connection.start(startOptions).done(function () { });

            return {
                on: function (eventName, callback) {
                    proxy.on(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                off: function (eventName, callback) {
                    proxy.off(eventName, function (result) {
                        $rootScope.$apply(function () {
                            if (callback) {
                                callback(result);
                            }
                        });
                    });
                },
                invoke: function (methodName, callback) {
                    proxy.invoke(methodName)
                        .done(function (result) {
                            $rootScope.$apply(function () {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                },
                connection: connection
            };
        };

        return signalRHubProxyFactory;
    }]);