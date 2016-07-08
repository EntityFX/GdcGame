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
        hub.client.getGameData = function (data) {
            $rootScope.$apply(function() {
                $rootScope.gameData = data;
                }
            );

        };
        $.connection.hub.start();

        gameData.then(function (data) {
            $rootScope.gameData = data;
            
        });

        $scope.perfotmManulaStep = function () {
            gdCameApiService.performManualStep()
                .then(function (value) {
                    if (value.data.ModifiedCountersInfo != undefined) {
                        $rootScope.gameData.Counters.TotalFunds = value.data.ModifiedCountersInfo.TotalFunds;
                        $rootScope.gameData.Counters.CurrentFunds = value.data.ModifiedCountersInfo.CurrentFunds;
                        value.data.ModifiedCountersInfo.Counters.forEach(function (item) {
                            var oldCounter = $rootScope.gameData.Counters.Counters.filter(function (counter) {
                                return counter.Id === item.Id;
                            })[0];

                            if (oldCounter != undefined) {

                                oldCounter = item;
                            }
                        });
                    }

                });
        }

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
        }
    }]);