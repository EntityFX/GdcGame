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
            gdCameApiService.performManualStep();
        }

        $scope.fightAgainstInflation = function () {
            gdCameApiService.fightAgainstInflation();
        }
    }]);