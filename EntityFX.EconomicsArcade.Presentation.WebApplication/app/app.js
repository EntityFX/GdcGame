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
    '$rootScope', '$scope', '$interval', 'gameData', 'gdCameApiService', '$location',
    function ($rootScope, $scope, $interval, gameData, gdCameApiService, $location) {

        $.connection.hub.url = $location.protocol() + "://" + $location.host() + ":8080/signalr";
        var hub = $.connection.gameDataHub;
        if (hub != null) {
            hub.client.getGameData = function (data) {
                $rootScope.$apply(function () {
                    $rootScope.gameData = data;
                }
                );

            };
            $.connection.hub.start();
        }

        gameData.then(function (data) {
            $rootScope.gameData = data;
            
        });

        $scope.perfotmManulaStep = function () {
            gdCameApiService.performManualStep()
                .then(function (value) {
                    if (value.data.ModifiedCountersInfo != undefined) {

                        if (value.data.ModifiedCountersInfo != undefined) {
                            $rootScope.$broadcast('counters.update', value.data.ModifiedCountersInfo);
                        }

                    }

                });
        }

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
        }
    }]);