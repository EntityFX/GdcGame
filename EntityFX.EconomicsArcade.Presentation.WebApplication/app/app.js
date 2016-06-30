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

        $interval(function() {
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
            },
            1000);

        $scope.perfotmManulaStep = function() {
            gdCameApiService.performManualStep();
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
        }

        $scope.fightAgainstInflation = function() {
            gdCameApiService.fightAgainstInflation();
            gdCameApiService.getCounters().then(function (value) {
                $rootScope.gameData.Counters = value.data;
            });
        }
    }]);