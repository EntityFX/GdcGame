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

        $.connection.hub.url = "http://localhost:8080/signalr";
        var hub = $.connection.gameDataHub;
        hub.client.getGameData = function (data) {
            $rootScope.gameData = data;
        };
        $.connection.hub.start();

        gameData.then(function (data) {
            $rootScope.gameData = data;
            
        });

        $interval(function () {
           // hub.client.getGameData(data);
        },
            1000);


        $scope.perfotmManulaStep = function () {
            gdCameApiService.performManualStep();
            //gdCameApiService.getCounters().then(function (value) {
            //    //  $rootScope.gameData.Counters = value.data;
            //});
        }

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
            //gdCameApiService.getCounters().then(function (value) {
            //    // $rootScope.gameData.Counters = value.data;
            //});
        }
    }]);